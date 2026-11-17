using OpenDCP.Framework.Configuration.Model.Responses.Datasource;

namespace OpenDCP.Framework.Connections.DCPCore.Models
{
    public class DCPCoreGenericResponse<T> : DatasourceResponse
    {
        public T Response { get; set; } = default!;
    }
}
