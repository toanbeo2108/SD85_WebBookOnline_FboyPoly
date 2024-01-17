using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GhnApiController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GhnApiController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("get-provinces")]
        public async Task<IActionResult> GetProvinces()
        {
            var ghnApiUrl = "https://online-gateway.ghn.vn/shiip/public-api/master-data/province";
            var token = "8fbfedf6-b458-11ee-b6f7-7a81157ff3b1";

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                httpClient.DefaultRequestHeaders.Add("Token", token);

                var response = await httpClient.GetAsync(ghnApiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Xử lý dữ liệu và trả về
                    return Ok(content);
                }
                else
                {
                    return BadRequest("Error calling GHN API");
                }
            }
        }

        [HttpGet("get-districts")]
        public async Task<IActionResult> GetDistricts(int provinceId)
        {
            try
            {
                var ghnApiUrl = $"https://online-gateway.ghn.vn/shiip/public-api/master-data/district?province_id={provinceId}";
                var token = "8fbfedf6-b458-11ee-b6f7-7a81157ff3b1";

                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Token", token);

                    var response = await httpClient.GetAsync(ghnApiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return Ok(content);
                    }
                    else
                    {
                        return BadRequest("Error calling GHN API");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("get-wards")]
        public async Task<IActionResult> GetWards(int districtId)
        {
            try
            {
                var ghnApiUrl = $"https://online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id={districtId}";
                var token = "8fbfedf6-b458-11ee-b6f7-7a81157ff3b1";

                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Token", token);

                    var response = await httpClient.GetAsync(ghnApiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return Ok(content);
                    }
                    else
                    {
                        return BadRequest("Error calling GHN API");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("get-services")]
        public async Task<IActionResult> GetServices(int from_district_id, int to_district_id)
        {
            try
            {
                var ghnApiUrl = $"https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/available-services?shop_id=4185061&from_district={from_district_id}&to_district={to_district_id}";
                var token = "8fbfedf6-b458-11ee-b6f7-7a81157ff3b1";

                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Token", token);

                    var response = await httpClient.GetAsync(ghnApiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return Ok(content);
                    }
                    else
                    {
                        return BadRequest("Error calling GHN API");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpGet("get-feeShip")]
        public async Task<IActionResult> GetfeeShip(int to_district_id, int to_ward_code, int weight)
        {
            try
            {
                var ghnApiUrl = $"https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee?service_type_id=2&insurance_value=0&from_district_id=1825&to_district_id={to_district_id}&to_ward_code={to_ward_code}&height=20&length=30&weight={weight}&width=20&form_ward_code=220316";
                var token = "8fbfedf6-b458-11ee-b6f7-7a81157ff3b1";

                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Token", token);

                    var response = await httpClient.GetAsync(ghnApiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return Ok(content);
                    }
                    else
                    {
                        return BadRequest("Error calling GHN API");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
