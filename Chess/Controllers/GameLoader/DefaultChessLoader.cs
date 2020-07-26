using Chess.Controllers.Configuration;
using Chess.Models.Board;
using Chess.Models.Classes;
using Chess.Models.Exceptions;
using Chess.Models.Game;
using Chess.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Controllers.GameLoader
{
    public class DefaultChessLoader : IChessGameLoader<DefaultChessConfiguration>
    {
        public const int BOARD_SIZE = 8;

        /// <summary>
        /// Creates the default chess board
        /// </summary>
        /// <exception cref="InvalidPlayerException"></exception>
        public Chessboard CreateBoard(DefaultChessConfiguration config)
        {
            if (config == null)
                return null;

            try { ValidatePlayer(config.White); }
            catch (InvalidPlayerException e) { throw new InvalidPlayerException("Invalid white side Player: " + e.Message); }

            try { ValidatePlayer(config.Black); }
            catch (InvalidPlayerException e) { throw new InvalidPlayerException("Invalid black side Player: " + e.Message); }

            if (config.White.Id == config.Black.Id)
                throw new InvalidPlayerException("Players can't have the same id");

            var board = new Chessboard(BOARD_SIZE, BOARD_SIZE);
            var BoardState = board.BoardState;

            for (int i = 0; i < BoardState.Width; i++)
            {
                BoardState[i, 1].Piece = new Pawn(new Position(i, 1), config.White, Position.Up);
                BoardState[i, 6].Piece = new Pawn(new Position(i, 6), config.Black, Position.Down);
            }

            BoardState[0, 0].Piece = new Rook(new Position(0, 0), config.White);
            BoardState[7, 0].Piece = new Rook(new Position(7, 0), config.White);
            BoardState[0, 7].Piece = new Rook(new Position(0, 7), config.Black);
            BoardState[7, 7].Piece = new Rook(new Position(7, 7), config.Black);

            BoardState[1, 0].Piece = new Knight(new Position(1, 0), config.White);
            BoardState[6, 0].Piece = new Knight(new Position(6, 0), config.White);
            BoardState[1, 7].Piece = new Knight(new Position(1, 7), config.Black);
            BoardState[6, 7].Piece = new Knight(new Position(6, 7), config.Black);

            BoardState[2, 0].Piece = new Bishop(new Position(2, 0), config.White);
            BoardState[5, 0].Piece = new Bishop(new Position(5, 0), config.White);
            BoardState[2, 7].Piece = new Bishop(new Position(2, 7), config.Black);
            BoardState[5, 7].Piece = new Bishop(new Position(5, 7), config.Black);

            BoardState[3, 0].Piece = new Queen(new Position(3, 0), config.White);
            BoardState[4, 0].Piece = new King(new Position(4, 0), config.White);
            BoardState[3, 7].Piece = new Queen(new Position(3, 7), config.Black);
            BoardState[4, 7].Piece = new King(new Position(4, 7), config.Black);

            return board;
        }

        /// <summary>
        /// Validates a player
        /// </summary>
        /// <exception cref="InvalidPlayerException"></exception>
        /// <param name="player"></param>
        private void ValidatePlayer(Player player)
        {
            if (player == null)
                throw new InvalidPlayerException("Player can't be null");

            if (string.IsNullOrEmpty(player.Id))
                throw new InvalidPlayerException("Player needs to have a valid Id");
        }
    }
}