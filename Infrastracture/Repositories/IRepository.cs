using Koshelekpy_Test.Domain.Entities;

namespace Koshelekpy_Test.Infrastracture.Repositories
{
    public interface IRepository<T>
    {
        public Task<Guid> Create(T obj);
        public Task<Guid> Update(T obj);
        public Task<Guid> Delete(Guid id);
        public Task<IEnumerable<T>> GetAll();
        public Task<List<Message>> GetAsync(DateTime from, DateTime to);
        public void CreateTransaction();
        public void Rollback();
        public Task CommitAsync(CancellationToken cancellationToken);
        public void EndTransaction();
    }
}
