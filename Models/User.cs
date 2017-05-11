using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StarterPack.Core;
using StarterPack.Core.Models;
using StarterPack.Core.Persistence;

namespace StarterPack.Models
{
    public class User : Model<User>
    {
        [JsonIgnore]
        private string _salt;

        [Required, JsonIgnore, MaxLength(100)]
        public string Salt { 
            get { return _salt; }
        } 

        /// <summary>
        /// Define o salt
        /// </summary>
        private void SetSalt(){           
            _salt = StringHelper.GenerateSalt();
        }               

        [NotMapped, JsonIgnore]
        private string _plainPassword;

        [Required, MaxLength(255)]
        public string Name { get; set; }
        
        [Required, MaxLength(255)]       
        public string Email { get; set; }

        [Required, MaxLength(100)]
        public string Password { get; set; }   

        [NotMapped]
        public string PasswordConfirmation { get; set; }         

        [JsonIgnore]       
        public List<UserRole> UserRoles { get; set; }

        [NotMapped]
        public List<Role> Roles { get; set; }

        [JsonIgnore, NotMapped]       
        public string PlainPassword { 
            get {
                return _plainPassword;
            }
        }   

        [JsonIgnore]      
        public string ResetToken { get; set; } 

        [JsonIgnore]  
        public DateTime? ResetTokenDate { get; set; }  

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

        public bool ShouldSerializePasswordConfirmation()
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
        public void DefinePassword(string pass = null){
            // Se a senha ainda não tiver sido ddefinida, a define   
            if(pass != null) {
                this._plainPassword = pass;
            } else {                
                this._plainPassword = StringHelper.GeneratePassword();                     
            }
            SetSalt();
            
            this.Password = StringHelper.GenerateHash( this.PlainPassword + this.Salt );           
        }

        /// <summary>
        /// Atualiza a a senha e define como nulo o reset token de um usuário [mas não persiste]
        /// </summary>
        /// <param name="resetLogin"></param>
        public void UpdatePassword(Login resetLogin) {
            this._plainPassword = resetLogin.Password;           
            if(ValidateResetToken()){
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
            string now  = DateTime.UtcNow.ToString();
            this.ResetToken = null;
            if(this.Salt == null) {
                SetSalt();
            }
            this.ResetToken = StringHelper.GenerateHash(StringHelper.GeneratePassword()); 
            this.ResetTokenDate = DateTime.UtcNow;
        }  

        /// <summary>
        /// Valida se o resettoken ainda não expirou
        /// </summary>
        /// <returns></returns>
        public bool ValidateResetToken() {           
            return this.ResetTokenDate.Value.AddHours(1) > DateTime.UtcNow;
        }   
        
        /// <summary>
        /// Verifica se o usuário tem um role especificado
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool HasRole(string role) {
            if(this.UserRoles.Any(ur => ur.Role.Slug.ToLower() == role.ToLower())) {
                return true;
            }	
            return false;
        }

        /// <summary>
        /// Verifica se o usuário todos os roles de uma lista especificada
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public bool HasRoles(List<string> roles) {
            foreach (string role in roles)
            {	
                if (!HasRole(role)){
                    return false;
                }
            }
            return true;	
        }
    }   
}