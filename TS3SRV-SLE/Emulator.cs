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
            TS3ServerListEmulator Emu = new TS3ServerListEmulator(serverProperties);
            for (;;)
                Console.ReadKey(true);
        }

        public static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
