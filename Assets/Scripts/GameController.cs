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

        void Start()
        {
            mainCamera = Camera.main;
            map = new Map();
        }

        void Update()
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                map.SetTileOutLine(hit.point, colors[current_player_id], true);


                if (Input.GetMouseButtonUp(0))
                {
                    map.SetTileOutLine(hit.point, colors[current_player_id], false);
                    map.SetTileColor(hit.point, colors[current_player_id]);
                    current_player_id++;
                    if (current_player_id == players.Length)
                        current_player_id = 0;
                }
            }
        }
    }
}
