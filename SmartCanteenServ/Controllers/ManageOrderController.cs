using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartCanteenServ.Models;
using System.Web.Http.Cors;

namespace SmartCanteenServ.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ManageOrderController : ApiController
    {
        ManageOrderRepository mor = new ManageOrderRepository();

        public string Post(List<FoodOrderMaster>[] objFOM, string type)
        {
            string OrderNumber = "";

            if (type == "addcart")
            {
                OrderNumber = mor.AddtoCart(objFOM);
            }
            else if (type == "placeorder")
            {
                OrderNumber = mor.RequestOrder(objFOM);
            }
            else if (type == "directorder")
            {
                OrderNumber = mor.DirectOrder(objFOM);
            }

            return OrderNumber;
        }

        public string Post(string OrderIdOrEmpId, string type)
        {
            string result = "";
            if (type == "item")
            {
                result = mor.DeleteItemFromCartOrder(OrderIdOrEmpId);
            }
            else
            {
                result = mor.DeleteCartOrder(OrderIdOrEmpId);
            }
            return result;
        }

        public IEnumerable<ShowCart> Get(string Empid, string type, string floorid)
        {
            int FloorId = Convert.ToInt16(floorid);
            if (type == "getcart")
            {
                return mor.GetCart(Empid, FloorId);
            }
            else if (type == "getnotification")
            {
                return mor.Getnotifications(Empid);
            }
            else if (type == "ordersummary")
            {
                return mor.GetPlacedOrderSummary(Empid, FloorId);
            }
            else
            {
                return mor.GetPlacedOrders(Empid);
            }
        }
    }
}
