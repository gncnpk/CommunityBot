using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord.Commands;
using System.Text.RegularExpressions;
using WazeBotDiscord.Find;

namespace WazeBotDiscord.Modules
{
    [Group("find")]
    public class FindModule : ModuleBase
    {
        readonly FindService _findSvc;

        public FindModule(FindService findSvc)
        {
            _findSvc = findSvc;
        }

        [Command("segment")]
        public async Task FindObject([Remainder] string message = null)
        {
            Regex regURL = new Regex(@"^\d{1,10}");
            if (regURL.Matches(message).Count == 1)
            {
                string urlNA = "https://www.waze.com/Descartes/app/HouseNumbers?ids=" + message.Trim();
                var result = _findSvc.GetWebRequest(urlNA, "application/json; charset=utf-8").Result;
                HouseNumberResult HNResult = JsonConvert.DeserializeObject<HouseNumberResult>(result);
                StringBuilder reply = new StringBuilder();
                if (HNResult.editAreas.objects.Count > 0)
                {
                    GeoPoint centroid = _findSvc.GetCentroid(HNResult.editAreas.objects[0].geometry.coordinates[0]);
                    reply.AppendLine($"<https://www.waze.com/en-US/editor/?env=usa&lon={centroid.x}&lat={centroid.y}&zoom=6&segments={message}>");
                }

                string urlROW = "https://www.waze.com/row-Descartes/app/HouseNumbers?ids=" + message.Trim();
                result = _findSvc.GetWebRequest(urlROW, "application/json; charset=utf-8").Result;
                HNResult = JsonConvert.DeserializeObject<HouseNumberResult>(result);
                if (HNResult.editAreas.objects.Count > 0)
                {
                    GeoPoint centroid = _findSvc.GetCentroid(HNResult.editAreas.objects[0].geometry.coordinates[0]);
                    reply.AppendLine($"<https://www.waze.com/editor/?env=row&lon={centroid.x}&lat={centroid.y}&zoom=6&segments={message}>");
                }

                string urlIL = "https://www.waze.com/il-Descartes/app/HouseNumbers?ids=" + message.Trim();
                result = _findSvc.GetWebRequest(urlIL, "application/json; charset=utf-8").Result;
                HNResult = JsonConvert.DeserializeObject<HouseNumberResult>(result);
                if (HNResult.editAreas.objects.Count > 0)
                {
                    GeoPoint centroid = _findSvc.GetCentroid(HNResult.editAreas.objects[0].geometry.coordinates[0]);
                    reply.AppendLine($"<https://www.waze.com/editor/?env=il&lon={centroid.x}&lat={centroid.y}&zoom=6&segments={message}>");
                }

                if (reply.Length > 0)
                    await ReplyAsync(reply.ToString());
                else
                    await ReplyAsync("Segment not found");
            }
            else
                await ReplyAsync("Incorrect segment ID format.");
        }

        #region Helper Classes

        public class HouseNumberResult
        {
            public SegmentHouseNumber segmentHouseNumbers { get; set; }
            public EditAreas editAreas { get; set; }
            public Users users { get; set; }
        }

        public class SegmentHouseNumber
        {
            public List<HouseNumber> objects { get; set; }
        }

        public class EditAreas
        {
            public List<EditArea> objects { get; set; }
        }

        public class Users
        {
            public List<User> objects { get; set; }
        }

        public class EditArea
        {
            public int id { get; set; }
            public EditAreaGeometry geometry { get; set; }
        }

        public class HouseNumber
        {
            public string id { get; set; }
            public string number { get; set; }
            public int side { get; set; }
            public Geometry geometry { get; set; }
            public FractionPoint fractionPoint { get; set; }
            public bool valid { get; set; }
            public bool forced { get; set; }
            public string segID { get; set; }
        }

        public class Geometry
        {
            public string type { get; set; }
            public List<double> coordinates { get; set; }
        }

        public class EditAreaGeometry
        {
            public string type { get; set; }
            public List<List<List<double>>> coordinates { get; set; }
        }

        public class FractionPoint
        {
            public string type { get; set; }
            public List<double> coordinates { get; set; }
        }

        public class User
        {
            public int id { get; set; }
            public string userName { get; set; }
            public int rank { get; set; }
        }
        #endregion
    }

}
