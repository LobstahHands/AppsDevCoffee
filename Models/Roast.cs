namespace AppsDevCoffee.Models
{
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
}
