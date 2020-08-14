using System;
using AngleSharp.Parser.Html;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using AngleSharp.Dom.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WazeBotDiscord.Wikisearch
{
    public class WikisearchService
    {
        
        public async Task<List<SearchItem>> SearchWikiAsync(string searchPhrase)
        {
            List<SearchItem> _items = new List<SearchItem>();
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString("https://wazeopedia.waze.com/wiki/USA/api.php?action=opensearch&format=json&search=" + searchPhrase + "&namespace=0&limit=10");

                var fuzzyresults = JsonConvert.DeserializeObject(json);
                var fuzzyArray = ((JArray)fuzzyresults).ToArray();

                var searchTerm = fuzzyArray[0];
                var fuzzyMatches = fuzzyArray[1].ToArray();
                var fuzzyMatchLinks = fuzzyArray[3].ToArray();

                if (fuzzyMatches.Length == 0)
                {
                    var parser = new HtmlParser();
                    HttpClient _client = new HttpClient();
                    var resp = await _client.GetStringAsync("https://wazeopedia.waze.com/wiki/USA/index.php?title=Special%3ASearch&search=" + searchPhrase);
                    var doc = await parser.ParseAsync(resp);
                    var searchResults = doc.QuerySelectorAll(".mw-search-results li");
                    if (searchResults.Length > 0)
                    {
                        foreach (var myRow in searchResults)
                        {
                            var row = (IHtmlListItemElement)myRow;
                            _items.Add(new SearchItem { Title = row.Children[0].Children[0].Attributes["title"].Value, URL = "https://wazeopedia.waze.com" + row.Children[0].Children[0].Attributes["href"].Value });
                        }
                    }
                    else
                    {
                        if (doc.ContentType == "text/html") {
                            _items.Add(new SearchItem { Title = doc.Title.Replace(" - Wazeopedia", ""), URL = "https://wazeopedia.waze.com" + ((IHtmlAnchorElement)doc.QuerySelectorAll("#ca-main a")[0]).Href.Replace(@"about://", "") });
                        }
                    }
                }
                else
                {
                    for (var i=0; i<fuzzyMatches.Length; i++)
                    {
                        _items.Add(new SearchItem { Title = fuzzyMatches[i].ToString(), URL = fuzzyMatchLinks[i].ToString() });
                    }
                }
            }
            return _items;
        }
    }
}
