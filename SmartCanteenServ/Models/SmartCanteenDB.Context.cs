﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SmartCanteenDBEntities : DbContext
    {
        public SmartCanteenDBEntities()
            : base("name=SmartCanteenDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CouponMaster> CouponMasters { get; set; }
        public virtual DbSet<CouponNotification> CouponNotifications { get; set; }
        public virtual DbSet<DailyMenuMaster> DailyMenuMasters { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Floor_Master> Floor_Master { get; set; }
        public virtual DbSet<FoodCategoryMaster> FoodCategoryMasters { get; set; }
        public virtual DbSet<FoodOrderMaster> FoodOrderMasters { get; set; }
        public virtual DbSet<MenuItem_Master> MenuItem_Master { get; set; }
        public virtual DbSet<UserMaster> UserMasters { get; set; }
        public virtual DbSet<Weekday_Master> Weekday_Master { get; set; }
        public virtual DbSet<Other_Expense> Other_Expense { get; set; }
    }
}
