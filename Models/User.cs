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

        [Required]
        public string Password { get; set; }   

        [Required]
        public string Salt { get; set; }  

        [JsonIgnore]
        public List<UserRole> UserRoles { get; set; }

        [NotMapped]
        public List<Role> Roles { get; set; }

        /// <summary>
        /// Método ShouldSerialize* utilizado Json.NET para informar se um atributo deve ser serializado ou não.
        /// O comportamento da deserialização não é modificado.
        /// http://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// 
        /// </summary>
        /// <returns>true ou false</returns>
        public bool ShouldSerializePassword()
        {
            return false;
        }

        public bool ShouldSerializeSalt()
        {
            return false;
        }        

        public void mapToRoles() {
            this.Roles = new List<Role>(); 

            this.UserRoles?.ForEach(userRole => {
                this.Roles.Add(userRole.Role);
            });
        }

        public void mapFromRoles() {
            this.UserRoles = new List<UserRole>();

            this.Roles?.ForEach(role => {
                this.UserRoles.Add(new UserRole(role.Id));
            });
        }        
    }   
}