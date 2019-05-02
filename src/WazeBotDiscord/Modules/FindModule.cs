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
    public class FindModule : ModuleBase
    {
        readonly FindService _findSvc;

        public FindModule(FindService findSvc)
        {
            _findSvc = findSvc;
        }

        [Command("find")]
        public async Task FindObject([Remainder] string message = null)
        {
            string url = "https://www.waze.com/Descartes/app/HouseNumbers?ids=" + message.Trim();
            var result = _findSvc.GetWebRequest(url, "application/json; charset=utf-8").Result;
            HouseNumberResult HNResult = JsonConvert.DeserializeObject<HouseNumberResult>(result);
            GeoPoint centroid = _findSvc.GetCentroid(HNResult.editAreas.objects[0].geometry.coordinates[0]);

            await (ReplyAsync($"<https://www.waze.com/en-US/editor/?env=usa&lon={centroid.x}&lat={centroid.y}&zoom=6&segments={message}>"));
        }

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
            public List<string> objects { get; set; }
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
    }

}
