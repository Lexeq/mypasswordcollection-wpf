namespace MPC.Model
{
    public class PasswordItem
    {
        public virtual string Site { get; set; }

        public virtual string Login { get; set; }

        public virtual string Password { get; set; }

        public PasswordItem()
        {
            Site = "";
            Login = "";
            Password = "";
        }

        public override string ToString()
        {
            return Site;
        }
    }
}
