using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    public class MarakeshModelController
    {
        private GameObject marakeshGameObject;
        public Action move_finished;
        public Tile currentTile;
        public LookingSide lookingSide;
        public MarakeshModelController(GameObject marakeshGameObject, Tile tile)
        {
            this.marakeshGameObject = Object.Instantiate(marakeshGameObject);
            this.marakeshGameObject.transform.position = tile.posV3;
            currentTile = tile;
            lookingSide = LookingSide.up;
        }

        public void Rotate()
        {
            marakeshGameObject.transform.rotation = Quaternion.Euler(0, 90, 0) * marakeshGameObject.transform.rotation;
            lookingSide = LookSide.NextLookingSide(lookingSide);
        }

        public void Move(Tile tile)
        {
            marakeshGameObject.transform.position = tile.posV3;
            move_finished();
            currentTile = tile;
        }
    }
}
