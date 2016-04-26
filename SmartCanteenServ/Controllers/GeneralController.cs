using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SmartCanteenServ.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GeneralController : ApiController
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
       
        public Int32 Get()
        {
            //DateTime newDate = date.AddHours(5.5);
            return ((date.Hour * 3600) + (date.Minute * 60) + (date.Second));
        }
       
    }
}
