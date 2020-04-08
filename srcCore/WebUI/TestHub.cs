using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebUI {
    public class TestHub : Hub {
        public async Task SendMessage(int value) {
            await Clients.All.SendAsync("ReceiveMessage", value);
        }
    }
}
