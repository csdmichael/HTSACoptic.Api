using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace TokenAuth.Utils
{
    /// <summary>
    /// Wrapper class to maintain compatibility with existing code that used RelationalDataReader.DbDataReader
    /// </summary>
    public class DataReaderWrapper : IDisposable
    {
        public DbDataReader DbDataReader { get; }
        private readonly DbConnection _connection;
        private readonly bool _ownsConnection;

        public DataReaderWrapper(DbDataReader reader, DbConnection connection = null, bool ownsConnection = false)
        {
            DbDataReader = reader;
            _connection = connection;
            _ownsConnection = ownsConnection;
        }

        public void Dispose()
        {
            DbDataReader?.Dispose();
            if (_ownsConnection && _connection != null)
            {
                _connection.Dispose();
            }
        }
    }

    public static class RDFacadeExtensions
    {
        public static DataReaderWrapper ExecuteSqlQuery(this DatabaseFacade databaseFacade, string sql, params object[] parameters)
        {
            var connection = databaseFacade.GetDbConnection();
            var connectionOpened = false;
            
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                connectionOpened = true;
            }

            var command = connection.CreateCommand();
            command.CommandText = sql;

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    if (param is DbParameter dbParam)
                    {
                        command.Parameters.Add(dbParam);
                    }
                }
            }

            var reader = command.ExecuteReader(connectionOpened ? CommandBehavior.CloseConnection : CommandBehavior.Default);
            return new DataReaderWrapper(reader, connection, connectionOpened);
        }

        public static async Task<DataReaderWrapper> ExecuteSqlCommandAsync(this DatabaseFacade databaseFacade,
                                                             string sql,
                                                             CancellationToken cancellationToken = default(CancellationToken),
                                                             params object[] parameters)
        {
            var connection = databaseFacade.GetDbConnection();
            var connectionOpened = false;

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
                connectionOpened = true;
            }

            var command = connection.CreateCommand();
            command.CommandText = sql;

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    if (param is DbParameter dbParam)
                    {
                        command.Parameters.Add(dbParam);
                    }
                }
            }

            var reader = await command.ExecuteReaderAsync(connectionOpened ? CommandBehavior.CloseConnection : CommandBehavior.Default, cancellationToken);
            return new DataReaderWrapper(reader, connection, connectionOpened);
        }
    }
}
