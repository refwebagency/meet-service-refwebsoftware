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

            CreateMap<Client, ReadClientDTO>();
            CreateMap<CreateClientDTO, Client>();
            CreateMap<UpdateClientDTO, Client>();
            // Partie RabbitMQ
            CreateMap<ClientUpdatedDto, Client>();

            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}