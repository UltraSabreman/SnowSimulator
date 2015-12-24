using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SnowSimulator {
	class SnowFlake {
        static Random randGen = new Random();
        static Timer melting = new Timer();
        static List<SnowFlake> toMelt = new List<SnowFlake>();
        const double flakeDragCoef = 100;
        const double maxVelX = 1;
        const double meltRate = 0.001;

        Texture2D flake;
        SpriteBatch theBatch;

        double velX, velY;
        double maxX, maxY;
        double idleVel;

		double size = -1;
        double density = -1;
        int level = 0;

        bool landed = false;
        

        public bool removeMe { get; set; } = false;
        public double posX { get; set; } = 0;
        public double posY { get; set; } = 0;

        static SnowFlake() {
            melting.Interval = 10;
            melting.AutoReset = true;
            melting.Enabled = true;
            melting.Elapsed += (o, e) => {
                for (int i = 0; i < toMelt.Count; i++) {
                    toMelt[i].size -= meltRate;
                    if (toMelt[i].size <= 0) {
                        toMelt[i].removeMe = true;
                        toMelt.RemoveAt(i);
                    }
                }
            };
            melting.Start();
        }

		public SnowFlake(SpriteBatch b, Texture2D texture, double maxX, double maxY) {
            flake = texture;
            theBatch = b;
            this.maxX = maxX;
            this.maxY = maxY;

            size = Math.Max(0.1, randGen.NextDouble());
            density = 1 - size;
            level = randGen.Next(-1, 1);

            idleVel = (randGen.NextDouble() - 0.5) / 4;


            posX = randGen.NextDouble() * maxX;
            posY = -200;
            velX = idleVel;
            velY = size  + 1;
        }

		public void DoTick(double WindX) {
            if (removeMe || landed) return;

            if (WindX == 0) {
                if (Math.Abs(velX) > Math.Abs(idleVel))
                    velX -= (velX / flakeDragCoef);
            } else {
                if (velX > -(maxVelX / size) && velX < (maxVelX / size))
                    velX += (WindX) / flakeDragCoef;
            }

			posX += velX;
			if (posX < 0)
				posX = maxX + posX;
			else if (posX >= maxX)
				posX = posX - (maxX * Math.Floor(posX / maxX));

            if (posY < maxY - (20 * size)) {
                double rem = maxY - posY;
                posY += (velY > rem ? rem : velY);
            } else if (!landed) {
                landed = true;
                toMelt.Add(this);
            }
        }

		public void DoDraw() {
            try {
                float col = (float)size;
                theBatch.Draw(flake, new Vector2((float)posX, (float)posY), null, new Color(col, col, col, 1f), 0f, Vector2.Zero, (float)size, SpriteEffects.None, 0f);
            } catch (Exception) { }
		}

	}

}
