using AutoMapper;
using MeetService.Dtos;
using MeetService.Models;

namespace MeetService.Profiles
{
    public class MeetProfile : Profile
    {
        public MeetProfile()
        {
            CreateMap<Meet, ReadMeetDTO>();
            CreateMap<CreateMeetDTO, Meet>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Client, opt => opt.Ignore());
            CreateMap<UpdateMeetDTO, Meet>();
        }
    }
}