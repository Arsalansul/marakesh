using System.Collections.Generic;

namespace Assets.Scripts
{
    public class OutlinedTiles : HashSet<Tile>
    {
        public new void Add(Tile item)
        {
            item.Outline = true;
            base.Add(item);
        }

        public new void Remove(Tile item)
        {
            item.Outline = false;
            base.Remove(item);
        }

        public new void Clear()
        {
            foreach (var outlinedTile in this)
            {
                outlinedTile.Outline = false;
            }
            base.Clear();
        }

    }
}
