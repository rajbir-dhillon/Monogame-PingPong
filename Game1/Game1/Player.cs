using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    public class Player : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        GraphicsDevice graphics;
        private Texture2D _texture;

        int width;
        int heigth;
        public int posX { get; set; }
        public int posY { get; set; }
        public int speed = 5;
        Rectangle rectangle;
        public Player(GraphicsDevice graphics, SpriteBatch spriteBatch, Game game, int width, int height, Vector2 position) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;
            this.posY = (int)position.Y;
            this.posX = (int)position.X;
            this.width = width;
            this.heigth = height;

            _texture = new Texture2D(graphics, 1, 1);
            _texture.SetData(new Color[] { Color.White });
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            rectangle = new Rectangle(posX, posY, width, heigth);
            spriteBatch.Draw(_texture, rectangle, Color.White);
            base.Draw(gameTime);

        }

        public void KeyUp()
        {
            posY -= speed;
        }
        public void KeyDown()
        {
            posY += speed;   
        }
    }
}
