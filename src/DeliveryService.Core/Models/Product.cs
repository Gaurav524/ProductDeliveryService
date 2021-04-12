using System;
using System.Collections.Generic;

namespace DeliveryService.Core.Models
{
    public sealed class Product
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public List<string> DeliveryDays { get; set; }
        public ProductType ProductType { get; set; } = ProductType.Normal;

        public int DaysInAdvance { get; set; } = 0;

        public DateTime OrderTime { get; set; } = DateTime.Now;
    }
}
