using AutoMapper;
using ProjectD_API.Data.Messages;
using ProjectD_API.Data.Models;
namespace ProjectD_API.Data.Profiles
{
    public class PlayerSkillProfile : Profile
    {
        public PlayerSkillProfile()
        {
            CreateMap<PlayerSkill, PlayerSkill>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<PlayerSkill, PlayerSkillRequest>();
        }
    }
}
