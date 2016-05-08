using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Adventure
{
    public class Particle
    {
        public bool Alive = true;
        public Vector2 Position = new Vector2(640, 360);
        public float Rotation = 0;
        public Vector2 Velocity = new Vector2 (0, 0);
        public int type = 0;

        public int Timer = AdventureGame.RandomNumber.Next(500,1500);

        public Particle(Vector2 position, Vector2 velocity, int typee = 0)
        {
            Position = position;
            Velocity = velocity;
            type = typee;
            if (type == 1)
                Rotation = 2;
            else if (type == 2)
                Rotation = -2;
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity;

            Velocity.X = MathHelper.Lerp(Velocity.X, 0, .01f);
            Velocity.Y = MathHelper.Lerp(Velocity.Y, 0, .01f);


            Timer -= gameTime.ElapsedGameTime.Milliseconds;
            if (Timer < 0)
                Alive = false;
            

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
            if (type == 0)
                spriteBatch.Draw(AdventureGame.tylerSquare, leftCorner + Position, new Rectangle(0, 0, 1, 1), Color.White, Rotation, new Vector2(0, 0), 1, SpriteEffects.None, 1);
            else 
                spriteBatch.Draw(AdventureGame.tylerSquare, leftCorner + Position, new Rectangle(0, 0, 14, 1), Color.White, Rotation, new Vector2(7, 0), 1, SpriteEffects.None, 1);
        }
    }
}
