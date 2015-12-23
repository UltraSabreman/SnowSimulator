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
        static double flakeDragCoef = 100;
        static double maxVelX = 5;
        static int maxSize = 8;


        Texture2D flake;
        SpriteBatch theBatch;

		
        double velX, velY;
        double maxX, maxY;


		double size = -1;
        double density = -1;
        int level = 0;

        public bool removeMe { get; set; } = false;
        public double posX { get; set; } = 0;
        public double posY { get; set; } = 0;

		public SnowFlake(SpriteBatch b, Texture2D texture, double maxX, double maxY) {
            flake = texture;
            theBatch = b;
            this.maxX = maxX;
            this.maxY = maxY;

            size = Math.Max(0.3, randGen.NextDouble());
            density = 1 - size;
            level = randGen.Next(-1, 1);

			posX = randGen.NextDouble() * maxX;
            posY = 0;
            velX = 0;
            velY = density / 4 + 1;
		}

		public void DoTick(double WindX) {
           
			if (posY >= maxY) { removeMe = true; return; }

            if (WindX == 0) {
                velX -= (velX / flakeDragCoef);
            } else {
                if (velX > -(maxVelX / size) && velX < (maxVelX / size))
                    velX += (WindX / (density / 2)) / 300;
            }

			double rem = maxY - posY;
			posY += (velY > rem ? rem : velY);

			posX += velX;
			if (posX < 0)
				posX = maxX + posX;
			else if (posX >= maxX)
				posX = posX - (maxX * Math.Floor(posX / maxX));
		}

		public void DoDraw() {
            if (posY < 0) return;
            try {
                theBatch.Draw(flake, new Vector2((float)posX, (float)posY), null, Color.White, 0f, Vector2.Zero, (float)size, SpriteEffects.None, 0f);
            } catch (Exception) { }
		}

	}

}
