using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenDCP.Framework.Configuration.Model.Constants;
using OpenDCP.Framework.Connections.DCPCore.Converters;
using OpenDCP.Framework.Connections.DCPCore.Models;
using OpenDCP.Framework.Exceptions;
using OpenDCP.Framework.HttpClient;
using OpenDCP.Framework.ServiceDiscovery.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenDCP.Framework.Connections.Core
{
    public class DCPCoreConnection : IDCPCoreConnection
    {
        private readonly IConfiguration _configuration;
        private readonly DCPHttpClient _httpClient;
        private readonly IServiceDiscovery _serviceDiscovery;
        private readonly ILogger _logger;

        private JwtSecurityToken? _token = null;
        private JwtSecurityToken? _refreshToken = null;

        public DCPCoreConnection(IConfiguration configuration, IServiceDiscovery serviceDiscovery, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _serviceDiscovery = serviceDiscovery;
            _httpClient = new DCPHttpClient(configuration);
            _logger = loggerFactory.CreateLogger<DCPCoreConnection>();
        }
        public async Task<DCPCoreGenericResponse<TResponse>> GetAsync<TResponse>(string url)
        {
            var result = new DCPCoreGenericResponse<TResponse>();

            var token = await GetToken();

            var baseUrl = await GetGatewayBaseUrl();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = {
                    new JObjectConverter(),
                    new JArrayConverter(),
                    new JsonStringEnumConverter(),
                    new JTokenConverter()
                }
            };

            var response = await _httpClient.GetAsync(
                           uri: BuildUrl(baseUrl, url),
                            (h) =>
                            {
                                h.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            }
                        );

            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                result.Errors.Add(new Configuration.Model.Errors.DatasourceError
                {
                    Error = responseBody,
                    ErrorCode = (byte)GeneralErrorCode.NoSuccessStatusCode
                });

                return result;
            }

            if (typeof(TResponse) == typeof(string))
            {
                result.Response = (TResponse)(object)responseBody;
                return result;
            }
            else
            {
                result.Response = JsonSerializer.Deserialize<TResponse>(
                 responseBody,
                 options)!;
            }

            if (result.Response == null)
            {
                result.Errors.Add(new Configuration.Model.Errors.DatasourceError
                {
                    Error = @$"Error while parsing. {responseBody}",
                    ErrorCode = (byte)GeneralErrorCode.ParsingError
                });
            }

            return result;
        }
        public async Task<DCPCoreGenericResponse<bool>> GetAsync(string url)
        {
            var result = new DCPCoreGenericResponse<bool>();

            var token = await GetToken();

            var baseUrl = await GetGatewayBaseUrl();

            var response = await _httpClient.GetAsync(
                           uri: BuildUrl(baseUrl, url),
                            (h) =>
                            {
                                h.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            }
                        );

            if (!response.IsSuccessStatusCode)
            {
                result.Errors.Add(new Configuration.Model.Errors.DatasourceError
                {
                    Error = await response.Content.ReadAsStringAsync(),
                    ErrorCode = (byte)GeneralErrorCode.NoSuccessStatusCode
                });

                result.Response = false;

                return result;
            }

            result.Response = true;

            return result;
        }
        public async Task<DCPCoreGenericResponse<TResponse>> PostAsync<TResponse, TRequest>(string url, TRequest model)
        {
            var result = new DCPCoreGenericResponse<TResponse>();

            var token = await GetToken();

            var baseUrl = await GetGatewayBaseUrl();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = {
                    new JObjectConverter(),
                    new JArrayConverter(),
                    new JsonStringEnumConverter(),
                    new JTokenConverter()
                }
            };

            var response = await _httpClient.PostAsync(
                           uri: BuildUrl(baseUrl, url),
                           data: JsonSerializer.Serialize(model, options),
                           (h) =>
                           {
                               h.Authorization = new AuthenticationHeaderValue("Bearer", token);
                           }
                        );

            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                result.Errors.Add(new Configuration.Model.Errors.DatasourceError
                {
                    Error = responseBody,
                    ErrorCode = (byte)GeneralErrorCode.NoSuccessStatusCode
                });

                return result;
            }

            if (typeof(TResponse) == typeof(string))
            {
                result.Response = (TResponse)(object)responseBody;
                return result;
            }
            else
            {
                result.Response = JsonSerializer.Deserialize<TResponse>(
                 responseBody,
                 options)!;
            }

            if (result.Response == null)
            {
                result.Errors.Add(new Configuration.Model.Errors.DatasourceError
                {
                    Error = @$"Error while parsing. {responseBody}",
                    ErrorCode = (byte)GeneralErrorCode.ParsingError
                });
            }

            return result;
        }
        public async Task<DCPCoreGenericResponse<bool>> PostAsync<TRequest>(string url, TRequest model)
        {
            var result = new DCPCoreGenericResponse<bool>();

            var token = await GetToken();

            var baseUrl = await GetGatewayBaseUrl();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = {
                    new JObjectConverter(),
                    new JArrayConverter(),
                    new JsonStringEnumConverter(),
                    new JTokenConverter()
                }
            };

            var response = await _httpClient.PostAsync(
                           uri: BuildUrl(baseUrl, url),
                           data: JsonSerializer.Serialize(model, options),
                           (h) =>
                           {
                               h.Authorization = new AuthenticationHeaderValue("Bearer", token);
                           }
                        );

            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                result.Errors.Add(new Configuration.Model.Errors.DatasourceError
                {
                    Error = responseBody,
                    ErrorCode = (byte)GeneralErrorCode.NoSuccessStatusCode
                });

                result.Response = false;

                return result;
            }

            result.Response = true;

            return result;
        }

        public async Task<DCPCoreGenericResponse<TResponse>> PutAsync<TResponse, TRequest>(string url, TRequest model)
        {
            var result = new DCPCoreGenericResponse<TResponse>();

            var token = await GetToken();

            var baseUrl = await GetGatewayBaseUrl();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = {
                    new JObjectConverter(),
                    new JArrayConverter(),
                    new JsonStringEnumConverter(),
                    new JTokenConverter()
                }
            };

            var response = await _httpClient.PutAsync(
                           uri: BuildUrl(baseUrl, url),
                           data: JsonSerializer.Serialize(model, options),
                           (h) =>
                           {
                               h.Authorization = new AuthenticationHeaderValue("Bearer", token);
                           }
                        );

            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                result.Errors.Add(new Configuration.Model.Errors.DatasourceError
                {
                    Error = responseBody,
                    ErrorCode = (byte)GeneralErrorCode.NoSuccessStatusCode
                });

                return result;
            }

            if (typeof(TResponse) == typeof(string))
            {
                result.Response = (TResponse)(object)responseBody;
                return result;
            }
            else
            {
                result.Response = JsonSerializer.Deserialize<TResponse>(
                 responseBody,
                 options)!;
            }

            if (result.Response == null)
            {
                result.Errors.Add(new Configuration.Model.Errors.DatasourceError
                {
                    Error = @$"Error while parsing. {responseBody}",
                    ErrorCode = (byte)GeneralErrorCode.ParsingError
                });
            }

            return result;
        }

        public async Task<DCPCoreGenericResponse<bool>> PutAsync<TRequest>(string url, TRequest model)
        {
            var result = new DCPCoreGenericResponse<bool>();

            var token = await GetToken();

            var baseUrl = await GetGatewayBaseUrl();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = {
                    new JObjectConverter(),
                    new JArrayConverter(),
                    new JsonStringEnumConverter(),
                    new JTokenConverter()
                }
            };

            var response = await _httpClient.PutAsync(
                           uri: BuildUrl(baseUrl, url),
                           data: JsonSerializer.Serialize(model, options),
                           (h) =>
                           {
                               h.Authorization = new AuthenticationHeaderValue("Bearer", token);
                           }
                        );

            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                result.Errors.Add(new Configuration.Model.Errors.DatasourceError
                {
                    Error = responseBody,
                    ErrorCode = (byte)GeneralErrorCode.NoSuccessStatusCode
                });

                result.Response = false;

                return result;
            }

            result.Response = true;

            return result;
        }

        public async Task<DCPCoreGenericResponse<bool>> DeleteAsync(string url)
        {
            var result = new DCPCoreGenericResponse<bool>();

            var token = await GetToken();

            var baseUrl = await GetGatewayBaseUrl();

            var response = await _httpClient.DeleteAsync(
                           uri: BuildUrl(baseUrl, url),
                           (h) =>
                           {
                               h.Authorization = new AuthenticationHeaderValue("Bearer", token);
                           }
                        );

            if (!response.IsSuccessStatusCode)
            {
                result.Errors.Add(new Configuration.Model.Errors.DatasourceError
                {
                    Error = await response.Content.ReadAsStringAsync(),
                    ErrorCode = (byte)GeneralErrorCode.NoSuccessStatusCode
                });

                result.Response = false;

                return result;
            }

            result.Response = true;

            return result;
        }
        public async Task<string> GetToken()
        {
            if (_token == null)
            {
                await RequestToken();
            }
            else
            {

                if (DateTime.SpecifyKind(_token.ValidTo, DateTimeKind.Utc) < DateTime.UtcNow)
                {
                    await RequestToken();
                }
            }

            if (_token == null)
            {
                throw new DCPInternalServerErrorException("Token could not be fetched");
            }

            return _token.RawData;
        }

        public void SetToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            _token = handler.ReadJwtToken(token);
        }

        public async Task<string> GetBaseUrl()
        {
            var env = _configuration["Host:EnvironmentSlug"];
            var tags = new List<string> { "backend", "api", "core" };

            var services = await _serviceDiscovery
                .QueryActiveServices(tags, env);

            var url = services
                .Select(x => $"https://{x.Host}:{x.Port}")
                .FirstOrDefault();

            if (url == null)
            {
                _logger.LogError("Core Service URL could not be read from the service registry");
                throw new DCPInternalServerErrorException("Missing Core Service URL");
            }


            _logger.LogTrace("Using API serverd at: {Url}", url);

            return url;
        }
        private async Task RequestToken()
        {
            if (string.Equals(_configuration["Host:CoreAPIVersion"], "1"))
            {
                await GetLegacyTokenAsync();
                return;
            }

            var url = await GetBaseUrl();

            if (_refreshToken != null)
            {
                if (DateTime.SpecifyKind(_refreshToken.ValidTo, DateTimeKind.Utc) < DateTime.UtcNow)
                {
                    url = $"{url}/api/v2/token" +
                            $"?grant_type=refresh_token" +
                            $"&refresh_token={_refreshToken.RawData}";
                }
                else
                {
                    url = $"{url}/api/v2/token" +
                        $"?grant_type=client_credentials" +
                        $"&clientId={_configuration["IdP:Settings:Client:Id"]}" +
                        $"&clientSecret={_configuration["IdP:Settings:Client:Secret"]}";
                }
            }
            else
            {
                url = $"{url}/api/v2/token" +
                        $"?grant_type=client_credentials" +
                        $"&clientId={_configuration["IdP:Settings:Client:Id"]}" +
                        $"&clientSecret={_configuration["IdP:Settings:Client:Secret"]}";
            }

            var response = await _httpClient.PostAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Token request failed");
            }

            var tokenString = await response.Content.ReadAsStringAsync();
            var token = JsonDocument.Parse(tokenString).RootElement;

            if (!token.TryGetProperty("access_token", out JsonElement accessToken)
                || !token.TryGetProperty("refresh_token", out JsonElement refreshToken))
            {
                throw new DCPInternalServerErrorException("Tokens could not be fetched");
            }

            var handler = new JwtSecurityTokenHandler();

            _token = handler.ReadJwtToken(accessToken.GetString());
            _refreshToken = handler.ReadJwtToken(refreshToken.GetString());
        }
        private async Task<string> GetGatewayBaseUrl()
        {
            var env = _configuration["Host:EnvironmentSlug"];
            var tags = new List<string> { "gateway", "internal" };

            var services = await _serviceDiscovery
                .QueryActiveServices(tags, env);

            var url = services
                .Select(x => $"https://{x.Host}:{x.Port}")
                .FirstOrDefault();

            if (url == null)
            {
                _logger.LogError("Core Service URL could not be read from the service registry");
                throw new DCPInternalServerErrorException("Missing Core Service URL");
            }


            _logger.LogTrace("Using API serverd at: {Url}", url);

            return url;
        }
        private async Task GetLegacyTokenAsync()
        {
            var url = await GetBaseUrl() + "/token";

            var requestParams = new Dictionary<string, string>();

            if (_refreshToken != null)
            {
                if (DateTime.SpecifyKind(_refreshToken.ValidTo, DateTimeKind.Utc) < DateTime.UtcNow)
                {
                    requestParams.Add("grant_type", "refresh_token");
                    requestParams.Add("refresh_token", _refreshToken.RawData);
                }
                else
                {
                    requestParams.Add("grant_type", "client_credentials");
                    requestParams.Add("clientId", _configuration["IdP:Settings:Client:Id"] ?? throw new ArgumentNullException("Client Id configuration value cannot be null."));
                    requestParams.Add("clientSecret", _configuration["IdP:Settings:Client:Secret"] ?? throw new ArgumentNullException("Client Secret configuration value cannot be null."));
                }
            }
            else
            {
                requestParams.Add("grant_type", "client_credentials");
                requestParams.Add("clientId", _configuration["IdP:Settings:Client:Id"] ?? throw new ArgumentNullException("Client Id configuration value cannot be null."));
                requestParams.Add("clientSecret", _configuration["IdP:Settings:Client:Secret"] ?? throw new ArgumentNullException("Client Secret configuration value cannot be null."));
            }

            var requestContent = new FormUrlEncodedContent(requestParams);

            var response = await _httpClient.PostAsync(url, requestContent);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Token request failed");
            }

            var tokenString = await response.Content.ReadAsStringAsync();
            var token = JsonDocument.Parse(tokenString).RootElement;

            if (!token.TryGetProperty("access_token", out JsonElement accessToken))
            {
                throw new DCPInternalServerErrorException("Tokens could not be fetched");
            }

            var handler = new JwtSecurityTokenHandler();

            _token = handler.ReadJwtToken(accessToken.GetString());
        }

        private string BuildUrl(string baseUrl, string url)
        {
            var pattern = _configuration["Debug:RouteReWrite:Pattern"];

            if (!string.IsNullOrEmpty(pattern))
            {
                string regexPattern = $"^{pattern}/?";
                url = Regex.Replace(url, regexPattern, "", RegexOptions.IgnoreCase);
                return $"{_configuration["Debug:RouteReWrite:Replacement"]!.TrimEnd('/')}/{url.TrimStart('/')}";
            }
            else
            {
                return $"{baseUrl}/{url}";
            }
        }
    }
}
