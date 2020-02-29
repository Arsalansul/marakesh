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

        private Quaternion startRot;
        public MarakeshModelController(GameObject marakeshGameObject, Tile tile)
        {
            this.marakeshGameObject = Object.Instantiate(marakeshGameObject);
            this.marakeshGameObject.transform.position = tile.posV3;
            currentTile = tile;
            lookingSide = LookingSide.up;
            startRot = marakeshGameObject.transform.rotation;
        }

        public void Rotate()
        {
            lookingSide = LookSide.NextLookingSide(lookingSide);
            CheckLookingSide();
        }

        public void Move(Tile tile)
        {
            marakeshGameObject.transform.position = tile.posV3;
            move_finished();
            currentTile = tile;
        }

        public void CheckLookingSide()
        {
            marakeshGameObject.transform.rotation = Quaternion.Euler(0, 90 * (int)lookingSide, 0) * startRot;
        }
    }
}
