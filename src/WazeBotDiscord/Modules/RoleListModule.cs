using Discord;
using Discord.Commands;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WazeBotDiscord.Utilities;

namespace WazeBotDiscord.Modules
{
    [Group("roles")]
    [RequireAdmin]
    public class RoleListModule : ModuleBase
    {
        [Command]
        public async Task ListRoles([Remainder]string unused = null)
        {
            var orderedRoles = Context.Guild.Roles.OrderByDescending(r => r.Position);

            var replySb = new StringBuilder("__Roles__\n");
            string reply = "";
            foreach (var role in orderedRoles)
            {
                var roleName = role.Name;
                if (roleName == "@everyone")
                    roleName = "(@)everyone";
                var roleLine = $"{roleName}: {role.Id}";
                if(replySb.Length + roleLine.Length < 2000)
                    replySb.AppendLine(roleLine);
                else
                {
                    reply = replySb.ToString();
                    reply = reply.TrimEnd('\\', 'n');
                    await ReplyAsync(reply);
                    replySb.Clear();
                    replySb.AppendLine(roleLine);
                }
                
            }

            reply = replySb.ToString();
            reply = reply.TrimEnd('\\', 'n');

            await ReplyAsync(reply);
        }
    }
}
