namespace StarterPack.Core.Mail
{
    public class MailAddress
    {
        public string Name;
        public string Email;

        public MailAddress(string email, string name = null) {
            Name = name;
            Email = email;
        }
    }
}