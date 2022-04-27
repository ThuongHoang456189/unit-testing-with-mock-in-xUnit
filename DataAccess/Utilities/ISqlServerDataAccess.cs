using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.Utilities
{
    public interface ISqlServerDataAccess
    {
        DataTable LoadData(string sql);
        bool IsDataExisting(string sql);
        bool SaveData(string sql);
    }
}
