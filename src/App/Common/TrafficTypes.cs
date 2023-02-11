using System.Collections.Generic;
using PacketDotNet;

namespace EFR.NetworkObservability.Common;
/// <summary>
/// Holds the traffic type constants
/// </summary>
public static class TrafficTypes
{
	/// <summary>
	/// Dictionary that hold constants
	/// </summary>
	public static readonly Dictionary<(int port, ProtocolType protocol), string> values = new()
	{
		{ (6969, ProtocolType.Udp), "CoT" },
		{ (161, ProtocolType.Udp), "SNMP" },
		{ (67, ProtocolType.Udp), "DHCP" },

		{ (4242, ProtocolType.Tcp), "CoT" },
		{ (80, ProtocolType.Tcp), "HTTP" },
		{ (25, ProtocolType.Tcp), "SMTP" },
		{ (20, ProtocolType.Tcp), "FTP" }
	};
}
