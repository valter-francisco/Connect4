﻿using System;

namespace Connect4
{
    internal class Program
    {
        static void Main(string[] args)
        {

            #region Board Array

            string[,] board = new string[16, 16];

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = " ";
                }
            }


            #endregion

            #region Main Menu
            bool menu = false;
            do
            {
                Console.WriteLine("  CONNECT 4");
                Console.WriteLine("1 - PLAY \n2 - INSTRUCTIONS");
                string menuChoice = Console.ReadLine();
                menuChoice = menuChoice.Trim();

                switch (menuChoice)
                {
                    case "1":
                        menu = true; //breaks out of the Menu and enters the Game region
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("The game is played between two players, you and your opponent take turns. \nChoose a row to place your piece and it will fall to the bottom of the board.\nYou win when you connect 4, be it in a line, row or diagonally. \nIf neither player can connect 4 then the game is a tie. ");
                        Console.WriteLine();
                        DrawBoard(board);
                        Console.WriteLine();
                        Console.Write("Press any key to return to main menu.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Insert a valid input.");
                        break;
                }
            } while (menu == false);
            #endregion

            #region Game
            int turns = 1;


            do //game loop
            {
                Console.Clear();
                DrawBoard(board);

                if (turns % 2 == 0)
                {
                    Console.WriteLine("Player 2 insert a row.");
                }
                else
                {
                    Console.WriteLine("Player 1 insert a row.");
                }


                bool correctInput = false;
                int inp = 0;

                do //row choice verification loop
                {
                    string input = Console.ReadLine();

                    try
                    {
                        input = input.Trim();
                        inp = Convert.ToInt32(input);

                        if (inp <= 0 || inp >= 7)
                        {
                            return;
                        }
                        else
                        {
                            inp = ConvertInput(inp);
                            correctInput = true;
                        }

                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Insert a valid input!");
                    }
                } while (correctInput == false); //row choice verification loop

                bool availableRow = AvailableRow(inp, board);

                //inserting piece if row is available (if... else)
                if (availableRow == true)
                {
                    InsertPiece(inp, board, turns);
                    turns++;
                }

                VictoryVertical(board);

                VictoryHorizontal(board);

                VictoryDiagonalDown(board);

                VictoryDiagonalUP(board);

            } while (turns <= 41); //game loop

            Console.WriteLine("You tied!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);

            #endregion
        }

