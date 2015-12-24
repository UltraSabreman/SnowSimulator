using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SnowSimulator {
    class SnowController {
        static public object locker = new object();
        static public Double Gravity = 10;

        Random randomGen = new Random();
        SpriteBatch theBatch;
        double maxX, maxY;


        //SnowPile SnowLayer = new SnowPile();
        List<SnowFlake> SnowFlakes = new List<SnowFlake>();

        Timer windDelay = new Timer();

        bool isWindBlowing = false;
        double windX = 0;

        int maxFlakes = 1000;

        List<Texture2D> flakes = new List<Texture2D>();

        public SnowController(SpriteBatch batch, double maxWinX, double maxWinY) {
            theBatch = batch;
            maxX = maxWinX;
            maxY = maxWinY;
        }

        private void SetRandWindDelay() {
            windDelay.Interval = randomGen.Next(5, 10) * 1000;
        }

        public void Init() {
            SetRandWindDelay();
            windDelay.Elapsed += DoWindGust;
            windDelay.AutoReset = false;

            DoWindGust(null, null);
        }

        public void AddFlakeTexture(Texture2D t) {
            flakes.Add(t);
        }


        public void Tick() {
            //SnowLayer.Tick();
            lock (locker) {
                SnowFlakes.RemoveAll(X => X.removeMe);

                if (SnowFlakes.Count != maxFlakes && randomGen.Next(1) == 0)
                    SnowFlakes.Add(SpawnFlake());

                SnowFlakes.ForEach(X => X.DoTick(windX));

                //SnowFlakes.ForEach(X => SnowLayer.AbsorbFlakeIfAble(X));
            }
        }

        public void Draw() {
            //Printer.PrintCharAtX('0', 0, 0);

            //Console.Clear();
            lock (locker) {
                //SnowLayer.Draw();
                SnowFlakes.ForEach(X => X.DoDraw());
            }

        }

        private void DoWindGust(object o, EventArgs e) {
            int chance = randomGen.Next(2);

            if (chance == 0) {
                windX = 0;
            } else {
                double temp = randomGen.NextDouble();
                if (chance == 1)
                    temp = -temp;
                windX += temp;

            }
            
            SetRandWindDelay();
            windDelay.Start();
        }

        private SnowFlake SpawnFlake() {
            SnowFlake temp = new SnowFlake(theBatch, flakes[randomGen.Next(flakes.Count)], maxX, maxY);


            return temp;
        }
    }
}
