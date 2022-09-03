using System;
using System.Numerics;

namespace ticktacktoe
{
    class GameEngine
    {
        private readonly int _boardSize;

        private readonly int _connect;

        private readonly GameRules _rules;

        private int _noOfMoves;

        public GameEngine(int boardSize, int connect)
        {
            this._boardSize = boardSize;
            this._connect = connect;
            this._noOfMoves = boardSize * connect;

        }

        public GameEngine()
        {
            this._boardSize = 3;
            this._connect = 3;
            this._noOfMoves = this._boardSize * this._connect;
            this._rules = new GameRules();
        }

        public bool PlayAgain()
        {
            Console.WriteLine("Do you want to play again?");
            string answer = Console.ReadLine();

            return answer != null && answer.ToLower() == "y";
        }

        public void PlayGame()
        {
            int [,] board = new int[this._boardSize, this._boardSize];
            this.PrintBoard(board);

            this._noOfMoves = this._boardSize * this._boardSize;

            while (this._noOfMoves > 0)
            {
                Vector2 p = this.HumanMove(board);
                int score = this._rules.Score(p, board, this._connect, 1, this._boardSize);

                board[(int)p.X, (int)p.Y] = 1;
                this.PrintBoard(board);

                if (score == -1)
                {
                    Console.WriteLine("Human Win!");
                    break;
                }

                this._noOfMoves--;

                if (this._noOfMoves == 0)
                {
                    break;
                }

                int predictedScore = this.ComputerMove(board, 2, true, ref p, 0);

                score = this._rules.Score(p, board, this._connect, 2, this._boardSize);

                board[(int)p.X, (int)p.Y] = 2;

                if (predictedScore == 1 && score != 1)
                {
                    Console.WriteLine("Computer is going to Win!");
                }

                this.PrintBoard(board);

                if (score == 1)
                {
                    Console.WriteLine("Computer Win!");
                    break;
                }

                this._noOfMoves--;
            }

            if (this._noOfMoves == 0)
            {
                Console.WriteLine("Draw!");
            }
        }

        private int ComputerMove(int[,] board, int player, bool firstMove, ref Vector2 firstPoint, int depth)
        {
            int max = int.MinValue;
            int min = int.MaxValue;

            for (int i = 0; i < this._boardSize; i++)
            {
                for (int j = 0; j < this._boardSize; j++)
                {
                    if(this._rules.IsValidMove(board, i, j, this._boardSize))
                    {
                        int score = this._rules.Score(new Vector2(i, j), board, this._connect, player, this._boardSize);

                        if (score == 1)
                        {
                            // If this is a winning move for the computer 
                            // then just set the move to this point and return
                            if(firstMove)
                            {
                                firstPoint.X = i;
                                firstPoint.Y = j;
                            }

                            return score;
                        }

                        if (score == -1)
                        {
                            // if this results in a human winning then return -1;
                            return score;
                        }

                        if (score == 0)
                        {
                            if(this._noOfMoves == 1)
                            {
                                // if there are no spaces left in the board then the only
                                // space to go in is this space.
                                firstPoint.X = i;
                                firstPoint.Y = j;

                                // return 0 for a draw in this instance
                                return 0;
                            }

                            // If this is a draw then put the piece in the board
                            // and continue down the game tree
                            board[i, j] = player;

                            // try the next move with the other player
                            score = this.ComputerMove(board, SwapPlayer(player), false, ref firstPoint, ++depth);

                            // apply min/max
                            IntermediateScore(ref min, ref max, score, player, i, j, ref firstPoint, firstMove);

                            // remove the piece from the board read for the next time.
                            board[i,j] = 0;
                        }
                    }   
                }
            }

            if (player == 2)
            {
                if (max == int.MinValue)
                {
                    max = 0;
                }

                // If the computer player then return the max value
                return max;
            }

            if (min == int.MaxValue)
            {
                min = 0;
            }

            // if not the computer player then return min
            return min;
        }

        private static int GetValidCoordinateInput()
        {
            int result;

            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Please enter a number.");
            }

            return result;
        }

        private static int SwapPlayer(int player)
        {
            return player == 1 ? 2 : 1;
        }

        private static void IntermediateScore(ref int min, ref int max, int next, int player, int i, int j, ref Vector2 firstPoint, bool firstMove)
        {
            if (player == 2)
            {
                if (next > max)
                {
                    // if this is the computer and this move is better than the previous best then save it as the first move.
                    max = next;
                    if (firstMove)
                    {
                        firstPoint.X = i;
                        firstPoint.Y = j;
                    }
                }
            }
            else
            {
                // if not the computer move then just keep the best score.
                min = Math.Min(next, min);
            }
        }

        private Vector2 HumanMove(int[,] board)
        {
            int x;
            int y;

            do{
                Console.WriteLine("Make Move x:");
                x = GetValidCoordinateInput();

                Console.WriteLine("Make Move y:");
                y = GetValidCoordinateInput();
            }

            while (!this._rules.IsValidMove(board, x, y, this._boardSize));

            return new Vector2(x, y);
        }


        private void PrintBoard(int [,] board)
        {
            Console.WriteLine("Game Board");
            Console.WriteLine();
            

            for(int i = 0; i < this._boardSize; i++)
            {
                Console.Write("|");

                for (int j = 0; j < this._boardSize; j++)
                {
                    string cell;
                    switch (board[i, j])
                    {
                        case 0:
                            cell = " ";
                            break;
                        case 1:
                            cell = "X";
                            break;
                        default:
                            cell = "O";
                            break;
                    }

                    Console.Write("{0}|",  cell);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
