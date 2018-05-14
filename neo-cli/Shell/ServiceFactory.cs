using Neo.Services;

namespace Neo.Shell
{
    internal static class ServiceFactory
    {
        public static ConsoleServiceBase Create(string[] args)
        {
            ConsoleServiceBase service = null;

            if(args.Length > 2 &&
                (args[1] == "/rpc-service" || args[1] == "--rpc-service" || args[1] == "-rpc-service"))
            {
                service = new RPCOnlyService();
            }
            
            if (service == null)
                service = new MainService();

            return service;
        }
    }
}
