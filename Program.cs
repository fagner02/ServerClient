// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;


IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("localhost");
IPAddress ipAddress = ipHostInfo.AddressList[0];
IPEndPoint ipEndPoint = new(IPAddress.Parse(""), 11_000);
using Socket client = new Socket(new AddressFamily(), SocketType.Stream, ProtocolType.IPv4);
client.Accept();
client.Connect(ipEndPoint);
Console.WriteLine("Hello, World!");
