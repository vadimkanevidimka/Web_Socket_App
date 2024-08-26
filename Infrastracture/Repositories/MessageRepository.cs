using Npgsql;
using Koshelekpy_Test.Domain.Entities;

namespace Koshelekpy_Test.Infrastracture.Repositories
{
    public class MessageRepository : IRepository<Message>
    {
        private NpgsqlCommand? _command;
        private readonly NpgsqlConnection _connection;
        private readonly ILogger _logger;
        public MessageRepository(IConfiguration connection, ILogger<MessageRepository> logger)
        {
            _connection = new NpgsqlConnection(connection.GetConnectionString("DefaultConnection"));
            _logger = logger;
        }

        /// <summary>
        /// Need implement methods Save for execution quaries
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<Guid> Create(Message obj)
        {
            if (_connection.State != System.Data.ConnectionState.Open) await _connection.OpenAsync();
            obj.Timestamp = DateTime.UtcNow;
            _command.CommandText = $"INSERT INTO messages (text, timestamp, sequencenumber) VALUES ('{obj.Text}','{obj.Timestamp}' ,'{obj.SequenceNumber}')\n";

            return Guid.NewGuid();
        }

        /// <summary>
        /// Need implement methods Save for execution quaries
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<Guid> Delete(Guid id)
        {
            if (_connection.State != System.Data.ConnectionState.Open) await _connection.OpenAsync();

            _command.CommandText = $"DELETE from messages WHERE id = {id}\n";

            return Guid.NewGuid();
        }

        public async Task<IEnumerable<Message>> GetAll()
        {
            if (_connection.State != System.Data.ConnectionState.Open) await _connection.OpenAsync();

            _command.CommandText = "SELECT * FROM messages\n";
            NpgsqlDataReader reader = await _command.ExecuteReaderAsync();
            var result = new List<Message>();

            while (await reader.ReadAsync())
            {
                result.Add(new Message(
                    id: reader.GetInt32(0),
                    text: reader.GetString(1),
                    timestamp: reader.GetDateTime(2),
                    sequenceNumber: reader.GetInt32(3)
                    ));
            }

            return result;
        }

        /// <summary>
        /// Need implement methods Save for execution quaries
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<Guid> Update(Message obj)
        {
            if (_connection.State != System.Data.ConnectionState.Open) await _connection.OpenAsync();

            _command.CommandText = $"UPDATE messages" +
            $" SET text='{obj.Text}'," +
            $"timestamp='{obj.Timestamp}'," +
            $"sequencenumber='{obj.SequenceNumber}'" +
            $" WHERE id = {obj.Id}\n";

            _logger.LogInformation($"Excecuted succsefully command:\n{_command.CommandText}\n");
            return Guid.NewGuid();
        }

        public async Task<List<Message>> GetAsync(DateTime from, DateTime to)
        {
            var messages = new List<Message>();

            if (_connection.State != System.Data.ConnectionState.Open) await _connection.OpenAsync();

            _command = new NpgsqlCommand($"SELECT Id, Text, Timestamp, SequenceNumber FROM Messages WHERE Timestamp BETWEEN '{from}' AND '{to}'", _connection);

            using var reader = await _command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                messages.Add(new Message(
                    id: reader.GetInt32(0),
                    text: reader.GetString(1),
                    timestamp: reader.GetDateTime(2),
                    sequenceNumber: reader.GetInt32(3)
                    ));
            }
            _logger.LogInformation($"Excecuted succsefully command:\n{_command.CommandText}\n");
            return messages;
        }

        public void CreateTransaction()
        {
            _command = new NpgsqlCommand("", _connection);
        }

        public void Rollback()
        {
            _command.CommandText = string.Empty;
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_command != null && !string.IsNullOrEmpty(_command.CommandText))
                    _logger.LogInformation($"Excecuting command:\n{_command.CommandText}\n");
                    await _command.ExecuteNonQueryAsync(cancellationToken);
            }
            catch (NpgsqlException ex)
            {

            }
        }

        public void EndTransaction()
        {
            _command.CommandText = string.Empty;
            _connection.Close();
        }
    }
}
