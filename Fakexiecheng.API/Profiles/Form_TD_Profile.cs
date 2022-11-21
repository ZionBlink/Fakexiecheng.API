using AutoMapper;
using Fakexiecheng.API.Dtos;
using Fakexiecheng.API.Models;

namespace Fakexiecheng.API.Profiles
{

    public class Form_TD_Profile : Profile
    {

        public Form_TD_Profile()
        {
            CreateMap<Form_TD, Form_TD_Dto>()
                .ForMember(destination => destination.FordOrderGuid, source => source.MapFrom(i => i.Ford_Order_Guid))
                .ForMember(destination => destination.ChannelCode, source => source.MapFrom(i => i.Channel_Code))
                  .ForMember(destination => destination.ChannelComment, source => source.MapFrom(i => i.Channel_Comment))
                    .ForMember(destination => destination.PurchaseIntention, source => source.MapFrom(i => i.Purchase_Intention))
                      .ForMember(destination => destination.PurchaseTime, source => source.MapFrom(i => i.Purchase_Time))
                        .ForMember(destination => destination.BusinessType, source => source.MapFrom(i => i.Business_Type))
                            // .ForMember(destination => destination.FordBusinessInfo, source => source.MapFrom(i => i.Channel_Code))
                            .ForMember(destination => destination.TdOrder, source => source.MapFrom(i => i.Td_Order))
                              .ForMember(destination => destination.TdStartTime, source => source.MapFrom(i => i.Td_Start_Time))
                                .ForMember(destination => destination.TdEndTime, source => source.MapFrom(i => i.Td_End_Time))
                                  .ForMember(destination => destination.TdDuration, source => source.MapFrom(i => i.Td_Duration))
                                    .ForMember(destination => destination.TdCity, source => source.MapFrom(i => i.Td_City))
                                    .ForMember(destination => destination.TdDistance, source => source.MapFrom(i => i.Td_Distance))
                                    .ForMember(destination => destination.TdEvaluate, source => source.MapFrom(i => i.Td_Evaluate))
                                    .ForMember(destination => destination.TdMerit, source => source.MapFrom(i => i.Td_Merit))
                                    .ForMember(destination => destination.TdPurchaseDate, source => source.MapFrom(i => i.Td_Purchase_Date))
                                    .ForMember(destination => destination.TdComment, source => source.MapFrom(i => i.Td_Comment));
            CreateMap<Form_TD_Dto, Form_TD>()
                 .ForMember(destination => destination.Ford_Order_Guid, source => source.MapFrom(i => i.FordOrderGuid))
                .ForMember(destination => destination.Channel_Code, source => source.MapFrom(i => i.ChannelCode))
                  .ForMember(destination => destination.Channel_Comment, source => source.MapFrom(i => i.ChannelComment))
                    .ForMember(destination => destination.Purchase_Intention, source => source.MapFrom(i => i.PurchaseIntention))
                      .ForMember(destination => destination.Purchase_Time, source => source.MapFrom(i => i.PurchaseTime))
                        .ForMember(destination => destination.Business_Type, source => source.MapFrom(i => i.BusinessType))
                            // .ForMember(destination => destination.FordBusinessInfo, source => source.MapFrom(i => i.Channel_Code))
                            .ForMember(destination => destination.Td_Order, source => source.MapFrom(i => i.TdOrder))
                              .ForMember(destination => destination.Td_Start_Time, source => source.MapFrom(i => i.TdStartTime))
                                .ForMember(destination => destination.Td_End_Time, source => source.MapFrom(i => i.TdEndTime))
                                  .ForMember(destination => destination.Td_Duration, source => source.MapFrom(i => i.TdDuration))
                                    .ForMember(destination => destination.Td_City, source => source.MapFrom(i => i.TdCity))
                                    .ForMember(destination => destination.Td_Distance, source => source.MapFrom(i => i.TdDistance))
                                    .ForMember(destination => destination.Td_Evaluate, source => source.MapFrom(i => i.TdEvaluate))
                                    .ForMember(destination => destination.Td_Merit, source => source.MapFrom(i => i.TdMerit))
                                    .ForMember(destination => destination.Td_Purchase_Date, source => source.MapFrom(i => i.TdPurchaseDate))
                                    .ForMember(destination => destination.Td_Comment, source => source.MapFrom(i => i.TdComment));

        }
    }
}


