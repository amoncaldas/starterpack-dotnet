

using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace StarterPack.Models
{
    public class User : Model<User>
    {       
        public string Name { get; set; }
        
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }   
           
    }

    public class UserValidator : AbstractValidator<User> {
        public UserValidator() {
            RuleFor(user => user.Name).NotNull().WithMessage("minha mensagem de erro");
            
        }
    }
}