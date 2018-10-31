using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_Server
{
	public class UDP_Server
	{
		public static void Main(string[] args)
		{
			byte[] data = new byte[1024];
			IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
			UdpClient newsock = new UdpClient(ipep);
			Console.WriteLine("The server is started");

			while (true)
			{
				IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);           
                data = newsock.Receive(ref sender);
                string result = Encoding.ASCII.GetString(data, 0, data.Length);
                Console.WriteLine(result);
				switch (result)
				{
					case "U":
					case "u":
						string uptime = System.IO.File.ReadAllText("/proc/uptime").TrimEnd('\r', '\n');
                        byte[] uptime_bytes = Encoding.ASCII.GetBytes(uptime);
                        newsock.Send(uptime_bytes, uptime_bytes.Length, sender);
						break;
					case "L":
					case "l":
						string loadavg = System.IO.File.ReadAllText("/proc/loadavg").TrimEnd('\r', '\n');
						byte[] loadavg_bytes = Encoding.ASCII.GetBytes(loadavg);
						newsock.Send(loadavg_bytes, loadavg_bytes.Length, sender);
						break;
					default:
						break;
				}
			}
		}
	}
}
