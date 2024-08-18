using System.Globalization;

namespace Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public DeviceData Data { get; set; } 
    }


    public class DeviceData
    {
        public string Color { get; set; } = string.Empty;
        public string Capacity { get; set; } = string.Empty;
        public decimal?  Price { get; set; }
        public string Generation { get; set; } = string.Empty;
        public int? Year { get; set; }
        public string CPUmodel { get; set; } = string.Empty;
        public string HardDiskSize { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StapColor { get; set; } = string.Empty;
        public string CaseSize { get; set; } = string.Empty;
    }
}
