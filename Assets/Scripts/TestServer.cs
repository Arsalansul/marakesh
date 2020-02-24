using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class TestServer : MonoBehaviour
    {
        const int port = 8888; // порт для прослушивания подключений
        void Start()
        {
            Screen.SetResolution(640, 480, false);
        }

        void OnGUI()
        {
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 - 25, 100, 50), "Start server"))
            {
                try
                {
                    var tokenSource = new CancellationTokenSource();
                    //tokenSource.Cancel();
                    Start_server(tokenSource.Token);

                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
                finally
                {

                }
            }
        }

        private void Start_server(CancellationToken token)
        {
            Debug.Log("server started");
            TcpListener server = null;

            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(localAddr, port);

            // запуск слушателя
            server.Start();
            Task.Run(() =>
            {
                ProcessServerLoop(server, token);
            });
        }

        private void ProcessServerLoop(TcpListener server, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Debug.Log("Ожидание подключений... ");

                TcpClient client = server.AcceptTcpClient();
                Debug.Log("Подключен клиент. Выполнение запроса...");

                // получаем сетевой поток для чтения и записи
                NetworkStream stream = client.GetStream();

                // сообщение для отправки клиенту
                string response = "Привет мир";
                // преобразуем сообщение в массив байтов
                byte[] data = Encoding.UTF8.GetBytes(response);

                // отправка сообщения
                stream.Write(data, 0, data.Length);
                Debug.Log("Отправлено сообщение: " + response);
                // закрываем поток
                stream.Close();
                client.Close();

                Thread.Sleep(1000);
            }
        }
    }
}
