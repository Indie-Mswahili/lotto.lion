using Newtonsoft.Json;
using OdinSdk.BaseLib.Serialize;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace LottoLion.BaseLib.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiClient : IDisposable
    {
        private string __api_url = "";

        protected string __connect_key;
        protected string __secret_key;

        protected const string __content_type = "application/json";
        protected const string __user_agent = "btc-trading/5.2.2017.01";

        /// <summary>
        /// 
        /// </summary>
        public ApiClient(string api_url, string connect_key, string secret_key)
        {
            __api_url = api_url;
            __connect_key = connect_key;
            __secret_key = secret_key;
        }

        private static char[] __to_digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        public byte[] EncodeHex(byte[] data)
        {
            int l = data.Length;
            byte[] _result = new byte[l << 1];

            // two characters form the hex value.
            for (int i = 0, j = 0; i < l; i++)
            {
                _result[j++] = (byte)__to_digits[(0xF0 & data[i]) >> 4];
                _result[j++] = (byte)__to_digits[0x0F & data[i]];
            }

            return _result;
        }

        public string EncodeURIComponent(Dictionary<string, object> rgData)
        {
            string _result = String.Join("&", rgData.Select((x) => String.Format("{0}={1}", x.Key, x.Value)));

            _result = System.Net.WebUtility.UrlEncode(_result)
                        .Replace("+", "%20").Replace("%21", "!")
                        .Replace("%27", "'").Replace("%28", "(")
                        .Replace("%29", ")").Replace("%26", "&")
                        .Replace("%3D", "=").Replace("%7E", "~");

            return _result;
        }

        public IRestClient CreateJsonClient(string baseurl)
        {
            var _client = new RestClient(baseurl);
            {
                _client.RemoveHandler(__content_type);
                _client.AddHandler(__content_type, new RestSharpJsonNetDeserializer());
                _client.Timeout = 10 * 1000;
                _client.UserAgent = __user_agent;
            }

            return _client;
        }

        public IRestRequest CreateJsonRequest(string resource, Method method = Method.GET)
        {
            var _request = new RestRequest(resource, method)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new RestSharpJsonNetSerializer()
            };

            return _request;
        }

        private string __access_token = null;

        public async Task<string> AccessToken()
        {
            if (__access_token != null)
            {
                var _web_token = (new JwtSecurityTokenHandler()).ReadJwtToken(__access_token);

                var _expiration = _web_token
                                        .Claims
                                        .Where(c => c.Type == JwtRegisteredClaimNames.Exp)
                                        .FirstOrDefault();
                if (_expiration != null)
                    _result = _expiration.Value;


                if (__access_token.CheckExpired() == true)
                    __access_token = await GetRefreshToken(__access_token);
            }
            else if (String.IsNullOrEmpty(__connect_key) == false)
                __access_token = await GetAccessToken();

            return __access_token;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessToken()
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("client_id", __connect_key);
                _params.Add("client_secret", __secret_key);
                _params.Add("username", __user_name);
                _params.Add("password", __user_password);
                _params.Add("grant_type", "password");
            }

            return await this.CallApiPostAsync<string>("/v1/oauth2/access_token", _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetRefreshToken(string access_token)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("client_id", __connect_key);
                _params.Add("client_secret", __secret_key);
                _params.Add("refresh_token", access_token.refresh_token);
                _params.Add("grant_type", "refresh_token");
            }

            return await this.CallApiPostAsync<string>("/v1/oauth2/access_token", _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<T> CallApiPostAsync<T>(string endpoint, Dictionary<string, object> args = null) where T : new()
        {
            var _request = CreateJsonRequest(endpoint, Method.POST);
            {
                var _params = new Dictionary<string, object>();
                {
                    _params.Add("endpoint", endpoint);
                    if (args != null)
                    {
                        foreach (var a in args)
                            _params.Add(a.Key, a.Value);
                    }
                }

                foreach (var a in _params)
                    _request.AddParameter(a.Key, a.Value);
            }

            var _access_token = await AccessToken();
            if (_access_token != null)
                _request.AddHeader("Authorization", $"Bearer {_access_token}");

            var _client = CreateJsonClient(__api_url);
            {
                var tcs = new TaskCompletionSource<T>();
                _client.ExecuteAsync(_request, response =>
                {
                    tcs.SetResult(JsonConvert.DeserializeObject<T>(response.Content));
                });

                return await tcs.Task;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<T> CallApiGetAsync<T>(string endpoint, Dictionary<string, object> args = null) where T : new()
        {
            var _request = CreateJsonRequest(endpoint, Method.GET);

            if (args != null)
            {
                foreach (var a in args)
                    _request.AddParameter(a.Key, a.Value);
            }

            var _client = CreateJsonClient(__api_url);
            {
                var tcs = new TaskCompletionSource<T>();
                _client.ExecuteAsync(_request, response =>
                {
                    tcs.SetResult(JsonConvert.DeserializeObject<T>(response.Content));
                });

                return await tcs.Task;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
        }
    }
}