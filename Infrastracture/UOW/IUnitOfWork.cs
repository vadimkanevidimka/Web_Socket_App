using Koshelekpy_Test.Domain.Entities;
using Koshelekpy_Test.Infrastracture.Repositories;

namespace Koshelekpy_Test.Infrastracture.UOW
{
    public interface IUnitOfWork
    {
        void CreateTransaction();
        void Rollback();
        Task Save();
        void EndTransaction();
    }
}
