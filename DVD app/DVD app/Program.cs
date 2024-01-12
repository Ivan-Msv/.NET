using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DVD_app
{
    class Program
    {
        public static void Main()
        {
            int windowWidth = Console.WindowWidth;
            int windowLength = Console.WindowHeight;

            int centerX = windowWidth / 2;
            int centerY = windowLength / 2;

            int speedX = 5;
            int speedY = 5;
            int textX = centerX;
            int textY = centerY;
            int textWidth = 4;
            int textLength = 1;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Black;

            while (true)
            {
                Thread.Sleep(33);
                textX += speedX;
                textY += speedY;

                if (textX >= windowWidth - textWidth)
                {
                    textX = windowWidth - textWidth;
                    speedX *= -1;
                }
                else if (textX < 0)
                {
                    textX = 0;
                    speedX *= -1;
                }
                if (textY >= windowLength - textLength)
                {
                    textY = windowLength - textLength;
                    speedY *= -1;
                }
                else if (textY < 0)
                {
                    textY = 0;
                    speedY *= -1;
                }
                Console.Clear();
                Console.SetCursorPosition(textX, textY);
                Console.Write("DVD");
            }
        }
    }
}
