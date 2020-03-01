﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public interface IMarakeshServer
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
