using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcp
{
    class file_server
    {
        /// <summary>
        /// The PORT
        /// </summary>
        const int PORT = 9000;
        /// <summary>
        /// The BUFSIZE
        /// </summary>
        const int BUFSIZE = 1000;

        /// <summary>
        /// Initializes a new instance of the <see cref="file_server"/> class.
        /// Opretter en socket.
        /// Venter på en connect fra en klient.
        /// Modtager filnavn
        /// Finder filstørrelsen
        /// Kalder metoden sendFile
        /// Lukker socketen og programmet
         /// </summary>
        private file_server ()
        {
            TcpListener serverSocket = new TcpListener(IPAddress.Any,PORT);         
            TcpClient clientSocket = default(TcpClient);
            
            serverSocket.Start();

			byte[] bytes = new byte[BUFSIZE];
            string data;
                     
            while(true)
            {
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine("Accept conn from client");

				NetworkStream network = clientSocket.GetStream();
                
				try
				{
					data = LIB.readTextTCP(network);
					Console.WriteLine("{0}", data);
					long fileLength = LIB.check_File_Exists(data);

                    string fileName = LIB.extractFileName(data);

                    Console.WriteLine("File name: {0}", fileName);
                    Console.WriteLine("File length: {0}", fileLength);

                    LIB.writeTextTCP(network, fileLength.ToString());

                    sendFile(data, fileLength, network);

                    Console.WriteLine("Closing connection to client...");
                    Console.WriteLine("Returning to idle.");

					clientSocket.GetStream().Close();
                    clientSocket.Close();               
				}
				catch (FileNotFoundException ex)
				{
				    Console.WriteLine("File not found..!");
					throw ex;
				}
            }         
        }

        /// <summary>
        /// Sends the file.
        /// </summary>
        /// <param name='fileName'>
        /// The filename.
        /// </param>
        /// <param name='fileSize'>
        /// The filesize.
        /// </param>
        /// <param name='io'>
        /// Network stream for writing to the client.
        /// </param>
        private void sendFile (String fileName, long fileSize, NetworkStream io)
        {
			// TO DO Your own code       

			FileStream fs = File.Open(fileName, FileMode.Open);
			byte[] chunks = new byte[BUFSIZE];

			Console.WriteLine("Sending file: {0}", fileName);

            while(fileSize > 0)
			{
				int m = fs.Read(chunks, 0, BUFSIZE);

                //Console.WriteLine("Read is succesful");

				io.Write(chunks, 0, m);
                
				//Console.WriteLine("Write is succesful");

				fileSize -= m;
			}
			fs.Close();
            Console.WriteLine("File was sent succesfully...");
            

        }

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name='args'>
        /// The command-line arguments.
        /// </param>
        public static void Main (string[] args)
        {
            Console.WriteLine ("Server booted...");
            new file_server();
            Console.ReadKey();
            
        }
    }
}
