namespace NextFarma.Models
{
    public class Login
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public Login() { }

        public Login(string email, string senha)
        {
            Email = email;
            Senha = senha;
        }

    }
}