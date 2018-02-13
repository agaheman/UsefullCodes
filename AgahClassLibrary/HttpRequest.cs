using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgahClassLibrary
{
    class HttpRequest
    {

        static async Task<string> PostAsync(string baseUri, string requestUri)
        {
            using (var client = new System.Net.Http.HttpClient() { BaseAddress = new Uri(baseUri) })
            {

                var content = new System.Net.Http.FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("", "login")
                });

                var result = await client.PostAsync(requestUri, content);

                if (result.IsSuccessStatusCode)
                {
                    return await result.Content.ReadAsStringAsync();
                }
                else
                {
                    return result.ReasonPhrase;
                }

            }
        }

    }
}
