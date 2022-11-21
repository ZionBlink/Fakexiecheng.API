using System;

namespace Fakexiecheng.API.Dtos
{
    public class Form_TD_Dto
    {





        public int ID { get; set; }



        public Guid FordOrderGuid { get; set; }


        public string Name { get; set; }


        public string Mobile { get; set; }


        public string Channel { get; set; } = "异业合作";


        public string ChannelCode { get; set; } = "crossCooperate";



        public string ChannelComment { get; set; }


        public string District { get; set; }


        public string Address { get; set; }


        public string Gender { get; set; }


        public string Active { get; set; } = "芯霖租车试驾留资";


        public string ActCode { get; set; } = "727HDBH221017002";



        public int PurchaseIntention { get; set; }



        public DateTime? PurchaseTime { get; set; }




        public int BusinessType { get; set; }









        public string TdOrder { get; set; }



        public DateTime? TdStartTime { get; set; }


        public DateTime? TdEndTime { get; set; }


        public string TdDuration { get; set; }


        public string TdCity { get; set; }


        public string TdDistance { get; set; }


        public string TdEvaluate { get; set; }


        public string TdMerit { get; set; }


        public string TdPurchaseDate { get; set; }




        public string TdComment { get; set; }



        public bool Reg_Info_Completed { get; set; }



        public bool Update_Td_User_Info_Completed { get; set; }




        public bool Update_Td_Order_Info_Completed { get; set; }



        public bool Update_Td_Info_Completed { get; set; }



        public bool Req_Td_Info_Completed { get; set; }



        public bool Sync_Td_Info_Completed { get; set; }





        public DateTime Update_Td_User_Info_Complete_At { get; set; }



        public DateTime Reg_Info_Complete_At { get; set; }



        public DateTime Update_Td_Order_Info_Complete_At { get; set; }



        public DateTime Update_Td_Info_Complete_At { get; set; }



        public DateTime Req_Td_Info_Complete_At { get; set; }



        public DateTime Sync_Td_Info_Complete_At { get; set; }



        public string Customer_Wx_Weapp_Openid { get; set; }



        public string State_Current { get; set; }



        public string State_Name_Current { get; set; }



        public string State_Next { get; set; }



        public string State_Name_Next { get; set; }


        public string Request_Result { get; set; }



        public bool Sync_Is_Success { get; set; }


        public string Intention_Province { get; set; }


        public string Intention_City { get; set; }


        public bool Is_Connect_Success { get; set; }


        public DateTime? Connect_Success_At { get; set; }
    }
}


