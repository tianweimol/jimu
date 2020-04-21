using System;
using System.Collections.Generic;
using System.Text;

namespace Mol.DBOperate.EF
{
    [Flags]
    public enum DbType
    {
        MySQL = 0,
        SQLServer = 1,
        Oracle = 2,
        PostgreSQL = 3,
        SQLite = 4
    }
}
