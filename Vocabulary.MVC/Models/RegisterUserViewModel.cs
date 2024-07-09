namespace Vocabulary.MVC.Models
{
    public record RegisterUserViewModel (string Username, string Email, string Password) 
    {
        public RegisterUserViewModel(): this(string.Empty, string.Empty, string.Empty)
        {
            
        }
    }
}
