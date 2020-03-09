using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts;

namespace Marakesh.Common
{
    public interface IMarakeshClient
    {
        Task<int> GetPlayerCount(CancellationToken token);
        int GetActivePlayerId();

        Action activePlayerChanged { get; set; }
        int GetMyPlayerID();
        (int, LookingSide) GetMarkeshPosition();
        void SetMarkeshPosition(int tile, LookingSide lookingSide);

        List<int> GetLastCarpetPosition();
        void SetLastCarpetPosition(List<int> positions);

        void EndTurn();
    }
}
