using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WazeBotDiscord.Scripts
{
    public class ScriptsService
    {
        readonly HttpClient _client;
       // List<SheetToSearch> _sheets;

        public ScriptsService(HttpClient client)
        {
            _client = client;
        }

        /*public async Task InitAsync()
        {
            using (var db = new WbContext())
            {
                _sheets = await db.SheetsToSearch.ToListAsync();
            }
        }*/

        public string GetChannelSheetUrl(ulong channelId)
        {
            /*var sheet = _sheets.Find(s => s.ChannelId == channelId);
            if (sheet == null)
                return "This channel is not configured to search a spreadsheet.";*/

            return $"<https://docs.google.com/spreadsheets/u/1/d/1yrEZMrQyMjhgBAJuNj7Y8z0GxdKWgIEkHIQBhUM2H9k>";
        }

        public async Task<string> SearchSheetAsync( string origSearchString)
        {
            var searchString = origSearchString.ToLowerInvariant();
            var parser = new HtmlParser();

            string sheetURL = $"https://docs.google.com/spreadsheets/u/1/d/1yrEZMrQyMjhgBAJuNj7Y8z0GxdKWgIEkHIQBhUM2H9k/pubhtml";
            var resp = await _client.GetAsync(sheetURL);

            //if (!resp.IsSuccessStatusCode)
            //    return "Spreadsheet is not configured correctly.";

            var doc = await parser.ParseAsync(await resp.Content.ReadAsStringAsync());

            var tblHeader = doc.QuerySelectorAll("table.waffle > tbody > tr:first-of-type");
            var headerRowRaw = tblHeader.FirstOrDefault();
            if (headerRowRaw == null)
                return "Spreadsheet is not configured correctly.";

            var headerRow = (IHtmlTableRowElement)headerRowRaw;
            var headerFields = headerRow.Cells
                .Where(c => !string.IsNullOrWhiteSpace(c.TextContent))
                .Select(c => c.TextContent)
                .ToList();

            var contentRows = doc.QuerySelectorAll("table.waffle > tbody > tr:not(:first-of-type)");
            var matches = new List<List<string>>();

            foreach (var thisRow in contentRows)
            {
                var row = (IHtmlTableRowElement)thisRow;
                var match = false;

                foreach (var cell in row.Cells)
                {
                    if (cell.TextContent.ToLowerInvariant().Replace("-", " ").Contains(searchString.Replace("-"," ")))
                    {
                        match = true;
                        break;
                    }
                }

                if (match)
                    matches.Add(row.Cells.Select(c => c.TextContent).ToList());
            }

            return GenerateResult(headerFields, matches, origSearchString);
        }

        string GenerateResult(List<string> headers, List<List<string>> matches, string searchString)
        {
            var result = new StringBuilder();

            var matchCount = matches.Count;
            if (matchCount == 0)
            {
                return $"No results found for `{searchString}`.";
            }
            else if (matchCount > 10)
            {
                matchCount = 10;
                result.Append($"Total of {matches.Count} results for `{searchString}`; only the top 10 are shown.\n");
            }
            else
                result.Append($"{matchCount} results found for `{searchString}`.\n");

            for (var i = 0; i < matchCount; i++)
            {
                //result.AppendLine("```");

                for (var j = 1; j < matches[i].Count - 1; j++)
                {
                    if (string.IsNullOrWhiteSpace(matches[i][j]))
                        continue;

                        result.Append(matches[i][j]);
                        result.Append(" | ");
                }
                result.Remove(result.Length - 3, 3);
                if (matchCount > 0 && i != matchCount - 1)
                    result.AppendLine(Environment.NewLine);

                //result.AppendLine("```");
            }
            string resultString = result.ToString();
            Regex regURL = new Regex(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&\/=]*)");
            Match matchNA = regURL.Match(resultString);
            foreach (Match itemMatch in regURL.Matches(resultString))
            {
                resultString = resultString.Replace(itemMatch.ToString(), "<" + itemMatch.ToString() + ">");
            }
            resultString = Regex.Replace(resultString, "<{2,}","<");
            resultString = Regex.Replace(resultString, ">{2,}", ">");
            return resultString;
        }

        /*public SheetToSearch GetExistingLookupSheet(ulong channelId, ulong guildId)
        {
            return _sheets.Find(r => r.ChannelId == channelId && r.GuildId == guildId);
        }*/
    }
}
