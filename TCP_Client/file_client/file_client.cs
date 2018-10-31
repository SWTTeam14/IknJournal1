using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcp
{
    class file_client
    {
        /// <summary>
        /// The PORT.
        /// </summary>
        const int PORT = 9000;
        /// <summary>
        /// The BUFSIZE.
        /// </summary>
        const int BUFSIZE = 1000;

        long fileSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="file_client"/> class.
        /// </summary>
        /// <param name='args'>
        /// The command-line arguments. First ip-adress of the server. Second the filename
        /// </param>
        private file_client (string[] args)
        {
            // TO DO Your own code
            TcpClient clientSocket = new TcpClient();
            
			clientSocket.Connect(args[0], PORT);

            Console.WriteLine("Client connected...");
            
            NetworkStream network = clientSocket.GetStream();
                     
            LIB.writeTextTCP(network, args[1]);
                     
			fileSize = LIB.getFileSizeTCP(network);
			Console.WriteLine("File size from server: {0}", fileSize, " bytes");
            
            if(fileSize == 0)
			{
				throw new Exception("File not found..!");            
			}
			else
			{            
                receiveFile(args[1], network);
                Console.WriteLine("Received file(s)...");            
			}         
		}

        /// <summary>
        /// Receives the file.
        /// </summary>
        /// <param name='fileName'>
        /// File name.
        /// </param>
        /// <param name='io'>
        /// Network stream for reading from the server
        /// </param>

        private void receiveFile(String fileName, NetworkStream io)
        {
            FileStream fs = File.Create(fileName);

            byte[] chunks = new byte[BUFSIZE];

            while(fileSize > 0)
            {
				int m = io.Read(chunks, 0, BUFSIZE);
    
                fs.Write(chunks, 0, m);
                     
                fileSize -= m;
                Console.WriteLine("Filesize: {0}", fileSize);
            }
            Console.WriteLine("File named {0} was created succesfully...", fileName);
        }

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name='args'>
        /// The command-line arguments.
        /// </param>
        public static void Main (string[] args)
        {
            Console.WriteLine ("Client running...");
            new file_client(args);
            Console.ReadKey();
        }
    }
}
