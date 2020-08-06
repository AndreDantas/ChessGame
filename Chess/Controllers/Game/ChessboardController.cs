using Chess.Controllers.GameLoader;
using Chess.Models.Board;
using Chess.Models.Game;
using Chess.Models.Game.Action;
using Chess.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Controllers.Game
{
    public class ChessboardController
    {
        protected PlayerData White = new PlayerData();
        protected PlayerData Black = new PlayerData();

        public Player currentPlayer { get; private set; }
        public Chessboard board { get; private set; }

        private ChessboardController()
        {
        }

        public static ChessboardController CreateNewBoard(Player white, Player black)
        {
            ChessboardController controller = new ChessboardController();

            controller.White.player = white;
            controller.Black.player = black;

            DefaultChessLoader loader = new DefaultChessLoader();

            controller.board = loader.CreateBoard(new Configuration.DefaultChessConfiguration { Black = black, White = white });

            controller.currentPlayer = white;

            return controller;
        }

        public void CalculateMovesForCurrentPlayer()
        {
            var movesDict = board.GetAllMoves(currentPlayer);
            var otherPlayer = currentPlayer == White.player ? Black.player : White.player;

            var temp_board = new Chessboard(board);

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
        }
    }
}