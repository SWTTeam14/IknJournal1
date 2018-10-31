using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_Client2
{
	public class UDP_Client2
	{
		public static void Main(string[] args)
		{
			string input, stringData;
			input = args[1];

            if (input !="U" && input !="u" && input !="L" && input !="l")
			{
				Console.WriteLine("You can only write L/l or U/u to continue this program");
				System.Environment.Exit(1);
			}

			byte[] data = new byte[1024];
			UdpClient server = new UdpClient(args[0], 9050);
			IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
			server.Send(Encoding.ASCII.GetBytes(input), input.Length);
			data = server.Receive(ref sender);
			stringData = Encoding.ASCII.GetString(data, 0, data.Length);
			Console.WriteLine(stringData);
            server.Close();         
		}
	}
}
