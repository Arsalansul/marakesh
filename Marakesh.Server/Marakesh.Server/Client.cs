using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Marakesh.Server.Model;
using UnityEngine;

namespace Marakesh.Server
{
    public class TestClient 
    {
        const int port = 8888; // порт для прослушивания подключений

        private void Start_client()
        {
            Debug.Log("client started");
            try
            {
                TcpClient client = new TcpClient();
                client.Connect("127.0.0.1", port);

                // byte[] data = new byte[256];
                StringBuilder response = new StringBuilder();
                NetworkStream stream = client.GetStream();

                var ourTurn = new Turn();

                var newTurnData = JsonUtility.ToJson(ourTurn);

                // преобразуем сообщение в массив байтов
                byte[] data = Encoding.UTF8.GetBytes(newTurnData);

                // отправка сообщения
                stream.Write(data, 0, data.Length);

                Thread.Sleep(1000);

                do
                {
                    int bytes = stream.Read(data, 0, data.Length);
                    response.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable); // пока данные есть в потоке

                Debug.Log(response.ToString());

                // Закрываем потоки
                stream.Close();
                client.Close();
            }
            catch (SocketException e)
            {
                Debug.Log("SocketException: " + e);
            }
            catch (Exception e)
            {
                Debug.Log("Exception: " + e.Message);
            }

            Debug.Log("Запрос завершен...");
            //Console.Read();
        }
    }
}
