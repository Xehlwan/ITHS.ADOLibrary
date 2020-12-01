using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DBContactLibrary
{
    internal static class SqlErrorHelper
    {
        public static bool IsErrorDuplicateKey(this SqlException ex) 
            => ex.Errors.OfType<SqlError>().Any(err => err.Number == 2627);
    }
}