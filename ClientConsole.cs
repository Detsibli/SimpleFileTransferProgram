using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ClientConsole
{
    class Client
    {
        static void Main(string[] args)
        {
            try
            {
                //Let's the user input the IP to connect to.
                Console.WriteLine("Please enter the IP address: ");

                //This binds the client program to user defind IP.
                string UserIPAddress = Console.ReadLine();
                IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(UserIPAddress), 2215);
                Socket clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

                //This let's the user choose the filepath and filename of the file that will be sent.
                Console.WriteLine("Please enter the FilePath where you file is located (Use /): ");
                string filePath = Console.ReadLine();
                Console.WriteLine("Please enter the FileName of the file you would like to send: ");
                string fileName = Console.ReadLine();

                //This prepares the file to be sent
                byte[] fileNameByte = Encoding.ASCII.GetBytes(fileName);
                byte[] fileData = File.ReadAllBytes(filePath + fileName);
                byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];
                byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);

                //This section starts sending the selected file to the server.
                fileNameLen.CopyTo(clientData, 0);
                fileNameByte.CopyTo(clientData, 4);
                fileData.CopyTo(clientData, 4 + fileNameByte.Length);

                //This sections starts sending the file.
                clientSock.Connect(ipEnd);
                clientSock.Send(clientData);

                //For show
                for (int i = 0; i < 5; i++)
                {
                    Console.Clear();
                    Console.WriteLine("Connecting to server.");
                    Console.Clear();
                    Console.WriteLine("Connecting to server..");
                    Console.Clear();
                    Console.WriteLine("Connecting to server...");
                }

                //Let's the user know the file was send successfully.
                Console.WriteLine("File: " + "{0}" + "has been successfully sent.", fileName);

                //Closes the socket and waits for user input.
                clientSock.Close();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Failed to recieve file: " + ex.Message);
            }

        }
    }

}
