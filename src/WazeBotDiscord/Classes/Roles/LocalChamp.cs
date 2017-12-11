using System;
using System.Collections.Generic;
using System.Text;

namespace WazeBotDiscord.Classes.Roles
{
    class LocalChamp
    {
        public static IReadOnlyDictionary<ulong, ulong> Ids = new Dictionary<ulong, ulong>
        {
            [300471946494214146] = 300494182403801088, // National x
            [299563059695976451] = 299566508659441674, // Great Lakes x
            [313435914540154890] = 313436090436681728, // Northwest x
            [301113669696356352] = 301115394540699648, // Southwest x
            [313433524130545664] = 313433662010032129, // Plains x
            [313431377724964876] = 313431741593288704, // South Central x
            [300737538384199681] = 300758322905350164, // South Atlantic x
            [313428729739083776] = 313428948837072897, // Southeast x
            [300482201198395417] = 313425490989809664, // New England x
            [300481818619150336] = 302527984118530048, // Northeast (NOR) x
            [299676784327393281] = 299677614874951680, // Mid Atlantic x
            [347386780074377217] = 347412459172265987, // Global/Map Raid x
            [356076662812573698] = 356439769284345870, // VEOC
            [352258352417603587] = 352271229194993664  // Territories x
        };
    }
}
