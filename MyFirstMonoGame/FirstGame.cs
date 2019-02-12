using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyFirstMonoGame
{
    public class FirstGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Textures & Font
        private Texture2D target_Sprite;
        private Texture2D crosshair_Sprite;
        private Texture2D background_Sprite;

        private SpriteFont gameFont;

        Vector2 targetPosition = new Vector2(300,300);
        const int TARGET_RADIUS = 45; // Radius of the Target

        //Mouse Section
        private MouseState mState;
        private bool mReleased = true;
        private float mouseTargetDistance;

        //Score & Timer Section
        private int score = 0;
        float timer = 10f;

        public FirstGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            target_Sprite = Content.Load<Texture2D>("target");
            crosshair_Sprite = Content.Load<Texture2D>("crosshairs");
            background_Sprite = Content.Load<Texture2D>("sky");

            gameFont = Content.Load<SpriteFont>("galleryFont");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (timer > 0)
            {
                timer -= (float) gameTime.ElapsedGameTime.TotalSeconds; // Type Cast Float
            }

            mState = Mouse.GetState();
            mouseTargetDistance = Vector2.Distance(targetPosition, new Vector2(mState.X, mState.Y));

            if (mState.LeftButton == ButtonState.Pressed && mReleased == true)
            {
                if (mouseTargetDistance < TARGET_RADIUS && timer > 0)
                {
                    score++;

                    Random rand = new Random();

                    targetPosition.X = rand.Next(TARGET_RADIUS, graphics.PreferredBackBufferWidth - TARGET_RADIUS + 1);
                    targetPosition.Y = rand.Next(TARGET_RADIUS, graphics.PreferredBackBufferHeight - TARGET_RADIUS + 1);
                }

                mReleased = false;
            }

            if (mState.LeftButton == ButtonState.Released)
            {
                mReleased = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(background_Sprite, new Vector2(0, 0), Color.White);

            if (timer > 0)
            {
                spriteBatch.Draw(target_Sprite,
                    new Vector2(targetPosition.X - TARGET_RADIUS, targetPosition.Y - TARGET_RADIUS), Color.White);
            }

            spriteBatch.DrawString(gameFont, $"Score: {score}", new Vector2(3,3), Color.White);
            spriteBatch.DrawString(gameFont, $"Time: {Math.Ceiling(timer)}", new Vector2(3,40), Color.White);
            spriteBatch.Draw(crosshair_Sprite, new Vector2(mState.X -25, mState.Y -25), Color.White);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
