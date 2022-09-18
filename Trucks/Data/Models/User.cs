using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LeagueChampsDb.Data.Models
{
    public class User
    {
        public User()
        {
            this.UsersChampions = new HashSet<UserChampion>();
        }

        [Key]
        public int Id { get; set; }

        [Required, MinLength(3) , MaxLength(16), RegularExpression(@"^[A-Za-z0-9]{3,16}$")]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6), MaxLength(50)]
        public string Password { get; set; }

        public ICollection<UserChampion> UsersChampions { get; set; }
    }
}
