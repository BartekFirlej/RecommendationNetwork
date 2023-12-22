using AutoMapper;
using ProductStore.Models;
using ProductStore.DTOs;


public class StoreMapper : Profile
{
    public StoreMapper()
    {
        CreateMap<Customer, CustomerResponse>();
        CreateMap<ProductType, ProductTypeResponse>();
        CreateMap<Product, ProductResponse>();
        CreateMap<Product, ProductPostResponse>();
    }

}

