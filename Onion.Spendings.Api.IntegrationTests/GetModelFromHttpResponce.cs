using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Onion.Spendings.Api.IntegrationTests
{
    public static class GetModelFromHttpResponce
    {
        public static async Task<T> GetSingle<T>(HttpResponseMessage responce)
        {
            var byteResult = await responce.Content.ReadAsByteArrayAsync();
            var stringResult = Encoding.UTF8.GetString(byteResult);
            var record = JsonConvert.DeserializeObject<T>(stringResult);
            return record;
        }

       public static async Task<List<T>> GetList<T>(HttpResponseMessage responce)
        {
            var byteResult = await responce.Content.ReadAsByteArrayAsync();
            var stringResult = Encoding.UTF8.GetString(byteResult);
            var records = JsonConvert.DeserializeObject<List<T>>(stringResult);
            return records;
        }
    }
}
