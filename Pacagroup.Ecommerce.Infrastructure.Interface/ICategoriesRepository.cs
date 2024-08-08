using Pacagroup.Ecommerce.Domain.Entity;

namespace Pacagroup.Ecommerce.Infrastructure.Interface
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Categories>> GetAll();
    }
}
