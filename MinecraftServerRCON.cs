using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace LearningCSharp
{
    public class MinecraftServerRCON
    {
        enum MessageType
        {
            Login = 3,
            Command = 2,
            Response = 0,
            InvalidAuth = -1
        };

        private static readonly Random Rand = new Random();

        private string _host;
        private int _port;

        private TcpClient _client;
        private NetworkStream _stream;

        public MinecraftServerRCON(string h, int p)
        {
            _host = h;
            _port = p;
        }

        private byte[] PackInt(int x)
        {
            return BitConverter.GetBytes(x);
        }

        private int UnpackInt(byte[] bytes, int offset = 0)
        {
            return BitConverter.ToInt32(bytes);
        }

        private byte[] ReadExactly(int n)
        {
            var outBytes = new List<byte>();
            var buffer = new byte[1];

            while (outBytes.Count < n)
            {
                if (_stream.Read(buffer, 0, 1) > 0) outBytes.Add(buffer[0]);
            }

            return outBytes.ToArray();
        }

        private Tuple<string, MessageType> SendMessage(MessageType type, string message)
        {
            var requestId = Rand.Next(0, 2147483647);
            
            // pack message
            var packetData = PackInt(requestId)
                .Concat(PackInt((int) type))
                .Concat(System.Text.Encoding.UTF8.GetBytes(message))
                .Concat(new byte[]{0, 0})
                .ToArray();
            
            // write message to server
            _stream.Write(PackInt(packetData.Length).Concat(packetData).ToArray());
            
            // unpack size of upcoming message and create new buffer that size
            var remainingLength = UnpackInt(ReadExactly(4));
            var data = ReadExactly(8); // contains the in message type + id

            var inMessageType = (MessageType) UnpackInt(data.Take(4).ToArray());
            if (inMessageType == MessageType.InvalidAuth) throw new Exception("Invalid authorization");
            
            // read rest of message
            data = ReadExactly(remainingLength - 8);

            return new Tuple<string, MessageType>(System.Text.Encoding.UTF8.GetString(data), inMessageType);
        }

        public void Connect(string password)
        {
            _client = new TcpClient(_host, _port);
            _stream = _client.GetStream();

            SendMessage(MessageType.Login, password);
        }

        public void Close()
        {
            _stream.Close();
            _client.Close();
        }

        public string SendCommand(string command)
        {
            var (response, responseType) = SendMessage(MessageType.Command, command);

            if (responseType != MessageType.Response) throw new Exception("Invalid response from server");
            
            return response;
        }

        ~MinecraftServerRCON()
        {
            Close();
        }

        public static void Test()
        {
            Console.Write("Enter a host: ");
            var host = Console.ReadLine();
            
            Console.Write("Enter a port: ");
            var port = Int32.Parse(Console.ReadLine());
            
            Console.Write("Enter a password: ");
            var password = Console.ReadLine();
            
            var client = new MinecraftServerRCON(host, port);
            client.Connect(password);

            var input = ".";

            while (!string.IsNullOrEmpty(input))
            {
                Console.Write("Enter a command: ");
                input = Console.ReadLine();
                
                if (string.IsNullOrEmpty(input)) continue;
                
                var (response, responseType) = client.SendMessage(MessageType.Command, input);
                
                Console.WriteLine(response);
            }
        }
    }
}