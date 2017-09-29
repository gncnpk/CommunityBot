using System.Collections.Generic;

namespace WazeBotDiscord.Classes.Roles
{
    public static class Admin
    {
        public static IReadOnlyDictionary<ulong, ulong> Ids = new Dictionary<ulong, ulong>
        {
            //[300471946494214146] = , // National
            //[313435914540154890] = , // Northwest
            //[301113669696356352] = , // Southwest
            //[313433524130545664] = , // Plains
            //[313431377724964876] = , // South Central
            //[299563059695976451] = , // Great Lakes
            //[300737538384199681] = , // South Atlantic
            //[313428729739083776] = , // Southeast
            //[300482201198395417] = , // New England
            //[300481818619150336] = , // Northeast
            //[299676784327393281] = , // Mid Atlantic
            [347386780074377217] = 361110624958152704 // Map Raid
            //[360595895965843456] = , // Community Test Server
            //[356076662812573698] =   // VEOC
        };
    }
}
