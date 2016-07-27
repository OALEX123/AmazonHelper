namespace AmazonHelper.DataAccess.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        /* relations */
        public virtual CommonSettings CommonSettings { get; set; }
    }
}
