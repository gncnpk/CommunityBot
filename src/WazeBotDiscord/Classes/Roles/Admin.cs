using System.Collections.Generic;

namespace WazeBotDiscord.Classes.Roles
{
    public static class Admin
    {
        public static IReadOnlyDictionary<ulong, ulong> Ids = new Dictionary<ulong, ulong>
        {
            [300471946494214146] = 300490518356295680, // National - corrected
            [313435914540154890] = 313435983636856833, // Northwest - corrected
            [301113669696356352] = 363437599181897729, // Southwest
            [313433524130545664] = 313433556468760586, // Plains - corrected
            [313431377724964876] = 313431621002854400, // South Central - corrected
            [299563059695976451] = 299564786973278208, // Great Lakes - corrected
            [300737538384199681] = 300784192797540362, // South Atlantic - corrected
            //[313428729739083776] = , // Southeast
            [300482201198395417] = 300816449289584642, // New England - corrected
            [300481818619150336] = 300816001606352897, // Northeast (NOR) - corrected
            [299676784327393281] = 299704715405557760, // Mid Atlantic - corrected
            [347386780074377217] = 347410199579197450, // Map Raid - corrected
            //[360595895965843456] = , // Community Test Server
            [356076662812573698] = 356519948786597888, // VEOC - corrected
            [352258352417603587] = 352263231965888512  // Territories - corrected
        };
    }
}
