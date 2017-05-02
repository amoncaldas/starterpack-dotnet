using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using StarterPack.Core;

namespace StarterPack.Models
{
    public class User : Model<User>
    {
        [MaxLength(255)]
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

        [JsonIgnore, NotMapped]       
        public string PlainPassword { get; set; }   

        [JsonIgnore]      
        public string ResetToken { get; set; }  

        protected override List<string> Fill { get; set; }

        public User() {
             this.Fill = new List<string>() { "*" };
             this.DontFill = new List<string>() {"Password", "Salt", "UserRoles", "CreatedAt", "UpdatedAt"};
        }

        /// <summary>
        /// Método ShouldSerialize* utilizado Json.NET para informar se um atributo deve ser serializado ou não.
        /// O comportamento da deserialização não é modificado.
        /// http://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// 
        /// </summary>
        /// <returns>true ou false</returns>
        public bool ShouldSerializeSalt()
        {
            return false;
        }   

        public bool ShouldSerializePassword()
        {
            return false;
        }             

        public void mapToRoles() {
            this.Roles = new List<Role>(); 

            this.UserRoles?.ForEach(userRole => {
                this.Roles.Add(userRole.Role);
            });
        }

        /// <summary>
        /// Mapeia os roles definidos no transiente 'Roles' de um usuário para o relacionamento UserRoles
        /// </summary>
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

        /// <summary>
        /// Define uma senha e o salt aleatória para o objeto do usuário, se ainda não tiver sido definida [mas não persiste]
        /// </summary>
        public void DefinePassword(){
            // Se a senha ainda não tiver sido ddefinida, a define            
            if (this.PlainPassword == null) {
                this.Salt = StringHelper.GenerateSalt();
                this.PlainPassword = StringHelper.GeneratePassword();                     
            }
            
            this.Password = StringHelper.GenerateHash( this.PlainPassword + this.Salt );           
        }

        /// <summary>
        /// Atualiza a a senha e define como nulo o reset token de um usuário [mas não persiste]
        /// </summary>
        /// <param name="resetLogin"></param>
        public void UpdatePassword(Login resetLogin) {
            this.PlainPassword = resetLogin.Password;
            // TODO: check if the token is still valid
            if(this.ResetToken == resetLogin.Token){
                this.ResetToken = null;
                this.DefinePassword();                
            }
            else {
                throw new System.Exception("passwords.invalidPasswordResetToken");
            }           
        }

        /// <summary>
        /// Atualiza o reset token de um usuário usando uma string aleatória [mas não persiste]
        /// </summary>
        public void UpdateResetPasswordToken(){
             // TODO: generate a token that has a timestamp stored in, so we can check if it is valid
            this.ResetToken = StringHelper.GenerateResetPasswordToken();
        }

                
    }   
}