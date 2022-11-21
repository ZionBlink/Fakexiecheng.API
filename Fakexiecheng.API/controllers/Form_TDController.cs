using AutoMapper;
using Fakexiecheng.API.Database;
using Fakexiecheng.API.Dtos;
using Fakexiecheng.API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimplePatch;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static Fakexiecheng.API.TestFor.Class;

namespace Fakexiecheng.API.controllers
{


    /// <summary>
    /// 福特线索接口控制器
    /// </summary>
    [Route("api/ser_forms/[controller]")]
    [ApiController]

    public class Form_TDController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _clientFactory;
        private readonly FordStateChange _stateService;
        public Form_TDController
            (
           AppDbContext context,
            IMapper mapper,
            IHttpClientFactory clientFactory
            )
        {
            _context = context;
            _mapper = mapper;
            _clientFactory = clientFactory;
            _stateService = new FordStateChange(_clientFactory, _context);
        }

        /// <summary>
        /// 创建一个福特订单
        /// </summary>
        /// <param name="fordCustomerInfo"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost]
        public async Task<IActionResult> CreateFordOrder([FromBody] Form_TD_Dto fordCustomerInfo)
        {
            fordCustomerInfo.Reg_Info_Completed = true;
            fordCustomerInfo.Reg_Info_Complete_At = DateTime.Now;
            fordCustomerInfo.State_Current = "reg_info_completed";
            fordCustomerInfo.State_Name_Current = "新增试驾报名信息";
            fordCustomerInfo.State_Next = "update_td_user_info_completed";
            fordCustomerInfo.State_Name_Next = "补全试驾反馈信息";
            var TdModel = _mapper.Map<Form_TD>(fordCustomerInfo);

            await _context.Form_TD.AddAsync(TdModel);
            await _context.SaveChangesAsync();
            var TdModelReturn = _mapper.Map<Form_TD_Dto>(TdModel);
            return Ok(new { errcode = 0, errmsg = "success", data = TdModelReturn });
        }



