

using System.ComponentModel.DataAnnotations;

namespace StarterPack.Models
{
    public class User : Model<User>
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Email { get; set; }

        [Required, StringLength(10, ErrorMessage="A senha deve ter no mínimo 10 caracteres")]
        public string Password { get; set; }   
           
    }
}