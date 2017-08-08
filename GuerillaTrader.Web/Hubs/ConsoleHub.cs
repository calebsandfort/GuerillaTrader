using Abp.Dependency;
using GuerillaTrader.Shared.Dtos;
using Microsoft.AspNet.SignalR;

namespace GuerillaTrader.Web.Hubs
{
    public class ConsoleHub : Hub, ISingletonDependency
    {
        public void WriteLine(ConsoleWriteLineInput input)
        {
            Clients.All.writeLine(input.Line);
        }
    }
}