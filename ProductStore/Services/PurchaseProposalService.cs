using AutoMapper;
using ProductStore.Repositories;

namespace ProductStore.Services
{
    public interface IPurchaseProposalService
    {

    }
    public class PurchaseProposalService : IPurchaseProposalService
    {
        private readonly IPurchaseProposalRepository _purchaseProposalRepository;
        private readonly IMapper _mapper;
    }
}
