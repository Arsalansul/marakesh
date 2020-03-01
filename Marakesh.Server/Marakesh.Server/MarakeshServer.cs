using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Marakesh.Common;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Marakesh.Server.Model;
using Newtonsoft.Json;
using UnityEngine;

namespace Marakesh.Server
{

    public class MarakeshServer
    {
        private TcpListener _tcpListener;
        private const int _maxConnections = 10;

        public string IpAddress { get; }

        public MarakeshServer()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                throw new IOException("No networks available");
            var ipAddress = GetLocalIpAddress();
            Debug.Log($"ip address {ipAddress}");
            IpAddress = ipAddress;

            _tcpListener = new TcpListener(IPAddress.Any, Constants.DefaultPort);
            _tcpListener.Start(_maxConnections);
        }

        public void StartServer(CancellationToken token)
        {
            //TODO: AggregateException handling here
            Task.Run(() =>
            {
                ProcessServerLoop(_tcpListener, token);
            }, token);
        }

        private void ProcessServerLoop(TcpListener server, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Debug.Log("Ожидание подключений... ");

                try
                {
                    using (TcpClient client = server.AcceptTcpClient())
                    {
                        Debug.Log("Подключен клиент. Выполнение запроса...");

                        using (NetworkStream stream = client.GetStream())
                        {
                            var firstRequestSize = FirstRequest.GetSize();
                            var buffer = new byte[firstRequestSize];

                            stream.Read(buffer, 0, buffer.Length);
                            string result = System.Text.Encoding.UTF8.GetString(buffer);
                            var firstRequest = JsonConvert.DeserializeObject<FirstRequest>(result);

                            var response = new Response();

                            switch (firstRequest.RequestType)
                            {
                                case RequestType.PlayerCount:
                                    response.PlayerCount = 4;
                                    break;
                                default:
                                    Debug.Assert(false);
                                    Debug.Break();
                                    break;
                            }

                            var responseJson = JsonUtility.ToJson(response);
                            var responseData = Encoding.UTF8.GetBytes(responseJson);
                            Debug.Log($"Отправлено сообщение: {responseData}");
                            stream.Write(responseData, 0, responseData.Length);

                            Debug.Log($"Отправлено сообщение: {responseData}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log($"exception on server: {ex}");
                    throw;
                }
                finally
                {
                    //TODO: check that does not block. I know it is not, but check
                    Thread.Sleep(1000);
                }
                
            }
        }


        private static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new IOException("No network adapters with an IPv4 address in the system!");
        }
    }

}
