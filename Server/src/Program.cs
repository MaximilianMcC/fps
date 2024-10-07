using System.Net;
using System.Net.Sockets;
using System.Text;
using Shared;

namespace Server;
class Program
{
	public static void Main(string[] args)
	{
		Logger.Log("Kia ora!");

		// TODO: Get this from args
		const int port = 54321;

		// Make the server
		UdpClient server = new UdpClient(port);
		IPEndPoint incomingEndpoint = new IPEndPoint(IPAddress.Any, 0);

		// Start the server
		Logger.Log("Listening rn (all ears)");
		while (true)
		{
			// Get any incoming transmissions then decode them
			byte[] receivedBytes = server.Receive(ref incomingEndpoint);
			Packet packet = Packet.ParsePacket(receivedBytes);

			// Handle the packet based on the type
			if (packet.Type == PacketType.Connection) {  }


			// Echo for now
			server.Send(receivedBytes, receivedBytes.Length, incomingEndpoint);
		}
	}
}