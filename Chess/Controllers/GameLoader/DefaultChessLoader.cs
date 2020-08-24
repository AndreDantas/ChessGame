using Chess.Controllers.Configuration;
using Chess.Models.Board;
using Chess.Models.Classes;
using Chess.Models.Exceptions;
using Chess.Models.Game;
using Chess.Models.Pieces;
using System;

namespace Chess.Controllers.GameLoader
{
    public class DefaultChessLoader : IChessGameLoader<DefaultChessConfiguration>
    {
        public const int BOARD_SIZE = 8;

        /// <summary>
        /// Creates the default chess board
        /// </summary>
        /// <exception cref="InvalidPlayerException"> </exception>
        /// <exception cref="ArgumentNullException"> </exception>
        public Chessboard CreateBoard(DefaultChessConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException("config", "Configuration can't be null");

            try { ValidatePlayer(config.White); }
            catch (InvalidPlayerException e) { throw new InvalidPlayerException("Invalid white side Player: " + e.Message); }

            try { ValidatePlayer(config.Black); }
            catch (InvalidPlayerException e) { throw new InvalidPlayerException("Invalid black side Player: " + e.Message); }

            if (config.White.Id == config.Black.Id)
                throw new InvalidPlayerException("Players can't have the same id");

            var board = new Chessboard(BOARD_SIZE, BOARD_SIZE);

            for (int i = 0; i < board.Width; i++)
            {
                board.AddPiece(new Pawn(new Position(i, 1), config.White, Position.Up));
                board.AddPiece(new Pawn(new Position(i, 6), config.Black, Position.Down));
            }

            board.AddPiece(new Rook(new Position(0, 0), config.White));
            board.AddPiece(new Rook(new Position(7, 0), config.White));
            board.AddPiece(new Rook(new Position(0, 7), config.Black));
            board.AddPiece(new Rook(new Position(7, 7), config.Black));

            board.AddPiece(new Knight(new Position(1, 0), config.White));
            board.AddPiece(new Knight(new Position(6, 0), config.White));
            board.AddPiece(new Knight(new Position(1, 7), config.Black));
            board.AddPiece(new Knight(new Position(6, 7), config.Black));

            board.AddPiece(new Bishop(new Position(2, 0), config.White));
            board.AddPiece(new Bishop(new Position(5, 0), config.White));
            board.AddPiece(new Bishop(new Position(2, 7), config.Black));
            board.AddPiece(new Bishop(new Position(5, 7), config.Black));

            board.AddPiece(new Queen(new Position(3, 0), config.White));
            board.AddPiece(new King(new Position(4, 0), config.White));
            board.AddPiece(new Queen(new Position(3, 7), config.Black));
            board.AddPiece(new King(new Position(4, 7), config.Black));

            return board;
        }

        /// <summary>
        /// Validates a player
        /// </summary>
        /// <exception cref="InvalidPlayerException"> </exception>
        /// <param name="player"> </param>
        private void ValidatePlayer(Player player)
        {
            if (player == null)
                throw new InvalidPlayerException("Player can't be null");

            if (string.IsNullOrEmpty(player.Id))
                throw new InvalidPlayerException("Player needs to have a valid Id");
        }
    }
}