//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartCanteenServ.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CouponMaster
    {
        public long CM_ID { get; set; }
        public string CM_Empid { get; set; }
        public System.DateTime CM_Date { get; set; }
        public int CM_FLR_ID { get; set; }
        public bool CM_Requested_bln { get; set; }
        public bool CM_Accepted_bln { get; set; }
    }
}
