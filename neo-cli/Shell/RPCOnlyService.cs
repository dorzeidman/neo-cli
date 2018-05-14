using System;
using System.Threading;

namespace Neo.Shell
{
    internal class RPCOnlyService : MainService
    {
        public override string ServiceName => "NEO-RPC";
        private bool _keepRunning = true;

        protected internal override bool OnStart(string[] args)
        {
            if(!ParseArgs(args, out string path, out string password, out int? rpcPort))
            {
                Console.WriteLine("Service stopping");
                return false;
            }
            
            OnStart(useRPC: true, rpcPort: rpcPort);

            if(!OpenWalletCommand(path, password))
            {
                Console.WriteLine("Open Wallet Failed. Service stopping");
                return false;
            }

            return true;
        }

        protected override void RunConsole()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            while (_keepRunning)
            {
                Thread.Sleep(1000);
            }
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            _keepRunning = false;
        }

        private bool ParseArgs(string[] args, out string path, out string password, out int? rpcPort)
        {
            path = null;
            password = null;
            rpcPort = null;

            if (args.Length < 3)
            {
                Console.WriteLine("Error: Args missing. e.g: --rpc-service /var/wallet.json pass1234 8080");
                return false;
            }

            path = args[1];
            password = args[2];

            if(args.Length >= 4)
            {
                if(int.TryParse(args[3], out int rpcPortTemp))
                {
                    rpcPort = rpcPortTemp;
                }
            }
            return true;
        }
    }
}
