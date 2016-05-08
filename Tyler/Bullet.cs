using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Adventure
{
    public class Bullet
    {
        public bool Alive = true;
        public Vector2 Position = new Vector2(640, 360);
        public float Rotation = 0;
        public Vector2 Velocity = new Vector2 (0, 0);
        public bool isPlayers = true;

        public Bullet(Vector2 position, Vector2 velocity, bool isplayers = true)
        {
            Position = position;
            Velocity = velocity;
            isPlayers = isplayers;
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity;

            if (Position.X < 0)
                Alive = false;
            if (Position.Y < 0)
                Alive = false;
            if (Position.X > 800)
                Alive = false;
            if (Position.Y > 600)
                Alive = false;


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 leftCorner = new Vector2((1280 - 800) / 2, (720 - 600) / 2);
            spriteBatch.Draw(AdventureGame.tylerSquare, leftCorner + Position, new Rectangle(0, 0, 1, 1), Color.White, Rotation, new Vector2(0, 0), 1, SpriteEffects.None, 1);

        }
    }
}
