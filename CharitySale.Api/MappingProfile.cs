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
            
            // Item mappings
            CreateMap<Item, Shared.Models.Item>();
            
            // Sale mappings
            CreateMap<Sale, Shared.Models.Sale>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.SaleItems))
                .ForMember(dest => dest.Change, opt => opt.Ignore()); // Change is calculated in service
                
            // SaleItem mappings
            CreateMap<SaleItem, Shared.Models.SaleItem>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.Name));
                
            // Shared Model -> Entity mappings
            
            // Item mappings
            CreateMap<CreateItem, Item>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.SaleItems, opt => opt.Ignore());
                
            // Sale mappings
            CreateMap<CreateSale, Sale>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.SaleItems, opt => opt.Ignore())
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()); // Will be calculated in service
                
            // SaleItem mappings
            CreateMap<CreateSaleItem, SaleItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Sale, opt => opt.Ignore())
                .ForMember(dest => dest.SaleId, opt => opt.Ignore())
                .ForMember(dest => dest.Item, opt => opt.Ignore())
                .ForMember(dest => dest.UnitPrice, opt => opt.Ignore()); // Will be set from Item.Price in service
                
            // Receipt mapping
            CreateMap<Sale, Receipt>()
                .ForMember(dest => dest.SaleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ReceiptNumber, opt => opt.MapFrom(src => $"R-{src.Id.ToString().Substring(0, 8)}"))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.SaleItems))
                .ForMember(dest => dest.AmountPaid, opt => opt.Ignore())
                .ForMember(dest => dest.ChangeAmount, opt => opt.Ignore())
                .ForMember(dest => dest.Change, opt => opt.Ignore());
        }
    }
}