using Ordering.Application.Models;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Infrastructure
{
    public interface IEMailService
    {
        Task<bool> SendEmail(Email email);
    }
}
