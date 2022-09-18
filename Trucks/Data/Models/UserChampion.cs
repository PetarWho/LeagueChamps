using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueChampsDb.Data.Models
{
    public class UserChampion
    {
        [Required, ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required, ForeignKey(nameof(Champion))]
        public int ChampionId { get; set; }
        public Champion Champion { get; set; }
    }
}
