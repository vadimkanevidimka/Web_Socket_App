using Koshelekpy_Test.Domain.Entities;
using Koshelekpy_Test.Infrastracture.Repositories;

namespace Koshelekpy_Test.Infrastracture.UOW
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// Global Data Base Transaction variable
        /// </summary>
        /// <summary>
        /// Services for Repositories
        /// </summary>
        private IRepository<Message> _messages;


        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        /// <summary>
        /// State
        /// </summary>
        private bool disposed = false;
        public UnitOfWork(IRepository<Message> repo, IConfiguration configuration, ILogger<UnitOfWork> logger)
        {
            _messages = repo;
            _configuration = configuration;
            _logger = logger;
        }

        public IRepository<Message> Messages
        {
            get
            {
                if (_messages == null) return null;
                return _messages;
            }
        }

        public void CreateTransaction()
        {
            _messages?.CreateTransaction();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Rollback()
        {
            _messages?.Rollback();
        }

        public async Task Save()
        {
            try
            {
                await _messages.CommitAsync(new CancellationTokenSource().Token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void EndTransaction()
        {
            try
            {
                _messages.EndTransaction();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
