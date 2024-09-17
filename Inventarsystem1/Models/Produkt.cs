using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventarsystem1.Models
{
    [Table("TblProdukt")]
    public partial class Produkt
    {
        public int ProduktId { get; set; }
        public int UserId { get; set; }
        public int ArtikelId { get; set; }
        public string Produktname { get; set; } = null!;
        public string Kommentar { get; set; } = null!;
        public int RaumId { get; set; }
        public DateTime Datum { get; set; }

        public virtual Artikel Artikel { get; set; } = null!;
        public virtual Raum Raum { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual Seriennummer? TblSeriennummer { get; set; }
    }
}
