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
    public class OtherExpenseController : ApiController
    {
        EmployeeRepository er = new EmployeeRepository();

        public string Post(Other_Expense objOthExp)
        {
            return er.SaveExpenses(objOthExp);
        }
    }
}
