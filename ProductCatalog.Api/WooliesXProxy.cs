using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Http;

namespace ProductCatalog.Api
{
    public class WooliesXProxy
    {
        public static async Task TrolleyCalculator(HttpContext context)
        {
            var proxyUrl = "http://dev-wooliesx-recruitment.azurewebsites.net/api/resource/trolleyCalculator";
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
            var postRequestContent = await reader.ReadToEndAsync();
            //Instead of passing token in query param we should use JWT for API Authentication. We can think of using HTTP-Only header as well Same-Client = true;
            var postJsonAsync = proxyUrl
                .SetQueryParam("token", "25a4f06f-8fd5-49b3-a711-c013c156f8c8")
                .WithHeader("Accept", "application/json")
                .WithHeader("Content-Type", "application/json-patch+json")
                .PostAsync(new StringContent(postRequestContent));

            //we should not call the property postJsonAsync.Result as it initiate the synchronous call. We can think of avoiding that and using await as this method behaviour is asynchronous
            #region Commentedcode
            //var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json-patch+json");
            //var response = await httpClient.PostAsync(proxyUrl, new StringContent(postRequestContent));
            //var responseContent = await response.Content.ReadAsStringAsync();
            //await context.Response.WriteAsync(responseContent);
            #endregion 

            var readAsStringAsync = await postJsonAsync.Result.Content.ReadAsStringAsync();

            await context.Response.WriteAsync(readAsStringAsync);
        }
    }
}