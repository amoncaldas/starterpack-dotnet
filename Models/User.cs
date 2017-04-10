

namespace StarterPack.Models
{
    public class User : Model<User>
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }   
           
    }
}