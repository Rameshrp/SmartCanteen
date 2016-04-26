using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SmartCanteenServ.Models;
using System.Web.Http.Cors;

namespace SmartCanteenServ.Controllers
{
    //Allow cross origins to acces the API
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmployeeController : ApiController
    {
        EmployeeRepository er = new EmployeeRepository();

        public IEnumerable<Employee> Get(string EmpLoginId, string EmpPassword)
        {
            var results = er.GetEmpById_Pwd(EmpLoginId, EmpPassword);
            return results;
        }

        public IEnumerable<Employee> Get()
        {
            var result = er.GetAllEmployees();
            return result.ToList();
        }

        public Boolean Get(string EmpLoginId)
        {
            var count = er.GetEmpById(EmpLoginId).Count();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string Post(Employee obj)
        {
            var EmpLoginId = obj.EmpLoginId;

            var count = er.GetEmpById(EmpLoginId).Count();

            if (count > 0)
            {
                return "Employee already exist.";
            }
            else
            {
                Boolean result = er.AddNewEmp(obj);

                if (result)
                {
                    return "Congratulations. Your account has been created.";
                }
                else
                {
                    return "Error while creating account.Please retry.";
                }
            }
        }

        #region For Rakesh with Multiple Employees
        public string Post(List<Employee>[] objEmp)
        {
            var result = er.AddMultipleEmployees(objEmp);
            return result;
        }
        #endregion

        public IEnumerable<CustomerOrderReport> Get(int Month, int Year, Boolean PaymentStatus, string type, string EmpId)
        {
                return er.GetOrderList(Month, Year, PaymentStatus,EmpId);
        }

        public IEnumerable<CustomerOrderReportGraph> Get(int Month, int Year, Boolean PaymentStatus,  string EmpId)
        {
            return er.GetOrderListGraph(Month, Year, PaymentStatus, EmpId);
        }
    }
}
