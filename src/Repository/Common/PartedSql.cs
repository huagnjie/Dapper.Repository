using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    /// <summary>
    /// sql info
    /// </summary>
    public struct PartedSql
    {
        public string Raw;
        public string Select;
        public string Body;
        public string OrderBy;
    }
}
