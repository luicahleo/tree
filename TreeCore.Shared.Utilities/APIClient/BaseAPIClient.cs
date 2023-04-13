using System;
using System.Collections.Generic;
using System.Net.Http;
using TreeCore.Shared.ROP;
using System.Threading.Tasks;
using TreeCore.Shared.DTO.Query;
using Newtonsoft.Json;
using IdentityModel.Client;
using TreeCore.Shared.DTO.Auth;

namespace TreeCore.APIClient
{

    public class BaseAPIClient<T>
    {
        internal class Token
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }

            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }
        }

        private readonly string _rutaObjecto;
        private readonly string _rutaAPI;
        private readonly string _usuario;
        private readonly string _pass;
        private HttpClient client;
        private Token oToken;
        private string _bearerToken;

        public BaseAPIClient()
            :this(null)
        {
        }

        public BaseAPIClient(string token)
        {
            _bearerToken = token;
            _rutaObjecto = APIObjects.GetRuta(typeof(T));
            _rutaAPI = APIObjects.rutaAPI;
            _usuario = "client";
            _pass = "secret";
            WinHttpHandler handler = new WinHttpHandler();
            client = new HttpClient(handler);
        }

        public async Task<ResultDto<TokenDTO>> Login(string email, string password)
        {
            var loginDto = new LoginDTO()
            {
                email = email,
                password = password
            };

            var request = new HttpRequestMessage
            {
                Method = System.Net.Http.HttpMethod.Post,
                RequestUri = new Uri($"{_rutaAPI + APIObjects.LOGIN}"),
                Content = new StringContent(JsonConvert.SerializeObject(loginDto), System.Text.Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<ResultDto<TokenDTO>>();

            //WinHttpHandler handler = new WinHttpHandler();
            //client = new HttpClient(handler);
            //var disco = await client.GetDiscoveryDocumentAsync(_rutaOAuth);
            //// request token
            //var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            //{
            //    Address = disco.TokenEndpoint,
            //    ClientId = _usuario,
            //    ClientSecret = _pass,
            //    Scope = "api"
            //});
            //client = new HttpClient(handler);
            //client.SetBearerToken(tokenResponse.AccessToken);
            //return client;
        }

        public async Task<ResultDto<List<T>>> GetList(QueryDTO oQueryDTO = null)
        {
            client.SetBearerToken(_bearerToken);
            
            if (oQueryDTO == null)
            {
                oQueryDTO = new QueryDTO();
            }
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_rutaAPI + _rutaObjecto}"),
                Content = new StringContent(JsonConvert.SerializeObject(oQueryDTO), System.Text.Encoding.UTF8, "application/json"),
            };
            var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<ResultDto<List<T>>>();
        }

        public async Task<ResultDto<T>> GetByCode(string code)
        {
            client.SetBearerToken(_bearerToken);

            var request = new HttpRequestMessage
            {
                Method = System.Net.Http.HttpMethod.Get,
                RequestUri = new Uri($"{_rutaAPI + _rutaObjecto}/{code}")
            };
            var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<ResultDto<T>>();
        }

        public async Task<ResultDto<T>> AddEntity(T oEntidad)
        {
            client.SetBearerToken(_bearerToken);

            var request = new HttpRequestMessage
            {
                Method = System.Net.Http.HttpMethod.Post,
                RequestUri = new Uri($"{_rutaAPI + _rutaObjecto}"),
                Content = new StringContent(JsonConvert.SerializeObject(oEntidad), System.Text.Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<ResultDto<T>>();
        }

        public async Task<ResultDto<T>> DeleteEntity(string code)
        {
            client.SetBearerToken(_bearerToken);

            var request = new HttpRequestMessage
            {
                Method = System.Net.Http.HttpMethod.Delete,
                RequestUri = new Uri($"{_rutaAPI + _rutaObjecto}/{code}")
            };
            var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<ResultDto<T>>();
        }

        public async Task<ResultDto<T>> UpdateEntity(string code, T oEntidad)
        {
            client.SetBearerToken(_bearerToken);

            var request = new HttpRequestMessage
            {
                Method = System.Net.Http.HttpMethod.Put,
                RequestUri = new Uri($"{_rutaAPI + _rutaObjecto}/{code}"),
                Content = new StringContent(JsonConvert.SerializeObject(oEntidad), System.Text.Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<ResultDto<T>>();
        }

    }
}