using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ServerConsole
{
    class Server
    {
        static void Main(string[] args)
        {
            try
            {
                //Let's the user choose where the file will be recieved to.
                Console.WriteLine("Please enter the FilePath where you want your file to be recieved to (Use /): ");
                string receivedPath = Console.ReadLine();

                //This lets the server listen to "any" ip address,
                //which is the host address and port to wait and see if the client connects to send the file
                IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, 2215);
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                sock.Bind(ipEnd);
                sock.Listen(100);
                Socket clientSock1 = sock.Accept();

                //After the client connects it starts sending the data.
                byte[] clientData = new byte[1024 * 5000];

                int receivedBytesLen = clientSock1.Receive(clientData);
                int fileNameLen = BitConverter.ToInt32(clientData, 0);
                string fileName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);

                //For show
                for (int i = 0; i < 5; i++)
                {
                    Console.Clear();
                    Console.WriteLine("Waiting for client to connect.");
                    Console.Clear();
                    Console.WriteLine("Waiting for client to connect..");
                    Console.Clear();
                    Console.WriteLine("Waiting for client to connect...");
                }

                //Let's the user when the client connects and starts sending the file.
                Console.WriteLine("Client: " + "{0}" + "has connected, now recieving " + "File {1}" , clientSock1.RemoteEndPoint, fileName);

                //This sections lets the program recieved the file that the client is sending.
                BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + fileName, FileMode.Append)); ;
                bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);

                //Let's the user know that the
                Console.WriteLine("File: {0} has been received & saved at path: {1}", fileName, receivedPath);

                //Closes the binary writer and client sockets.
                bWrite.Close();
                clientSock1.Close();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                //Let's the user know if there is any error plus system messages
                Console.WriteLine("Error: Failed to recieve file: " + ex.Message);
            }
        }
    }
}
