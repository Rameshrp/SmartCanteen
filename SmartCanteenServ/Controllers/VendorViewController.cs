using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using SmartCanteenServ.Models;

namespace SmartCanteenServ.Controllers
{
    [EnableCors(origins: "*", methods: "*", headers: "*")]
    public class VendorViewController : ApiController
    {
        VendorViewRepository vr = new VendorViewRepository();

        public IEnumerable<ShowOrders> Get(int Id, string type)
        {
            if (type == "ordersummary")
            {
                return vr.GetPlacedOrdersSummary(Id); //Parameter FloorId
            }
            else if (type == "vendorplacedorders")
            {
                return vr.GetPlacedOrdersDetail(Id); //Parameter OrderNUmber
            }
            else
            {
                return vr.GetAllSnacksList(Id); //Parameter FloorId
            }
        }

        public string Post(int Id)
        {
            return vr.VendorAcceptOrder(Id);
        }


        public IEnumerable<OrderReport> Get(int Month, int Year, Boolean PaymentStatus, int CategoryId, int FloorId, string type)
        {
            if (type == "report")
            {
                return vr.GetOrderList(Month, Year, PaymentStatus, CategoryId, FloorId);
            }
            else
            {
                return vr.GetOrderListGraph(Month, Year, PaymentStatus, CategoryId, FloorId);
            }
        }

        public IEnumerable<CouponOrderReport> Get(int Month, int Year, int FloorId)
        {
            return vr.GetCouponsReport(Month, Year, FloorId);
        }
    }
}
