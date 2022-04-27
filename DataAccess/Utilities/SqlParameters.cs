using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataAccess.Utilities
{
    public class SqlParameters
    {
        public delegate SqlParameter SqlParametersDelegate(string parameterName, object value);
        public delegate void SetupParamDelegate();
        public SqlParametersDelegate SetupParam { get; set; }
        public SetupParamDelegate SetupParams { get; set; }
    }
}
