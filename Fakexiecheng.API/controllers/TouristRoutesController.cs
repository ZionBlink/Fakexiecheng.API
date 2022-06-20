using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fakexiecheng.API.Dtos;
using Fakexiecheng.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Text.RegularExpressions;
using Fakexiecheng.API.ResoureceParameters;
using Fakexiecheng.API.Moldes;
using Microsoft.AspNetCore.JsonPatch;
using FakeXiecheng.API.Helper;
using Microsoft.AspNetCore.Authorization;
using Fakexiecheng.API.helper;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fakexiecheng.API.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        private readonly IUrlHelper _urlHelper;


        public TouristRoutesController(
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor  actionContextAccessor

            )
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        private string GenerateTouristRouteResourceURL(
            [FromQuery] TouristRouteResoureceParameters parameters,
            [FromQuery] PaginationResourceParamaters paginationResourceParamaters,
            ResourceUrlType type
            )
        {
            return type switch
            {
                ResourceUrlType.PreviousPage => _urlHelper.Link("GetTouristRoutes",
                new
                {
                    keyword = parameters.Keyword,
                    rating = parameters.Rating,
                    pageNumber = paginationResourceParamaters.PageNumber - 1,
                    pageSize = paginationResourceParamaters.PageSize


                }
                ),
                ResourceUrlType.NextPage => _urlHelper.Link("GetTouristRoutes", new
                {
                    keyword = parameters.Keyword,
                    rating = parameters.Rating,
                    pageNumber = paginationResourceParamaters.PageNumber + 1,
                    pageSize = paginationResourceParamaters.PageSize

                }),
                _ => _urlHelper.Link("GetTouristRoutes",
                new
                {
                    keyword = parameters.Keyword,
                    rating = parameters.Rating,
                    pageNumber = paginationResourceParamaters.PageNumber,
                    pageSize = paginationResourceParamaters.PageSize


                })





            };
        }

        [HttpGet(Name = "GetTouristRoutes")]
        [HttpHead]
        public async Task<IActionResult> GetTouristRoutes(
            [FromQuery] TouristRouteResoureceParameters parameters
            ,[FromQuery] PaginationResourceParamaters paginationResourceParamaters

            )
        {
            /* Regex regex = new Regex(@"([A-Za-z0-9\-]+)(\d+)");
             string operatorType =" ";
             int raringVlaue = -1;
             Match match = regex.Match(rating);
             if (match.Success) {
                 operatorType = match.Groups[1].Value;
                 raringVlaue = Int32.Parse( match.Groups[2].Value);

             }*/


            var touristRoutesFromRepo = await _touristRouteRepository.GetTouristRoutesAsync(
                parameters.Keyword,
                parameters.RatingOperator,
                parameters.RatingValue,
                paginationResourceParamaters.PageSize,
                paginationResourceParamaters.PageNumber,
                parameters.OrderBy
                );
            if (touristRoutesFromRepo == null || touristRoutesFromRepo.Count() <= 0) {
                return NotFound("没有旅游路线");
            }
            var touristRoutesDto = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);

            var previousPageLink = touristRoutesFromRepo.HasPrevious
                ? GenerateTouristRouteResourceURL(parameters, paginationResourceParamaters, ResourceUrlType.PreviousPage)
                : null;

            var nextPageLink = touristRoutesFromRepo.HasNext
               ? GenerateTouristRouteResourceURL(parameters, paginationResourceParamaters, ResourceUrlType.NextPage)
               : null;

            // x-pagination
            var paginationMetadata = new
            {
                previousPageLink,
                nextPageLink,
                totalCount = touristRoutesFromRepo.TotalCount,
                pageSize = touristRoutesFromRepo.PageSize,
                currentPage = touristRoutesFromRepo.CurrentPage,
                totalPages = touristRoutesFromRepo.TotalPages

            };

            Response.Headers.Add("x-pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(touristRoutesDto);
        }



        [HttpGet("{touristRouteId}", Name = "GetTouristRouteById")]

        public async Task<IActionResult> GetTouristRouteById(Guid touristRouteId)
        {
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            if (touristRouteFromRepo == null) {
                return NotFound($"这条路线{touristRouteId}找不到");
            }
            /*var touristRouteDto = new TouristrouteDto() 
            {   Id=touristRouteFromRepo.Id,
                Title= touristRouteFromRepo.Title,
                Description = touristRouteFromRepo.Description
               

            };*/
            var touristRouteDto = _mapper.Map<TouristRouteDto>(touristRouteFromRepo);
            return Ok(touristRouteDto);

        }
        [HttpPost]
        [Authorize(AuthenticationSchemes ="Bearer")]
        [Authorize]
        public async Task<IActionResult> CreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreationDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            await _touristRouteRepository.SaveAsync();
            var touristRouteToReture = _mapper.Map<TouristRouteDto>(touristRouteModel);
            return CreatedAtRoute(
                "GetTouristRouteById",
                new { touristRouteId = touristRouteToReture.Id },
                touristRouteToReture
            );
        }


        [HttpPut("{touristRouteId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTourisRoute(
            [FromRoute] Guid touristRouteId,
            [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto)


        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游路线找不到");

            }

            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            _mapper.Map(touristRouteForUpdateDto, touristRouteFromRepo);


            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }
        [HttpPatch("{touristRouteId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PartiallyUpdateTouristRoute([FromRoute] Guid touristRouteId,
            [FromBody] JsonPatchDocument<TouristRouteForUpdateDto> patchDocument) {

            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游路线找不到");

            }
            var touristRouteFromRepo = _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            var touristRouteToPatch = _mapper.Map<TouristRouteForUpdateDto>(touristRouteFromRepo);
            patchDocument.ApplyTo(touristRouteToPatch, ModelState);

            if (!TryValidateModel(touristRouteToPatch)) {
                return ValidationProblem(ModelState);


            }

            await _mapper.Map(touristRouteToPatch, touristRouteFromRepo);


            await _touristRouteRepository.SaveAsync();

            return NoContent();

        }

        [HttpDelete("{touristRouteId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTouristRouteAsync([FromRoute]Guid touristRouteId) 
        {
            if (! (await  _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游路线找不到");

            }

            var touristRoute =await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            _touristRouteRepository.DeleteTouristRoute(touristRoute);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }


        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteByIDs(   [ModelBinder(typeof(ArrayModelBinder))][FromRoute]IEnumerable<Guid> touristIDs)
        
        {
            if (touristIDs == null) {

                return BadRequest();
            }
            var touristRoutesFromRepo = _touristRouteRepository.GetTouristRoutesByIDListAsync(touristIDs);
            _touristRouteRepository.DeleteTouristRoutes(touristRoutesFromRepo);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

    }
}