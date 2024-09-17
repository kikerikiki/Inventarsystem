using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventarsystem1.Models
{
    [Table("TblRole")]
    public partial class Role
    {
        public Role()
        {
            TblUserRole = new HashSet<UserRole>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;

        public virtual ICollection<UserRole> TblUserRole { get; set; }
    }
}
