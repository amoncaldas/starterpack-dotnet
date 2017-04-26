using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace StarterPack.Models
{
    public class User : Model<User>
    {       
        public string Name { get; set; }
        
        [Required]
        public string Email { get; set; }

        [JsonIgnore]
        [Required]
        public string Password { get; set; }   


        [JsonIgnore, NotMapped]       
        public string PlainPassword { get; set; }   

        [JsonIgnore]
        [Required]
        public string Salt { get; set; }  

        public string Token { get; set; }  

        public List<UserRole> UserRoles { get; set; }   
                  
    }   
}