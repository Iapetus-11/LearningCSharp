using System.Linq;
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
        
        private static readonly string _request = "\x01\x00\x00\x00\x00\x00\x00\x00\x00\x00\xff\xff\x00\xfe\xfe\xfe\xfe\xfd\xfd\xfd\xfd\x124Vx";
        private IPAddress _ipAddress;
        private int _port;

        public MinecraftBedrockServer(IPAddress i, int p)
        {
            _ipAddress = i;
            _port = p;
        }

        public MinecraftBedrockStatus FetchStatus()
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            byte[] sendBuf = Encoding.ASCII.GetBytes(_request);
            IPEndPoint endpoint = new IPEndPoint(_ipAddress, _port);

            sock.SendTo(sendBuf, endpoint);

            byte[] recvBuf = { };
        }
    }
}