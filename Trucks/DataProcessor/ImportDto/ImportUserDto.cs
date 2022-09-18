using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueChampsDb.DataProcessor.ImportDto
{
    public class ImportUserDto
    {
        [Required, MinLength(3), MaxLength(16), RegularExpression(@"^[A-Za-z0-9]{3,16}$")]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6), MaxLength(50)]
        public string Password { get; set; }
    }
}
