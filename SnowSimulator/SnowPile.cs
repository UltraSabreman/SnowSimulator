using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowSimulator {
    class SnowPile {
        static object theLock = new object();
        static int maxPileHight = 10;
        static int maxRowsUsed = 10;

        Dictionary<int, int> piles = new Dictionary<int, int>();

        public SnowPile() {
            for (int i = 0; i < Console.WindowWidth + 500; i++) {
                piles[i] = 0;
            }
        }

        public void Draw() {
            //return;
            Random gen = new Random();

            int stepSize = maxPileHight / maxRowsUsed;
            lock (theLock) {
                for (int posX = 0; posX < Console.WindowWidth - 1; posX++) {

                    //TODO: make generic based on upper limit.

                    int pileSize = piles[posX] / maxRowsUsed;
                    int bottom = Console.WindowHeight - 1;
                    int upperLimit = (Console.WindowHeight - 1) - pileSize;
                    for (int i = bottom; i >= upperLimit; i--) {
                        //Console.SetCursorPosition(posX, i);

                        if (i == upperLimit) {
                            //Printer.PrintCharAtX('-', posX, i);
                            //Console.Write("-");
                            /*if (posX == 0) {
                                if (piles[posX + 1] > pile)

                            } else if (posX == Console.WindowWidth) {

                            } else {

                            }*/
                        } else {
                            //Printer.PrintCharAtX('.', posX, i);

                            //Console.Write(".");
                        }

                    }
                }
            }
        }

        //private void DrawPilePice(int X, int Y, )

        public void AbsorbFlakeIfAble(SnowFlake flake) {
            lock (theLock) {
                if (!piles.ContainsKey((int)flake.posX)) {
                    piles[(int)flake.posX] = 0;
                }

                if (Console.WindowHeight - piles[(int)flake.posX] <= flake.posY) {
                    flake.removeMe = true;


                    if (piles[(int)flake.posX] < maxPileHight)
                        piles[(int)flake.posX] += 1; //TODO: change based on flake size?
                }
            }
        }

        public void Tick() {
            //piles.Capacity = Console.WindowWidth;

            //TODO: melt on a random chance?
        }
    }
}