        /// <summary>
        /// Method used to draw the board
        /// </summary>
        /// <param name="board">array used to store the board variables</param>
        public static void DrawBoard(string[,] board)
        {
            //Console.WriteLine("        R  O  W  S   ");
            //Console.WriteLine("| 1 | 2 | 3 | 4 | 5 | 6 | 7 |");
            //Console.WriteLine("-----------------------------");
            //Console.WriteLine("| {0} | {1} | {2} | {3} | {4} | {5} | {6} |", board[0, 0], board[0, 1], board[0, 2], board[0, 3], board[0, 4], board[0, 5], board[0, 6]);
            //Console.WriteLine("-----------------------------");
            //Console.WriteLine("| {0} | {1} | {2} | {3} | {4} | {5} | {6} |", board[1, 0], board[1, 1], board[1, 2], board[1, 3], board[1, 4], board[1, 5], board[1, 6]);
            //Console.WriteLine("-----------------------------");
            //Console.WriteLine("| {0} | {1} | {2} | {3} | {4} | {5} | {6} |", board[2, 0], board[2, 1], board[2, 2], board[2, 3], board[2, 4], board[2, 5], board[2, 6]);
            //Console.WriteLine("-----------------------------");
            //Console.WriteLine("| {0} | {1} | {2} | {3} | {4} | {5} | {6} |", board[3, 0], board[3, 1], board[3, 2], board[3, 3], board[3, 4], board[3, 5], board[3, 6]);
            //Console.WriteLine("-----------------------------");
            //Console.WriteLine("| {0} | {1} | {2} | {3} | {4} | {5} | {6} |", board[4, 0], board[4, 1], board[4, 2], board[4, 3], board[4, 4], board[4, 5], board[4, 6]);
            //Console.WriteLine("-----------------------------");
            //Console.WriteLine("| {0} | {1} | {2} | {3} | {4} | {5} | {6} |", board[5, 0], board[5, 1], board[5, 2], board[5, 3], board[5, 4], board[5, 5], board[5, 6]);
            //Console.WriteLine("-----------------------------");

            Console.WriteLine("        R  O  W  S   ");

            for (int repeat = 1; repeat < board.GetLength(1) + 1; repeat++)
            {
                if (repeat <= 9)
                {
                    Console.Write("| " + repeat + " ");
                }
                else
                {
                    Console.Write("| " + repeat);
                }
            }
            Console.WriteLine("|");

            for (int repeat = 0; repeat < board.GetLength(1); repeat++)
                Console.Write("----");
            Console.WriteLine();

            for (int i = 0; i < board.GetLength(0); i++)
            {

                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Console.Write("| " + board[i, j] + " ");
                }
                Console.WriteLine("|");

                for (int repeat = 0; repeat < board.GetLength(1); repeat++)
                    Console.Write("----");

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Method used to subtract 1 to the user input in order to be used by the array
        /// </summary>
        /// <param name="input">int used after verification</param>
        /// <returns>returns inp to be used as row in the board</returns>
        public static int ConvertInput(int input)
        {
            int inp = input - 1;
            return inp;
        }

        /// <summary>
        /// Method used to verify if the row is full of pieces
        /// </summary>
        /// <param name="inp">row chosen by the player</param>
        /// <param name="board">board array</param>
        /// <returns>returns false if the top line in the board is filled by a piece</returns>
        public static bool AvailableRow(int inp, string[,] board)
        {

            if (board[0, inp] != " ")
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// Method used to insert piece
        /// </summary>
        /// <param name="inp">Row chosen by the player, used after verification</param>
        /// <param name="board">Board array</param>
        public static void InsertPiece(int inp, string[,] board, int turns)
        {
            string input;
            if (turns % 2 == 0)
            {
                input = "0";
            }
            else
            {
                input = "X";
            }
            for (int i = 0; i < board.GetLength(0); i++)
            {
                if (board[i, inp] != " ")
                {
                    board[(i - 1), inp] = input;
                    return;
                }
                else if (board[i, inp] == " " && i == 5)
                {
                    board[i, inp] = input;
                    return;
                }
            }
        }

        ///// <summary>
        ///// Method used to insert piece X
        ///// </summary>
        ///// <param name="inp">Row chosen by the player, used after verification</param>
        ///// <param name="board">Board array</param>
        //public static void InsertPieceX(int inp, string[,] board)
        //{
        //    for (int i = 0; i < 6; i++)
        //    {
        //        if(board[i, inp] != " ")
        //        {
        //            board[(i-1), inp] = "X";
        //            return; 
        //        }
        //        else if (board[i, inp] == " " && i == 5)
        //        {
        //            board[i, inp] = "X";
        //            return;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Method used to insert piece O
        ///// </summary>
        ///// <param name="inp">Row chosen by the player, used after verification</param>
        ///// <param name="board">Board array</param>
        //public static void InsertPieceO(int inp, string[,] board)
        //{
        //    for (int i = 0; i < 6; i++)
        //    {
        //        if (board[i, inp] != " ")
        //        {
        //            board[(i - 1), inp] = "O";
        //            return;
        //        }
        //        else if (board[i, inp] == " " && i == 5)
        //        {
        //            board[i, inp] = "O";
        //            return;
        //        }
        //    }
        //}

        /// <summary>
        /// Win condition vertical verification
        /// </summary>
        /// <param name="board">Board array</param> 
        public static void VictoryVertical(string[,] board)
        {
            for (int i = 0; i < (board.GetLength(0) - 3); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if ((board[i, j] == board[(i + 1), j] && board[i, j] == board[(i + 2), j] && board[i, j] == board[(i + 3), j]) && board[i, j] == "X")
                    {
                        Console.Clear();
                        Console.WriteLine("Player 1 WINS! \nPress any key to exit...");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                    else if ((board[i, j] == board[(i + 1), j] && board[i, j] == board[(i + 2), j] && board[i, j] == board[(i + 3), j]) && board[i, j] == "O")
                    {
                        Console.Clear();
                        Console.WriteLine("Player 2 WINS! \nPress any key to exit...");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
            }
            return;
        }

        /// <summary>
        /// Win condition horizontal verification
        /// </summary>
        /// <param name="board">Board array</param>
        public static void VictoryHorizontal(string[,] board)
        {
            for (int j = 0; j < (board.GetLength(1) - 3); j++)
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    if ((board[i, j] == board[i, (j + 1)] && board[i, j] == board[i, (j + 2)] && board[i, j] == board[i, (j + 3)]) && board[i, j] == "X")
                    {
                        Console.Clear();
                        Console.WriteLine("Player 1 WINS! \nPress any key to exit...");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                    else if ((board[i, j] == board[i, (j + 1)] && board[i, j] == board[i, (j + 2)] && board[i, j] == board[i, (j + 3)]) && board[i, j] == "O")
                    {
                        Console.Clear();
                        Console.WriteLine("Player 2 WINS! \nPress any key to exit...");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
            }
            return;
        }

        /// <summary>
        /// Win condition diagonal down (viewing from left to right)
        /// </summary>
        /// <param name="board"> Board array</param>
        public static void VictoryDiagonalDown(string[,] board)
        {
            for (int i = 0; i < (board.GetLength(0) - 3); i++)
            {
                for (int j = 0; j < (board.GetLength(1) - 3); j++)
                {
                    if ((board[i, j] == board[(i + 1), (j + 1)] && board[i, j] == board[(i + 2), (j + 2)] && board[i, j] == board[(i + 3), (j + 3)]) && board[i, j] == "X")
                    {
                        Console.Clear();
                        Console.WriteLine("Player 1 WINS! \nPress any key to exit...");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                    else if ((board[i, j] == board[(i + 1), (j + 1)] && board[i, j] == board[(i + 2), (j + 2)] && board[i, j] == board[(i + 3), (j + 3)]) && board[i, j] == "O")
                    {
                        Console.Clear();
                        Console.WriteLine("Player 2 WINS! \nPress any key to exit...");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
            }
            return;
        }

        /// <summary>
        /// Win conditrion diagonal up (viewing from left to right) but running through the array from right to left
        /// </summary>
        /// <param name="board">Board array</param>
        public static void VictoryDiagonalUP(string[,] board)
        {
            for (int i = 0; i < (board.GetLength(0) - 3); i++)
            {
                for (int j = board.GetLength(1); j > 2; j--)
                {
                    if ((board[i, j] == board[(i + 1), (j - 1)] && board[i, j] == board[(i + 2), (j - 2)] && board[i, j] == board[(i + 3), (j - 3)]) && board[i, j] == "X")
                    {
                        Console.Clear();
                        Console.WriteLine("Player 1 WINS! \nPress any key to exit...");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                    else if ((board[i, j] == board[(i + 1), (j - 1)] && board[i, j] == board[(i + 2), (j - 2)] && board[i, j] == board[(i + 3), (j - 3)]) && board[i, j] == "O")
                    {
                        Console.Clear();
                        Console.WriteLine("Player 2 WINS! \nPress any key to exit...");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
            }
            return;
        }
    }
}

