using Chess.Controllers.Configuration;
using Chess.Controllers.GameLoader;
using Chess.Models.Board;
using Chess.Models.Classes;
using Chess.Models.Exceptions;
using Chess.Models.Game;
using Chess.Models.Game.Action;
using Chess.Models.Pieces;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ChessTest
{
    public class ChessboardTest
    {
        private Chessboard empty_2x2_board;
        private DefaultChessConfiguration chess_config;
        private DefaultChessLoader loader;
        private Player black;
        private Player white;

        [SetUp]
        public void Setup()
        {
            black = new Player(id: "black");
            white = new Player(id: "white");
            empty_2x2_board = new Chessboard(2, 2);
            chess_config = new DefaultChessConfiguration { Black = black, White = white };
            loader = new DefaultChessLoader();
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

            Assert.IsTrue(new_board.Width == empty_2x2_board.Width);
            Assert.IsTrue(new_board.Height == empty_2x2_board.Height);
            Assert.IsTrue(new_board.BoardState == empty_2x2_board.BoardState);
        }

        [Test]
        public void CreateFullBoard_DefaultChessBoard_Success()
        {
            var complete_board = loader.CreateBoard(chess_config);

            Assert.IsTrue(complete_board.Width == 8 && complete_board.Height == 8);
        }

        [Test]
        public void GetTile_ValidPosition_ReturnsTile()
        {
            var rook = new Rook(Position.Zero, default);
            empty_2x2_board.BoardState[0, 0].Piece = rook;

            var tile = empty_2x2_board.GetTile(new Position(0, 0));

            Assert.IsNotNull(tile);
            Assert.IsTrue(tile.Piece.Equals(rook));
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

        [Test]
        public void IsKingInCheck_ValidCheck_ReturnsTrue()
        {
            empty_2x2_board.BoardState[0, 0].Piece = new Pawn(new Position(0, 0), white, Position.Up);
            empty_2x2_board.BoardState[1, 1].Piece = new King(new Position(0, 0), black);

            Assert.IsTrue(empty_2x2_board.IsKingInCheck(white));
        }

        [Test]
        public void IsKingInCheck_NotInCheck_ReturnsFalse()
        {
            empty_2x2_board.BoardState[0, 0].Piece = new Rook(new Position(0, 0), white);
            empty_2x2_board.BoardState[1, 1].Piece = new King(new Position(0, 0), black);

            Assert.IsFalse(empty_2x2_board.IsKingInCheck(white));
        }
    }
}