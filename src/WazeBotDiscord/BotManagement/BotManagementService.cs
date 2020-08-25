using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WazeBotDiscord.BotManagement
{
    public class BotManagementService
    {
        HttpClient _client;
        string _endpointURL;
        string _validationKey;

        public BotManagementService(HttpClient client, string endpointURL, string validationKey)
        {
            _client = client;
            _endpointURL = endpointURL;
            _validationKey = validationKey;
        }

        public async Task<string> ExecuteBotService(string command)
        {
            try
            {
                await _client.GetAsync(_endpointURL + command + "/" + _validationKey);
                return "";
            }
            catch(Exception ex)
            {
                var log = "Endpoint: " + _endpointURL +
                    "\nValidation key: " + _validationKey +
                    "\n\n Error: " + ex.ToString();
                Console.WriteLine(log);
                return log;
            }
            
        }
    }
}
