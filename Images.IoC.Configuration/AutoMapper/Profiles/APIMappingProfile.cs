using AutoMapper;
using DC = Images.API.DataContracts;
using S = Images.Services.Model;

namespace Images.IoC.Configuration.AutoMapper.Profiles
{
    public class APIMappingProfile : Profile
    {
        public APIMappingProfile()
        {
            CreateMap<DC.Image, S.Image>().ReverseMap();
        }
    }
}
