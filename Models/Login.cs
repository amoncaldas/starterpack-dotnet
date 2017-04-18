

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace StarterPack.Models
{
    public class Login
    {       
        [Required]
        public string Email { get; set; }

        [Required, MinLength(10, ErrorMessage="A senha deve ter no mínimo 10 caracteres")]
        public string Password { get; set; }   
    }
}