using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventarsystem1.Models
{
    [Table("TblKategorie")]
    public partial class Kategorie
    {
        public Kategorie()
        {
            TblArtikel = new HashSet<Artikel>();
        }

        public int KategorieId { get; set; }
        public string KategorieName { get; set; } = null!;

        public virtual ICollection<Artikel> TblArtikel { get; set; }
    }
}
