using System.Collections.Generic;
using System.Web.Http;
using SmartCanteenServ.Models;
using System.Web.Http.Cors;

namespace SmartCanteenServ.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ViewMenuController : ApiController
    {
        ViewMenuRepository vr = new ViewMenuRepository();

        public IEnumerable<TodaysMenu> Get(int FloorId, int CategoryId, int FoodType)
        {
            var results = vr.GetTodaysEmployeeMenuByFloor_Category(FloorId, CategoryId, FoodType);
            return results;
        }

        public IEnumerable<MenuByCategory> Get(int CategoryId)
        {
            var results = vr.GetMenuForSelectionByCategory(CategoryId);
            return results;
        }
    }
}
