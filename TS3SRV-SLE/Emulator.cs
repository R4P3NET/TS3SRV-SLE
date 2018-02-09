using System;
using CommandLine;
using TS3SRV_SLE.Internal;
using TS3SRV_SLE.Network;

namespace TS3SRV_SLE
{
    class Emulator
    {
        static void Main(string[] args)
        {
            var serverProperties = new ServerProperties();
            Parser.Default.ParseArguments<ServerProperties>(args).WithParsed(opts => { serverProperties = opts; })
                .WithNotParsed(errors =>
                {
                    Console.WriteLine("Failed to parse commands, program will now exit!");
                    Console.ReadKey();
                    Environment.Exit(1);

                });
            Console.SetOut(new PrefixedWriter());
            var emu = new Ts3ServerListEmulator(serverProperties);
            for (;;)
                Console.ReadKey(true);
        }
    }
}
