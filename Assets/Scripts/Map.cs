using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Map
    {
        public int size = 7;
        public Tile[] tiles;

        public OutlinedTiles outlinedTiles;

        public Map()
        {
            tiles = new Tile[size * size];
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new Tile(GetTilePos(i), new Vector2Int(i % size, i/size));
            }

            outlinedTiles = new OutlinedTiles();
            Camera.main.transform.position = new Vector3(size / 2f * Tile.size, 7, size / 2f * Tile.size);
        }

        public void SetTileColor(Vector3 worldPos, Color color)
        {
            var tile = GetTileByPosition(worldPos);
            tile.SetColor(color);
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
            if (worldPos.z > Tile.size * (size - 0.5f))
                worldPos.z = Tile.size * (size - 0.5f);
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
            return GetTilePos(new Vector2(tileIndex % size, tileIndex / size));
        }

        public Vector3 GetTilePos(Vector2 posV2)
        {
            return new Vector3(Tile.size * (posV2.x + 0.5f), 0, Tile.size * (posV2.y + 0.5f));
        }

        public Tile GetTile(Vector2Int posV2)
        {
            return tiles[posV2.x + posV2.y * size];
        }
        public void SetTilesOutLine(Vector3 worldPos, Color color, SelectionOrientation orientation, bool activate)
        {
            outlinedTiles.Clear();
            if (!activate)
                return;

            foreach (var selectedTile in GetSelectedTiles(worldPos, orientation))
            {
                outlinedTiles.Add(selectedTile);
            }
           
        }

        public IEnumerable<Tile> GetSelectedTiles(Vector3 worldPos, SelectionOrientation orientation)
        {
            switch (orientation)
            {
                case SelectionOrientation.Horizontal:
                    yield return GetTileByPosition(worldPos + Vector3.right * Tile.size/2);
                    yield return GetTileByPosition(worldPos - Vector3.right * Tile.size/2);
                    break;
                case SelectionOrientation.Vertival:
                    yield return GetTileByPosition(worldPos + Vector3.forward * Tile.size/2);
                    yield return GetTileByPosition(worldPos - Vector3.forward * Tile.size/2);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        public Tile GetNextTile(Tile currentTile, ref LookingSide lookingSide)
        {
            var tilePosV2 = currentTile.position + GetDirection(lookingSide);
            if (tilePosV2.x >= size)
            {
                lookingSide = LookSide.NextLookingSide(lookingSide);
                tilePosV2 = currentTile.position + GetDirection(lookingSide);
                lookingSide = LookSide.NextLookingSide(lookingSide);
            }
            if (tilePosV2.x < 0)
            {
                lookingSide = LookSide.NextLookingSide(lookingSide);
                tilePosV2 = currentTile.position + GetDirection(lookingSide);
                lookingSide = LookSide.NextLookingSide(lookingSide);
            }
            if (tilePosV2.y >= size)
            {
                lookingSide = LookSide.NextLookingSide(lookingSide);
                tilePosV2 = currentTile.position + GetDirection(lookingSide);
                lookingSide = LookSide.NextLookingSide(lookingSide);
            }
            if (tilePosV2.y < 0)
            {
                lookingSide = LookSide.NextLookingSide(lookingSide);
                tilePosV2 = currentTile.position + GetDirection(lookingSide);
                lookingSide = LookSide.NextLookingSide(lookingSide);
            }
            return GetTile(tilePosV2);
        }

        private Vector2Int GetDirection(LookingSide lookingSide)
        {
            switch (lookingSide)
            {
                case LookingSide.up:
                    return new Vector2Int(0, 1);
                case LookingSide.right:
                    return new Vector2Int(1, 0);
                case LookingSide.down:
                    return new Vector2Int(0, -1);
                case LookingSide.left:
                    return new Vector2Int(-1, 0);
                default:
                    return new Vector2Int(0, 1);
            }
        }
    }
}
