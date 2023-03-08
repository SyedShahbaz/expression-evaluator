using AutoMapper;
using ULSolutions.Core.Entities;

namespace ULSolutions.Core.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<string, ExpressionResponse>().ForMember(des 
            => des.Response, opt 
            => opt.MapFrom(src => src));
    }
}