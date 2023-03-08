using AutoMapper;
using FluentAssertions;
using ULSolutions.Core.Entities;
using ULSolutions.Core.Mapper;

namespace ULSolutions.Core.Tests.Automapper;

public class MappingProfileTests
{
    private readonly MappingProfile _mappingProfile = new();
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var configuration = new MapperConfiguration(cfg 
            => cfg.AddProfile(_mappingProfile));
        _mapper = new AutoMapper.Mapper(configuration);
    }

    [Fact]
    public void Maps_String_To_ExpressionResponse()
    {
        const string evaluatedExpression = "27";

        var mappedData = _mapper.Map<ExpressionResponse>(evaluatedExpression);

        mappedData.Response.Should().BeEquivalentTo(evaluatedExpression);
    }
}