using Ambev.DeveloperEvaluation.Data.NoSql.Models;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Data.NoSql.MappingProfiles;

/// <summary>
/// AutoMapper profile for mapping between ProductDocument and Product entities.
/// </summary>
public sealed class ProductProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductProfile"/> class.
    /// </summary>
    public ProductProfile()
    {
        CreateMap<Models.ProductRating, Domain.Entities.ProductRating>()
            .ReverseMap();

        CreateMap<ProductDocument, Product>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ReverseMap()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
