using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LearningCSharp
{
    public readonly struct MinecraftBedrockStatus
    {
        public readonly string Motd, Version, World, Gamemode;
        public readonly int Protocol, OnlinePlayers, MaxPlayers;

        public MinecraftBedrockStatus(string m, string v, string w, string g, int p, int oP, int mP)
        {
            Motd = m;
            Version = v;
            World = w;
            Gamemode = g;
            Protocol = p;
            OnlinePlayers = oP;
            MaxPlayers = mP;
        }
    }
    
    public class MinecraftBedrockServer
    {
        
        // private static readonly string _request = "\x01\x00\x00\x00\x00\x00\x00\x00\x00\x00\xff\xff\x00\xfe\xfe\xfe\xfe\xfd\xfd\xfd\xfd\x124Vx";
        private static readonly byte[] _request =
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 0, 254, 254, 254, 254, 253, 253, 253, 253, 18, 52, 86, 120};
        private string _host;
        private int _port;

        public MinecraftBedrockServer(string h, int p)
        {
            _host = h;
            _port = p;
        }

        public MinecraftBedrockStatus FetchStatus()
        {
            var udpClient = new UdpClient();
            udpClient.Connect(_host, _port);

            udpClient.Send(_request, _request.Length);

            var remote = new IPEndPoint(IPAddress.Any, 0);
            var data = Encoding.UTF8.GetString(udpClient.Receive(ref remote));
            var split = data.Split(';');

            return new MinecraftBedrockStatus(split[1], split[3], split[7], split[8], Int32.Parse(split[2]), Int32.Parse(split[4]), Int32.Parse(split[5]));
        }

        public void Test()
        {
            var status = FetchStatus();
            
            Console.WriteLine($"Server: {_host}:{_port}");
            Console.WriteLine($"MOTD: {status.Motd}");
            Console.WriteLine($"Players: {status.OnlinePlayers} / {status.MaxPlayers}");
            Console.WriteLine($"Version: {status.Version}");
            Console.WriteLine($"Protocol: {status.Protocol}");
            Console.WriteLine($"World: {status.World}");
            Console.WriteLine($"Gamemode: {status.Gamemode}");
        }
    }
}