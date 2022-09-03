using System;

namespace ticktacktoe
{
    class Program
    {
        static void Main(string[] args)
        {
            GameEngine engine = new GameEngine();

            do
            {
                engine.PlayGame();
            } while (engine.PlayAgain());
        }


    }
}
