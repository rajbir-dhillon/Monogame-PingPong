using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    public class Ball : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        GraphicsDevice graphics;
        Texture2D texture;
        Random rand;

        int ballSize;

        public bool gameRun { get; set; }
        public int dirx { get; set; }
        public int diry { get; set; }
        public int posx { get; set; }
        public int posy { get; set; }
        public int speed { get; set; } = 1;
        Rectangle rectangle;

        public Ball(GraphicsDevice graphics, SpriteBatch spritebatch, Game game, int ballSize) : base(game)
        {
            this.spriteBatch = spritebatch;
            this.graphics = graphics;
            this.ballSize = ballSize;

            texture = new Texture2D(graphics, 1, 1);
            texture.SetData(new Color[] { Color.White });

            rand = new Random();
        }

        public void ResetDirection()
        {
            
            do
            {
                dirx = rand.Next(-10, 10);
            } while (dirx == 0);
            do
            {
                diry = rand.Next(-10, 10);
            } while (diry == 0);


        }

        public override void Draw(GameTime gameTime)
        {
            rectangle = new Rectangle(posx, posy, ballSize, ballSize);
            spriteBatch.Draw(texture, rectangle, Color.White);

            base.Draw(gameTime);
        }

        public void ResetBall()
        {
            posx = graphics.Viewport.Width / 2 - ballSize / 2;
            posy = graphics.Viewport.Height / 2 - ballSize / 2;
            gameRun = false;
            ResetDirection();
        }

        public void CheckCollision()
        {
            //top bottom collision
            if (posy <= 0 || posy + ballSize > graphics.Viewport.Height)
            {
                diry = -diry;
            }
            if (posx <= 0)
            {
                ResetBall();
                Game1.Player2score();
            }
            if (posx + ballSize > graphics.Viewport.Width){
                ResetBall();
                Game1.Player1score();
            }


        }
        public override void Update(GameTime gameTime)
        {
            
            posy += diry * speed;
            posx += dirx * speed;

            CheckCollision();
            base.Update(gameTime);
        }
    }
}
