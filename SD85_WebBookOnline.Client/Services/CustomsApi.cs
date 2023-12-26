using Newtonsoft.Json;
using System.Text;

namespace SD85_WebBookOnline.Client.Services
{
    public class CustomsApi<T> where T : class
    {
        public async Task<List<T>> ApiGetListobj(string data)
        {
            var url = $"https://localhost:7079/api/";
            var httpClient = new HttpClient();
            var respones = httpClient.GetAsync(url + data).Result;
            var dataapi = respones.Content.ReadAsStringAsync().Result;
            var dataobj = JsonConvert.DeserializeObject<List<T>>(dataapi);
            return dataobj.ToList();
        }
        public async Task<T> ApiGetObj(string data)
        {
            var url = $"https://localhost:7079/api/";
            var httpClient = new HttpClient();
            var respones = httpClient.GetAsync(url + data).Result;
            var dataapi = respones.Content.ReadAsStringAsync().Result;
            var dataobj = JsonConvert.DeserializeObject<T>(dataapi);
            return dataobj;
        }
        public async Task<bool> ApiCreateObj(T obj, string name)
        {
            string data = JsonConvert.SerializeObject(obj);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            string requestURL = $"https://localhost:7079/api/";
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(requestURL + name, content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> ApiUpdateObj(T obj, string name)
        {
            string data = JsonConvert.SerializeObject(obj);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            string requestURL = $"https://localhost:7079/api/";
            var httpClient = new HttpClient();
            var response = await httpClient.PutAsync(requestURL + name, content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> ApiDeleteObj(string name)
        {
            string requestURL = $"https://localhost:7079/api/";
            var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync(requestURL + name);

            string apiData = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode == false)
            {
                return false;
            }
            return true;

        }
    }
}
