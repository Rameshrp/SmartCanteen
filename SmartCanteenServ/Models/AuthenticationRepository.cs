using System;
using System.Net.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using SmartCanteenServ.Models;
using System.Linq;

namespace SmartCanteenServ.Models
{
    public class AuthenticationRepository
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string client { get; set; }

        public string AppId { get; set; }
    }

    public class AuthenticationResult
    {
        public int status { get; set; }
        public string statusmessage { get; set; }
        public string objUser { get; set; }
    }

    public class UserInformation
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class MastekAuthenticationAPI
    {
        SmartCanteenDBEntities db = new SmartCanteenDBEntities();

        public string CallValidateUserAPI(string json)
        {
            HttpClient client = new HttpClient();
            string Url = "https://mysite.masteknet.com/MicroServices/MastAuth/ValidateUser";
            client.BaseAddress = new Uri(Url);
            HttpResponseMessage response = client.PostAsJsonAsync(client.BaseAddress, json).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseData = JsonConvert.DeserializeObject<AuthenticationResult>(response.Content.ReadAsStringAsync().Result);
                return responseData.statusmessage.ToString();
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public UserInformation CallGetUserDetailsAPI(string Token, string Uname, string ClientId)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://mysite.masteknet.com/MicroServices/MastAuth/GetUserDetails/" + Uname + "/" + ClientId);
            //client.DefaultRequestHeaders.Add("Token", Token);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", Token);
            HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseData = JsonConvert.DeserializeObject<UserInformation>(response.Content.ReadAsStringAsync().Result);
                return responseData;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Employee> CheckVendorLogin(string UName, string Pwd)
        {
            var results = (from item in db.Employees
                           where item.EmpLoginId == UName && item.EmpPassword == Pwd && item.EmpType == 2
                           select item).ToList();

            return results;
        }
    }
}