using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCanteenServ.Models
{
    public class CustomerOrderReport
    {
        public string OrderDate { get; set; }
        public long? OrderId { get; set; }
        public double Amount { get; set; }
    }

    public class CustomerOrderReportGraph
    {
        public int OrderDate { get; set; }
        public long? OrderId { get; set; }
        public double Amount { get; set; }
    }


    public class EmployeeRepository
    {
        SmartCanteenDBEntities db = new SmartCanteenDBEntities();

        public IEnumerable<Employee> GetAllEmployees()
        {
            return db.Employees.ToList();
        }

        public IEnumerable<Employee> GetEmpById_Pwd(string empLoginId, string EmpPassword)
        {
            var result = (from item in db.Employees
                          where item.EmpLoginId == empLoginId
                          select item).ToList();
            return result;
        }

        public IEnumerable<Employee> GetEmpById(string empLoginId)
        {
            var result = (from item in db.Employees
                          where item.EmpLoginId == empLoginId
                          select item).ToList();
            return result;
        }

        public Boolean AddNewEmp(Employee employee)
        {
            try
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public string AddMultipleEmployees(List<Employee>[] objEmp)
        {
            string strInsertedEmployees = "";
            string strRejectedEmployees = "";
            string strReturnMessage = "";
            foreach (var item in objEmp)
            {
                foreach (var x in item)
                {
                    try
                    {
                        var count = GetEmpById(x.EmpLoginId).Count();
                        if (count > 0)
                        {
                            x.EmpType = null;
                            x.Emp_FM_ID = null;

                            db.Employees.Add(x);
                            db.SaveChanges();
                            strInsertedEmployees = strInsertedEmployees + "," + x.EmpLoginId;
                        }
                        else
                        {
                            strRejectedEmployees = strRejectedEmployees + "," + x.EmpLoginId;
                        }
                    }
                    catch (Exception e)
                    {
                        strRejectedEmployees = strRejectedEmployees + "," + x.EmpLoginId + e.ToString();
                    }
                }
            }
            strReturnMessage = "Total accepted employees are: " + strInsertedEmployees + " \n Total rejected employees are: " + strRejectedEmployees;
            return strReturnMessage;
        }

        ///////////////////////////////////////////////////////////////////////
        public IEnumerable<CustomerOrderReport> GetOrderList(int Month, int Year, Boolean PaymentStatus, string EmpId)
        {
            List<CustomerOrderReport> results = new List<CustomerOrderReport>();

            if (PaymentStatus)
            {
                results = (from item in db.FoodOrderMasters
                           where item.FOM_ConfirmedBln == true &&
                           item.FOM_Date.Month == Month &&
                           //item.FOM_Date > FDate &&
                           //item.FOM_Date < TDate &&
                           item.FOM_Date.Year == Year &&
                           item.FOM_EmpLoginId == EmpId
                           group item by new
                           {
                               OrderNumber = item.FOM_OrderNumber,
                               OrderDate = item.FOM_Date.Month + "/" + item.FOM_Date.Day + "/" + item.FOM_Date.Year
                           } into GroupItem
                           select new CustomerOrderReport
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
                           item.FOM_EmpLoginId == EmpId &&
                           item.FOM_RequestedBln == true &&
                           item.FOM_ToCart == true &&
                           item.FOM_CancelledBln == false &&
                           item.FOM_ConfirmedBln == false
                           group item by new
                           {
                               OrderNumber = item.FOM_OrderNumber,
                               OrderDate = item.FOM_Date.Month + "/" + item.FOM_Date.Day + "/" + item.FOM_Date.Year
                           } into GroupItem
                           select new CustomerOrderReport
                           {
                               OrderId = GroupItem.Key.OrderNumber,
                               OrderDate = GroupItem.Key.OrderDate,
                               Amount = GroupItem.Sum(amt => amt.FOM_OrderCost)
                           }).ToList();
            }
            return results;
        }

        public IEnumerable<CustomerOrderReportGraph> GetOrderListGraph(int Month, int Year, Boolean PaymentStatus, string EmpId)
        {
            //DateTime FDate = Convert.ToDateTime(FromDate).AddDays(-1); //TodaysDate;
            //DateTime TDate = Convert.ToDateTime(ToDate).AddDays(1); //TodaysDate;
            List<CustomerOrderReportGraph> results = new List<CustomerOrderReportGraph>();

            if (PaymentStatus)
            {
                results = (from item in db.FoodOrderMasters
                           where item.FOM_ConfirmedBln == true &&
                           item.FOM_Date.Month == Month &&
                           item.FOM_Date.Year == Year &&
                           item.FOM_EmpLoginId == EmpId
                           group item by new
                           {
                               OrderDate = item.FOM_Date.Day
                           } into GroupItem
                           select new CustomerOrderReportGraph
                           {
                               OrderId = 1,
                               OrderDate = GroupItem.Key.OrderDate,
                               Amount = GroupItem.Sum(amt => amt.FOM_OrderCost) // Sum of amount for same date of specific employee from FoodOrderMasters table
                           }).Union(from item in db.Other_Expense
                                    where item.EmpLoginId == EmpId &&
                                    item.Expense_Date.Month == Month &&
                                    item.Expense_Date.Year == Year
                                    group item by new
                                    {
                                        OrderDate = item.Expense_Date.Day
                                    } into GroupItem
                                    select new CustomerOrderReportGraph
                                    {
                                        OrderId = 1,
                                        OrderDate = GroupItem.Key.OrderDate,
                                        Amount = GroupItem.Sum(amt => amt.Expense_Amount) // Sum of amount for same date of specific employee from Other_Expense table
                                    })
                                  .GroupBy(grpitem => new { grpitem.OrderDate, grpitem.OrderId })
                                  .OrderBy(orderitem => orderitem.Key.OrderDate)
                                  .Select(selItem => new CustomerOrderReportGraph
                                  { OrderId = 1, OrderDate = selItem.Key.OrderDate, Amount = selItem.Sum(amt => amt.Amount) }) // Sum of amount from the union of above two results
                                  .ToList();
            }
            else
            {
                results = (from item in db.FoodOrderMasters
                           where item.FOM_Date.Month == Month &&
                           item.FOM_Date.Year == Year &&
                           item.FOM_EmpLoginId == EmpId &&
                           item.FOM_RequestedBln == true &&
                           item.FOM_ToCart == true &&
                           item.FOM_CancelledBln == false &&
                           item.FOM_ConfirmedBln == false
                           group item by new
                           {
                               OrderDate =  item.FOM_Date.Day
                           } into GroupItem
                           select new CustomerOrderReportGraph
                           {
                               OrderId = 1,
                               OrderDate = GroupItem.Key.OrderDate,
                               Amount = GroupItem.Sum(amt => amt.FOM_OrderCost)
                           }).OrderBy(orderitem => orderitem.OrderDate).ToList();

                //IEnumerable<CustomerOrderReport> expense = FoodOrderMaster;
            }
            return results;
        }
        //////////////////////////////////////////////////////////////////////////


        public string SaveExpenses(Other_Expense OthExp)
        {
            DateTime date = Convert.ToDateTime(OthExp.Expense_Date);
            OthExp.Expense_Date = date;
            try {

                //OthExp.Expense_Date = date.Date;

                db.Other_Expense.Add(OthExp);
                db.SaveChanges();

                return "Expense Saved";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }
    }
}