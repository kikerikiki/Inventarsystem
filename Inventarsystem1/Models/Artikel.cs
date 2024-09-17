using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventarsystem1.Models
{
    [Table("TblArtikel")]
    public partial class Artikel
    {
        public Artikel()
        {
            TblProdukt = new HashSet<Produkt>();
        }

        public int ArtikelId { get; set; }
        public string ArtikelName { get; set; } = null!;
        public int KategorieId { get; set; }
        public bool? HatSeriennummer { get; set; }

        public virtual Kategorie Kategorie { get; set; } = null!;
        public virtual ICollection<Produkt> TblProdukt { get; set; }
    }
}
