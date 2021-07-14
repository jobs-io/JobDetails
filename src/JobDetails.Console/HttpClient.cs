using System.Threading.Tasks;
using JobDetails.Core;

namespace JobDetails.Console
{
    public class HttpClient : IHttpClient
    {
        public Task<System.Net.Http.HttpResponseMessage> GetAsync(string requestUri)
        {
            return new System.Net.Http.HttpClient().GetAsync(requestUri);
        }
    }
}
