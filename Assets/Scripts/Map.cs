using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Map
    {
        public int size = 7;
        public Tile[] tiles;

        public OutlinedTiles outlinedTiles;

        private HashSet<Tile> neighbourTiles;
        public Map()
        {
            tiles = new Tile[size * size];
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new Tile(GetTilePos(i), new Vector2Int(i % size, i/size));
            }

            outlinedTiles = new OutlinedTiles();
            neighbourTiles = new HashSet<Tile>();
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

        public Tile GetTileByPosition(Vector3 worldPos)
        {
            var x = (int)worldPos.x / Tile.size;
            var y = (int)worldPos.z / Tile.size;
            return GetTile(new Vector2Int(x,y));
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
            if (posV2.x >= size)
                posV2.x = size - 1;
            if (posV2.y >= size)
                posV2.y = size - 1;
            if (posV2.x < 0)
                posV2.x = 0;
            if (posV2.y < 0)
                posV2.y = 0;
            return tiles[posV2.x + posV2.y * size];
        }
        public void SetTilesOutLine(Vector3 worldPos, Color color, SelectionOrientation orientation, bool activate)
        {
            outlinedTiles.Clear();
            if (!activate)
                return;
            GetSelectedTiles(worldPos, orientation);
        }

        public void GetSelectedTiles(Vector3 worldPos, SelectionOrientation orientation)
        {
            Tile tile_0, tile_1;
            switch (orientation)
            {
                case SelectionOrientation.Horizontal:
                    tile_0 = GetTileByPosition(worldPos + Vector3.right * Tile.size / 2);
                    tile_1 = GetTileByPosition(worldPos - Vector3.right * Tile.size / 2);
                    if (neighbourTiles.Contains(tile_0) || neighbourTiles.Contains(tile_1))
                    {
                        outlinedTiles.Add(tile_0);
                        outlinedTiles.Add(tile_1);
                    }
                    break;
                case SelectionOrientation.Vertival:
                    tile_0 = GetTileByPosition(worldPos + Vector3.forward * Tile.size / 2);
                    tile_1 = GetTileByPosition(worldPos - Vector3.forward * Tile.size / 2);
                    if (neighbourTiles.Contains(tile_0) || neighbourTiles.Contains(tile_1))
                    {
                        outlinedTiles.Add(tile_0);
                        outlinedTiles.Add(tile_1);
                    }
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

        public void GetNeighbourTiles(Tile tile)
        {
            neighbourTiles.Clear();
            var lookSide = LookingSide.up;
            for (int i = 0; i < 4; i++)
            {
                neighbourTiles.Add(GetTile(tile.position + GetDirection(lookSide)));
                lookSide = LookSide.NextLookingSide(lookSide);
            }
        }
    }
}
