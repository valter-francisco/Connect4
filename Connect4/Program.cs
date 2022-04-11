using System;
using System.Linq;

namespace Connect4
{
    internal class Program
    {
        static void Main(string[] args)
        {


            #region Board Array

            string[,] board = new string[16, 16];
            //for (int i = 0; i < board.GetLength(0); i++)
            //{
            //    for (int j = 0; j < board.GetLength(1); j++)
            //    {
            //        board[i, j] = " ";
            //    }
            //}

            string[,] board2 = new string[16, 16];

            #endregion

            #region Main Menu
            bool menu = false;
            do
            {
                Console.WriteLine("  CONNECT 4");
                Console.WriteLine("1 - Player vs. Player\n2 - Player vs. CPU easy\n3 - Player vs. CPU hard \n4 - Instructions");
                string menuChoice = Console.ReadLine();
                menuChoice = menuChoice.Trim();

                switch (menuChoice)
                {
                    case "1":
                        PvP(board, board2);
                        break;
                    case "2":
                        PvCPUeasy(board, board2);
                        break;
                    case "3":
                        PvCPUhard(board, board2);
                        break;
                    case "4":
                        Console.Clear();
                        Console.WriteLine("INSTRUCTIONS" +
                            "\n" +
                            "\nChoose a row to place your piece and it will fall to the bottom of the board." +
                            "\nYou win when you connect 4, be it in a line, row or diagonally. " +
                            "\nIf neither player can connect 4 then the game is a tie." +
                            "\nVictories get you 3 points, losses get you 0 and ties get you 1." +
                            "\nThe first to get to 10 points wins" +
                            "\n" +
                            "\nIn the Player vs.Player format, the game is played between two players, you and your opponent take turns." +
                            "\nIn the Player vs. CPU easy format, the computer will be Player 2 and will play random moves automatically" +
                            "\nIn the Player vs. CPU hard format, the computer will be Player 2 and will make a play near yours.");
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

        }

        /// <summary>
        /// Menu function for Player versus Player mode
        /// </summary>
        /// <param name="board">board array to draw</param>
        /// <param name="board2">board array to store and draw information</param>
        public static void PvP(string[,] board, string[,] board2)
        {
            int turns = 1;
            int playerVictory;
            int scoreP1 = 0;
            int scoreP2 = 0;
            bool oneMore = false;
            board = FillBoardStart(board);
            DrawBoard(board);
            do
            {
                do //game loop
                {
                    playerVictory = 0;
                    //Console.Clear();
                    //DrawBoard(board);

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

                            if (inp <= 0 || inp >= (board2.GetLength(1) + 1))
                            {
                                continue;
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

                    bool availableRow = AvailableRow(inp, board2);

                    //inserting piece if row is available (if... else)
                    if (availableRow == true)
                    {
                        InsertPiece(inp, board2, turns);
                        turns++;


                    }
                    int vVertical = VictoryVertical(board);
                    int vHorizontal = VictoryHorizontal(board);
                    int vDiagUp = VictoryDiagonalUP(board);
                    int vDiagDown = VictoryDiagonalDown(board);

                    if (vVertical != 0)
                    {
                        playerVictory = vVertical;
                        Console.Clear();
                        //DrawBoard(board);
                        Console.WriteLine("Player {0} wins!", playerVictory);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    }
                    else if (vHorizontal != 0)
                    {
                        playerVictory = vHorizontal;
                        Console.Clear();
                        //DrawBoard(board);
                        Console.WriteLine("Player {0} wins!", playerVictory);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    }
                    else if (vDiagDown != 0)
                    {
                        playerVictory = vDiagDown;
                        Console.Clear();
                        DrawBoard(board);
                        Console.WriteLine("Player {0} wins!", playerVictory);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    }
                    else if (vDiagUp != 0)
                    {
                        playerVictory = vDiagUp;
                        Console.Clear();
                        DrawBoard(board);
                        Console.WriteLine("Player {0} wins!", playerVictory);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    }

                } while (turns <= 41); //game loop

                if (turns >= 41)
                {
                    Console.WriteLine("You tied");
                    scoreP1 = scoreP1 + 1;
                    scoreP2 = scoreP2 + 1;
                }
                else if (playerVictory == 1)
                {
                    scoreP1 = scoreP1 + 3;
                }
                else if (playerVictory == 2)
                {
                    scoreP2 = scoreP2 + 3;
                }

                PlayAgainCycle(scoreP1, scoreP2, oneMore, board2);
                turns = 1;
            }
            while (scoreP1 <= 10 || scoreP2 <= 10);

            if (scoreP1 <= 10)
            {
                Console.WriteLine("Player 1 wins the tournament!");
                Console.WriteLine("Press any key to exit the application. . .");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Player 2 wins the tournament!");
                Console.WriteLine("Press any key to exit the application. . .");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Menu function for Player versus CPU easy mode
        /// </summary>
        /// <param name="board">board array to draw</param>
        /// <param name="board2">board array to store and draw information</param>
        public static void PvCPUeasy(string[,] board, string[,] board2)
        {
            int turns = 1;
            int playerVictory;
            int scoreP1 = 0;
            int scoreP2 = 0;
            bool oneMore = false;

            do
            {
                do //game loop
                {
                    playerVictory = 0;
                    Console.Clear();
                    DrawBoard(board);

                    if (turns % 2 == 0)
                    {
                        bool availableRow = false;
                        do
                        {
                            Random inp = new Random();
                            int input = inp.Next(0, board2.GetLength(1));

                            availableRow = AvailableRow(input, board2);
                            if (availableRow == true)
                            {
                                InsertPiece(input, board2, turns);
                            }
                        }
                        while (availableRow == false);
                        turns++;
                    }
                    else
                    {
                        Console.WriteLine("Player 1 insert a row.");
                        bool correctInput = false;
                        int inp = 0;

                        do //row choice verification loop
                        {
                            string input = Console.ReadLine();

                            try
                            {
                                input = input.Trim();
                                inp = Convert.ToInt32(input);

                                if (inp <= 0 || inp >= (board2.GetLength(1) + 1))
                                {
                                    continue;
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

                        bool availableRow = AvailableRow(inp, board2);

                        //inserting piece if row is available (if... else)
                        if (availableRow == true)
                        {
                            InsertPiece(inp, board2, turns);
                            turns++;
                        }
                    }

                    if (VictoryVertical(board2) != 0)
                    {
                        playerVictory = VictoryVertical(board2);
                        Console.Clear();
                        DrawBoard(board);
                        Console.WriteLine("Player {0} wins!", playerVictory);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    }
                    else if (VictoryHorizontal(board2) != 0)
                    {
                        playerVictory = VictoryHorizontal(board2);
                        Console.Clear();
                        DrawBoard(board);
                        Console.WriteLine("Player {0} wins!", playerVictory);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    }
                    else if (VictoryDiagonalDown(board2) != 0)
                    {
                        playerVictory = VictoryDiagonalDown(board2);
                        Console.Clear();
                        DrawBoard(board);
                        Console.WriteLine("Player {0} wins!", playerVictory);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    }
                    else if (VictoryDiagonalUP(board2) != 0)
                    {
                        playerVictory = VictoryDiagonalUP(board2);
                        Console.Clear();
                        DrawBoard(board);
                        Console.WriteLine("Player {0} wins!", playerVictory);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    }

                } while (turns <= 41); //game loop


                if (turns >= 41)
                {
                    Console.WriteLine("You tied");
                    scoreP1 = scoreP1 + 1;
                    scoreP2 = scoreP2 + 1;
                }
                else if (playerVictory == 1)
                {
                    scoreP1 = scoreP1 + 3;
                }
                else if (playerVictory == 2)
                {
                    scoreP2 = scoreP2 + 3;
                }

                PlayAgainCycle(scoreP1, scoreP2, oneMore, board2);
                turns = 1;
            }
            while (scoreP1 <= 10 || scoreP2 <= 10);

            if (scoreP1 <= 10)
            {
                Console.WriteLine("Player 1 wins the tournament!");
                Console.WriteLine("Press any key to exit the application. . .");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Player 2 wins the tournament!");
                Console.WriteLine("Press any key to exit the application. . .");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Menu funtion for Player versus CPU hard mode
        /// </summary>
        /// <param name="board">board array to draw</param>
        /// <param name="board2">board array to store and draw information</param>
        public static void PvCPUhard(string[,] board, string[,] board2)
        {
            int inp = 0;
            int turns = 1;
            int playerVictory;
            int scoreP1 = 0;
            int scoreP2 = 0;
            bool oneMore = false;

            do
            {
                do //game loop
                {
                    playerVictory = 0;
                    Console.Clear();
                    DrawBoard(board);
                    int lastPlayerInput = StoreLastPlay(inp);

                    if (turns % 2 == 0)
                    {
                        bool availableRow = false;
                        do
                        {
                            Random nearby = new Random();
                            int input = lastPlayerInput + nearby.Next(-1, 2);

                            availableRow = AvailableRow(input, board2);
                            if (availableRow == true)
                            {
                                InsertPiece(input, board2, turns);
                            }
                        }
                        while (availableRow == false);
                        turns++;
                    }
                    else
                    {
                        Console.WriteLine("Player 1 insert a row.");
                        bool correctInput = false;


                        do //row choice verification loop
                        {
                            string input = Console.ReadLine();

                            try
                            {
                                input = input.Trim();
                                inp = Convert.ToInt32(input);

                                if (inp <= 0 || inp >= (board2.GetLength(1) + 1))
                                {
                                    continue;
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

                        bool availableRow = AvailableRow(inp, board2);

                        //inserting piece if row is available (if... else)
                        if (availableRow == true)
                        {
                            InsertPiece(inp, board2, turns);
                            turns++;
                        }

                    }


                    if (VictoryVertical(board2) != 0)
                    {
                        playerVictory = VictoryVertical(board2);
                        Console.Clear();
                        DrawBoard(board);
                        Console.WriteLine("Player {0} wins!", playerVictory);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    }
                    else if (VictoryHorizontal(board2) != 0)
                    {
                        playerVictory = VictoryHorizontal(board2);
                        Console.Clear();
                        DrawBoard(board);
                        Console.WriteLine("Player {0} wins!", playerVictory);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    }
                    else if (VictoryDiagonalDown(board2) != 0)
                    {
                        playerVictory = VictoryDiagonalDown(board2);
                        Console.Clear();
                        DrawBoard(board);
                        Console.WriteLine("Player {0} wins!", playerVictory);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    }
                    else if (VictoryDiagonalUP(board2) != 0)
                    {
                        playerVictory = VictoryDiagonalUP(board2);
                        Console.Clear();
                        DrawBoard(board);
                        Console.WriteLine("Player {0} wins!", playerVictory);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    }


                } while (turns <= 41); //game loop


                if (turns >= 41)
                {
                    Console.WriteLine("You tied");
                    scoreP1 = scoreP1 + 1;
                    scoreP2 = scoreP2 + 1;
                }
                else if (playerVictory == 1)
                {
                    scoreP1 = scoreP1 + 3;
                }
                else if (playerVictory == 2)
                {
                    scoreP2 = scoreP2 + 3;
                }

                PlayAgainCycle(scoreP1, scoreP2, oneMore, board2);
                turns = 1;
            }
            while (scoreP1 <= 10 || scoreP2 <= 10);

            if (scoreP1 <= 10)
            {
                Console.WriteLine("Player 1 wins the tournament!");
                Console.WriteLine("Press any key to exit the application. . .");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Player 2 wins the tournament!");
                Console.WriteLine("Press any key to exit the application. . .");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Function used at the end of a game to know if the players want to go another round
        /// </summary>
        /// <param name="scoreP1">Player 1 score</param>
        /// <param name="scoreP2">Player 2 score</param>
        /// <param name="oneMore">boolean to alter the value in order to repeat the cycle</param>
        public static void PlayAgainCycle(int scoreP1, int scoreP2, bool oneMore, string[,] board2)
        {
            do
            {
                Console.Clear();
                Console.WriteLine("The scores are: \nPlayer 1 : {0} \nPlayer 2 : {1}", scoreP1, scoreP2);
                Console.WriteLine("Want to play again? Yes(y) No(n)");
                string playAgain = Console.ReadLine();
                playAgain = playAgain.Trim();
                playAgain = playAgain.ToLower();

                switch (playAgain)
                {
                    case "y":
                        for (int i = 0; i < board2.GetLength(0); i++)
                        {
                            for (int j = 0; j < board2.GetLength(1); j++)
                            {
                                board2[i, j] = "";
                            }
                        }

                        oneMore = true;
                        break;
                    case "n":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Please insert a valid input.");
                        break;
                }
            }
            while (oneMore == false);
        }

        public static string[,] FillBoardStart(string[,] board)
        {
            int lines = board.GetLength(0) - 1;
            int columns = board.GetLength(1) - 1;
            string emptyChar = " ";

            // Fill the hole board with char
            for (int i = 0; i < lines; i++)
                for (int j = 0; j < columns; j++)
                    board[lines, columns] = emptyChar;

            return board;
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

            int lines = board.GetLength(0) - 1;
            int columns = board.GetLength(1) - 1;


            // Fill the hole board with char
            for (int i = 0; i < lines; i++)
            {

                for (int j = 0; j < columns; j++)
                {
                    if (j == columns)
                        Console.Write("| " + board[lines, columns] + " |");

                    else
                        Console.Write("| " + board[lines, columns] + " ");
                }
                Console.WriteLine("|");
                for (int repeat = 0; repeat < board.GetLength(1); repeat++)
                    Console.Write("----");
                Console.WriteLine();
            }

            //for (int i = 0; i < board2.GetLength(0); i++)
            //{

            //    for (int j = 0; j < board2.GetLength(1); j++)
            //    {
            //        bool isEmpty = String.IsNullOrEmpty(board2[i, j]);
            //        if (isEmpty == false)
            //        {
            //            Console.Write("| " + board2[i, j] + " ");
            //        }
            //        else
            //        {
            //            Console.Write("| " + board[i, j] + " ");
            //        }


            //    }
            //    Console.WriteLine("|");

            //    for (int repeat = 0; repeat < board.GetLength(1); repeat++)
            //        Console.Write("----");

            //    Console.WriteLine();
            //}
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
        /// Function used in Player versus CPU hard to store player's last play and use it to decide the CPU play
        /// </summary>
        /// <param name="input">player input, integer</param>
        /// <returns>returns the integer</returns>
        public static int StoreLastPlay(int input)
        {
            return input;
        }

        /// <summary>
        /// Method used to verify if the row is full of pieces
        /// </summary>
        /// <param name="inp">row chosen by the player</param>
        /// <param name="board">board array</param>
        /// <returns>returns false if the top line in the board is filled by a piece</returns>
        public static bool AvailableRow(int inp, string[,] board2)
        {
            bool isEmpty = String.IsNullOrEmpty(board2[0, inp]);
            if (isEmpty == false)
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
        public static string[,] InsertPiece(int inp, string[,] board, int turns)
        {
            string input;
            if (turns % 2 == 0)
            {
                input = "O";

            }
            else
            {
                input = "X";
            }

            for (int i = (board.GetLength(0) - 1); i >= 0; i--)
            {

                bool isEmpty = String.IsNullOrEmpty(board[i, inp]);
                if (isEmpty == true && i == board.GetLength(0) - 1)
                {
                    board[i, inp] = input;


                }
                else if (isEmpty == true)
                {
                    board[(i), inp] = input;

                }
            }
            return board;
        }

        /// <summary>
        /// Win condition vertical
        /// </summary>
        /// <param name="board2">board to store information</param>
        /// <returns>1 - player 1 wins
        /// 2 - player 2 wins
        /// 0 - no win</returns>
        public static int VictoryVertical(string[,] board2)
        {
            for (int i = 0; i < (board2.GetLength(0) - 3); i++)
            {
                for (int j = 0; j < board2.GetLength(1); j++)
                {
                    if ((board2[i, j] == board2[(i + 1), j] && board2[i, j] == board2[(i + 2), j] && board2[i, j] == board2[(i + 3), j]) && board2[i, j] == "X")
                    {
                        return 1;
                    }
                    else if ((board2[i, j] == board2[(i + 1), j] && board2[i, j] == board2[(i + 2), j] && board2[i, j] == board2[(i + 3), j]) && board2[i, j] == "O")
                    {
                        return 2;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Win condition horizontal
        /// </summary>
        /// <param name="board2">board to store information</param>
        /// <returns>1 - player 1 wins
        /// 2 - player 2 wins
        /// 0 - no win</returns>
        public static int VictoryHorizontal(string[,] board2)
        {
            for (int j = 0; j < (board2.GetLength(1) - 3); j++)
            {
                for (int i = 0; i < board2.GetLength(0); i++)
                {
                    if ((board2[i, j] == board2[i, (j + 1)] && board2[i, j] == board2[i, (j + 2)] && board2[i, j] == board2[i, (j + 3)]) && board2[i, j] == "X")
                    {
                        return 1;
                    }
                    else if ((board2[i, j] == board2[i, (j + 1)] && board2[i, j] == board2[i, (j + 2)] && board2[i, j] == board2[i, (j + 3)]) && board2[i, j] == "O")
                    {
                        return 2;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Win condition diagonal down (viewing from left to right)
        /// </summary>
        /// <param name="board2">board to store information</param>
        /// <returns>1 - player 1 wins
        /// 2 - player 2 wins
        /// 0 - no win</returns>
        public static int VictoryDiagonalDown(string[,] board2)
        {
            for (int i = 0; i < (board2.GetLength(0) - 3); i++)
            {
                for (int j = 0; j < (board2.GetLength(1) - 3); j++)
                {
                    if ((board2[i, j] == board2[(i + 1), (j + 1)] && board2[i, j] == board2[(i + 2), (j + 2)] && board2[i, j] == board2[(i + 3), (j + 3)]) && board2[i, j] == "X")
                    {
                        return 1;
                    }
                    else if ((board2[i, j] == board2[(i + 1), (j + 1)] && board2[i, j] == board2[(i + 2), (j + 2)] && board2[i, j] == board2[(i + 3), (j + 3)]) && board2[i, j] == "O")
                    {
                        return 2;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Win condition diagonal up (viewing from left to right)
        /// </summary>
        /// <param name="board2">board to store information</param>
        /// <returns>1 - player 1 wins
        /// 2 - player 2 wins
        /// 0 - no win</returns>
        public static int VictoryDiagonalUP(string[,] board2)
        {
            for (int i = 0; i < (board2.GetLength(0) - 3); i++)
            {
                for (int j = (board2.GetLength(1) - 1); j > 2; j--)
                {
                    if ((board2[i, j] == board2[(i + 1), (j - 1)] && board2[i, j] == board2[(i + 2), (j - 2)] && board2[i, j] == board2[(i + 3), (j - 3)]) && board2[i, j] == "X")
                    {
                        return 1;
                    }
                    else if ((board2[i, j] == board2[(i + 1), (j - 1)] && board2[i, j] == board2[(i + 2), (j - 2)] && board2[i, j] == board2[(i + 3), (j - 3)]) && board2[i, j] == "O")
                    {
                        return 2;
                    }
                }
            }
            return 0;
        }
    }
}

