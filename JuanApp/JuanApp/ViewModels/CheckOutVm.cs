namespace JuanApp.ViewModels
{
    public class CheckOutVm
    {
        public OrderVm OrderVm { get; set; }
        public List<CheckOutItemVm> CheckOutItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
