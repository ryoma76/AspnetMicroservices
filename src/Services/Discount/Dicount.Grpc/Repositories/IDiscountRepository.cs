using Discount.Grpc.Entities;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);
        Task<bool> CreateDiscount(Coupon copuon);
        Task<bool> UpdateDiscount(Coupon copuon);
        Task<bool> DeleteDiscount(string productName);
    }
}
