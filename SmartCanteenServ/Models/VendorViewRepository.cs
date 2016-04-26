using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCanteenServ.Models
{
    public class OrderReport
    {
        public string OrderDate { get; set; }
        public long? OrderId { get; set; }
        public double Amount { get; set; }
    }

    public class CouponOrderReport
    {
        public string OrderDate { get; set; }
        public double CouponCount { get; set; }
        public double Amount { get; set; }
    }

    public class ShowOrders
    {
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public string MenuItem { get; set; }
        public int MenuItemId { get; set; }
        public Nullable<double> Rate { get; set; }
        public int Quantity { get; set; }
        public string EmployeeID { get; set; }
        public long? OrderID { get; set; }
    }

    public class VendorViewRepository
    {
        SmartCanteenDBEntities db = new SmartCanteenDBEntities();
        DateTime date = DateTime.Now;

        public IEnumerable<ShowOrders> GetPlacedOrdersDetail(int OrderNumber)
        {
            var result = (from FOMitem in db.FoodOrderMasters
                          join FCMitem in db.FoodCategoryMasters on
                          FOMitem.FOM_CategoryId equals FCMitem.FCM_ID
                          join MNUitem in db.MenuItem_Master on
                          FOMitem.FOM_MenuItemId equals MNUitem.MI_ID
                          where FOMitem.FOM_Date.Year == date.Year &&
                          FOMitem.FOM_Date.Month == date.Month &&
                          FOMitem.FOM_Date.Day == date.Day &&
                          FOMitem.FOM_ToCart == true &&
                          FOMitem.FOM_RequestedBln == true &&
                          FOMitem.FOM_CancelledBln == false &&
                          FOMitem.FOM_ConfirmedBln == false &&
                          FCMitem.FCM_ID == MNUitem.MI_FCM_ID &&
                          FOMitem.FOM_OrderNumber == OrderNumber
                          select new ShowOrders
                          {
                              Category = FCMitem.FCM_Type,
                              CategoryId = FOMitem.FOM_CategoryId,
                              MenuItem = MNUitem.MI_Description,
                              MenuItemId = FOMitem.FOM_MenuItemId,
                              Rate = FOMitem.FOM_OrderCost,
                              Quantity = FOMitem.FOM_Qty,
                              EmployeeID = FOMitem.FOM_EmpLoginId,
                              OrderID = FOMitem.FOM_OrderNumber
                          }).ToList();

            return result;
        }

        public IEnumerable<ShowOrders> GetPlacedOrdersSummary(int FloorId)
        {
            var result = (from FOMitem in db.FoodOrderMasters
                          join FCMitem in db.FoodCategoryMasters on
                          FOMitem.FOM_CategoryId equals FCMitem.FCM_ID
                          join MNUitem in db.MenuItem_Master on
                          FOMitem.FOM_MenuItemId equals MNUitem.MI_ID
                          where FOMitem.FOM_Date.Year == date.Year &&
                          FOMitem.FOM_Date.Month == date.Month &&
                          FOMitem.FOM_Date.Day == date.Day &&
                          FOMitem.FOM_ToCart == true &&
                          FOMitem.FOM_RequestedBln == true &&
                          FOMitem.FOM_CancelledBln == false &&
                          FOMitem.FOM_ConfirmedBln == false &&
                          FCMitem.FCM_ID == MNUitem.MI_FCM_ID &&
                          FOMitem.FOM_FloorId == FloorId
                          select new ShowOrders
                          {
                              EmployeeID = FOMitem.FOM_EmpLoginId,
                              OrderID = FOMitem.FOM_OrderNumber
                          }).Distinct();

            return result;
        }

        public IEnumerable<ShowOrders> GetAllSnacksList(int Snacksid)
        {
            var results = (from item in db.MenuItem_Master
                           where item.MI_FCM_ID == Snacksid
                           select new ShowOrders
                           {
                               MenuItemId = item.MI_ID,
                               MenuItem = item.MI_Description
                           }
                           ).ToList();

            return results;
        }

        public string VendorAcceptOrder(int OrderNumber)
        {
            try
            {
                var results = (from item in db.FoodOrderMasters
                               where item.FOM_OrderNumber == OrderNumber
                               select item).ToList();

                if (results != null)
                {
                    foreach (var item in results)
                    {
                        item.FOM_ConfirmedBln = true;
                    }
                    db.SaveChanges();
                }
                return "Order Served";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        //public IEnumerable<OrderReport> GetOrderList(string FromDate, string ToDate, Boolean PaymentStatus, int CategoryId, int FloorId)
        public IEnumerable<OrderReport> GetOrderList(int Month, int Year, Boolean PaymentStatus, int CategoryId, int FloorId)
        {
            //DateTime FDate = Convert.ToDateTime(FromDate).AddDays(-1); //TodaysDate;
            //DateTime TDate = Convert.ToDateTime(ToDate).AddDays(1); //TodaysDate;
            List<OrderReport> results = new List<OrderReport>();

            if (PaymentStatus)
            {
                results = (from item in db.FoodOrderMasters
                           where item.FOM_ConfirmedBln == true &&
                           item.FOM_Date.Month == Month &&
                               //item.FOM_Date > FDate &&
                               //item.FOM_Date < TDate &&
                           item.FOM_Date.Year == Year &&
                           item.FOM_CategoryId == CategoryId &&
                           item.FOM_FloorId == FloorId
                           group item by new
                           {
                               OrderNumber = item.FOM_OrderNumber,
                               OrderDate = item.FOM_Date.Month + "/" + item.FOM_Date.Day + "/" + item.FOM_Date.Year
                           } into GroupItem
                           select new OrderReport
                           {
                               OrderId = GroupItem.Key.OrderNumber,
                               OrderDate = GroupItem.Key.OrderDate,
                               Amount = GroupItem.Sum(amt => amt.FOM_OrderCost)
                           }).ToList();
            }
            else
            {
                results = (from item in db.FoodOrderMasters
                           where item.FOM_Date.Month == Month &&
                           item.FOM_Date.Year == Year &&
                               //item.FOM_Date > FDate &&
                               //item.FOM_Date < TDate &&
                           item.FOM_CategoryId == CategoryId &&
                           item.FOM_FloorId == FloorId &&
                           item.FOM_RequestedBln == true &&
                           item.FOM_ToCart == true &&
                           item.FOM_CancelledBln == false &&
                           item.FOM_ConfirmedBln == false
                           group item by new
                           {
                               OrderNumber = item.FOM_OrderNumber,
                               OrderDate = item.FOM_Date.Month + "/" + item.FOM_Date.Day + "/" + item.FOM_Date.Year
                           } into GroupItem
                           select new OrderReport
                           {
                               OrderId = GroupItem.Key.OrderNumber,
                               OrderDate = GroupItem.Key.OrderDate,
                               Amount = GroupItem.Sum(amt => amt.FOM_OrderCost)
                           }).ToList();
            }
            return results;
        }

        public IEnumerable<OrderReport> GetOrderListGraph(int Month, int Year, Boolean PaymentStatus, int CategoryId, int FloorId)
        {
            //DateTime FDate = Convert.ToDateTime(FromDate).AddDays(-1); //TodaysDate;
            //DateTime TDate = Convert.ToDateTime(ToDate).AddDays(1); //TodaysDate;
            List<OrderReport> results = new List<OrderReport>();

            if (PaymentStatus)
            {
                results = (from item in db.FoodOrderMasters
                           where item.FOM_ConfirmedBln == true &&
                           item.FOM_Date.Month == Month &&
                               //item.FOM_Date > FDate &&
                               //item.FOM_Date < TDate &&
                           item.FOM_Date.Year == Year &&
                           item.FOM_CategoryId == CategoryId &&
                           item.FOM_FloorId == FloorId
                           group item by new
                           {
                               OrderDate = item.FOM_Date.Month + "/" + item.FOM_Date.Day + "/" +item.FOM_Date.Year
                           } into GroupItem
                           select new OrderReport
                           {
                               OrderId = 1,
                               OrderDate = GroupItem.Key.OrderDate,
                               Amount = GroupItem.Sum(amt => amt.FOM_OrderCost)
                           }).ToList();
            }
            else
            {
                results = (from item in db.FoodOrderMasters
                           where item.FOM_Date.Month == Month &&
                           item.FOM_Date.Year == Year &&
                               //item.FOM_Date > FDate &&
                               //item.FOM_Date < TDate &&
                           item.FOM_CategoryId == CategoryId &&
                           item.FOM_FloorId == FloorId &&
                           item.FOM_RequestedBln == true &&
                           item.FOM_ToCart == true &&
                           item.FOM_CancelledBln == false &&
                           item.FOM_ConfirmedBln == false
                           group item by new
                           {
                               OrderDate = item.FOM_Date.Month + "/" + item.FOM_Date.Day + "/" +item.FOM_Date.Year
                           } into GroupItem
                           select new OrderReport
                           {
                               OrderId = 1,
                               OrderDate = GroupItem.Key.OrderDate,
                               Amount = GroupItem.Sum(amt => amt.FOM_OrderCost)
                           }).ToList();
            }
            return results;
        }

        //public IEnumerable<CouponOrderReport> GetCouponsReport(string FromDate, string ToDate, int FloorId)
        public IEnumerable<CouponOrderReport> GetCouponsReport(int Month, int Year, int FloorId)
        {
            //DateTime FDate = Convert.ToDateTime(FromDate).AddDays(-1); //TodaysDate;
            //DateTime TDate = Convert.ToDateTime(ToDate).AddDays(1); //TodaysDate;
            List<CouponOrderReport> results = new List<CouponOrderReport>();

            results = (from item in db.CouponMasters
                       where item.CM_Accepted_bln == true &&
                       item.CM_FLR_ID == FloorId &&
                       item.CM_Date.Month == Month &&
                       item.CM_Date.Year == Year
                       //item.CM_Date > FDate &&
                       //item.CM_Date < TDate
                       group item by new
                       {
                           OrderDate = item.CM_Date.Day + "/" + item.CM_Date.Month + "/" + item.CM_Date.Year
                       } into GroupItem
                       select new CouponOrderReport
                       {
                           OrderDate = GroupItem.Key.OrderDate,
                           CouponCount = GroupItem.Count(cnt => cnt.CM_Accepted_bln),
                           Amount = GroupItem.Count(cnt => cnt.CM_Accepted_bln) * 16
                       }).ToList();

            return results;
        }
    }
}
