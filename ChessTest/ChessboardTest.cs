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

            Assert.IsTrue(board_8x8.Width == 8 && board_8x8.Height == 8);
        }

        [Test]
        public void Constructor_CreateBoardWithOtherBoard_Success()
        {
            empty_2x2_board.AddPiece(new Pawn(new Position(1, 0), white, Position.Up));

            Chessboard new_board = new Chessboard(empty_2x2_board);

            Assert.IsTrue(new_board.CompareBoardStates(empty_2x2_board));
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
            var rook = new Rook(Position.Zero, white);
            empty_2x2_board.AddPiece(rook);

            var piece = empty_2x2_board.GetPiece(new Position(0, 0));

            Assert.IsTrue(piece.Equals(rook));
        }

        [Test]
        public void ExecuteAction_ValidMove_Success()
        {
            var pawn = empty_2x2_board.AddPiece(new Pawn(new Position(1, 0), white, Position.Up));

            empty_2x2_board.ExecuteAction(pawn.GetMoves(empty_2x2_board)[0]);

            Assert.IsTrue(empty_2x2_board.GetPiece(new Position(1, 1)).Equals(pawn));
        }

        [Test]
        public void ExecuteAction_ValidSpawnPiece_Success()
        {
            var rook = new Rook(Position.Zero, white);
            var spawn_piece = new SpawnPiece(rook, Position.Zero);

            empty_2x2_board.ExecuteAction(spawn_piece);

            Assert.IsTrue(empty_2x2_board.GetPiece(Position.Zero).Equals(rook));
        }

        [Test]
        public void ExecuteAction_InvalidMove_ThrowsInvalidActionException()
        {
            var pawn = empty_2x2_board.AddPiece(new Pawn(new Position(1, 0), white, Position.Up));
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
            var rook = new Rook(Position.Zero, white);
            var spawn_piece = new SpawnPiece(rook, Position.Zero);

            SpawnPiece invalid_spawn = new SpawnPiece(new King(), new Position(2, 2));

            Assert.Throws<InvalidActionException>(() => empty_2x2_board.ExecuteAction(invalid_spawn));
        }

        [Test]
        public void NavigateBack_ValidMove_CorrectBoardState()
        {
            var pawn = empty_2x2_board.AddPiece(new Pawn(new Position(1, 0), white, Position.Up));

            Chessboard board_before_move = new Chessboard(empty_2x2_board);

            empty_2x2_board.ExecuteAction(pawn.GetMoves(empty_2x2_board)[0]);

            empty_2x2_board.NavigateBack();

            Assert.IsTrue(empty_2x2_board.CompareBoardStates(board_before_move));
        }

        [Test]
        public void NavigateForward_ValidMove_CorrectBoardState()
        {
            var pawn = empty_2x2_board.AddPiece(new Pawn(new Position(1, 0), white, Position.Up));

            empty_2x2_board.ExecuteAction(pawn.GetMoves(empty_2x2_board)[0]);

            Chessboard board_after_move = new Chessboard(empty_2x2_board);

            empty_2x2_board.NavigateBack();
            empty_2x2_board.NavigateForward();

            Assert.IsTrue(empty_2x2_board.CompareBoardStates(board_after_move));
        }

        [Test]
        public void IsKingInCheck_ValidCheck_ReturnsTrue()
        {
            empty_2x2_board.AddPiece(new Pawn(new Position(0, 0), white, Position.Up));
            empty_2x2_board.AddPiece(new King(new Position(1, 1), black));

            Assert.IsTrue(empty_2x2_board.IsKingInCheck(white));
        }

        [Test]
        public void IsKingInCheck_NotInCheck_ReturnsFalse()
        {
            empty_2x2_board.AddPiece(new Rook(new Position(0, 0), white));
            empty_2x2_board.AddPiece(new King(new Position(1, 1), black));

            Assert.IsFalse(empty_2x2_board.IsKingInCheck(white));
        }
    }
}