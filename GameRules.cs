using System;
using System.Numerics;

namespace ticktacktoe
{
    public class GameRules
    {
        public static readonly Vector2 ForwardDiagonal = new Vector2(1,1);
        public static readonly Vector2 Horizontal = new Vector2(0,1);
        public static readonly Vector2 ReverseDiagonal = new Vector2(1,-1);
        public static readonly Vector2 Vertical = new Vector2(1,0);
       public bool IsValidMove(int[,] board, int i, int j, int boardSize)
       {
           return i >= 0 && j >= 0 && i < boardSize && j < boardSize && board[i, j] == 0;
       }

       public int Score(Vector2 original, int [,] board, int connect, int player, int boardSize)
       {
           int hScore = this.LineScore(original, board, Horizontal, connect, player, boardSize);
           int vScore = this.LineScore(original, board, Vertical, connect, player, boardSize);
           int fdScore = this.LineScore(original, board, ForwardDiagonal, connect, player, boardSize);
           int bdScore = this.LineScore(original, board, ReverseDiagonal, connect, player, boardSize);

            if (hScore == 1 || vScore ==1 || fdScore == 1 || bdScore == 1)
            {
                return 1;
            }

            if (hScore == -1 || vScore == -1 || fdScore == -1 || bdScore == -1)
            {
                return -1;
            }

            return 0;

       }

       private int LineScore(Vector2 original, int[,] board, Vector2 direction, int connect, int player, int boardSize)
       {
           int myPlayerCount = 0;
           int otherPlayerCount = 0;

           Vector2 start = this.GetStarting(original, direction, connect, boardSize);
           Vector2 finish = this.GetFinishing(original, direction, connect, boardSize);

           Vector2 p = start;

           while (true)
           {
               if (board[(int)p.X, (int)p.Y] == 2 || (p == original && player == 2))
               {
                   myPlayerCount++;
                   otherPlayerCount = 0;
               }
               else if (board[(int)p.X, (int)p.Y] == 1 || (p == original && player == 1))
               {
                   otherPlayerCount++;
                   myPlayerCount = 0;
               }
               else
               {
                   myPlayerCount = 0;
                   otherPlayerCount = 0;
               }

               if(p == finish)
               {
                   break;
               }

               p += direction;
           }

           if (myPlayerCount == connect)
           {
               return 1;
           }
           
           if (otherPlayerCount == connect)
           {
               return -1;
           }

           return 0;
       }

       private Vector2 ApplyBounds(Vector2 p, int boardSize)
       {
           if(p.X < 0)
           {
               p.X = 0;
           }

           if (p.X > boardSize - 1)
           {
               p.X = boardSize - 1;
           }

           if (p.Y < 0)
           {
               p.Y = 0;
           }

           if (p.Y > boardSize - 1)
           {
               p.Y = boardSize - 1;
           }

           return p;
       }

       private Vector2 GetFinishing(Vector2 p, Vector2 v, int connect, int boardSize)
       {
           return this.ApplyBounds(p + (v * connect), boardSize);
       }

        private Vector2 GetStarting(Vector2 p, Vector2 v, int connect, int boardSize)
       {
           return this.ApplyBounds(p - (v * connect), boardSize);
       }
    }
}
