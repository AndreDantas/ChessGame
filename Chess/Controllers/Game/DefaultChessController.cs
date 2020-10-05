using Chess.Controllers.Configuration;
using Chess.Controllers.GameLoader;
using Chess.Models.Board;
using Chess.Models.Classes;
using Chess.Models.Game;
using Chess.Models.Game.Action;
using Chess.Models.Pieces;
using Chess.Utility;
using System.Collections.Generic;

namespace Chess.Controllers.Game
{
    public struct PlayerInfo
    {
        public Player player;
        public Dictionary<ChessPiece, List<Move>> moves;
    }

    public enum GameState
    {
        InProgress,
        Checkmate,
        Stalemate
    }

    public class DefaultChessController : ChessController
    {
        public Player White;
        public Player Black;
        public PlayerInfo CurrentPlayerInfo { get; protected set; }
        public GameState State { get; protected set; } = GameState.InProgress;
        public Chessboard CurrentBoardCopy => new Chessboard(this.Board);

        private DefaultChessController()
        {
        }

        public static DefaultChessController Create(DefaultChessConfiguration config)
        {
            DefaultChessController controller = new DefaultChessController();
            DefaultChessLoader loader = new DefaultChessLoader();

            controller.Board = loader.CreateBoard(config);
            controller.White = config.White;
            controller.Black = config.Black;
            controller.CurrentPlayerInfo = new PlayerInfo { player = config.White, moves = controller.Board.GetAllMoves(config.White) };

            return controller;
        }

        public List<Move> GetPieceMoves(ChessPiece piece)
        {
            return CurrentPlayerInfo.moves[piece];
        }

        public bool PlayerMove(Player player, ChessPiece piece, Position endPosition)
        {
            if (State != GameState.InProgress || player != CurrentPlayerInfo.player)
                return false;

            var moves = CurrentPlayerInfo.moves[piece];

            if (moves?.Count == 0)
                return false;

            var move = moves.Search(m => m.EndPosition == endPosition);

            if (EqualityComparer<Move>.Default.Equals(move, default))
                return false;

            if (!ExecuteAction(move))
                return false;

            NextTurn();

            return true;
        }

        protected bool ExecuteAction(IChessAction action)
        {
            try
            {
                Board.ExecuteAction(action);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected void NextTurn()
        {
            var otherPlayer = CurrentPlayerInfo.player == White ? Black : White;
            CurrentPlayerInfo = new PlayerInfo { player = otherPlayer, moves = CalculatePlayerMoves(otherPlayer) };

            UpdateGameState();

            if (State != GameState.InProgress)
                EndGame();
        }

        protected void EndGame()
        {
        }

        protected virtual void UpdateGameState()
        {
            bool currentPlayerHasMoves = false;
            foreach (var value in CurrentPlayerInfo.moves.Values)
            {
                if (value.Count > 0)
                {
                    currentPlayerHasMoves = true;
                    break;
                }
            }

            if (!currentPlayerHasMoves)
            {
                var otherPlayer = CurrentPlayerInfo.player == White ? Black : White;
                if (Board.IsKingInCheck(otherPlayer))
                    State = GameState.Checkmate;
                else
                    State = GameState.Stalemate;
            }
            else
            {
                State = GameState.InProgress;
            }
        }

        protected virtual Dictionary<ChessPiece, List<Move>> CalculatePlayerMoves(Player player)
        {
            var movesDict = Board.GetAllMoves(player);
            var otherPlayer = player == White ? Black : White;

            var temp_board = new Chessboard(Board);

            foreach (var dict in movesDict)
            {
                var moveList = dict.Value;
                var piece = dict.Key;

                for (int i = moveList.Count - 1; i >= 0; i--)
                {
                    temp_board.ExecuteAction(moveList[i]);

                    if (temp_board.IsKingInCheck(otherPlayer))
                        movesDict.Remove(piece);

                    temp_board.NavigateBack();
                }
            }

            return movesDict;
        }
    }
}