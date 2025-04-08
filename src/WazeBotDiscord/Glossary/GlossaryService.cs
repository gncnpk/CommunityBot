using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WazeBotDiscord.Glossary
{
    public class GlossaryService
    {
        readonly HttpClient _httpClient;

        List<GlossaryItem> _items;

        public GlossaryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _items = new List<GlossaryItem>();
        }

        public async Task InitAsync()
        {
            // 4/8/2025 (MapOMatic) Temporarily disabled due to move to Discuss (DOM structure has changed)
            // await UpdateGlossaryItemsAsync();
        }

        public GlossaryItem GetGlossaryItem(string term)
        {
            var item = _items.FirstOrDefault(i => i.Ids.Select(d => d.ToLowerInvariant().Replace('_', ' ')).Contains(term));
            if (item != null)
                return item;

            return _items.FirstOrDefault(i => i.Term.ToLowerInvariant() == term);
        }

        async Task UpdateGlossaryItemsAsync()
        {
            var parser = new HtmlParser();
            var body = await _httpClient.GetStringAsync("https://www.waze.com/discuss/t/glossary/377948");
            var doc = await parser.ParseDocumentAsync(body);

            var tblRows = doc.QuerySelectorAll("tr");

            _items.Clear();

            foreach (var thisRow in tblRows)
            {
                var row = (IHtmlTableRowElement)thisRow;
                if (row.Cells.Length > 2)
                {
                    var dtString = row.Cells[3].TextContent.Trim();
                    dtString = dtString.Split(null)[0];

                    var dt = DateTime.ParseExact(dtString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime.SpecifyKind(dt, DateTimeKind.Utc);

                    var alternates = row.Cells[1].TextContent.Trim();
                    if (string.IsNullOrEmpty(alternates) || alternates == "~")
                        alternates = "_(none)_";

                    var term = row.Cells[0].Children.First(c => c.TagName == "B").TextContent.Trim();
                    var ids = row.Cells[0].Children.Where(c => c.TagName == "SPAN").Select(c => c.Id.Trim());
                    row.Cells[2].InnerHtml = row.Cells[2].InnerHtml.Replace("<p>", "\n").Replace("</p>", "").Replace("<br>", "\n");
                    _items.Add(new GlossaryItem
                    {
                        Ids = ids.ToList(),
                        Term = term,
                        Alternates = alternates,
                        Description = row.Cells[2].TextContent.Trim(),
                        ModifiedAt = dt
                    });
                }
            }
        }

        public async Task ReloadGlossaryAsync()
        {
            _items.Clear();
            await InitAsync();
        }
    }
}
