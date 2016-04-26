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
    public class ManageCouponController : ApiController
    {
        ManageCouponRepository mcr = new ManageCouponRepository();

        public IEnumerable<CouponCode> Get(string EmpId)
        {
            return mcr.GetCoupon(EmpId);
        }

        public IEnumerable<CouponCode> Post(CouponMaster objCpm)
        {
            objCpm.CM_Date = DateTime.Now;
            objCpm.CM_FLR_ID = 0;
            objCpm.CM_Requested_bln = false;
            objCpm.CM_Accepted_bln = false;

            var couponid = mcr.GetCoupon(objCpm.CM_Empid);

            if (couponid.Count() == 0)
            {
                Boolean result = mcr.AssignCoupon(objCpm);
                couponid = mcr.GetCoupon(objCpm.CM_Empid);
            }

            return couponid;
        }

        public string Post(int FloorId, string Empid, string type)
        {
            if (type == "apply")
            {
                return mcr.ApplyCoupon(FloorId, Empid);
            }
            else if (type == "accept")
            {
                return mcr.AcceptCoupons(Empid);
            }
            else
            {
                return "";
            }
        }

        public IEnumerable<AcceptCouponList> Get(int FloorId)
        {
            var CouponList = mcr.GetToAcceptCouponsList(FloorId);
            return CouponList;
        }
    }
}