    using System;
    using System.Collections.Generic;

    namespace AppsDevCoffee.Models
    {
       

    //Each of these classes were generated with ChatGPT using my Database Diagram.

        public class Order
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public float TotalPaid { get; set; }
            public DateTime OrderDate { get; set; }
            public DateTime? PaidDate { get; set; }
            public float SubtotalCost { get; set; }
            public float PriceAdjustment { get; set; }
            public float TotalCost { get; set; }

            // Navigation property
            public User User { get; set; }

            // Navigation property
            public List<OrderItem> OrderItems { get; set; }
        }

        public class OrderItem
        {
            public int Id { get; set; }
            public int OrderId { get; set; }
            public int CurrentInventoryId { get; set; }
            public float OzQuantity { get; set; }
            public float CostPerOz { get; set; }

            // Navigation property
            public Order Order { get; set; }

            // Navigation property
            public CurrentInventory CurrentInventory { get; set; }
        }

        public class CurrentInventory
        {
            public int Id { get; set; }
            public int OzQuantity { get; set; }
            public DateTime DateAdded { get; set; }
            public DateTime DateLastModified { get; set; }
            public int RoastId { get; set; }
            public float PerOzPrice { get; set; }
            public int OriginTypeId { get; set; }
            public int TierTypeId { get; set; }

            // Navigation property
            public Roast Roast { get; set; }

            // Navigation property
            public OriginType OriginType { get; set; }

            // Navigation property
            public TierType TierType { get; set; }

            // Navigation property
            public List<InventoryLog> InventoryLogs { get; set; }
        }

        public class Roast
        {
            public int Id { get; set; }
            public int RoastTypeId { get; set; }
            public TimeSpan TotalRoastTime { get; set; }
            public TimeSpan FirstCrackTime { get; set; }
            public TimeSpan SecondCrackTime { get; set; }
            public TimeSpan CoolAt { get; set; }
            public float GreenWeightOz { get; set; }
            public float RoastedWeightOz { get; set; }
        }

        public class UserType
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int Active { get; set; }
            public DateTime DateAdded { get; set; }
        }

        public class OriginType
        {
            public int Id { get; set; }
            public string Country { get; set; }
            public string SupplierNotes { get; set; }
            public string RoasterNotes { get; set; }
        }

        public class RoastType
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int Active { get; set; }
        }

        public class TierType
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int Active { get; set; }
            public DateTime DateAdded { get; set; }
        }

        public class InventoryLog
        {
            public int Id { get; set; }
            public int CurrentInventoryId { get; set; }
            public string FieldUpdated { get; set; }
            public string PreviousFieldValue { get; set; }
            public string NewFieldValue { get; set; }
            public DateTime DateLastModified { get; set; }
            public string ModifyingUserId { get; set; }

            // Navigation property
            public CurrentInventory CurrentInventory { get; set; }
        }
    }
