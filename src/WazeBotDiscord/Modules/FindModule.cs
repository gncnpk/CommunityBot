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
        public async Task FindSegment([Remainder] string message = null)
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

        [Command("place")]
        public async Task FindPlace([Remainder] string placeID = null)
        {
            if (placeID == null)
                await ReplyAsync("Please provide a Place ID");
            Regex regURL = new Regex(@"^\d*\.\d*.\d*");
            if (regURL.Matches(placeID).Count == 1)
            {
                string urlNA = "https://www.waze.com/SearchServer/mozi?max_distance_kms=&lon=-84.22637&lat=39.61097&format=PROTO_JSON_FULL&venue_id=" + placeID.Trim();
                var result = _findSvc.GetWebRequest(urlNA, "application/json; charset=utf-8").Result;
                PlaceResponse PlaceResult = JsonConvert.DeserializeObject<PlaceResponse>(result);
                StringBuilder reply = new StringBuilder();

                if (PlaceResult.venue != null)
                {
                    GeoPoint centroid = new GeoPoint(PlaceResult.venue.location.x, PlaceResult.venue.location.y);
                    reply.AppendLine($"<https://www.waze.com/en-US/editor/?env=usa&lon={centroid.x}&lat={centroid.y}&zoom=6&venues={placeID}>");
                }

                string urlROW = "https://www.waze.com/row-SearchServer/mozi?max_distance_kms=&lon=-84.22637&lat=39.61097&format=PROTO_JSON_FULL&venue_id=" + placeID.Trim();
                result = _findSvc.GetWebRequest(urlROW, "application/json; charset=utf-8").Result;
                PlaceResult = JsonConvert.DeserializeObject<PlaceResponse>(result);
                if (PlaceResult.venue != null)
                {
                    GeoPoint centroid = new GeoPoint(PlaceResult.venue.location.x, PlaceResult.venue.location.y);
                    reply.AppendLine($"<https://www.waze.com/editor/?env=row&lon={centroid.x}&lat={centroid.y}&zoom=6&venues={placeID}>");
                }

                string urlIL = "https://www.waze.com/il-SearchServer/mozi?max_distance_kms=&lon=-84.22637&lat=39.61097&format=PROTO_JSON_FULL&venue_id=" + placeID.Trim();
                result = _findSvc.GetWebRequest(urlIL, "application/json; charset=utf-8").Result;
                PlaceResult = JsonConvert.DeserializeObject<PlaceResponse>(result);
                if (PlaceResult.venue != null)
                {
                    GeoPoint centroid = new GeoPoint(PlaceResult.venue.location.x, PlaceResult.venue.location.y);
                    reply.AppendLine($"<https://www.waze.com/editor/?env=il&lon={centroid.x}&lat={centroid.y}&zoom=6&venues={placeID}>");
                }

                if (reply.Length > 0)
                    await ReplyAsync(reply.ToString());
                else
                    await ReplyAsync("Place not found");
            }
            else
                await ReplyAsync("Incorrect Place ID format.");
        }

        #region Segment Helper Classes

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

        #region Place Helper Classes
        public class PlaceResponse
        {
            public string id { get; set; }
            public Venue venue { get; set; }
            public string info_url { get; set; }
            public bool info_url_append_client_data { get; set; }
            public string provider { get; set; }
            public bool updateable { get; set; }
        }

        public class Venue
        {
            public InternalVenueID internal_venue_id { get; set; }
            public List<string> categories { get; set; }
            public string name { get; set; }
            public Point location { get; set; }
            public List<Point> polygon { get; set; }
            public string house_number { get; set; }
            public string street { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string country { get; set; }
            public int cityId { get; set; }
            public int street_id { get; set; }
            public List<ExternalProvider> external_providers { get; set; }
            public double area { get; set; }
            public List<Image> images { get; set; }
            public List<EntryExitPoint> entry_exit_points { get; set; }
            public ulong creation_date { get; set; }
            public ulong created_by { get; set; }
            public ulong last_update_date { get; set; }
            public List<ulong> last_update_by { get; set; }
            public int lock_level { get; set; }
            public bool approved { get; set; }
            public bool residential { get; set; }
            public string currency { get; set; }
            public string venue_id { get; set; }
            public int country_id { get; set; }
            public EditorInfo created_by_info { get; set; }
            public EditorInfo last_updated_by_info { get; set; }
            public bool has_more_data { get; set; }
        }
        
        public class EntryExitPoint
        {
            public Point point { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public bool primary { get; set; }
        }

        public class Point
        {
            public double x { get; set; }
            public double y { get; set; }
        }

        public class Image
        {
            public EditorInfo created_by_info { get; set; }
            public bool by_me { get; set; }
            public string id { get; set; }
            public ulong date { get; set; }
            public Point location { get; set; }
            public int creatorUserId { get; set; }
            public bool street { get; set; }
            public bool approved { get; set; }
            public int likes { get; set; }
            public bool liked { get; set; }
        }

        public class ExternalProvider
        {
            public string provider { get; set; }
            public string id { get; set; }
        }

        public class InternalVenueID
        {
            public int t10 { get; set; }
            public int t1 { get; set; }
            public string id { get; set; }
        }
        
        public class EditorInfo
        {
            public ulong id { get; set; }
            public string name { get; set; }
            public int rank { get; set; }
            public int points { get; set; }
            public int mood { get; set; }
            public bool is_staff { get; set; }
            public bool is_registered { get; set; }
            public bool is_ad_operator { get; set; }
            public bool is_system_trusted { get; set; }
        }
        #endregion
    }

}
