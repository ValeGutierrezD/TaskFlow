using System.Data;
using TaskFlow.Core.Enum;

namespace TaskFlow.Core.Interfaces
{
    public interface IDbConnectionFactory
    {
        DatabaseProvider Provider { get; }
        IDbConnection CreateConnection();
    }
}
