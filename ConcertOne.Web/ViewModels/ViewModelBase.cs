namespace ConcertOne.Web.ViewModels
{
    public class ViewModelBase
    {
        public bool IsLoggedIn { get; set; }

        public string UserName { get; set; }

        public ViewModelBase()
        {
            IsLoggedIn = false;
            UserName = string.Empty;
        }
    }
}
