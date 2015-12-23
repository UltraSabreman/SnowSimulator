using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SnowSimulator {
    public class SnowSimulator : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SnowController controller;


        public SnowSimulator() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            controller = new SnowController(spriteBatch, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
            controller.Init();
            base.Initialize();
        }

          protected override void LoadContent() {

            Texture2D flake = Content.Load<Texture2D>("Textures/flake");
            controller.AddFlakeTexture(flake);
        }

        
        protected override void UnloadContent() {}

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            controller.Tick();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            controller.Draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
