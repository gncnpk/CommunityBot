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

        public async Task<Boolean> ExecuteBotService(string command)
        {
            try
            {
                await _client.GetAsync(_endpointURL + command + "/" + _validationKey);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception:" + ex.ToString());
                return false;
            }
            
        }
    }
}
