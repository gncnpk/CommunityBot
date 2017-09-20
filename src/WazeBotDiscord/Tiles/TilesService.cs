using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System;

namespace WazeBotDiscord.Tiles
{
    class TilesService
    {
        readonly HttpClient _httpClient;

        TilesResult _tilesResult;

        public TilesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task InitTilesServiceAsync()
        {
            await UpdateParseUpdatesAsync();
        }

        public TilesResult GetTileInfo()
        {
            return _tilesResult;
        }

        async Task UpdateParseUpdatesAsync()
        {
            HttpClient httpClient = new HttpClient();
            string body = await httpClient.GetStringAsync("https://wazestatus.wordpress.com/category/main/feed");
            Regex regNA = new Regex("<title>NA map tiles were successfully updated to:(.*?)<\\/title>(.|\\n)*?<pubDate>(.*?) \\+0000<\\/pubDate>");
            Match matchNA = regNA.Match(body);
            Regex regINTL = new Regex("<title>INTL map tiles were successfully updated to:(.*?)<\\/title>(.|\\n)*?<pubDate>(.*?) \\+0000<\\/pubDate>");
            Match matchINTL = regINTL.Match(body);

            _tilesResult.NATileDate = "NA: " + matchNA.Groups[1].ToString();
            _tilesResult.NAUpdatePerformed = "(performed: " + matchNA.Groups[3].ToString() + " UTC)";
            _tilesResult.INTLTileDate = "NA: " + matchINTL.Groups[1].ToString();
            _tilesResult.INTLUpdatePerformed = "(performed: " + matchINTL.Groups[3].ToString() + " UTC)";
            //string NA = "NA: " + (matchNA.Groups[1].ToString() + (Environment.NewLine + ("(performed: " + (matchNA.Groups[3].ToString() + " UTC)"))));
            //string INTL = "INTL: " + (matchINTL.Groups[1].ToString() + (Environment.NewLine + ("(performed: " + (matchINTL.Groups[3].ToString() + " UTC)"))));
        }
    }
}
