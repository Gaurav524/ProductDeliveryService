using DeliveryService.Core.Models;
using Newtonsoft.Json;

namespace DeliveryService.Api.Contracts
{
    public class DeliveryResponse
    {
        public string Status { get; set; }
        public string Product { get; set; }
        public Data Data { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(
                this, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
    }
}
