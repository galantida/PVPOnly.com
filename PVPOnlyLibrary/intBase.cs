using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using System.Threading.Tasks; 4.5 only
using System.Data.SqlClient;

namespace PVPOnlyLibrary
{
    public interface intBase
    {
        bool populateBySqlDataReader(SqlDataReader sqlReader);        
    }
}
