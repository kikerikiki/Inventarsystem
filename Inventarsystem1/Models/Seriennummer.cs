using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventarsystem1.Models
{
    [Table("TblSeriennummer")]
    public partial class Seriennummer
    {
        public int SeriennummerId { get; set; }
        [Column("Seriennummer")]
        public int Srnummer { get; set; }

        public virtual Produkt SeriennummerNavigation { get; set; } = null!;
    }
}
