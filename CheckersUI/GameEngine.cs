using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SbsSW.SwiPlCs;

namespace CheckersUI
{
    /// <summary>
    /// The prolog game engine
    /// </summary>
    class GameEngine :IDisposable
    {
        private Pawn[] mBoard; // Keep the current state of the board
        private string mHumanSign; // the sign of the human player ('x' or 'o')
        private string mComputerSign; // the sign of the computer player ('x' or 'o')
        private int mDifficulty; // how many steps ahead should the computer calculate (2-5).

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="path">The prolog file path</param>
        /// <param name="humanStart"><b>True</b> is the human player does the first move, otherwise <b>false</b>.</param>
        /// <param name="difficulty">The difficulty lever of the computer player (2-5)</param>
        public GameEngine(string path, bool humanStart, int difficulty)
        {
            PlEngine.Initialize(new string[] { "-q", "-f", path });

            mDifficulty = difficulty;

            mHumanSign = "o";
            mComputerSign = "x";

            PlQuery.PlCall("assert", new PlTermV(new PlTerm(string.Format("min_to_move({0} / _)", mHumanSign))));
            PlQuery.PlCall("assert", new PlTermV(new PlTerm(string.Format("max_to_move({0} / _)", mComputerSign))));

            using (PlQuery q = new PlQuery("init(B)"))
            {
                string b = q.SolutionVariables.First()["B"].ToString();
                mBoard = BoardString(b);
            }
        }

        /// <summary>
        /// Make the human move on the board
        /// </summary>
        /// <param name="fromL">From line</param>
        /// <param name="fromC">From column</param>
        /// <param name="toL">To line</param>
        /// <param name="toC">To column</param>
        /// <returns><b>True</b> if this is a valid move, otherwise <b>False</b>.</returns>
        public bool PlayHuman(int fromL, int fromC, int toL, int toC)
        {
            string board = StringBoard(mBoard);
            string query = string.Format("move({0}, {1}, {2}, {3}, {4}, NewBoard)", board, fromL, fromC, toL, toC);

            using (PlQuery q = new PlQuery(query))
            {
                if (q.SolutionVariables.Count() > 0)
                {
                    PlTerm newBoard = q.SolutionVariables.First()["NewBoard"];
                    string result = newBoard[0].ToString();
                    mBoard = BoardString(result);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Make the computer move on the board
        /// </summary>
        public void PlayComputer()
        {
            PlEngine.PlThreadAttachEngine();
            string board = StringBoard(mBoard);

            string query = string.Format(@"alphabeta({0}/{1}, -100, 100, NewBoard, Temp, {2})", mComputerSign, board, mDifficulty);

            using (PlQuery q = new PlQuery(query))
            {
                PlTerm newBoard = q.SolutionVariables.First()["NewBoard"];
                string result = newBoard[0].ToString();
                string next = result.Substring(0, 1);
                string b = result.Substring(2);
                mBoard = BoardString(b);
            }
            PlEngine.PlThreadDestroyEngine();
        }

        /// <summary>
        /// Check whether the human player has won the game.
        /// </summary>
        /// <returns><b>True</b> if the human won, otherwise <b>False</b>.</returns>
        public bool CheckHumanWon()
        {
            string board = StringBoard(mBoard);

            string query = string.Format(@"goal({0}, {1})", board, mHumanSign);

            return PlQuery.PlCall(query);
        }

        /// <summary>
        /// Check whether the computer player has won the game.
        /// </summary>
        /// <returns><b>True</b> if the computer won, otherwise <b>False</b>.</returns>
        public bool CheckComputerWon()
        {
            string board = StringBoard(mBoard);

            string query = string.Format(@"goal({0}, {1})", board, mComputerSign);

            return PlQuery.PlCall(query);
        }

        /// <summary>
        /// Returns the current state of the board.
        /// </summary>
        public Pawn[] Board
        {
            get
            {
                return mBoard;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Clean the memory
        /// </summary>
        public void Dispose()
        {
            PlEngine.PlCleanup();
        }

        #endregion

        /// <summary>
        /// Converts a C# board to a prolog relation.
        /// </summary>
        /// <param name="pions">A C# array that represent a board</param>
        /// <returns>A string of a prolog relation that represent a board</returns>
        private string StringBoard(Pawn[] pawns)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("b(");
            foreach (Pawn p in pawns)
            {
                string ps = null;
                switch (p)
                {
                    case Pawn.Empty: ps = "e"; break;
                    case Pawn.KingO: ps = "oo"; break;
                    case Pawn.KingX: ps = "xx"; break;
                    case Pawn.None: ps = "n"; break;
                    case Pawn.O: ps = "o"; break;
                    case Pawn.X: ps = "x"; break;
                }
                sb.Append(ps);
                sb.Append(", ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// Converts a prolog relation to a C# board.
        /// </summary>
        /// <param name="board">A string of a prolog relation that represent a board</param>
        /// <returns>A C# array that represent a board</returns>
        private Pawn[] BoardString(string board)
        {
            Pawn[] result = new Pawn[64];
            string b = board.Substring(2, board.Length - 3);

            string buffer = string.Empty;
            int i = 0;
            foreach (char c in b)
            {
                if (',' == c)
                {
                    switch (buffer)
                    {
                        case "xx": result[i] = Pawn.KingX; break;
                        case "oo": result[i] = Pawn.KingO; break;
                        case "x": result[i] = Pawn.X; break;
                        case "o": result[i] = Pawn.O; break;
                        case "e": result[i] = Pawn.Empty; break;
                        case "n": result[i] = Pawn.None; break;
                    }
                    i++;
                    buffer = string.Empty;
                }
                else if (Char.IsLetter(c))
                {
                    buffer += c;
                }
            }

            return result;
        }
    }
}
