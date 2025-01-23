namespace WoasApp.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Blocked { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsSelected { get; set; }
    }
}
