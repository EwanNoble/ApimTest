using System.Threading.Tasks;
using ApimTestApi.Entities;

namespace ApimTestApi.Interfaces
{
    public interface IBaseRepository
    {
        Task<ResponseEntity> ValidateAsync();
    }
}
