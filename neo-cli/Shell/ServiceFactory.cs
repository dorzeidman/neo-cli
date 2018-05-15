using Neo.Services;
using System.Linq;

namespace Neo.Shell
{
    internal static class ServiceFactory
    {
        public static ConsoleServiceBase Create(string[] args)
        {
            ConsoleServiceBase service = null;
            
            if(args.Any(x => x == "/rpc-service" || x == "--rpc-service" || x == "-rpc-service"))
            {
                service = new RPCOnlyService();
            }
            
            if (service == null)
                service = new MainService();

            return service;
        }
    }
}
