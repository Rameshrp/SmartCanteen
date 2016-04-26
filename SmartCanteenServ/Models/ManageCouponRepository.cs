using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCanteenServ.Models;
using System.Globalization;

namespace SmartCanteenServ.Models
{
    public class CouponCode
    {
        public long couponcode { get; set; }
    }

    public class AcceptCouponList
    {
        public string empid { get; set; }
        public long couponcode { get; set; }
    }

    public class ManageCouponRepository
    {
        SmartCanteenDBEntities db = new SmartCanteenDBEntities();

        public IEnumerable<CouponCode> GetCoupon(string Empid)
        {
            var date = DateTime.Now;
            var results = (from item in db.CouponMasters
                           where item.CM_Empid == Empid &&
                           item.CM_Date.Year == date.Year &&
                           item.CM_Date.Month == date.Month &&
                           item.CM_Date.Day == date.Day
                           select item).ToList();
            List<CouponCode> lstCouponCode = results.Select(s => new CouponCode() { couponcode = s.CM_ID }).ToList();
            return lstCouponCode;
        }

        public Boolean AssignCoupon(CouponMaster objCmp)
        {
            try
            {
                db.CouponMasters.Add(objCmp);
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

        public string ApplyCoupon(int FloorID, string EmpId)
        {
            try
            {
                var date = DateTime.Now;
                var resultCM = (from item in db.CouponMasters
                                where item.CM_Empid.ToLower() == EmpId.ToLower() &&
                               item.CM_Date.Year == date.Year &&
                               item.CM_Date.Month == date.Month &&
                               item.CM_Date.Day == date.Day &&
                               item.CM_Requested_bln == false
                                select item);

                if (resultCM.ToList().Count > 0)
                {
                    var result = resultCM.SingleOrDefault();

                    result.CM_FLR_ID = FloorID;
                    result.CM_Requested_bln = true;
                    db.SaveChanges();

                    var resultCN = (from item in db.CouponNotifications
                                    where item.CN_CM_Empid.ToLower() == EmpId.ToLower() &&
                                   item.CN_CM_Date.Year == date.Year &&
                                   item.CN_CM_Date.Month == date.Month &&
                                   item.CN_CM_Date.Day == date.Day &&
                                   item.CN_CM_Requested_bln == false
                                    select item).SingleOrDefault();

                    resultCN.CN_CM_FLR_ID = FloorID;
                    resultCN.CN_CM_Requested_bln = true;
                    db.SaveChanges();

                    return resultCN.CN_ID.ToString() ;
                }
                else
                {
                    return "You have not generated the coupon or you have already applied it.";
                }
            }
            catch
            {
                return "Please check your network connection";
            }
            finally
            {
            }
        }

        public string AcceptCoupons( string EmpId)
        {
            try
            {
                var date = DateTime.Now;
                var resultCM = (from item in db.CouponMasters
                                where item.CM_Empid.ToLower() == EmpId.ToLower() &&
                               item.CM_Date.Year == date.Year &&
                               item.CM_Date.Month == date.Month &&
                               item.CM_Date.Day == date.Day &&
                               item.CM_Requested_bln == true
                                select item).SingleOrDefault();

                resultCM.CM_Accepted_bln = true;
                db.SaveChanges();

                var resultCN = (from item in db.CouponNotifications
                                where item.CN_CM_Empid.ToLower() == EmpId.ToLower() &&
                               item.CN_CM_Date.Year == date.Year &&
                               item.CN_CM_Date.Month == date.Month &&
                               item.CN_CM_Date.Day == date.Day &&
                               item.CN_CM_Requested_bln == true
                                select item).SingleOrDefault();

                resultCN.CN_CM_Accepted_bln = true;
                db.SaveChanges();

                return "Coupon Accepted";
            }
            catch
            {
                return "Please check your network connection";
            }
            finally
            {
            }
        }

    public IEnumerable<AcceptCouponList> GetToAcceptCouponsList(int FloorId)
    {
        var date = DateTime.Now;
        var results = (from item in db.CouponMasters
                       where item.CM_Date.Year == date.Year &&
                       item.CM_Date.Month == date.Month &&
                       item.CM_Date.Day == date.Day &&
                       item.CM_FLR_ID == FloorId &&
                       item.CM_Requested_bln == true &&
                       item.CM_Accepted_bln == false
                       select item).ToList();
        List<AcceptCouponList> lstCouponCode = results.Select(s => new AcceptCouponList() { empid=s.CM_Empid, couponcode = s.CM_ID }).ToList();

        return lstCouponCode;
    }
}
}
