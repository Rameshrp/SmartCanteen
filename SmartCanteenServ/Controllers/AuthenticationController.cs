using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Web.Http;
using SmartCanteenServ.Models;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace SmartCanteenServ.Controllers
{
    //Allow cross origins to acces the API
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthenticationController : ApiController
    {
        // GET: api/Authentication/5
        MastekAuthenticationAPI objAPI = new MastekAuthenticationAPI();
        EmployeeRepository objEmp = new EmployeeRepository();
        UserInformation objUI = new UserInformation();

        public IEnumerable<Employee> Get(string UName, string Pwd)
        {
            var EmpData = objAPI.CheckVendorLogin(UName, Pwd);

            if (EmpData.Count() > 0)
            {
                return EmpData;
            }

            string clientId = "55D0E1DF-61CB-4A52-9C90-52B4DA429B8F";
            var AR = new AuthenticationRepository
            {
                UserName = UName,
                Password = Pwd,
                AppId = clientId
            };

            string json = new JavaScriptSerializer().Serialize(AR);
            string Token = objAPI.CallValidateUserAPI(json);

            if (Token.ToLower().Contains("error") || Token.ToLower().Contains("invalid"))
            {
                return null;
            }
            else
            {
                objUI = objAPI.CallGetUserDetailsAPI(Token, UName, clientId);

                string EmpLoginId = objUI.ID;
                string EmpLoginName = objUI.Name;

                EmpData = objEmp.GetEmpById_Pwd(EmpLoginId, Pwd);

                if (EmpData.Count() > 0)
                {
                    return EmpData;
                }
                else
                {
                    Employee objEmpTbl = new Employee();
                    objEmpTbl.EmpLoginId = EmpLoginId;
                    objEmpTbl.EmpType = 3;
                    objEmpTbl.EmpFirstName = EmpLoginName;

                    Boolean result = objEmp.AddNewEmp(objEmpTbl);

                    EmpData = objEmp.GetEmpById_Pwd(EmpLoginId, Pwd);
                    return EmpData;
                }
            }
        }
    }
}
