

using AutoMapper;
using StoreApi.Data.Dto;
using StoreApi.Data.Models;

namespace StoreApi.Mapping
{
    public class AutoMapperProfiles:Profile
    {

        public AutoMapperProfiles()
        {
            //CreateMap<User,UserDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ImportSetting, ImportSettingsDto>().ReverseMap();


        }
    }
}
