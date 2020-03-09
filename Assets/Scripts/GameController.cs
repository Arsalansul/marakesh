using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Marakesh.Common;
using Marakesh.Server;

namespace Assets.Scripts
{
    class GameController : MonoBehaviour
    {
        private const int playersCount = 4;
        public Color[] colors = new Color[playersCount];
        public Player[] players = new Player[playersCount];

        public GameObject marakeshModel;
        private MarakeshModelController marakeshModelController;

        private Map map;

        private int current_player_id = 0;
        private int my_player_id;

        private Camera mainCamera;

        private GameState currentState;
        private int gameStateLength;

        private SelectionOrientation selectionOrientation;
        private int orientationLength;

        private Marakesh.Common.IMarakeshClient marakeshClient;

        private Marakesh.Server.MarakeshServer marakeshServer;

        void Start()
        {

            var serverCancellationTokenSource = new CancellationTokenSource(6000);
            marakeshServer = new MarakeshServer();

            //TODO: moe this to UI button
            Task.Run(() =>
            {
                marakeshServer.StartServer(serverCancellationTokenSource.Token);
                Debug.Log($" marakeshServer.StartServer ");
            });

            marakeshClient = new MarakeshServerClient(marakeshServer.IpAddress);
            var clientCancellationTokenSource = new CancellationTokenSource(3000);
            
            //TODO: moe this to UI button
            Task.Run(async () =>
            {
                var playerCount = await marakeshClient.GetPlayerCount(clientCancellationTokenSource.Token);
                Debug.Log($" playerCount {playerCount}");
            });
            //TODO: catch exceptions


            mainCamera = Camera.main;
            map = new Map();

            selectionOrientation = SelectionOrientation.Horizontal;

            orientationLength = System.Enum.GetValues(typeof(SelectionOrientation)).Length;

            marakeshModelController = new MarakeshModelController(marakeshModel, map.GetTile(new Vector2Int(map.size / 2, map.size / 2)));

            currentState = GameState.Move;
            gameStateLength = System.Enum.GetValues(typeof(GameState)).Length;

            marakeshModelController.move_finished += GoToNextGameState;
        }

        void Update()
        {
            if (my_player_id != current_player_id)
                return;

            switch (currentState)
            {
                case GameState.Move:
                    MovingMarakesh();
                    return;
                case GameState.Tiling:
                    Tiling();
                    return;
            }
        }

        private void Tiling()
        {
            if (Input.GetMouseButtonUp(1))
            {
                selectionOrientation++;
                if ((int)selectionOrientation == orientationLength)
                    selectionOrientation = 0;
            }

            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                map.SetTilesOutLine(hit.point, colors[current_player_id], selectionOrientation, true);


                if (Input.GetMouseButtonUp(0) && map.outlinedTiles.Count >=2)
                { 
                    marakeshServer.SetLastCarpetPosition(map.outlinedTiles.Select(t => Array.IndexOf(map.tiles,t)).ToList());
                    map.SetTilesColor(hit.point, colors[current_player_id]);
                    map.SetTilesOutLine(hit.point, colors[current_player_id], selectionOrientation, false);
                    GoToNextGameState();
                    marakeshServer.EndTurn();
                }
            }
        }

        private void MovingMarakesh()
        {
            if (Input.GetMouseButtonUp(1))
            {
                marakeshModelController.Rotate();
            }

            if (Input.GetMouseButtonUp(0))
            {
                var nextPos = map.GetNextTile(marakeshModelController.currentTile, ref marakeshModelController.lookingSide);
                marakeshModelController.Move(nextPos);
                marakeshModelController.CheckLookingSide();

                marakeshServer.SetMarkeshPosition(Array.IndexOf(map.tiles, nextPos), marakeshModelController.lookingSide);
                map.GetNeighbourTiles(nextPos);
            }
        }

        private void GoToNextGameState()
        {
            currentState++;
            if ((int)currentState == gameStateLength)
                currentState = 0;
        }
    }
}
