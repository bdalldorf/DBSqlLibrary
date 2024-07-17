using System;
using System.Collections.Generic;
using System.Text;

namespace DBSqlLibrary
{
    public class DatabaseModel
    {
        private string _ConnectionString {get; set;}
        protected SQLDBStateless _SQLDBStateless { get; set; }

        public DatabaseModel(string connectionString)
        {
            if (_SQLDBStateless == null || _ConnectionString != connectionString)
            {
                _ConnectionString = connectionString;
                _SQLDBStateless = new SQLDBStateless(connectionString);
            }
        }
    }
}
