using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventarsystem1.Models
{
    [Table("TblUser")]
    public partial class User
    {
        public User()
        {
            TblProdukt = new HashSet<Produkt>();
            TblRaum = new HashSet<Raum>();
            TblUserRole = new HashSet<UserRole>();
        }

        
               
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Nachname { get; set; } = null!;
        public string Passwort { get; set; } = null!;

        [MaxLength(50)]
        [DefaultValue("")]
        public string Salt { get; set; }

        public virtual ICollection<Produkt> TblProdukt { get; set; }
        public virtual ICollection<Raum> TblRaum { get; set; }
        public virtual ICollection<UserRole> TblUserRole { get; set; }

        public bool CheckHashCode(string password)
        {
            byte[] salt_ = Convert.FromBase64String(Salt);
            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(
                    KeyDerivation.Pbkdf2(
                    password: password!,
                    salt: salt_,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));
            return hashed == Passwort;
        }

    }
}
