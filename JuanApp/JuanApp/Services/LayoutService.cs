using JuanApp.Data;
using JuanApp.Models;
using JuanApp.Models.Home.Product;

namespace PustokApp.Services
{
    public class LayoutService(JuanAppContext context)
    {
        public List<Setting> GetSettings()
        {
            return context.Setting.ToList();
        }
        public List<Category> GetCategories()
        {
            return context.Category.ToList();
        }
    }
}
