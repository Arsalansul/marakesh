using UnityEngine;

namespace Assets.Scripts
{
    class GameController : MonoBehaviour
    {
        public Color[] colors = new Color[4];
        public Player[] players = new Player[4];

        private Map map;
        private int current_player_id = 0;

        private Camera mainCamera;

        private SelectionOrientaion selectionOrientaion;

        void Start()
        {
            mainCamera = Camera.main;
            map = new Map();

            selectionOrientaion = SelectionOrientaion.Horizontal;
        }

        void Update()
        {
            if (Input.GetMouseButtonUp(1))
            {
                selectionOrientaion++;
                if ((int) selectionOrientaion == System.Enum.GetValues(typeof(SelectionOrientaion)).Length)
                    selectionOrientaion = 0;
            }

            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                map.SetTilesOutLine(hit.point, colors[current_player_id], selectionOrientaion, true);


                if (Input.GetMouseButtonUp(0))
                {
                    map.SetTilesColor(hit.point, colors[current_player_id]);
                    map.SetTilesOutLine(hit.point, colors[current_player_id], selectionOrientaion, false);

                    current_player_id++;
                    if (current_player_id == players.Length)
                        current_player_id = 0;
                }
            }
        }
    }
}
