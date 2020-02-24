using UnityEngine;

namespace Assets.Scripts
{
    public class Tile
    {
        public static int size = 1;
        public GameObject tile;
        
        private Material material;

        public bool Outline
        {
            set
            {
                if (value)
                {
                    SetColor(Color.black);
                    return;
                }

                SetColor(Color.white);

            }
        }

        public Tile(Vector3 pos)
        {
            tile = GameObject.CreatePrimitive(PrimitiveType.Quad);
            tile.transform.position = pos;
            tile.transform.rotation = Quaternion.Euler(90, 0, 0);
            material = tile.transform.GetComponent<Renderer>().material;
        }

        public void SetColor(Color color)
        {
            material.color = color;
        }


    }
}
