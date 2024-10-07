public class ConnectionPacket : Packet
{
	public override PacketType Type => PacketType.Connection;
	public string Username { get; private set; }

	public ConnectionPacket(string username)
	{
		// Assign all the stuff
		Username = username.Trim();

		// Serialize all the data
		SerializedData = $"{GetPacketType()}{username}";
	}

	public ConnectionPacket() { }
	public override ConnectionPacket FromSerialized(string serializedData)
	{
		// Get rid of the packet index from
		// the front of the string and make it
		// in a csv type format
		serializedData = serializedData.Remove(0, 1);
		string[] data = serializedData.Split(',');

		// Extract all the data
		Username = data[0];

		// Chuck back the populated packet we
		// just populated rn 
		return this;
	}
}