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
            ParseArgs(args, out string path, out string password);
            
            OnStart(useRPC: true);

            if (!string.IsNullOrEmpty(path))
            {
                if (!OpenWalletCommand(path, password))
                {
                    Console.WriteLine("Open Wallet Failed. Service stopping");
                    OnStop();
                    return false;
                }
                else
                    Console.WriteLine("Wallet is open!");
            }

            Console.WriteLine($"{ServiceName} Service Started!. RPC Port:{Settings.Default.RPC.Port}");

            return true;
        }

        protected internal override void OnStop()
        {
            base.OnStop();
            Console.WriteLine($"{ServiceName} Service Stopped!");
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

        private void ParseArgs(string[] args, out string path, out string password)
        {
            path = null;
            password = null;

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--wallet-path":
                    case "/wallet-path":
                    case "-wallet-path":
                        if (args.Length > i + 1)
                            path = args[i + 1];
                        break;
                    case "--wallet-password":
                    case "/wallet-password":
                    case "-wallet-password":
                        if (args.Length > i + 1)
                            password = args[i + 1];
                        break;
                    case "--rpc-port":
                    case "/rpc-port":
                    case "-rpc-port":
                        if (args.Length > i + 1)
                        {
                            if (ushort.TryParse(args[i + 1], out ushort rpcPort))
                            {
                                Settings.Default.RPC.Port = rpcPort;
                            }
                        }
                        break;
                }
            }
        }
    }
}
