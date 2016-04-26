using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartCanteenServ.Models
{
    public class TodaysMenu
    {
        public int MI_ID { get; set; }
        public string MI_Description { get; set; }
        public double? MI_Price { get; set; }
    }

    public class MenuByCategory
    {
        public string MI_Description { get; set; }
    }

    public class ViewMenuRepository
    {
        SmartCanteenDBEntities db = new SmartCanteenDBEntities();

        public IEnumerable<MenuByCategory> GetMenuForSelectionByCategory(int CategoryId)
        {
            var results = (from item in db.MenuItem_Master
                           where item.MI_FCM_ID == CategoryId
                           select item).ToList();
            List<MenuByCategory> lstTodaysMenu = results.Select(s=> new MenuByCategory() {MI_Description = s.MI_Description }).ToList();
            return lstTodaysMenu;
        }

        public IEnumerable<TodaysMenu> GetTodaysEmployeeMenuByFloor_Category(int FloorId, int CategoryId, int FoodType)
        {
            int dayofweek = (int)DateTime.Now.DayOfWeek;

            var results = (from item in db.DailyMenuMasters
                           join itemMenuItem in db.MenuItem_Master
                           on item.DMM_MI_ID equals itemMenuItem.MI_ID
                           where item.DMM_FM_ID == FloorId && 
                           item.DMM_MI_FCM_ID == CategoryId &&
                           itemMenuItem.MI_FoodType == FoodType &&
                           item.DMM_WDM_ID == dayofweek
                           select itemMenuItem).ToList();

            List<TodaysMenu> lstTodaysMenu = results.Select(s => new TodaysMenu() { MI_ID = s.MI_ID, MI_Description = s.MI_Description, MI_Price = s.MI_Price}).ToList();
            return lstTodaysMenu;
        }
    }
}
