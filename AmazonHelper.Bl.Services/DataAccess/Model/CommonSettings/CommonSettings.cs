namespace AmazonHelper.DataAccess.Models
{
    public class CommonSettings
    {
        //public int CommonSettingsId { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }

        /* relations */
        public virtual User User { get; set; }
    }
}
