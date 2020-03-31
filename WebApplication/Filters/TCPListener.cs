using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Web;


   
namespace WebApplication.Filters

{

    /// <summary>

    /// This is a simple TCP and UDP based server.

    /// </summary>

   public class TCPListener

    {

        /// <summary>

        /// Winsock ioctl code which will disable ICMP errors from being propagated to a UDP socket.

        /// This can occur if a UDP packet is sent to a valid destination but there is no socket

        /// registered to listen on the given port.

        /// </summary>

        public const int SIO_UDP_CONNRESET = -1744830452;



        /// <summary>

        /// This routine repeatedly copies a string message into a byte array until filled.

        /// </summary>

        /// <param name="dataBuffer">Byte buffer to fill with string message</param>

        /// <param name="message">String message to copy</param>

        static public void FormatBuffer(byte[] dataBuffer, string message)

        {

            byte[] byteMessage = System.Text.Encoding.ASCII.GetBytes(message);

            int index = 0;



            // First convert the string to bytes and then copy into send buffer

            while (index < dataBuffer.Length)

            {

                for (int j = 0; j < byteMessage.Length; j++)

                {

                    dataBuffer[index] = byteMessage[j];

                    index++;



                    // Make sure we don't go past the send buffer length

                    if (index >= dataBuffer.Length)

                    {

                        break;

                    }

                }

            }

        }



        /// <summary>

        /// Prints simple usage information.

        /// </summary>

        static void usage()

        {

            Console.WriteLine("Executable_file_name [-l bind-address] [-m message] [-n count] [-p port]");

            Console.WriteLine("                 [-t tcp|udp] [-x size]");

            Console.WriteLine("  -l bind-address        Local address to bind to");

            Console.WriteLine("  -m message             Text message to format into send buffer");

            Console.WriteLine("  -n count               Number of times to send a message");

            Console.WriteLine("  -p port                Local port to bind to");

            Console.WriteLine("  -t udp | tcp           Indicates which protocol to use");

            Console.WriteLine("  -x size                Size  of send and receive buffer");

            Console.WriteLine(" Else, default values will be used...");

        }



        /// <summary>

        /// This is the main routine that parses the command line and invokes the server with the

        /// given parameters. For TCP, it creates a listening socket and waits to accept a client

        /// connection. Once a client connects, it waits to receive a "request" message. The

        /// request is terminated by the client shutting down the connection. After the request is

        /// received, the server sends a response followed by shutting down its connection and

        /// closing the socket. For UDP, the socket simply listens for incoming packets. The "request"

        /// message is a single datagram received. Once the request is received, a number of datagrams

        /// are sent in return followed by sending a few zero byte datagrams. This way the client

        /// can determine that the response has completed when it receives a zero byte datagram.

        /// </summary>

        /// <param name="args">Command line arguments</param>

        static void Main(string[] args)

