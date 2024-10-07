using System.Text;
using Shared;

public abstract class Packet
{
	public abstract PacketType Type { get; }
	public string SerializedData;

	// Deserialize a packet (fake ctor thing)
	public abstract Packet FromSerialized(string serializedData);

	// Get the packet type index as a hex string
	protected string GetPacketType()
	{
		//? x means hex (0x)
		return ((int)Type).ToString("x");
	}

	// Fully parse a packet from a string into an object
	public static Packet ParsePacket(byte[]	dataBytes)
	{
		// Convert the data to a string
		string data = Encoding.UTF8.GetString(dataBytes);

		// Get the type of the packet
		//? The first character is always the packet type enum index
		int packetIndex = int.Parse(data[0].ToString(), System.Globalization.NumberStyles.HexNumber);
		PacketType packetType = (PacketType)packetIndex;

		// Give them back the packet constructed as an object
		// based on what the packet type is
		Packet packet = packetType switch
		{
			PacketType.Connection => new ConnectionPacket().FromSerialized(data),
			_ => null
		};

		// Check for if there was an issue
		// determining the packet type
		if (packet == null) Logger.Log("Not sure what packet index " + packetIndex + " is");

		// Give back the parsed packet
		return packet;
	}
}

public enum PacketType
{
	Connection = 0
}