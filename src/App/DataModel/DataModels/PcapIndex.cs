using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace EFR.NetworkObservability.DataModel;

/// <summary>
/// Model for a PCAP INDEX
/// </summary>
[Index(nameof(Timestamp), Name = "IX_PcapIndex_Timestamp")]
[Index(nameof(Protocol), nameof(Timestamp), Name = "IX_PcapIndex_Protocol_Timestamp")]
[Index(nameof(SourceIP), nameof(DestinationIP), Name = "IX_Pcapindices_IPs")]
[Table("PacketIndices")]
[ExcludeFromCodeCoverage]
public class PcapIndex
{
	/// <summary>
	/// Identity for the job that can be a reference key for other tables
	/// </summary>
	[Key, Column("packetID"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long? PcapIndexID
	{
		get;
		set;
	}

	/// <summary>
	/// Foreign key for the PcapFileProcessing Log table
	/// </summary>
	[ForeignKey("pcapFileProcessingLog")]
	[Column("pcapFileProcessingLogID")]
	public long? PcapFileProcessingLogID
	{
		get;
		set;
	}

	/// <summary>
	/// Foreign key for the PcapFileProcessing Log table
	/// </summary>
	public virtual PcapFileProcessingLog? PcapFileProcessingLog
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the offset of the packet in the PCap file.
	/// </summary>
	[Column("packetOffset")]
	public long? PacketOffset
	{
		get;
		set;
	}


	/// <summary>
	/// Gets the size of the packet.
	/// </summary>
	[Column("ipPacketSize")]
	public int? IpPacketSize
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the size of the packet.
	/// </summary>
	[Column("timestamp")]
	public DateTime? Timestamp
	{
		get;
		set;
	}


	/// <summary>
	/// Gets the source IP address of an IP packet.
	/// </summary>
	[MaxLength(39)]
	[Column("sourceIP")]
	public string? SourceIP
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the destination IP address of an IP packet.
	/// </summary>
	[MaxLength(39)]
	[Column("destinationIP")]
	public string? DestinationIP
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the value of the Identification field for an IPv4 packet.
	/// </summary>
	[Column("ipIdentification")]
	public int? IPIdentification
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the type of service for an IP packet.
	/// </summary>
	[Column("typeOfService")]
	public byte? TypeOfService
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the protocol type for an IP packet.
	/// </summary>
	[Column("protocol")]
	public byte? Protocol
	{
		get;
		set;
	}

	/// <summary>
	/// Gets Informational flags about this packet.
	/// </summary>
	[Column("infoFlags")]
	public byte? InfoFlags
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the source port for a TCP/UDP packet.
	/// </summary>
	[Column("sourcePort")]
	public int? SourcePort
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the destination port of a TCP/UDP packet.
	/// </summary>
	[Column("destinationPort")]
	public int? DestinationPort
	{
		get;
		set;
	}


	/// <summary>
	/// Gets the destination IP address of the outer tunnel wrapper.
	/// </summary>
	[MaxLength(39)]
	[Column("trafficType")]
	public string? TrafficType
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the julianDate of the packet insertion time.
	/// </summary>
	[Column("julianDay")]
	public int? JulianDay
	{
		get;
		set;
	}

	/// <summary>
	/// The data for this packet.
	/// </summary>
	[Column("data")]
	public byte[]? Data
	{
		get;
		set;
	}
}
