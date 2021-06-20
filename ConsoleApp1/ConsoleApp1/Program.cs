using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
 
    class Program
    {

       
        static void Main(string[] args)
        {

            Board board = new Board();
            Player player = new Player();
            
            board.Initialize(25, player);

            player.Initialize(1, 1, board);

            Console.CursorVisible = false;
            int lastTick = 0;

            const int Wait_Tick = 1000 / 30;

            while (true)
            {
                #region Frame Management

                int currentTick = System.Environment.TickCount;
                int elapsedTick = currentTick - lastTick;
                int deltaTime = currentTick - lastTick;
                if (elapsedTick < Wait_Tick)
                {
                    continue;
                }

                lastTick = currentTick;

                #endregion




                //입력

                //로직
                player.Update(deltaTime);


                //렌더링

                Console.SetCursorPosition(0, 0);
                board.Render();
            }
                


        }
    }
}
