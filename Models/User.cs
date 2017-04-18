

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace StarterPack.Models
{
    public class User : Model<User>
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Email { get; set; }

        [JsonIgnore]
        [Required, MinLength(10, ErrorMessage="A senha deve ter no mínimo 10 caracteres")]
        public string Password { get; set; }   

        [JsonIgnore]
        [Required]
        public string Salt { get; set; }  

        public List<UserRole> UserRoles { get; set; }  
           
    }
}