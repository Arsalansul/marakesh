using UnityEngine;

namespace Assets.Scripts
{
    class GameController : MonoBehaviour
    {
        public Color[] colors = new Color[4];
        public Player[] players = new Player[4];

        public GameObject marakeshModel;
        private MarakeshModelController marakeshModelController;

        private Map map;
        private int current_player_id = 0;

        private Camera mainCamera;

        private GameState currentState;
        private int gameStateLength;

        private SelectionOrientation selectionOrientation;
        private int orientationLength;
        void Start()
        {
            mainCamera = Camera.main;
            map = new Map();

            selectionOrientation = SelectionOrientation.Horizontal;

            orientationLength = System.Enum.GetValues(typeof(SelectionOrientation)).Length;

            marakeshModelController = new MarakeshModelController(marakeshModel, map.GetTile(new Vector2Int(map.size / 2, map.size / 2)));

            currentState = GameState.Move;
            gameStateLength = System.Enum.GetValues(typeof(GameState)).Length;

            marakeshModelController.move_finished += NextGameState;
        }

        void Update()
        {
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


                if (Input.GetMouseButtonUp(0))
                {
                    map.SetTilesColor(hit.point, colors[current_player_id]);
                    map.SetTilesOutLine(hit.point, colors[current_player_id], selectionOrientation, false);

                    NextPlayer();
                    NextGameState();
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
                marakeshModelController.Move(map.GetNextTile(marakeshModelController.currentTile, ref marakeshModelController.lookingSide));
                marakeshModelController.CheckLookingSide();
            }
        }

        private void NextGameState()
        {
            currentState++;
            if ((int)currentState == gameStateLength)
                currentState = 0;
        }

        private void NextPlayer()
        {
            current_player_id++;
            if (current_player_id == players.Length)
                current_player_id = 0;
        }
    }
}