        /// <summary>
        /// 同步订单信息至福特
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpPost("sync/{guid}")]
        public async Task<IActionResult> SynchronizeInfoToFordApi(Guid guid)
        {
            var order = _context.Form_TD.FirstOrDefault(s => s.Ford_Order_Guid == guid);
            var fordState = new FordStateChange(_clientFactory, _context);
            await fordState.ReqInfoCompleted(guid);
            var syncClient = _clientFactory.CreateClient();
            //var appToken = new GetAppToken(_produceContext, _logger, _clientFactory, _context);
            //var tokenFord = await appToken.FordToken();
            // syncClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenFord}");
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            var purchaseTime = DateTime.Now;
            if (order.Td_Start_Time != null)
            {
                startDate = (DateTime)order.Td_Start_Time;
            }
            if (order.Td_End_Time != null)
            {
                endDate = (DateTime)order.Td_End_Time;
            }
            if (order.Purchase_Time != null)
            {
                purchaseTime = (DateTime)order.Purchase_Time;
            }
            var data = new
            {
                name = order.Name,
                mobile = order.Mobile,
                channel = order.Channel,
                channelCode = order.Channel_Code,
                province = order.Intention_Province,
                city = order.Intention_City,
                active = "芯霖租车试驾留资",
                actCode = "727HDBH221017002",
                purchaseIntention = order.Purchase_Intention,
                purchaseTime = purchaseTime.ToString("yyyy-MM-dd"),
                businessType = 5,
                businessInfo = new
                {
                    tdOrder = order.Td_Order,
                    tdStartTime = startDate.ToString("yyyy-MM-dd"),
                    tdEndTime = endDate.ToString("yyyy-MM-dd"),
                    tdDuration = order.Td_Duration,
                    tdCity = order.Td_City,
                    tdDistance = order.Td_Distance,
                    tdEvaluate = order.Td_Evaluate,
                    tdMerit = order.Td_Merit,
                    tdPurchaseDate = order.Td_Purchase_Date
                }
            };
            /*var decryptData = new
            {
                province = order.Province,
                city = order.City,
                name = order.Name,
                mobile = order.Mobile,
                channelCode = order.ChannelCode
            };*/
            var key = "eWX5XClKa3LXbsqx";
            var iv = "cluecenter123456";
            var infos = JsonConvert.SerializeObject(data);
            var result = decrypt(infos, key, iv);

            var urlResult = HttpUtility.UrlDecode(result);
            var contentDecrypt = " {" + "\"params\":  " + " \"" + urlResult + "\"" + " }";
            // Console.WriteLine(content);
            Console.WriteLine(data);
            Console.WriteLine(contentDecrypt);
            // var content = JsonConvert.SerializeObject(data);
            // var syncContent = new StringContent(content, Encoding.UTF8, "application/json"); 
            // var syncRequest = await syncClient.PostAsync("https://api.dev.cx727.ford.com.cn/clue-center/customer_new/v2/leads", syncContent);            
            // var fordResult = await syncRequest.Content.ReadAsStringAsync();
            // var jsonFordResult = (JObject)JsonConvert.DeserializeObject(fordResult);
            var syncContentDecrypt = new StringContent(contentDecrypt, Encoding.UTF8, "application/json");
            var syncRequestDecrypt = await syncClient.PostAsync("https://api.dev.cx727.ford.com.cn/scrm/uat/clue-center/customer_new/v3/leads", syncContentDecrypt);
            var fordResultDecrypt = await syncRequestDecrypt.Content.ReadAsStringAsync();
            var jsonFordResultDecrypt = (JObject)JsonConvert.DeserializeObject(fordResultDecrypt);
            var syncState = jsonFordResultDecrypt["data"].ToString();
            var requestResult = order.Request_Result;

            //同步是否完成
            if (syncState == "True")
            {
                await fordState.SyncInfoCompleted(guid);
                requestResult = "成功";
            }
            else
            {
                order.Sync_Td_Info_Completed = false;
                order.State_Current = "req_td_info_completed";
                order.State_Name_Current = "外部信息同步失败";

                var requestMsg = jsonFordResultDecrypt["msg"].ToString();
                requestResult = requestMsg;

                await fordState.SaveAsync();
                return Ok(new { errcode = 10001, errmsg = requestMsg });
            }

            var json = jsonFordResultDecrypt;


            Console.WriteLine(json);

            await fordState.SaveAsync();
            return Ok(new { errcode = 0, errmsg = "success", data = json });
        }

        /*
                /// <summary>
                /// 测试用解密接口
                /// </summary>
                /// <param name="info"></param>
                /// <returns></returns>
                [HttpPost("encrypt")]
                public IActionResult EncryptInfoToFordApi([FromBody] string info)
                {

                    var key = "eWX5XClKa3LXbsqx";

                    var iv = "cluecenter123456";

                    var result = encrypt(info, key, iv);

                    var urlResult = System.Web.HttpUtility.UrlDecode(result);

                    return Ok(urlResult);
                }
        */

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string decrypt(string data, string key, string iv)
        {
            using (var rijndael = new RijndaelManaged())
            {
                var encryptedBytes = new byte[0];
                try
                {
                    rijndael.KeySize = 128;
                    rijndael.Padding = PaddingMode.PKCS7;
                    rijndael.Mode = CipherMode.CBC;
                    rijndael.BlockSize = 128;
                    rijndael.Key = Encoding.ASCII.GetBytes(key);
                    rijndael.IV = Encoding.ASCII.GetBytes(iv);
                    var decryptor = rijndael.CreateEncryptor();
                    var inputBytes = Encoding.UTF8.GetBytes(data);
                    encryptedBytes = decryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                }
                catch (Exception)
                {
                    throw;
                }

                return HttpUtility.UrlEncode(Convert.ToBase64String(encryptedBytes));
            }
        }



        /// <summary>
        /// 福特订单状态变更
        /// </summary>
        /// <param name="fordCustomerInfo"></param>
        /// <param name="guid"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpPatch("update/{guid}/{stateId}")]
        public async Task<IActionResult> FordOrderStateChange([FromBody] Delta<Form_TD> fordCustomerInfo, Guid guid, string stateId)
        {
            var order = _context.Form_TD.FirstOrDefault(s => s.Ford_Order_Guid == guid);

            fordCustomerInfo.Patch(order);
            await _stateService.SaveAsync();

            switch (stateId)
            {
                case "update_td_user_info_completed":
                    await _stateService.UserInfoCompleted(guid);
                    break;
                case "update_td_order_info_completed":
                    await _stateService.OrderInfoCompleted(guid);
                    break;
                default:
                    return Ok(new { errcode = 10001, errmsg = "未知的订单状态，请检查订单" });
            }

            if (fordCustomerInfo.ContainsKey("TdPurchaseDate") && !string.IsNullOrEmpty(fordCustomerInfo["TdPurchaseDate"].ToString()))
            {
                switch (fordCustomerInfo["TdPurchaseDate"].ToString())
                {
                    case "12个月以上":
                        order.Purchase_Time = order.C_Addition_Date.AddYears(1);
                        break;
                    case "6-12个月":
                        order.Purchase_Time = order.C_Addition_Date.AddMonths(6);
                        break;
                    case "3-6个月":
                        order.Purchase_Time = order.C_Addition_Date.AddMonths(3);
                        break;
                    case "1-3个月":
                        order.Purchase_Time = order.C_Addition_Date.AddMonths(1);
                        break;
                    case "1个月内":
                        order.Purchase_Time = order.C_Addition_Date.AddMonths(1);
                        break;
                }
            }

            fordCustomerInfo.Patch(order);
            await _stateService.SaveAsync();
            return Ok(new { errcode = 0, errmsg = "success", data = order });
        }
        /// <summary>
        /// 福特联系客户接口
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpPost("connect/{guid}")]
        public async Task<IActionResult> ConnectCustom(Guid guid)
        {

            var tdOrder = _context.Form_TD.FirstOrDefault(td => td.Ford_Order_Guid == guid);
            tdOrder.Is_Connect_Success = true;
            tdOrder.Connect_Success_At = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(new { errcode = 0, errmsg = "success", data = tdOrder });
        }
    }
}

