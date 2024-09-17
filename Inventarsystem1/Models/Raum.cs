using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventarsystem1.Models
{
    [Table("TblRaum")]
    public partial class Raum
    {
        public Raum()
        {
            TblProdukt = new HashSet<Produkt>();
        }

        public int RaumId { get; set; }
        public string RaumName { get; set; } = null!;
        public int Stockwerk { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Produkt> TblProdukt { get; set; }
    }
}
