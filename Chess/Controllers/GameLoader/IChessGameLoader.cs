using Chess.Controllers.Configuration;
using Chess.Models.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Controllers.GameLoader
{
    public interface IChessGameLoader<T> where T : GameConfiguration, new()
    {
        Chessboard CreateBoard(T config);
    }
}