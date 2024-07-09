namespace Vocabulary.MVC.Models
{
    public record LoginUserViewModel (string Email, string Password) 
    {
        public LoginUserViewModel(): this(string.Empty, string.Empty)
        {
            
        }
    }
}
