using AutoMapper;
using ProductStore.Models;
using ProductStore.DTOs;


public class StoreMapper : Profile
{
    public StoreMapper()
    {
        CreateMap<Customer, CustomerResponse>()
            .ForMember(dest => dest.VoivodeshipName, opt => opt.MapFrom(src => (string)null));
        CreateMap<ProductType, ProductTypeResponse>();
        CreateMap<Product, ProductResponse>();
        CreateMap<Product, ProductPostResponse>();
        CreateMap<Voivodeship,  VoivodeshipResponse>();
        CreateMap<PurchaseDetail,  PurchaseDetailResponse>();
        CreateMap<Purchase,  PurchaseResponse>();
        CreateMap<PurchaseProposal,  PurchaseProposalResponse>();
    }

}

