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
            CreateMap<CreateMeetDTO, Meet>();
            CreateMap<UpdateMeetDTO, Meet>();
        }
    }
}