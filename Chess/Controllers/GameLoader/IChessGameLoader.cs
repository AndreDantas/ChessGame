using Chess.Controllers.Configuration;
using Chess.Models.Board;

namespace Chess.Controllers.GameLoader
{
    public interface IChessGameLoader<T> where T : GameConfiguration, new()
    {
        Chessboard CreateBoard(T config);
    }
}