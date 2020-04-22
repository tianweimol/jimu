using System;
// using System.ComponentModel.DataAnnotations;

namespace Manphi.DBOperate.Model
{
    public class BaseEntity
    {
        //[Key]
        public int Id { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;
        public int Status { get; set; }
    }
}
