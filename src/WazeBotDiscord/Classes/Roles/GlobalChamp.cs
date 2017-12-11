using System;
using System.Collections.Generic;
using System.Text;

namespace WazeBotDiscord.Classes.Roles
{
    class GlobalChamp
    {
        public static IReadOnlyDictionary<ulong, ulong> Ids = new Dictionary<ulong, ulong>
        {
            [300471946494214146] = 300494132839841792, // National x
            [299563059695976451] = 299566576313827328, // Great Lakes x
            [313435914540154890] = 313436069762695169, // Northwest x
            [301113669696356352] = 301114924514541580, // Southwest x
            [313433524130545664] = 313433637855035402, // Plains x
            [313431377724964876] = 313431665357619201, // South Central x
            [300737538384199681] = 300764249930727424, // South Atlantic x
            [313428729739083776] = 313428925718069262, // Southeast x
            [300482201198395417] = 313425461063712770, // New England x 
            [300481818619150336] = 302527961939181570, // Northeast (NOR) x
            [299676784327393281] = 299677806512570378, // Mid Atlantic x
            [347386780074377217] = 347412351152291871, // Global/Map Raid x
            [356076662812573698] = 356439692822052874, // VEOC
            [352258352417603587] = 352269451305156610  // Territories x
        };
    }
}
