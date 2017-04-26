using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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

        [Required, JsonIgnore]
        public string Salt { get; set; }  

        [JsonIgnore]
        public List<UserRole> UserRoles { get; set; }

        [NotMapped]
        public List<Role> Roles { get; set; }

        protected override List<string> Fill { get; set; }

        /// <summary>
        /// Método ShouldSerialize* utilizado Json.NET para informar se um atributo deve ser serializado ou não.
        /// O comportamento da deserialização não é modificado.
        /// http://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// 
        /// </summary>
        /// <returns>true ou false</returns>


        public User() {
             this.Fill = new List<string>() { "*" };
             this.DontFill = new List<string>() {"Password", "Salt", "UserRoles", "CreatedAt", "UpdatedAt"};
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
            if(this.UserRoles == null) {
                this.UserRoles = new List<UserRole>();

                this.Roles?.ForEach(role => {
                    Role roleLoaded = Role.Get(role.Id.Value);
                    if(roleLoaded != null)
                        this.UserRoles.Add(new UserRole(roleLoaded));
                });
            } else {
                //remove todo que não tem na lista de roles
                this.UserRoles.RemoveAll(ur => {
                    if(ur.Role == null && ur.RoleId != null) {
                        ur.Role = new Role(ur.RoleId);
                    }
                    return !this.Roles.Contains(ur.Role);
                });

                //adiciona os que não ainda não existem
                this.Roles?.ForEach(role => {
                    if(!this.UserRoles.Select(ur => ur.Role).Contains(role)) {
                        Role roleLoaded = Role.Get(role.Id.Value);
                        if(roleLoaded != null)
                            this.UserRoles.Add(new UserRole(roleLoaded));
                    }
                });                
            }
        }        
    }   
}