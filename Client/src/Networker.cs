using System.Net;
using System.Net.Sockets;
using System.Text;
using Shared;

class Networker
{
	private static UdpClient client;
	private static IPEndPoint server;

	// Connect to the server and join the game
	public static void Network(int port, string ip, string username)
	{
		// Connect to the server
		server = new IPEndPoint(IPAddress.Parse(ip), port);
		Logger.Log("Connected to the server");

		// Join the game
		Logger.Log("Asking to join rn");
		ConnectionPacket packet = new ConnectionPacket(username);
		SendToServer(packet);

		
	}

	// Send a packet to the server
	private static void SendToServer(Packet packet)
	{
		// Get the packets data and convert it
		// to bytes for sending
		byte[] dataBytes = Encoding.UTF8.GetBytes(packet.SerializedData);

		// Send it to the server
		client.Send(dataBytes, dataBytes.Length, server);
	}
}