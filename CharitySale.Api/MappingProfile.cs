using AutoMapper;
using CharitySale.Shared.Models;
using Item = CharitySale.Api.Entities.Item;
using Sale = CharitySale.Api.Entities.Sale;
using SaleItem = CharitySale.Api.Entities.SaleItem;

namespace CharitySale.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity -> Shared Model mappings
            CreateMap<Item, Shared.Models.Item>();

            CreateMap<Sale, Shared.Models.Sale>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.SaleItems))
                .ForMember(dest => dest.AmountPaid, opt => opt.MapFrom(src => src.AmountPaid))
                .ForMember(dest => dest.ChangeAmount, opt => opt.MapFrom(src => src.ChangeAmount))
                .ForMember(dest => dest.Change, opt => opt.Ignore());

            CreateMap<SaleItem, Shared.Models.SaleItem>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.Name))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId));

            // Shared Model -> Entity mappings
            CreateMap<CreateItem, Item>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.SaleItems, opt => opt.Ignore());

            CreateMap<CreateSale, Sale>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.SaleItems, opt => opt.Ignore())
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
                .ForMember(dest => dest.ChangeAmount, opt => opt.Ignore());  // This will be calculated in the service

            CreateMap<CreateSaleItem, SaleItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Sale, opt => opt.Ignore())
                .ForMember(dest => dest.SaleId, opt => opt.Ignore())
                .ForMember(dest => dest.Item, opt => opt.Ignore())
                .ForMember(dest => dest.UnitPrice, opt => opt.Ignore());
        }
    }
}