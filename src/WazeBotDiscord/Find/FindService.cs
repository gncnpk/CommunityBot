using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;

namespace WazeBotDiscord.Find
{
    public class FindService
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

        public GeoPoint GetCentroid(List<List<double>> coordinates)
        {
            var len = coordinates.Count;
            if (len > 0 && len <= 2)
            {
                return new GeoPoint(coordinates[0][0], coordinates[0][1]);
            }
            else if (len > 2)
            {
                var sumX = 0.0;
                var sumY = 0.0;
                var x0 = coordinates[0][0];
                var y0 = coordinates[0][1];
                var area = -1 * GetArea(coordinates);
                var x = 0.0;
                var y = 0.0;
                if (area != 0)
                {
                    for (var i = 0; i < len - 1; i++)
                    {
                        var b = coordinates[i];
                        var c = coordinates[i + 1];
                        sumX += (b[0] + c[0] - 2 * x0) * ((b[0] - x0) * (c[1] - y0) - (c[0] - x0) * (b[1] - y0));
                        sumY += (b[1] + c[1] - 2 * y0) * ((b[0] - x0) * (c[1] - y0) - (c[0] - x0) * (b[1] - y0));
                    }
                    x = x0 + sumX / (6 * area);
                    y = y0 + sumY / (6 * area);
                }
                else
                {
                    for (var i = 0; i < len - 1; i++)
                    {
                        sumX += coordinates[i][0];
                        sumY += coordinates[i][1];
                    }
                    x = sumX / (len - 1);
                    y = sumY / (len - 1);
                }
                return new GeoPoint(x,y);
            }
            else
            {
                return null;
            }
        }

        public double GetArea(List<List<double>> coordinates)
        {
            double area = 0.0;
            if (coordinates.Count > 2)
            {
                var sum = 0.0;
                int len = coordinates.Count;
                for (var i = 0; i < len - 1; i++)
                {
                    var b = coordinates[i];
                    var c = coordinates[i + 1];
                    sum += (b[0] + c[0]) * (c[1] - b[1]);
                }
                area = -sum / 2.0;
            }
            return area;
        }
    }

    public class GeoPoint
    {
        public GeoPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public double x { get; set; }
        public double y { get; set; }
    }
}
