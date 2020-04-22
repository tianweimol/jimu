using Mol.DBOperate.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manphi.DBOperate.Model
{
    public class Corporation: BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
