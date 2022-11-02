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
using Microsoft.Net.Http.Headers;
using System.Dynamic;

namespace Fakexiecheng.API.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        private readonly IUrlHelper _urlHelper;
        private readonly IPropertyMappingService _propertyMappingService;


        public TouristRoutesController(
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor  actionContextAccessor,


            IPropertyMappingService propertyMappingService
            )
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _propertyMappingService = propertyMappingService;
        }

        private string GenerateTouristRouteResourceURL(
             TouristRouteResoureceParameters parameters,
             PaginationResourceParamaters paginationResourceParamaters,
            ResourceUrlType type
            )
        {
            return type switch
            {
                ResourceUrlType.PreviousPage => _urlHelper.Link("GetTouristRoutes",
                new
                {  
                    
                    fields=parameters.Fields,
                    OrderBy = parameters.OrderBy,
                    keyword = parameters.Keyword,
                    rating = parameters.Rating,
                    pageNumber = paginationResourceParamaters.PageNumber - 1,
                    pageSize = paginationResourceParamaters.PageSize


                }
                ),
                ResourceUrlType.NextPage => _urlHelper.Link("GetTouristRoutes", new
                {
                    fields = parameters.Fields,
                    OrderBy = parameters.OrderBy,
                    keyword = parameters.Keyword,
                    rating = parameters.Rating,
                    pageNumber = paginationResourceParamaters.PageNumber + 1,
                    pageSize = paginationResourceParamaters.PageSize

                }),
                _ => _urlHelper.Link("GetTouristRoutes",
                new
                {
                    fields = parameters.Fields,
                    OrderBy = parameters.OrderBy,
                    keyword = parameters.Keyword,
                    rating = parameters.Rating,
                    pageNumber = paginationResourceParamaters.PageNumber,
                    pageSize = paginationResourceParamaters.PageSize


                })





            };
        }
        //application/ved.name.hateoas+json
        [Produces("application/json", 
            "application/vnd.blink.hateoas+json",
            "application/vnd.blink.touristRoute.simplify+json",
            "application/vnd.blink.touristRoute.simplify.hateoas+json")]
        [HttpGet(Name = "GetTouristRoutes")]
        [HttpHead]
        public async Task<IActionResult> GetTouristRoutes(
            [FromQuery] TouristRouteResoureceParameters parameters
            ,[FromQuery] PaginationResourceParamaters paginationResourceParamaters,
            [FromHeader(Name ="Accept")] string mediaType

            )
        {   //有list情况TryParseList
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parseMediaType)) {
                return BadRequest();
            
            }


            
            if (!_propertyMappingService.IsMappingExists<TouristRouteDto, TouristRoute>(parameters.OrderBy))
            {
                return BadRequest("请输入正确的排序参数");
            }

            if (!_propertyMappingService.IsPropertiesExists<TouristRouteDto>(parameters.Fields)) {

                return BadRequest("亲输入正确的塑型参数");
            }

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
         //   var touristRoutesDto = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);

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



            bool isHateoas = parseMediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

            var primaryMediaType = isHateoas
                ? parseMediaType.SubTypeWithoutSuffix
                .Substring(0, parseMediaType.SubTypeWithoutSuffix.Length - 8)
                : parseMediaType.SubTypeWithoutSuffix;

            //  var touristRoutesDto = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);

            //   var shapeDtoList = touristRoutesDto.ShapeDate(parameters.Fields);

            IEnumerable<object> touristRoutesDto;
            IEnumerable<ExpandoObject> shapedDtoList;

            if (primaryMediaType == "vnd.blink.touristRoute.simplify")
            {
                touristRoutesDto = _mapper
                    .Map<IEnumerable<TouristRouteSimplifyDto>>(touristRoutesFromRepo);

                shapedDtoList = ((IEnumerable<TouristRouteSimplifyDto>)touristRoutesDto)
                    .ShapeDate(parameters.Fields);
            }
            else
            {
                touristRoutesDto = _mapper
                    .Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);
                shapedDtoList =
                    ((IEnumerable<TouristRouteDto>)touristRoutesDto)
                    .ShapeDate(parameters.Fields);
            }




            if (isHateoas) {
                var linkDto = CreateLinksForTouristRouteList(parameters, paginationResourceParamaters);

                var shapedDtoWithLinklist = shapedDtoList.Select(t =>
                {
                    var touristRouteDictionary = t as IDictionary<string, object>;
                    var links = CreateLinkForTouristRoute(
                        (Guid)touristRouteDictionary["Id"], null);
                    touristRouteDictionary.Add("links", links);
                    return touristRouteDictionary;

                });
                var result = new
                {
                    Value = shapedDtoWithLinklist,
                    links = linkDto
                };

                return Ok(result);
            }

            return Ok(shapedDtoList);
           

           
        }
        private IEnumerable<LinkDto> CreateLinksForTouristRouteList( 
            TouristRouteResoureceParameters parameters
            ,  PaginationResourceParamaters paginationResourceParamaters
)
        {
            var links = new List<LinkDto>();
            //添加self
            links.Add(new LinkDto(
                    GenerateTouristRouteResourceURL(parameters, paginationResourceParamaters, ResourceUrlType.CurrentPage),
                    "self",
                    "GET"
                ));


            //“api/touristRoutes”
            links.Add(new LinkDto(
                Url.Link("CreateTouristRoute", null),
                "create_tourist_route",
                "POST"
                ));


            return links;
        
        }


        [HttpGet("{touristRouteId}", Name = "GetTouristRouteById")]

        public async Task<IActionResult> GetTouristRouteById(Guid touristRouteId,
            string fields
            )
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
            // return Ok(touristRouteDto);
            var linkDtos = CreateLinkForTouristRoute(touristRouteId, fields);

            var result = touristRouteDto.ShapeData(fields)
                as IDictionary<string,object>;

            result.Add("links", linkDtos);
            return Ok(result);
            // return Ok(touristRouteDto.ShapeData(fields));

        }
        private IEnumerable<LinkDto> CreateLinkForTouristRoute(Guid touristRouteId,
            string fields)
        {
            var links = new List<LinkDto>();
            links.Add(
                new LinkDto(Url.Link("GetTouristRouteById", new { touristRouteId ,fields}),
                "self",
                "GET"
                
                
                ));
            //update
            links.Add(
                new LinkDto(Url.Link("UpdateTouristRoute", new { touristRouteId })
                , "update", "PUT")
                );
            //partupdate
            links.Add(
                new LinkDto(Url.Link("PartiallyUpdateTouristRoute", new { touristRouteId })
                , "partially_update", "PATCH")
                );
            //delete
            links.Add(
                new LinkDto(Url.Link("DeleteTouristRoute", new { touristRouteId })
                , "delete", "DELETE")
                );
            //getpicture
            links.Add(
               new LinkDto(Url.Link("GetPictureListForTouristRoute", new { touristRouteId, fields }),
               "get_pictures",
               "GET"
                ));
            //postpicture
            links.Add(
               new LinkDto(Url.Link("CreateTouristRoutePicture", new { touristRouteId, fields }),
               "creat_picture",
               "POST"
                ));

            return links;
        }



        [HttpPost(Name = "CreateTouristRoute")]
        [Authorize(AuthenticationSchemes ="Bearer")]
        [Authorize]
        public async Task<IActionResult> CreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreationDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            await _touristRouteRepository.SaveAsync();
            var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRouteModel);

            var links = CreateLinkForTouristRoute(touristRouteModel.Id ,null);

            var result = touristRouteToReturn.ShapeData(null)
                as IDictionary<string,object>;

            result.Add("links", links);


            return CreatedAtRoute(
                "GetTouristRouteById",
                new { touristRouteId = result["Id"] },
                result
            );
        }


        [HttpPut("{touristRouteId}",Name = "UpdateTouristRoute")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTouristRoute(
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
        [HttpPatch("{touristRouteId}",Name = "PartiallyUpdateTouristRoute")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PartiallyUpdateTouristRoute([FromRoute] Guid touristRouteId,
            [FromBody] JsonPatchDocument<TouristRouteForUpdateDto> patchDocument) {

            if (!await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
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

        [HttpDelete("{touristRouteId}", Name = "DeleteTouristRoute")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTouristRoute([FromRoute]Guid touristRouteId) 
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