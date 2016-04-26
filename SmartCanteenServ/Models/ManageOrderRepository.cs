using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCanteenServ.Models
{
    public class ShowCart
    {
        public Int64 FOM_ID { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public string MenuItem { get; set; }
        public int MenuItemId { get; set; }
        public Nullable<double> Rate { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderDisplayTime { get; set; }
        public string OrderDisplayDate { get; set; }
        public string EmployeeID { get; set; }
        public int FloorId { get; set; }
        public long? OrderID { get; set; }
    }

    public class OrdersList
    {
        public int OrderId { get; set; }
    }

    public class ManageOrderRepository
    {
        SmartCanteenDBEntities db = new SmartCanteenDBEntities();
        DateTime date = DateTime.Now;

        public string AddtoCart(List<FoodOrderMaster>[] objFOM)
        {
            try
            {
                foreach (var item in objFOM)
                {
                    foreach (var x in item)
                    {
                        x.FOM_CancelledBln = false;
                        x.FOM_ConfirmedBln = false;
                        x.FOM_Date = date;
                        x.FOM_OrderNumber = 0; //Convert.ToInt64(OrderNumResult);
                        x.FOM_RequestedBln = false;
                        x.FOM_ToCart = true;
                        db.FoodOrderMasters.Add(x);
                        db.SaveChanges();
                    }
                }
                return "Order added to cart.";
            }
            catch
            {
                return "Orders coudnot be saved."; //+ OrderNumResult;
            }
        }

        public string DirectOrder(List<FoodOrderMaster>[] objFOM)
        {
            try
            {
                string OrderNumResult = GenerateNextRandomNumber();

                foreach (var item in objFOM)
                {
                    foreach (var x in item)
                    {
                        x.FOM_CancelledBln = false;
                        x.FOM_ConfirmedBln = false;
                        x.FOM_ToCart = true;
                        x.FOM_Date = date;
                        x.FOM_OrderNumber = Convert.ToInt64(OrderNumResult);
                        x.FOM_RequestedBln = true;
                        x.FOM_OrderCost = x.FOM_OrderCost * x.FOM_Qty;
                        db.FoodOrderMasters.Add(x);
                        db.SaveChanges();
                    }
                }
                return "Order placed. Order Number: " + OrderNumResult;
            }
            catch
            {
                return "Orders coudnot be placed.";
            }
        }

        public string DeleteItemFromCartOrder(string OrderId)
        {
            int Order = Convert.ToInt32(OrderId);
            try
            {
                var results = (from item in db.FoodOrderMasters
                               where item.FOM_ID == Order
                               select item).SingleOrDefault();

                results.FOM_CancelledBln = true;
                db.SaveChanges();

                return "Item cancelled from order";
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public string DeleteCartOrder(string Empid)
        {
            try
            {
                var results = (from item in db.FoodOrderMasters
                               where item.FOM_EmpLoginId == Empid &&
                                item.FOM_Date.Year == date.Year &&
                                item.FOM_Date.Month == date.Month &&
                                item.FOM_Date.Day == date.Day &&
                                item.FOM_ToCart == true &&
                                item.FOM_RequestedBln == false &&
                                item.FOM_CancelledBln == false
                               select item).ToList();

                if (results != null)
                {
                    foreach (var item in results)
                    {
                        item.FOM_CancelledBln = true;
                    }
                    db.SaveChanges();
                }
                return "Order Deleted.";
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public string RequestOrder(List<FoodOrderMaster>[] objFOM)
        {
            string OrderNumResult = GenerateNextRandomNumber();

            try
            {
                foreach (var item in objFOM)
                {
                    foreach (var data in item)
                    {
                        var results = (from dt in db.FoodOrderMasters
                                       where dt.FOM_Date.Year == date.Year &&
                                        dt.FOM_Date.Month == date.Month &&
                                        dt.FOM_Date.Day == date.Day &&
                                        dt.FOM_ToCart == true &&
                                        dt.FOM_RequestedBln == false &&
                                        dt.FOM_CancelledBln == false &&
                                        dt.FOM_ConfirmedBln == false &&
                                        dt.FOM_ID == data.FOM_ID
                                       select dt).SingleOrDefault();

                        if (results != null)
                        {
                            results.FOM_Qty = data.FOM_Qty;
                            results.FOM_OrderCost = data.FOM_Qty * results.FOM_OrderCost;
                            results.FOM_RequestedBln = true;
                            results.FOM_OrderNumber = Convert.ToInt32(OrderNumResult);
                            db.SaveChanges();
                        }
                    }

                }

                return "Order placed. Order Number: " + OrderNumResult;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string PlaceOrder(string Empid)
        {
            string OrderNumResult = GenerateNextRandomNumber();

            try
            {
                var results = (from item in db.FoodOrderMasters
                               where item.FOM_EmpLoginId == Empid &&
                                item.FOM_Date.Year == date.Year &&
                                item.FOM_Date.Month == date.Month &&
                                item.FOM_Date.Day == date.Day &&
                                item.FOM_ToCart == true &&
                                item.FOM_RequestedBln == false &&
                                item.FOM_CancelledBln == false &&
                                item.FOM_ConfirmedBln == false
                               select item).ToList();

                if (results != null)
                {
                    foreach (var item in results)
                    {
                        item.FOM_RequestedBln = true;
                        item.FOM_OrderNumber = Convert.ToInt32(OrderNumResult);
                    }
                    db.SaveChanges();
                }
                return "Order placed. Order Number: " + OrderNumResult;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GenerateNextRandomNumber()
        {
            Random OrderNum = new Random();
            string OrderNumResult = OrderNum.Next(0, 9999).ToString();
            OrderNumResult = OrderNumResult.PadLeft(4, '0');
            if (CheckDuplicateOrderNumber(OrderNumResult))
            {
                return OrderNumResult;
            }
            else
            {
                return GenerateNextRandomNumber();
            }
        }
        public IEnumerable<ShowCart> Getnotifications(string Empid)
        {
            var result = (from FOMitem in db.FoodOrderMasters
                          join FCMitem in db.FoodCategoryMasters on
                          FOMitem.FOM_CategoryId equals FCMitem.FCM_ID
                          join MNUitem in db.MenuItem_Master on
                          FOMitem.FOM_MenuItemId equals MNUitem.MI_ID
                          where FOMitem.FOM_Date.Year == date.Year &&
                          FOMitem.FOM_Date.Month == date.Month &&
                          FOMitem.FOM_Date.Day == date.Day &&
                          FOMitem.FOM_EmpLoginId == Empid &&
                          FOMitem.FOM_ToCart == true &&
                          FOMitem.FOM_RequestedBln == false &&
                          FOMitem.FOM_CancelledBln == false &&
                          FCMitem.FCM_ID == MNUitem.MI_FCM_ID &&
                          FOMitem.FOM_EmpLoginId == Empid
                          select new ShowCart
                          {
                              FOM_ID = FOMitem.FOM_ID,
                              Category = FCMitem.FCM_Type,
                              CategoryId = FOMitem.FOM_CategoryId,
                              MenuItem = MNUitem.MI_Description,
                              MenuItemId = FOMitem.FOM_MenuItemId,
                              Rate = FOMitem.FOM_OrderCost,
                              Quantity = FOMitem.FOM_Qty,
                              OrderDate = FOMitem.FOM_Date,
                              FloorId = FOMitem.FOM_FloorId,
                              EmployeeID = Empid,
                              OrderID = 0
                          }).ToList();

            return result;
        }
        public IEnumerable<ShowCart> GetCart(string Empid, int FloorId)
        {
            var result = (from FOMitem in db.FoodOrderMasters
                          join FCMitem in db.FoodCategoryMasters on
                          FOMitem.FOM_CategoryId equals FCMitem.FCM_ID
                          join MNUitem in db.MenuItem_Master on
                          FOMitem.FOM_MenuItemId equals MNUitem.MI_ID
                          where FOMitem.FOM_Date.Year == date.Year &&
                          FOMitem.FOM_Date.Month == date.Month &&
                          FOMitem.FOM_Date.Day == date.Day &&
                          FOMitem.FOM_EmpLoginId == Empid &&
                          FOMitem.FOM_ToCart == true &&
                          FOMitem.FOM_RequestedBln == false &&
                          FOMitem.FOM_CancelledBln == false &&
                          FCMitem.FCM_ID == MNUitem.MI_FCM_ID &&
                          FOMitem.FOM_FloorId == FloorId
                          select new ShowCart
                          {
                              FOM_ID = FOMitem.FOM_ID,
                              Category = FCMitem.FCM_Type,
                              CategoryId = FOMitem.FOM_CategoryId,
                              MenuItem = MNUitem.MI_Description,
                              MenuItemId = FOMitem.FOM_MenuItemId,
                              Rate = FOMitem.FOM_OrderCost,
                              Quantity = FOMitem.FOM_Qty,
                              OrderDate = FOMitem.FOM_Date,
                              FloorId = FOMitem.FOM_FloorId,
                              EmployeeID = Empid,
                              OrderID = 0
                          }).ToList();

            return result;
        }

        public IEnumerable<ShowCart> GetPlacedOrders(string OrderId)
        {
            long? OrderNumber = Convert.ToInt64(OrderId);

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
                          select new ShowCart
                          {
                              FOM_ID = FOMitem.FOM_ID,
                              Category = FCMitem.FCM_Type,
                              CategoryId = FOMitem.FOM_CategoryId,
                              MenuItem = MNUitem.MI_Description,
                              MenuItemId = FOMitem.FOM_MenuItemId,
                              Rate = FOMitem.FOM_OrderCost,
                              Quantity = FOMitem.FOM_Qty,
                              OrderDisplayDate = FOMitem.FOM_Date.Day + "-" + FOMitem.FOM_Date.Month + "-" + FOMitem.FOM_Date.Year,
                              OrderDisplayTime = FOMitem.FOM_Date.Hour + ":" + FOMitem.FOM_Date.Minute + ":" + FOMitem.FOM_Date.Second,
                              FloorId = FOMitem.FOM_FloorId,
                              EmployeeID = FOMitem.FOM_EmpLoginId,
                              OrderID = FOMitem.FOM_OrderNumber
                          }).ToList();

            return result;
        }

        public IEnumerable<ShowCart> GetPlacedOrderSummary(string Empid, int FloorId)
        {
            var result = (from FOMitem in db.FoodOrderMasters
                          where FOMitem.FOM_Date.Year == date.Year &&
                          FOMitem.FOM_Date.Month == date.Month &&
                          FOMitem.FOM_Date.Day == date.Day &&
                          FOMitem.FOM_EmpLoginId == Empid &&
                          FOMitem.FOM_ToCart == true &&
                          FOMitem.FOM_RequestedBln == true &&
                          FOMitem.FOM_CancelledBln == false &&
                          FOMitem.FOM_ConfirmedBln == false &&
                          FOMitem.FOM_FloorId == FloorId
                          select new ShowCart
                          {
                              OrderID = FOMitem.FOM_OrderNumber
                          }).Distinct();

            return result;
        }

        public Boolean CheckDuplicateOrderNumber(string OrderNumber)
        {
            long Orderid = Convert.ToInt64(OrderNumber);

            var result = (from item in db.FoodOrderMasters
                          where item.FOM_Date.Year == date.Year &&
                          item.FOM_Date.Month == date.Month &&
                          item.FOM_Date.Day == date.Day &&
                          item.FOM_OrderNumber == Orderid
                          select item).ToList();
            if (result.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
