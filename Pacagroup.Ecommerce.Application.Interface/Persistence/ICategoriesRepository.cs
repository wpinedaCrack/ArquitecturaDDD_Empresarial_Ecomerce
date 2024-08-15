using Pacagroup.Ecommerce.Domain.Entities;

namespace Pacagroup.Ecommerce.Application.Interface.Persistence
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Category>> GetAll();
    }
}
