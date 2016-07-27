namespace AmazonHelper.WebApp.Infrastructure
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class WebApiClientSettings
    {
        public string BaseUrl { get; set; }

        public string BearerAccessToken { get; set; }
    }

    public class WebApiClient
    {
        private readonly HttpClient _httpClient;

        public WebApiClient(Func<WebApiClientSettings> settings)
        {
            var settings1 = settings();
            _httpClient = new HttpClient { BaseAddress = new Uri(settings1.BaseUrl) };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // attaching access token
            if (!string.IsNullOrEmpty(settings1.BearerAccessToken))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + settings1.BearerAccessToken);
            }
        }

        /// <summary>
        /// Sends GET query wih provided url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<T> RetriveDataAsync<T>(string url)
        {
            return await SendDataAsync<T>(url, HttpMethod.Get);
        }

        /// <summary>
        /// Sends POST query wih provided url and data content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<T> PostDataAsync<T>(string url, HttpContent content)
        {
            return await SendDataAsync<T>(url, HttpMethod.Post, content);
        }

        private async Task<T> SendDataAsync<T>(string url, HttpMethod method, HttpContent content = null)
        {
            var response = await (method == HttpMethod.Get ? _httpClient.GetAsync(url)
                : _httpClient.PostAsync(url, content));

            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorReponse = JsonConvert.DeserializeObject<FailedWebApiResponse>(responseContent);
                throw new Exception(errorReponse.Message);
            }

            var data = JsonConvert.DeserializeObject<T>(responseContent);

            return data;
        }

        private struct FailedWebApiResponse
        {
            public string Message;

            public string MessageDetail;
        }
    }
}


//{
//  "message": "No HTTP resource was found that matches the request URI 'http://localhost/RMS.WebApi/api/productsad/1'.",
//  "messageDetail": "No type was found that matches the controller named 'productsad'."
//}
