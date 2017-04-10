namespace Starterpack.Models
{
    public class User : BaseModel<User>
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }   
            
    }
}