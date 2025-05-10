using JuanApp.Models.Home.Product;

namespace JuanApp.ViewModels
{
    public class UserProfileVm
    {
        public UserUpdateProfileVm UserUpdateProfileVm { get; set; }
        public List<Order> Orders { get; set; }
    }
}
