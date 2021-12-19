using CryptoSuggestion.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CryptoSuggestion.Controllers
{
    public class CryptosController : Controller
    {
        private readonly IHttpClientFactory _clientfactory;

        public CoinMarketData CoinMarketData { get; set; }

        const string BASE_URL = "https://pro-api.coinmarketcap.com";
        const string API_KEY = "a1d7197f-5ba9-4e9a-94f6-692628e4278e";

        public CryptosController(IHttpClientFactory clientFactory)
        {
            _clientfactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri($"{BASE_URL}/v1/cryptocurrency/listings/latest");
            message.Headers.Add("Accepts","application/json");
            message.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);

            var client = _clientfactory.CreateClient();

            var response = await client.SendAsync(message);

            if (response.IsSuccessStatusCode)
            {

                using var responseStream = await response.Content.ReadAsStreamAsync();



                CoinMarketData = await JsonSerializer.DeserializeAsync<CoinMarketData>(responseStream);
            }
            else
            {
                CoinMarketData = null;
            }

            return View(CoinMarketData);
        }
    }
}
