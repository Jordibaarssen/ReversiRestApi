using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ReversiApp.Hubs
{
    public class DoeZetHub : Hub
    {
        public async Task doeEenZet()
        {
            await Clients.All.SendAsync("");
        }
    }
}
