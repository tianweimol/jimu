using System;

namespace Mol.DBOperate.Model
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;
        public int Status { get; set; }
    }
}
