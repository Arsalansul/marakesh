using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Map
    {
        public int size = 7;
        public Tile[] tiles;

        private OutlinedTiles outlinedTiles;

        public Map()
        {
            tiles = new Tile[size * size];
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new Tile(GetTilePos(i));
            }

            outlinedTiles = new OutlinedTiles();
            Camera.main.transform.position = new Vector3(size / 2f * Tile.size, 7, size / 2f * Tile.size);
        }

        public void SetTilesColor(Vector3 worldPos, Color color)
        {
            foreach (var tile in outlinedTiles)
            {
                tile.SetColor(color);
            }
        }

        private Tile GetTileByPosition(Vector3 worldPos)
        {
            if (worldPos.x > Tile.size * size)
                worldPos.x = Tile.size * (size - 0.5f);
            if (worldPos.z > Tile.size * size)
                worldPos.z = Tile.size * size;
            if (worldPos.x < 0)
                worldPos.x = 0;
            if (worldPos.z < 0)
                worldPos.z = 0;

            var x = (int)worldPos.x / Tile.size;
            var z = (int)worldPos.z / Tile.size;
            return tiles[z * size + x];
        }

        private Vector3 GetTilePos(int tileIndex)
        {
            return new Vector3(Tile.size * (tileIndex % size + 0.5f), 0, Tile.size * (tileIndex / size + 0.5f));
        }

        public void SetTilesOutLine(Vector3 worldPos, Color color, bool activate)
        {
            outlinedTiles.Clear();
            foreach (var selectedTile in GetSelectedTiles(worldPos, SelectionOrientaion.Horizontal))
            {
                outlinedTiles.Add(selectedTile);
            }
           
        }

        public IEnumerable<Tile> GetSelectedTiles(Vector3 worldPos, SelectionOrientaion orientaion)
        {
            switch (orientaion)
            {
                case SelectionOrientaion.Horizontal:
                    yield return GetTileByPosition(worldPos + Vector3.right * Tile.size/2);
                    yield return GetTileByPosition(worldPos - Vector3.right * Tile.size/2);
                    break;
                case SelectionOrientaion.Vertival:
                    yield return GetTileByPosition(worldPos + Vector3.up * Tile.size/2);
                    yield return GetTileByPosition(worldPos - Vector3.down * Tile.size/2);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }

    
}
