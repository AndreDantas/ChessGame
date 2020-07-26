﻿using System;
using System.Collections.Generic;
using System.Text;
using Chess.Controllers.Configuration;
using Chess.Controllers.GameLoader;
using Chess.Models.Board;
using Chess.Models.Classes;
using Chess.Models.Exceptions;
using Chess.Models.Game;
using Chess.Models.Game.Action;
using Chess.Models.Pieces;
using Chess.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ChessTest
{
    public class ChessboardTest
    {
        private Chessboard empty_2x2_board;

        [SetUp]
        public void Setup()
        {
            empty_2x2_board = new Chessboard(2, 2);
        }

        [Test]
        public void Constructor_Create8x8Board_Success()
        {
            Chessboard board_8x8 = new Chessboard(8, 8);
            int board_width = board_8x8.BoardState.Width;
            int board_height = board_8x8.BoardState.Height;

            Assert.IsTrue(board_width == 8 && board_height == 8);
        }

        [Test]
        public void Constructor_CreateBoardWithOtherBoard_Success()
        {
            empty_2x2_board.BoardState[1, 0].Piece = new Pawn(new Position(1, 0), default, Position.Up);

            Chessboard new_board = new Chessboard(empty_2x2_board);

            Assert.IsTrue(new_board.Width == empty_2x2_board.Width &&
                          new_board.Height == empty_2x2_board.Height &&
                          new_board.BoardState == empty_2x2_board.BoardState);
        }

        [Test]
        public void GetTile_ValidPosition_ReturnsTile()
        {
        }

        [Test]
        public void ExecuteAction_ValidMove_Success()
        {
            var pawn = empty_2x2_board.BoardState[1, 0].Piece = new Pawn(new Position(1, 0), default, Position.Up);

            empty_2x2_board.ExecuteAction(pawn.GetMoves(empty_2x2_board)[0]);

            Assert.IsTrue(empty_2x2_board.BoardState[1, 1].Piece.Equals(pawn));
        }

        [Test]
        public void ExecuteAction_ValidSpawnPiece_Success()
        {
            var rook = new Rook(Position.Zero, default);
            var spawn_piece = new SpawnPiece(rook, Position.Zero);

            empty_2x2_board.ExecuteAction(spawn_piece);

            Assert.IsTrue(empty_2x2_board.BoardState[0, 0].Piece.Equals(rook));
        }

        [Test]
        public void ExecuteAction_InvalidMove_ThrowsInvalidActionException()
        {
            var pawn = empty_2x2_board.BoardState[1, 0].Piece = new Pawn(new Position(1, 0), default, Position.Up);
            Move invalid_move = new Move
            {
                StartPosition = new Position(0, 0),
                EndPosition = new Position(2, 2)
            };

            Assert.Throws<InvalidActionException>(() => empty_2x2_board.ExecuteAction(invalid_move));
        }

        [Test]
        public void ExecuteAction_InvalidSpawnPiece_ThrowsInvalidActionException()
        {
            var rook = new Rook(Position.Zero, default);
            var spawn_piece = new SpawnPiece(rook, Position.Zero);

            SpawnPiece invalid_spawn = new SpawnPiece(new King(), new Position(2, 2));

            Assert.Throws<InvalidActionException>(() => empty_2x2_board.ExecuteAction(invalid_spawn));
        }

        [Test]
        public void NavigateBack_ValidMove_CorrectBoardState()
        {
            var pawn = empty_2x2_board.BoardState[1, 0].Piece = new Pawn(new Position(1, 0), default, Position.Up);

            Chessboard board_before_move = new Chessboard(empty_2x2_board);

            empty_2x2_board.ExecuteAction(pawn.GetMoves(empty_2x2_board)[0]);

            empty_2x2_board.NavigateBack();

            Assert.IsTrue(empty_2x2_board.BoardState == board_before_move.BoardState);
        }

        [Test]
        public void NavigateForward_ValidMove_CorrectBoardState()
        {
            var pawn = empty_2x2_board.BoardState[1, 0].Piece = new Pawn(new Position(1, 0), default, Position.Up);

            empty_2x2_board.ExecuteAction(pawn.GetMoves(empty_2x2_board)[0]);

            Chessboard board_after_move = new Chessboard(empty_2x2_board);

            empty_2x2_board.NavigateBack();
            empty_2x2_board.NavigateForward();

            Assert.IsTrue(empty_2x2_board.BoardState == board_after_move.BoardState);
        }
    }
}