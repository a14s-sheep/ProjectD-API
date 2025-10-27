using AutoMapper;
using ProjectD_API.Data.Models;
using ProjectD_API.Data.Messages;

namespace ProjectD_API.Data.Profiles
{
    public class PlayerItemProfile : Profile
    {
        public PlayerItemProfile()
        {
            CreateMap<PlayerItemRequest, PlayerItem>();
            CreateMap<ClassDefaultItem, PlayerItem>();
        }
    }
}
