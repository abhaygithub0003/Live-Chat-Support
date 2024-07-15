using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Users
{
    public class ChatHub:Hub
    {
        public async Task SendMessage(string fromUser, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",fromUser, message);
        }

        //public async Task joingroup(int groupid)
        //{
        //    await groups.addtogroupasync(context.connectionid, groupid.ToString());
        //}

        //public async task leavegroup(int groupid)
        //{
        //    await groups.removefromgroupasync(context.connectionid, groupid.tostring());
        //}
    }
}
