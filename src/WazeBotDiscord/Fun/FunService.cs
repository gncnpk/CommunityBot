using System.IO;
using System.Threading.Tasks;
using System.Net;

namespace WazeBotDiscord.Fun
{
    public class FunService
    {
        public async Task<string> GetWebRequest(string url, string contentType)
        {
            var request = System.Net.WebRequest.Create(url);
            string result;
            request.ContentType = contentType; // "application/json; charset=utf-8";
            var response = await request.GetResponseAsync() as HttpWebResponse;

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }
    }
}
