using System.Collections.Generic;
using WebCore.Entities;

namespace WebModelCore.Menu
{
    public class MenuModel
    {
        public MenuItemInfo Menu { get; set; }
        public List<MenuModel> MenuChild { get; set; }
        public MenuModel()
        {
            MenuChild = new List<MenuModel>();
        }
    }
}
