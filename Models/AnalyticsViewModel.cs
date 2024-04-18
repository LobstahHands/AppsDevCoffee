using System;

namespace AppsDevCoffee.Models
{
    public class AnalyticsViewModel
    {
        public DateTime StartOfAnalytics { get; set; }
        public int WeeksSinceStartOfYear { get; set; }
        public int CountOfOrders { get; set; }
        public int CountOfOrderItems { get; set; }
        public float SumOfTotalCost { get; set; }
        public float SumOfTotalPaid { get; set; }
        public float AvgOrdersPerWeek { get; set; }
        public float AvgOrderItemsPerWeek { get; set; }
        public float AvgItemsPerOrder { get; set; }
        public float AvgCostPerOrder { get; set; }
        public float UnpaidOrderTotal { get; set; }
    }
}
