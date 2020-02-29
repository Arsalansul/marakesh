using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class FakeMarakeshServer: IMarakeshServer
    {
        private int activePlayerID = 0;
        private readonly int playersCount;
        private (int, LookingSide) MarkeshPosition;
        private List<int> LastCarpetPosition;

        private int id = 0;

        public FakeMarakeshServer(int playersCount)
        {
            this.playersCount = playersCount;
        }
        public int GetPlayerCount()
        {
            return playersCount;
        }

        public int GetActivePlayerId()
        {
            return activePlayerID;
        }

        public Action activePlayerChanged { get; set; }

        public int GetMyPlayerID()
        {
            return id++;
        }

        public (int, LookingSide) GetMarkeshPosition()
        {
            return MarkeshPosition;
        }

        public void SetMarkeshPosition(int tile, LookingSide lookingSide)
        {
            MarkeshPosition = (tile, lookingSide);
        }

        public List<int> GetLastCarpetPosition()
        {
            return LastCarpetPosition;
        }

        public void SetLastCarpetPosition(List<int> positions)
        {
            LastCarpetPosition = positions;
        }

        public void EndTurn()
        {
            activePlayerID++;
            if (activePlayerID == playersCount)
                activePlayerID = 0;
            activePlayerChanged();
        }
    }
}
