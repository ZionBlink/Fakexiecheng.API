using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fakexiecheng.API.Models
{
    /// <summary>
    /// 福特客户信息表
    /// </summary>
    public class Form_TD
    {

        public Form_TD() {

            C_Addition_Date = DateTime.Now;
        }

        public DateTime C_Addition_Date { get; set; }
        /// <summary>
        /// 福特电马客户线索ID
        /// 自动生成
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int ID { get; set; }

        /// <summary>
        /// 福特订单Guid
        /// </summary>
        [Key]
        
        [Column("ford_order_guid")]

        public Guid Ford_Order_Guid { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Column("mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// 渠道名称
        /// </summary>
        [Column("channel")]
        public string Channel { get; set; } = "异业合作";

        /// <summary>
        /// 渠道编号
        /// </summary>
        [Column("channel_code")]

        public string Channel_Code { get; set; } = "crossCooperate";

        /// <summary>
        /// 渠道编号
        /// </summary>
        [Column("channel_comment")]

        public string Channel_Comment { get; set; }
        
        /// <summary>
        /// 活动名称
        /// </summary>
        [Column("active")]
        public string Active { get; set; } = "芯霖租车试驾留资";

        /// <summary>
        /// 活动码
        /// </summary>
        [Column("act_code")]

        public string Act_Code { get; set; } = "727HDBH221017002";

        /// <summary>
        /// 购车意向（0/无意向 ，1/有意向）
        /// </summary>
        [Column("purchase_intention")]

        public int Purchase_Intention { get; set; }

        /// <summary>
        /// 预计购车日期
        /// </summary>
        [Column("purchase_time")]

        public DateTime? Purchase_Time { get; set; }

       

        /// <summary>
        /// business类型（固定5 芯霖异业合作）
        /// </summary>
        [Column("business_type")]
        public int Business_Type { get; set; }


        /// <summary>
        /// 芯霖系统的试驾单号
        /// </summary>
        [Column("td_order")]
        public string Td_Order { get; set; }

        /// <summary>
        /// 试驾开始时间
        /// </summary>
        [Column("td_start_time")]

        public DateTime? Td_Start_Time { get; set; }

        /// <summary>
        /// 试驾结束时间
        /// </summary>
        [Column("td_end_time")]
        public DateTime? Td_End_Time { get; set; }

        /// <summary>
        /// 试驾持续天数
        /// </summary>
        [Column("td_duration")]
        public string Td_Duration { get; set; }

        /// <summary>
        /// 试驾城市
        /// </summary>
        [Column("td_city")]
        public string Td_City { get; set; }

        /// <summary>
        /// 试驾距离(公里，小数点后1位)
        /// </summary>
        [Column("td_distance")]
        public string Td_Distance { get; set; }

        /// <summary>
        /// 评价您对福特电马的驾乘感受:非常满意，满意，不满意，非常不满意
        /// </summary>
        [Column("td_evaluate")]
        public string Td_Evaluate { get; set; }

        /// <summary>
        /// 选择福特电马吸引您的地方:品牌、驾控、外观、内饰、车机、空间、续航
        /// </summary>
        [Column("td_merit")]
        public string Td_Merit { get; set; }

        /// <summary>
        /// 请选择您的期望购车时间1个月内，1-3个月，3-6个月、6-12个月、12个月以上
        /// </summary>
        [Column("td_purchase_date")]
        public string Td_Purchase_Date { get; set; }



        /// <summary>
        /// 请描述您对福特电马满意或不满意的地方
        /// </summary>
        [Column("td_comment")]
        public string Td_Comment { get; set; }


        /// <summary>
        /// 报名信息已提交
        /// </summary>
        [Column("reg_info_completed")]
        public bool Reg_Info_Completed { get; set; }


        /// <summary>
        /// 用户反馈信息补全已完成
        /// </summary>
        [Column("update_td_user_info_completed")]
        public bool Update_Td_User_Info_Completed { get; set; }


        /// <summary>
        /// 订单信息补全已完成
        /// </summary>

        [Column("update_td_order_info_completed")]
        public bool Update_Td_Order_Info_Completed { get; set; }


        /// <summary>
        /// 完整信息补全已完成
        /// </summary>
        [Column("update_td_info_completed")]
        public bool Update_Td_Info_Completed { get; set; }


        /// <summary>
        /// 外部信息同步发起已完成
        /// </summary>
        [Column("req_td_info_completed")]
        public bool Req_Td_Info_Completed { get; set; }


        /// <summary>
        /// 外部信息同步已完成
        /// </summary>
        [Column("sync_td_info_completed")]
        public bool Sync_Td_Info_Completed { get; set; }




        /// <summary>
        /// 报名信息提交时间
        /// </summary>
        [Column("update_td_user_info_complete_at")]
        public DateTime Update_Td_User_Info_Complete_At { get; set; }


        /// <summary>
        /// 试驾反馈信息提交时间
        /// </summary>
        [Column("reg_info_complete_at")]
        public DateTime Reg_Info_Complete_At { get; set; }


        /// <summary>
        /// 订单信息补全提交时间
        /// </summary>
        [Column("update_td_order_info_complete_at")]
        public DateTime Update_Td_Order_Info_Complete_At { get; set; }


        /// <summary>
        /// 完整信息补全提交时间
        /// </summary>
        [Column("update_td_info_complete_at")]
        public DateTime Update_Td_Info_Complete_At { get; set; }


        /// <summary>
        /// 外部信息同步发起时间
        /// </summary>
        [Column("req_td_info_complete_at")]
        public DateTime Req_Td_Info_Complete_At { get; set; }


        /// <summary>
        /// 外部信息同步完成时间
        /// </summary>
        [Column("sync_td_info_complete_at")]
        public DateTime Sync_Td_Info_Complete_At { get; set; }


        /// <summary>
        /// 用户的小程序 openid
        /// </summary>
        [Column("customer_wx_weapp_openid")]
        public string Customer_Wx_Weapp_Openid { get; set; }


        /// <summary>
        /// 当前状态码
        /// </summary>
        [Column("state_current")]
        public string State_Current { get; set; }


        /// <summary>
        /// 当前状态名称
        /// </summary>
        [Column("state_name_current")]
        public string State_Name_Current { get; set; }


        /// <summary>
        /// 下一状态码
        /// </summary>
        [Column("state_next")]
        public string State_Next { get; set; }


        /// <summary>
        /// 下一状态名称
        /// </summary>
        [Column("state_name_next")]
        public string State_Name_Next { get; set; }

        /// <summary>
        /// 外部信息同步结果
        /// </summary>
        [Column("request_result")]
        public string Request_Result { get; set; }


        /// <summary>
        /// 同步完成
        /// </summary>
        [Column("sync_is_success")]
        public bool Sync_Is_Success { get; set; }

        /// <summary>
        /// 意向购车省份
        /// </summary>
        [Column("intention_province")]
        public string Intention_Province { get; set; }

        /// <summary> 
        /// 意向购车城市
        /// </summary>
        [Column("intention_city")]
        public string Intention_City { get; set; }

        /// <summary>
        /// 是否联系客户
        /// </summary>
        [Column("is_connect_success")]
        public bool Is_Connect_Success { get; set; }

        /// <summary> 
        /// 联系客户时间
        /// </summary>
        [Column("connect_success_at")]
        public DateTime? Connect_Success_At { get; set; }
    }
}

