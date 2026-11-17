using OpenDCP.Framework.Connections.DCPCore.Models;
using System.Threading.Tasks;

namespace OpenDCP.Framework.Connections.Core
{
    public interface IDCPCoreConnection
    {
        public Task<DCPCoreGenericResponse<TResponse>> GetAsync<TResponse>(string url);
        public Task<DCPCoreGenericResponse<bool>> GetAsync(string url);
        public Task<DCPCoreGenericResponse<TResponse>> PostAsync<TResponse, TRequest>(string url, TRequest model);
        public Task<DCPCoreGenericResponse<bool>> PostAsync<TRequest>(string url, TRequest model);
        public Task<DCPCoreGenericResponse<TResponse>> PutAsync<TResponse, TRequest>(string url, TRequest model);
        public Task<DCPCoreGenericResponse<bool>> PutAsync<TRequest>(string url, TRequest model);
        public Task<DCPCoreGenericResponse<bool>> DeleteAsync(string url);
        public Task<string> GetToken();
        public void SetToken(string token);
        public Task<string> GetBaseUrl();
    }
}