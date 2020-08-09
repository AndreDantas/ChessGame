using Chess.Models.Classes;
using Chess.Models.Exceptions;
using Chess.Models.Game;
using Chess.Models.Game.Action;
using Chess.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Models.Board
{
    public class Chessboard : Board, IEquatable<Chessboard>
    {
        public BoardState BoardState;
        public Stack<IChessAction> ActionHistory = new Stack<IChessAction>();
        public Stack<IChessAction> ActionCache = new Stack<IChessAction>();

        public Stack<Piece> RemovedPieces = new Stack<Piece>();

        private Chessboard()
        {
            BoardState = new BoardState(Width, Height);
        }

        public Chessboard(int width, int height) : base(width, height)
        {
            BoardState = new BoardState(Width, Height);
        }

        public Chessboard(Chessboard other)
        {
            Width = other.Width;
            Height = other.Height;
            ActionHistory = other.ActionHistory;

            foreach (var piece in other.RemovedPieces.Reverse())
                RemovedPieces.Push(piece.Clone());

            BoardState = new BoardState(other.BoardState);
        }

        public Chessboard(BoardState boardState)
        {
            if (boardState == null)
                BoardState = new BoardState(Width, Height);
            else
            {
                BoardState = new BoardState(boardState);
                Width = boardState.Width;
                Height = boardState.Height;
            }
        }

        public Chessboard ExecuteAction(IChessAction action)
        {
            try
            {
                action.Execute(this);
                ActionHistory.Push(action);
                ActionCache.Clear();
            }
            catch (Exception e)
            {
                throw new InvalidActionException("Invalid action: " + e.Message, e);
            }

            return this;
        }

        public Chessboard NavigateBack()
        {
            try
            {
                if (ActionHistory.Count == 0)
                    return this;

                ActionCache.Push(ActionHistory.Pop());
                ActionCache.Peek().Revert(this);
            }
            catch (Exception e)
            {
                throw new InvalidActionException("Invalid action: " + e.Message, e);
            }

            return this;
        }

        public Chessboard NavigateForward()
        {
            try
            {
                if (ActionCache.Count == 0)
                    return this;

                ActionHistory.Push(ActionCache.Pop());
                ActionHistory.Peek().Execute(this);
            }
            catch (Exception e)
            {
                throw new InvalidActionException("Invalid action: " + e.Message, e);
            }

            return this;
        }

        public Tile GetTile(Position pos)
        {
            if (BoardState.ValidIndex(pos.x, pos.y))
                return BoardState[pos.x, pos.y];

            return null;
        }

        public List<Piece> GetAllPieces(Player player = default)
        {
            var pieces = new List<Piece>();

            for (int i = 0; i < BoardState.Width; i++)
            {
                for (int j = 0; j < BoardState.Height; j++)
                {
                    var piece = BoardState[i, j].Piece;

                    if (piece != null && (piece.Player == player || string.IsNullOrEmpty(player.Id)))
                        pieces.Add(piece);
                }
            }

            return pieces;
        }

        public Dictionary<Piece, List<Move>> GetAllMoves(Player player = default)
        {
            var movesDict = new Dictionary<Piece, List<Move>>();
            var pieces = GetAllPieces(player);

            foreach (var piece in pieces)
            {
                movesDict[piece] = piece.GetMoves(this);
            }

            return movesDict;
        }

        public bool IsKingInCheck(Player opposingPlayer)
        {
            var movesDict = GetAllMoves(opposingPlayer);

            foreach (var dict in movesDict)
            {
                foreach (var move in dict.Value)
                {
                    foreach (var capture in move.Captures)
                    {
                        var tile = GetTile(capture);
                        if (tile?.Piece is King)
                            return true;
                    }
                }
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Chessboard);
        }

        public bool Equals(Chessboard other)
        {
            return other != null &&
                   Width == other.Width &&
                   Height == other.Height &&
                   EqualityComparer<BoardState>.Default.Equals(BoardState, other.BoardState) &&
                   EqualityComparer<Stack<IChessAction>>.Default.Equals(ActionHistory, other.ActionHistory) &&
                   EqualityComparer<Stack<IChessAction>>.Default.Equals(ActionCache, other.ActionCache) &&
                   EqualityComparer<Stack<Piece>>.Default.Equals(RemovedPieces, other.RemovedPieces);
        }

        public override int GetHashCode()
        {
            int hashCode = -399849685;
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<BoardState>.Default.GetHashCode(BoardState);
            hashCode = hashCode * -1521134295 + EqualityComparer<Stack<IChessAction>>.Default.GetHashCode(ActionHistory);
            hashCode = hashCode * -1521134295 + EqualityComparer<Stack<IChessAction>>.Default.GetHashCode(ActionCache);
            hashCode = hashCode * -1521134295 + EqualityComparer<Stack<Piece>>.Default.GetHashCode(RemovedPieces);
            return hashCode;
        }

        public static bool operator ==(Chessboard left, Chessboard right)
        {
            return EqualityComparer<Chessboard>.Default.Equals(left, right);
        }

        public static bool operator !=(Chessboard left, Chessboard right)
        {
            return !(left == right);
        }
    }
}