        {

            string textMessage = "Server: ServerResponse";

            int localPort = 5150, sendCount = 10, bufferSize = 4096;

            IPAddress localAddress = IPAddress.Any;

            SocketType sockType = SocketType.Stream;

            ProtocolType sockProtocol = ProtocolType.Tcp;



            Console.WriteLine();

            usage();

            Console.WriteLine();



            // Parse the command line

            for (int i = 0; i < args.Length; i++)

            {

                try

                {

                    if ((args[i][0] == '-') || (args[i][0] == '/'))

                    {

                        switch (Char.ToLower(args[i][1]))

                        {

                            case 'l':       // Local interface to bind to

                                localAddress = IPAddress.Parse(args[++i]);

                                break;

                            case 'm':       // Text message to put into the send buffer

                                textMessage = args[++i];

                                break;

                            case 'n':       // Number of times to send the response

                                sendCount = System.Convert.ToInt32(args[++i]);

                                break;

                            case 'p':       // Port number for the destination

                                localPort = System.Convert.ToInt32(args[++i]);

                                break;

                            case 't':       // Specified TCP or UDP

                                i++;

                                if (String.Compare(args[i], "tcp", true) == 0)

                                {

                                    sockType = SocketType.Stream;

                                    sockProtocol = ProtocolType.Tcp;

                                }

                                else if (String.Compare(args[i], "udp", true) == 0)

                                {

                                    sockType = SocketType.Dgram;

                                    sockProtocol = ProtocolType.Udp;

                                }

                                else

                                {

                                    usage();

                                    return;

                                }

                                break;

                            case 'x':       // Size of the send and receive buffers

                                bufferSize = System.Convert.ToInt32(args[++i]);

                                break;

                            default:

                                usage();

                                return;

                        }

                    }

                }

                catch

                {

                    usage();

                    return;

                }

            }



            Socket serverSocket = null;



            try

            {

                IPEndPoint localEndPoint = new IPEndPoint(localAddress, localPort), senderAddress = new IPEndPoint(localAddress, 0);

                Console.WriteLine("Server: IPEndPoint is OK...");

                EndPoint castSenderAddress;

                Socket clientSocket;

                byte[] receiveBuffer = new byte[bufferSize], sendBuffer = new byte[bufferSize];

                int rc;



                FormatBuffer(sendBuffer, textMessage);



                // Create the server socket

                serverSocket = new Socket(localAddress.AddressFamily, sockType, sockProtocol);



                Console.WriteLine("Server: Socket() is OK...");

                // Bind the socket to the local interface specified

                serverSocket.Bind(localEndPoint);



                Console.WriteLine("Server: {0} server socket bound to {1}", sockProtocol.ToString(), localEndPoint.ToString());



                if (sockProtocol == ProtocolType.Tcp)

                {

                    // If TCP socket, set the socket to listening

                    serverSocket.Listen(1);

                    Console.WriteLine("Server: Listen() is OK, I'm listening for connection buddy!");

                }

                else

                {

                    byte[] byteTrue = new byte[4];











                    // Set the SIO_UDP_CONNRESET ioctl to true for this UDP socket. If this UDP socket

                    //    ever sends a UDP packet to a remote destination that exists but there is

                    //    no socket to receive the packet, an ICMP port unreachable message is returned

                    //    to the sender. By default, when this is received the next operation on the

                    //    UDP socket that send the packet will receive a SocketException. The native

                    //    (Winsock) error that is received is WSAECONNRESET (10054). Since we don't want

                    //    to wrap each UDP socket operation in a try/except, we'll disable this error

                    //    for the socket with this ioctl call.

                    byteTrue[byteTrue.Length - 1] = 1;

                    serverSocket.IOControl(TCPListener.SIO_UDP_CONNRESET, byteTrue, null);

                    Console.WriteLine("Server: IOControl() is OK...");

                }



                // Service clients in a loop

                while (true)

                {

                    if (sockProtocol == ProtocolType.Tcp)

                    {

                        // Wait for a client connection

                        clientSocket = serverSocket.Accept();

                        Console.WriteLine("Server: Accept() is OK...");

                        Console.WriteLine("Server: Accepted connection from: {0}", clientSocket.RemoteEndPoint.ToString());



                        // Receive the request from the client in a loop until the client shuts

                        //    the connection down via a Shutdown.

                        Console.WriteLine("Server: Preparing to receive using Receive()...");

                        while (true)

                        {

                            rc = clientSocket.Receive(receiveBuffer);



                            Console.WriteLine("Server: Read {0} bytes", rc);

                            if (rc == 0)

                                break;

                        }



                        // Send the indicated number of response messages

                        Console.WriteLine("Server: Preparing to send using Send()...");

                        for (int i = 0; i < sendCount; i++)

                        {

                            rc = clientSocket.Send(sendBuffer);

                            Console.WriteLine("Server: Sent {0} bytes", rc);

                        }



                        // Shutdown the client connection

                        clientSocket.Shutdown(SocketShutdown.Send);

                        Console.WriteLine("Server: Shutdown() is OK...");

                        clientSocket.Close();

                        Console.WriteLine("Server: Close() is OK...");

                    }

                    else

                    {

                        castSenderAddress = (EndPoint)senderAddress;



                        // Receive the initial request from the client

                        rc = serverSocket.ReceiveFrom(receiveBuffer, ref castSenderAddress);

                        Console.WriteLine("Server: ReceiveFrom() is OK...");

                        senderAddress = (IPEndPoint)castSenderAddress;

                        Console.WriteLine("Server: Received {0} bytes from {1}", rc, senderAddress.ToString());



                        // Send the response to the client the requested number of times

                        for (int i = 0; i < sendCount; i++)

                        {

                            try

                            {

                                rc = serverSocket.SendTo(sendBuffer, senderAddress);

                                Console.WriteLine("Server: SendTo() is OK...");

                            }

                            catch

                            {

                                // If the sender's address is being spoofed we may get an error when sending

                                //    the response. You can test this by using IPv6 and using the RawSocket

                                //    sample to spoof a UDP packet with an invalid link local source address.

                                continue;

                            }

                            Console.WriteLine("Server: Sent {0} bytes to {1}", rc, senderAddress.ToString());

                        }



                        // Send several zero byte datagrams to indicate to client that no more data

                        //    will be sent from the server. Multiple packets are sent since UDP

                        //    is not guaranteed and we want to try to make an effort the client

                        //    gets at least one.

                        Console.WriteLine("Server: Preparing to send using SendTo(), on the way do sleeping, Sleep(250)...");

                        for (int i = 0; i < 3; i++)

                        {

                            serverSocket.SendTo(sendBuffer, 0, 0, SocketFlags.None, senderAddress);

                            // Space out sending the zero byte datagrams a bit. UDP is unreliable and

                            //   the local stack can even drop them before even hitting the wire!

                            System.Threading.Thread.Sleep(250);

                        }

                    }

                }

            }

            catch (SocketException err)

            {

                Console.WriteLine("Server: Socket error occurred: {0}", err.Message);

            }

            finally

            {

                // Close the socket if necessary

                if (serverSocket != null)

                {

                    Console.WriteLine("Server: Closing using Close()...");

                    serverSocket.Close();

                }

            }

        }

    }

}


