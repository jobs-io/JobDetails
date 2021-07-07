using System.Net.Http;
using System.Threading.Tasks;

namespace JobDetails.Core
{
    public interface IHttpClient
    {
       Task<HttpResponseMessage> GetAsync(string requestUri);
    }
}