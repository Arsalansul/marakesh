using cakeslice;
using UnityEngine;

namespace Assets.Scripts
{
    public class Tile
    {
        public static int size = 1;
        public GameObject tile;
        
        private Material material;
        private Outline outline;

        public Vector2Int position { get; }
        public Vector3 posV3 { get; }
        public bool Outline
        {
            set => outline.enabled = value;
        }

        public Tile(Vector3 pos, Vector2Int posV2)
        {
            tile = GameObject.CreatePrimitive(PrimitiveType.Quad);
            tile.transform.position = pos;
            tile.transform.rotation = Quaternion.Euler(90, 0, 0);
            material = tile.transform.GetComponent<Renderer>().material;
            outline = tile.AddComponent<Outline>();
            outline.enabled = false;
            posV3 = pos;
            position = posV2;
        }

        public void SetColor(Color color)
        {
            material.color = color;
        }
    }
}
