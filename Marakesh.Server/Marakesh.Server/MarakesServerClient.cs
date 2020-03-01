using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts;
using Marakesh.Common;
using Marakesh.Server.Model;
using Newtonsoft.Json;
using UnityEngine;

namespace Marakesh.Server
{
    public class MarakeshServerClient : IMarakeshServer
    {
        private const int maxTries = 5;
        private const int _timeBetweenTries = 100;


        public static IPAddress ServerAddress { get; private set; } 

        public MarakeshServerClient(string ipAddress)
        {
            if(!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                throw new IOException("No netwrk available");
            if(!IPAddress.TryParse(ipAddress, out var address))
                throw new IOException($"Cannot parse address: {ipAddress}");
            ServerAddress = address;
        }

        public async Task<int> GetPlayerCount(CancellationToken token)
        {
            using (var client = new TcpClient())
            {
                client.Connect(ServerAddress, Constants.DefaultPort);

                using (var stream = client.GetStream())
                {
                    var firstRequest = new FirstRequest()
                    {
                        RequestType = RequestType.PlayerCount,
                    };
                    var firstRequestJson = JsonUtility.ToJson(firstRequest);
                    var firstRequestData = Encoding.UTF8.GetBytes(firstRequestJson);
                    await stream.WriteAsync(firstRequestData, 0, firstRequestData.Length, token);

                    var payerCountRequest = new PlayerCountRequest()
                    {
                        RequestType = RequestType.PlayerCount,
                    };
                    var payerCountRequestJson = JsonUtility.ToJson(payerCountRequest);
                    var payerCountRequestData = Encoding.UTF8.GetBytes(payerCountRequestJson);
                    await stream.WriteAsync(payerCountRequestData, 0, payerCountRequestData.Length, token);

                    var responseSize = Response.GetSize();
                    var responseData = new byte[responseSize];
                    int triesCount = 0;
                    bool success = false;
                    do
                    {
                        try
                        {
                            await Task.Delay(_timeBetweenTries, token);
                            stream.Read(responseData, 0, responseData.Length);
                            success = true;
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.ToString());
                            triesCount++;
                            success = false;
                        }
                        finally
                        {

                        }
                    }
                    while (!success || triesCount > maxTries);
                    
                    if(!success)
                        throw new IOException("cannot connect to server");

                    string responseJson = System.Text.Encoding.UTF8.GetString(responseData);
                    var response = JsonConvert.DeserializeObject<Response>(responseJson);

                    return response.PlayerCount;

                }
                
            }

            
            
        }

        public int GetActivePlayerId()
        {
            throw new NotImplementedException();
        }

        public Action activePlayerChanged { get; set; }

        public int GetMyPlayerID()
        {
            throw new NotImplementedException();
        }

        public (int, LookingSide) GetMarkeshPosition()
        {
            throw new NotImplementedException();
        }

        public void SetMarkeshPosition(int tile, LookingSide lookingSide)
        {
            throw new NotImplementedException();
        }

        public List<int> GetLastCarpetPosition()
        {
            throw new NotImplementedException();
        }

        public void SetLastCarpetPosition(List<int> positions)
        {
            throw new NotImplementedException();
        }

        public void EndTurn()
        {
            throw new NotImplementedException();
        }

        public static byte[] ReadFully(NetworkStream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (stream.DataAvailable)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
                return buffer;
            }
        }
    }
}
