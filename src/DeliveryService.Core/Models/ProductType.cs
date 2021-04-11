using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace DeliveryService.Core.Models
{
    public enum ProductType
    {
        Normal = 0,
        External= 1,
        Temporary = 2
    }
}
