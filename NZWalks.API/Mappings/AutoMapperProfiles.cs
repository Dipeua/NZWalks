using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<AddRegionDto, Region>().ReverseMap();
            CreateMap<UpdateRegionDto, Region>().ReverseMap();

            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<AddWalkDto, Walk>().ReverseMap();
            CreateMap<UpdateWalkDto, Walk>().ReverseMap();

            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
        }
    }
}
