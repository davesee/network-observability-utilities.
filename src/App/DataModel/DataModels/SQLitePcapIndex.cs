using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EFR.NetworkObservability.DataModel;

/// <summary>
/// SQL packet indices model
/// </summary>
[Table("packetindices")]
[ExcludeFromCodeCoverage]
public class SQLitePcapIndex
{
	/// <summary>
	/// Gets the offset of the packet in the PCap file.
	/// </summary>
	public long? PacketOffset
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the id of the interface the packet was received on.
	/// </summary>
	public int? InterfaceId
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the size of the packet.
	/// </summary>
	public int? IPPacketSize
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the Rabin fingerprint of the packet.
	/// </summary>
	public long? Fingerprint
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the VLAN id the packet.
	/// </summary>
	public int? VLan
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the source IP address of an IP packet.
	/// </summary>
	public string? SourceIP
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the destination IP address of an IP packet.
	/// </summary>
	public string? DestinationIP
	{
		get;
		set;
	}


	/// <summary>
	/// Gets the value of the Identification field for an IPv4 packet.
	/// </summary>
	public int? IPIdentification
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the type of service for an IP packet.
	/// </summary>
	public byte? TypeOfService
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the protocol type for an IP packet.
	/// </summary>
	public byte? Protocol
	{
		get;
		set;
	}

	/// <summary>
	/// Gets Informational flags about this packet.
	/// </summary>
	public byte? InfoFlags
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the source port for a TCP/UDP packet.
	/// </summary>
	public int? SourcePort
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the destination port of a TCP/UDP packet.
	/// </summary>
	public int? DestinationPort
	{
		get;
		set;
	}


	/// <summary>
	/// Gets the source IP address of the outer tunnel wrapper.
	/// </summary>
	public string? TunnelSourceIP
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the destination IP address of the outer tunnel wrapper.
	/// </summary>
	public string? TunnelDestinationIP
	{
		get;
		set;
	}

	/// <summary>
	/// Time in utc
	/// </summary>
	public long? UtcTicks
	{
		get;
		set;
	}
}

