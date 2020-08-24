using Chess.Models.Classes;
using Chess.Models.Exceptions;
using Chess.Models.Game;
using Chess.Models.Game.Action;
using Chess.Models.Pieces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Models.Board
{
    public class Chessboard : Board, IEquatable<Chessboard>
    {
        /// <summary>
        /// A struct that holds superficial information about a tile.
        /// </summary>
        public struct TileInfo
        {
            /// <summary>
            /// The position of the tile
            /// </summary>
            public Position pos;

            /// <summary>
            /// If the tile has a piece
            /// </summary>
            public bool hasPiece;

            public bool IsValid { get; private set; }

            public TileInfo(Position pos, bool hasPiece)
            {
                this.pos = pos;
                this.hasPiece = hasPiece;
                this.IsValid = true;
            }
        }

        [JsonProperty]
        protected BoardState BoardState;

        [JsonProperty]
        public Stack<IChessAction> ActionHistory { get; protected set; } = new Stack<IChessAction>();

        [JsonProperty]
        public Stack<IChessAction> ActionCache { get; protected set; } = new Stack<IChessAction>();

        [JsonProperty]
        public Stack<ChessPiece> RemovedPieces { get; protected set; } = new Stack<ChessPiece>();

        private Dictionary<Player, List<ChessPiece>> pieceLists;

        protected Dictionary<Player, List<ChessPiece>> PieceLists
        {
            get
            {
                if (pieceLists == null)
                {
                    pieceLists = new Dictionary<Player, List<ChessPiece>>
                    {
                        [default] = new List<ChessPiece>()
                    };
                    for (int i = 0; i < BoardState.Width; i++)
                    {
                        for (int j = 0; j < BoardState.Height; j++)
                        {
                            var piece = BoardState[i, j].Piece;

                            if (piece != null)
                            {
                                if (!pieceLists.ContainsKey(piece.Player))
                                    pieceLists[piece.Player] = new List<ChessPiece>();

                                pieceLists[piece.Player].Add(piece);
                                pieceLists[default].Add(piece);
                            }
                        }
                    }
                }

                return pieceLists;
            }
        }

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

        /// <summary>
        /// Adds a piece to the board.
        /// </summary>
        /// <param name="piece"> </param>
        /// <returns> </returns>
        /// <exception cref="NullPieceException"> </exception>
        /// <exception cref="InvalidPositionException"> </exception>
        public virtual ChessPiece AddPiece(ChessPiece piece)
        {
            if (piece == null)
                throw new NullPieceException("Can't add a null piece to the board");

            Position pos = piece.CurrentPosition;

            Tile tile = GetTile(pos);

            if (tile == null)
                throw new InvalidPositionException("Can't put piece on invalid position " + pos.ToString());

            if (tile.Piece != null)
                throw new InvalidPositionException("There's already a piece on this position " + pos.ToString());

            tile.Piece = piece;

            pieceLists = null;

            return piece;
        }

        /// <summary>
        /// Removes a piece from the board.
        /// </summary>
        /// <param name="piece"> </param>
        /// <returns> </returns>
        /// <exception cref="InvalidPositionException"> </exception>
        public virtual ChessPiece RemovePiece(Position pos)
        {
            Tile tile = GetTile(pos);

            if (tile == null)
                throw new InvalidPositionException("Can't remove from invalid position " + pos.ToString());

            if (tile.Piece == null)
                throw new InvalidPositionException("There's not a piece on this position " + pos.ToString());

            var piece = tile.Piece;

            tile.Piece = null;
            pieceLists = null;

            return piece;
        }

        /// <summary>
        /// Moves a piece on the board.
        /// </summary>
        /// <param name="piece"> </param>
        /// <returns> </returns>
        /// <exception cref="InvalidPositionException"> </exception>
        /// <exception cref="NullPieceException"> </exception>
        public virtual ChessPiece MovePiece(Position startPosition, Position endPosition)
        {
            var startTile = GetTile(startPosition);

            if (startTile == null)
                throw new InvalidPositionException("Can't move piece from invalid position " + startPosition.ToString());

            if (startTile.Piece == null)
                throw new NullPieceException("There's no piece in the start position " + startPosition.ToString());

            var endTile = GetTile(endPosition);

            if (endTile == null)
                throw new InvalidPositionException("Can't move piece to invalid position " + endPosition.ToString());

            if (endTile.Piece != null)
                throw new InvalidPositionException("Can't move piece to occupied tile " + endPosition.ToString());

            var piece = startTile.Piece;
            piece.CurrentPosition = endPosition;

            endTile.Piece = piece;
            startTile.Piece = null;

            return piece;
        }

        /// <summary>
        /// Returns a tile in the board or null if the position is invalid
        /// </summary>
        /// <param name="pos"> </param>
        /// <returns> </returns>
        protected Tile GetTile(Position pos)
        {
            if (BoardState.ValidIndex(pos.x, pos.y))
                return BoardState[pos.x, pos.y];

            return null;
        }

        /// <summary>
        /// Returns info about the tile in the position
        /// </summary>
        /// <param name="pos"> </param>
        /// <returns> </returns>
        public TileInfo GetTileInfo(Position pos)
        {
            Tile tile = GetTile(pos);

            if (tile != null)
                return new TileInfo(pos, tile.Piece != null);

            return default;
        }

        public ChessPiece GetPiece(Position pos)
        {
            return GetTile(pos)?.Piece;
        }

        public List<ChessPiece> GetAllPieces(Player player = default)
        {
            return PieceLists[player];
        }

        public Dictionary<ChessPiece, List<Move>> GetAllMoves(Player player = default)
        {
            var movesDict = new Dictionary<ChessPiece, List<Move>>();
            var pieces = GetAllPieces(player);

            foreach (var piece in pieces)
            {
                movesDict[piece] = piece.GetMoves(this);
            }

            return movesDict;
        }

        /// <summary>
        /// Returns a copy of the BoardState
        /// </summary>
        /// <returns> </returns>
        public BoardState GetBoardStateCopy()
        {
            return new BoardState(this.BoardState);
        }

        public bool CompareBoardStates(Chessboard other)
        {
            if (other == null)
                return false;

            return BoardState.Equals(other.BoardState);
        }

        /// <summary>
        /// Executes a Chess Action on this board and returns it.
        /// </summary>
        /// <param name="action"> </param>
        /// <exception cref="InvalidActionException"> </exception>
        public virtual Chessboard ExecuteAction(IChessAction action)
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
                   EqualityComparer<Stack<ChessPiece>>.Default.Equals(RemovedPieces, other.RemovedPieces);
        }

        public override int GetHashCode()
        {
            int hashCode = -399849685;
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<BoardState>.Default.GetHashCode(BoardState);
            hashCode = hashCode * -1521134295 + EqualityComparer<Stack<IChessAction>>.Default.GetHashCode(ActionHistory);
            hashCode = hashCode * -1521134295 + EqualityComparer<Stack<IChessAction>>.Default.GetHashCode(ActionCache);
            hashCode = hashCode * -1521134295 + EqualityComparer<Stack<ChessPiece>>.Default.GetHashCode(RemovedPieces);
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