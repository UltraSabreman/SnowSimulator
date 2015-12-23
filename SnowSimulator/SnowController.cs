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
        Timer windGust = new Timer();

        bool isWindBlowing = false;
        double windX = 0;

        int maxFlakes = 100;

        List<Texture2D> flakes = new List<Texture2D>();

        public SnowController(SpriteBatch batch, double maxWinX, double maxWinY) {
            theBatch = batch;
            maxX = maxWinX;
            maxY = maxWinY;
        }

        private void SetRandWindDelay() {
            windDelay.Interval = randomGen.Next(5, 30) * 1000;
        }

        private void SetRandGustLength() {
            windGust.Interval = randomGen.Next(2, 15) * 1000;
        }

        public void Init() {
            SetRandWindDelay();
            windDelay.Elapsed += DoWindGust;
            windDelay.AutoReset = false;

            SetRandGustLength();
            windGust.Elapsed += FinishWindGust;
            windGust.AutoReset = false;

            windDelay.Start();
        }

        public void AddFlakeTexture(Texture2D t) {
            flakes.Add(t);
        }


        public void Tick() {
            //SnowLayer.Tick();
            lock (locker) {
                SnowFlakes.RemoveAll(X => X.removeMe);

                if (SnowFlakes.Count != maxFlakes && randomGen.Next(5) == 0)
                    SnowFlakes.Add(SpawnFlake());

                if (isWindBlowing)
                    SnowFlakes.ForEach(X => X.DoTick(windX));
                else
                    SnowFlakes.ForEach(X => X.DoTick(0));

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
            isWindBlowing = true;
            double temp = randomGen.NextDouble();
            if (temp > 0.5)
                windX += temp;
            else
                windX += -temp;

            SetRandGustLength();
            windGust.Start();
        }

        private void FinishWindGust(object o, EventArgs e) {
            isWindBlowing = false;

            SetRandWindDelay();
            windDelay.Start();
        }


        private SnowFlake SpawnFlake() {
            SnowFlake temp = new SnowFlake(theBatch, flakes[randomGen.Next(flakes.Count)], maxX, maxY);


            return temp;
        }
    }
}
