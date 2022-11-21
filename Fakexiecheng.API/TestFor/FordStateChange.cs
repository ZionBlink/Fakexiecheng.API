using Fakexiecheng.API.Models;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Fakexiecheng.API.Database;
using System.Linq;

namespace Fakexiecheng.API.TestFor
{
    public class Class
    {
    
        public class FordStateChange
        {
           
            
            private readonly IHttpClientFactory _clientFactory;
            private readonly AppDbContext _context;

            public FordStateChange( IHttpClientFactory clientFactory, AppDbContext context)
            {
                
                _clientFactory = clientFactory;
                _context = context;
            }

            /// <summary>
            /// 异步保存数据库
            /// </summary>
            /// <returns></returns>
            public async Task<bool> SaveAsync()
            {
                return (await _context.SaveChangesAsync() >= 0);
            }

            /// <summary>
            /// 用户反馈信息补全
            /// </summary>
            /// <param name="guid"></param>
            
            [Description("用户反馈信息补全")]
            public async Task<string> UserInfoCompleted(Guid guid)
            {
                var fordCustomerInfo = _context.Form_TD.FirstOrDefault(o => o.Ford_Order_Guid == guid) ?? new Form_TD();
                fordCustomerInfo.Update_Td_User_Info_Completed = true;
                fordCustomerInfo.Update_Td_User_Info_Complete_At = DateTime.Now;
                fordCustomerInfo.State_Current = "update_td_user_info_completed";
                fordCustomerInfo.State_Name_Current = "补全试驾反馈信息";
                fordCustomerInfo.State_Next = "update_td_order_info_completed";
                fordCustomerInfo.State_Name_Next = "补全试驾订单信息";
                if (fordCustomerInfo.Update_Td_Order_Info_Completed)
                {
                    await InfoCompleted(guid);
                }

                await SaveAsync();

                return "用户反馈信息补全已完成";
            }

            /// <summary>
            /// 订单信息补全
            /// </summary>
            /// <param name="guid"></param>
            /// <returns></returns>
            
            [Description("订单信息补全")]
            public async Task<string> OrderInfoCompleted(Guid guid)
            {
                var fordCustomerInfo = _context.Form_TD.FirstOrDefault(o => o.Ford_Order_Guid == guid) ?? new Form_TD();
                fordCustomerInfo.Update_Td_Order_Info_Completed = true;
                fordCustomerInfo.Update_Td_Order_Info_Complete_At = DateTime.Now;
                fordCustomerInfo.State_Current = "update_td_order_info_completed";
                fordCustomerInfo.State_Name_Current = "补全试驾订单信息";
                fordCustomerInfo.State_Next = "update_td_info_completed";
                fordCustomerInfo.State_Name_Next = "完成试驾信息补全";
                if (fordCustomerInfo.Update_Td_User_Info_Completed)
                {
                    await InfoCompleted(guid);
                }

                await SaveAsync();

                return "订单信息补全已完成";
            }

            /// <summary>
            /// 完整信息补全
            /// </summary>
            /// <param name="guid"></param>
            public async Task<string> InfoCompleted(Guid guid)
            {
                var fordCustomerInfo = _context.Form_TD.FirstOrDefault(o => o.Ford_Order_Guid == guid);
                fordCustomerInfo.Update_Td_Info_Completed = true;
                fordCustomerInfo.Update_Td_Info_Complete_At = DateTime.Now;
                fordCustomerInfo.State_Current = "update_td_info_completed";
                fordCustomerInfo.State_Name_Current = "完成试驾信息补全";
                fordCustomerInfo.State_Next = "req_td_info_completed";
                fordCustomerInfo.State_Name_Next = "发起试驾信息同步";

                await SaveAsync();

                return "完整信息补全已完成";
            }

            /// <summary>
            /// 外部信息同步发起
            /// </summary>
            /// <param name="guid"></param>
            /// <returns></returns>
            [Description("外部信息同步发起")]
            public async Task<string> ReqInfoCompleted(Guid guid)
            {
                var fordCustomerInfo = _context.Form_TD.FirstOrDefault(o => o.Ford_Order_Guid == guid);
                fordCustomerInfo.Req_Td_Info_Completed = true;
                fordCustomerInfo.Req_Td_Info_Complete_At = DateTime.Now;
                fordCustomerInfo.State_Current = "req_td_info_completed";
                fordCustomerInfo.State_Name_Current = "发起试驾信息同步";
                fordCustomerInfo.State_Next = "sync_td_info_completed";
                fordCustomerInfo.State_Name_Next = "完成试驾信息同步";

                await SaveAsync();

                return "试驾信息同步发起已完成";
            }

            /// <summary>
            /// 外部信息同步已完成
            /// </summary>
            /// <param name="guid"></param>
            /// <returns></returns>
            public async Task<string> SyncInfoCompleted(Guid guid)
            {
                var fordCustomerInfo = _context.Form_TD.FirstOrDefault(o => o.Ford_Order_Guid == guid);
                fordCustomerInfo.Sync_Td_Info_Completed = true;
                fordCustomerInfo.Sync_Td_Info_Complete_At = DateTime.Now;
                fordCustomerInfo.State_Current = "sync_td_info_completed";
                fordCustomerInfo.State_Name_Current = "外部信息同步已完成";
                fordCustomerInfo.State_Next = "";
                fordCustomerInfo.State_Name_Next = "同步完成，无下一状态";
                fordCustomerInfo.Sync_Is_Success = true;
                await SaveAsync();

                return "试驾信息同步已完成";
            }
        }
    }
}

