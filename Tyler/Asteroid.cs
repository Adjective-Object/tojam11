using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Adventure
{
    public class Asteroid
    {

        public bool Alive = true;
        public Vector2 Position = new Vector2(640, 360);
        public float Rotation = 0;
        public Vector2 Velocity = new Vector2 (0, 0);
        public int Size = 2;
        public int Column = AdventureGame.RandomNumber.Next(4);

        public Asteroid(Vector2 position, Vector2 velocity, int size = 2)
        {
            Position = position;
            Velocity = velocity;
            Size = size;
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity;

            if (Position.X < 0) Position.X = 800;
            if (Position.Y < 0) Position.Y = 600;
            if (Position.X > 800) Position.X = 0;
            if (Position.Y > 600) Position.Y = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 leftCorner = new Vector2 ((1280 - 800) / 2, (720 - 600) / 2);
            spriteBatch.Draw (AdventureGame.tylerSheet, new Vector2((int)(leftCorner.X + Position.X),(int)(leftCorner.Y + Position.Y)) , new Rectangle (Column * 64, 16 + (Size) * 64, 64, 64), Color.White, Rotation, new Vector2(32, 32), 1, SpriteEffects.None, 1);

        }

        public void Hit()
        {
            if (Size == 2)
            {
                AdventureGame.atari.Score += 20;
                AdventureGame.atari.asteroidList.Add(new Asteroid(Position, new Vector2(AdventureGame.RandomFloat(-30, 30, 10), AdventureGame.RandomFloat(-30, 30, 10)), 1));
                AdventureGame.atari.asteroidList.Add(new Asteroid(Position, new Vector2(AdventureGame.RandomFloat(-30, 30, 10), AdventureGame.RandomFloat(-30, 30, 10)), 1));
            }
            else if (Size == 1)
            {
                AdventureGame.atari.Score += 50;
                AdventureGame.atari.asteroidList.Add(new Asteroid(Position, new Vector2(AdventureGame.RandomFloat(-30, 30, 10), AdventureGame.RandomFloat(-30, 30, 10)), 0));
                AdventureGame.atari.asteroidList.Add(new Asteroid(Position, new Vector2(AdventureGame.RandomFloat(-30, 30, 10), AdventureGame.RandomFloat(-30, 30, 10)), 0));
            }
            else
            {
                AdventureGame.atari.Score += 100;
            }

            Alive = false;
        }
    }
}
