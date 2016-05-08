using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Adventure
{
	public class AtariSystem
	{
		public AtariSystem ()
		{
            blockList.Clear();
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    Color col = Color.Pink;
                    if (y == 1) col = Color.LightSalmon;
                    else if (y == 2) col = Color.Orange;
                    else if (y == 3) col = Color.Yellow;
                    else if (y == 4) col = Color.LightGreen;
                    else if (y == 5) col = Color.LightBlue;
                    
                    blockList.Add(new Block(new Point(x * 30, 80 + y * 18), col));
                }
            }
        }

		Vector2 leftCorner = new Vector2 ((1280 - 800) / 2, (720 - 600) / 2);

		Point ScreenSize = new Point (800, 600);
        public bool Running2 = false;
        public bool Running = false;
		public Vector2 Size = new Vector2 (0, 0);
		public int systemState = -1;
		float titleAlpha = 0;


		Vector2 Position = new Vector2(400, 300);
		float Rotation = 0;
		Vector2 Velocity = new Vector2 (0, 0);
		bool Alive = false;
		int Timer = 0;


		public List<Bullet> bulletList = new List<Bullet>();
		public List<Asteroid> asteroidList = new List<Asteroid>();
		public List<Particle> particleList = new List<Particle>();


		public int Score = 0;
		public int TopScore = 0;
		public int Lives = 3;

		public string[] letters = new string[] { "A", "A", "A" };
		public int letIndex = 0;

        public int gameSelection = 1;


        public class Block
        {
            public Block(Point pos, Color Col)
            {
                position = pos;
                colour = Col;
            }

            public Point position;
            public Color colour;
        }
        public List<Block> blockList = new List<Block>();
        Vector2 ballPositon = new Vector2(-1, -1);
        Vector2 ballVelocity = new Vector2(0, 0);
        Vector2 paddlePosition = new Vector2(300 - 40, 500 - 50);

        int ignoreCollision = 0;


		public void Update(GameTime gameTime)
		{
			if (Running2) {
				
                Running = true;

				#region turning tv on
                if (systemState == 0)
                {

                    if (Timer < 1)
                    {
                        if (Size.X < ScreenSize.X - 1)
                        {
                            Size.X = MathHelper.Lerp(Size.X, ScreenSize.X, .4f);
                            Size.Y = MathHelper.Lerp(Size.Y, 40, .2f);
                        }
                        else
                        {
                            Size.X = ScreenSize.X;
                            systemState = 1;
                        }
                    }
                    else Timer -= gameTime.ElapsedGameTime.Milliseconds;

                }
                else if (systemState == 1)
                {
                    if (Size.Y < ScreenSize.Y - 1)
                    {
                        Size.Y = MathHelper.Lerp(Size.Y, ScreenSize.Y, .4f);
                    }
                    else
                    {
                        Size.Y = ScreenSize.Y;
                        systemState = 2;
                    }
                }
                else if (systemState == 2)
                {

                    if (titleAlpha < 3.2f)
                        titleAlpha += .05f;
                    else
                    {
                        systemState = 3;
                        if(gameSelection == 1) Alive = true;
                    }
                }
                else if (systemState == 3)
                {
                    if (titleAlpha > 0f)
                        titleAlpha -= .05f;
                    else
                    {
                        systemState = 4;


                        asteroidList.Add(new Asteroid(new Vector2(AdventureGame.RandomFloat(0, 800, 1), AdventureGame.RandomFloat(0, 600, 1)), new Vector2(AdventureGame.RandomFloat(-30, 30, 10), AdventureGame.RandomFloat(-30, 30, 10)), 2));
                        asteroidList.Add(new Asteroid(new Vector2(AdventureGame.RandomFloat(0, 800, 1), AdventureGame.RandomFloat(0, 600, 1)), new Vector2(AdventureGame.RandomFloat(-30, 30, 10), AdventureGame.RandomFloat(-30, 30, 10)), 2));
                        asteroidList.Add(new Asteroid(new Vector2(AdventureGame.RandomFloat(0, 800, 1), AdventureGame.RandomFloat(0, 600, 1)), new Vector2(AdventureGame.RandomFloat(-30, 30, 10), AdventureGame.RandomFloat(-30, 30, 10)), 2));
                    }
                }
				#endregion


				else if (systemState > 3)
                {
                    #region asteroids logic
                    if (gameSelection == 0)
                    {
                        if (Velocity.X > 0)
                            Velocity.X -= .01f;
                        if (Velocity.Y > 0)
                            Velocity.Y -= .01f;
                        if (Velocity.X < 0)
                            Velocity.X += .01f;
                        if (Velocity.Y < 0)
                            Velocity.Y += .01f;

                        Position += Velocity;

                        if (Position.X < 0)
                            Position.X = 800;
                        if (Position.Y < 0)
                            Position.Y = 600;
                        if (Position.X > 800)
                            Position.X = 0;
                        if (Position.Y > 600)
                            Position.Y = 0;



                        if (systemState == 5)
                        {
                            if (Alive)
                            {
                                
                                int containsBig = 0;
                                for(int i = 0; i < asteroidList.Count;i++)
                                {
                                    if(asteroidList[i].Size == 2) containsBig++;
                                }

                                if(containsBig < 2 && asteroidList.Count < 8) 
                                {
                                    int rand = AdventureGame.RandomNumber.Next(4);
                                    if(rand == 0)
                                    {
                                        asteroidList.Add(new Asteroid(new Vector2(AdventureGame.RandomFloat(0, 800, 1), 0), new Vector2(AdventureGame.RandomFloat(-30, 30, 10), AdventureGame.RandomFloat(-30, 30, 10)), 2));
                                    }
                                    if(rand == 1)
                                    {
                                        asteroidList.Add(new Asteroid(new Vector2(AdventureGame.RandomFloat(0, 800, 1), 600), new Vector2(AdventureGame.RandomFloat(-30, 30, 10), AdventureGame.RandomFloat(-30, 30, 10)), 2));
                                    }
                                    if(rand == 2)
                                    {
                                        asteroidList.Add(new Asteroid(new Vector2(0, AdventureGame.RandomFloat(0, 600, 1)), new Vector2(AdventureGame.RandomFloat(-30, 30, 10), AdventureGame.RandomFloat(-30, 30, 10)), 2));
                                    }
                                    if(rand == 3)
                                    {
                                       asteroidList.Add(new Asteroid(new Vector2(800, AdventureGame.RandomFloat(0, 600, 1)), new Vector2(AdventureGame.RandomFloat(-30, 30, 10), AdventureGame.RandomFloat(-30, 30, 10)), 2));
                                    }
                                }


                                if (TInput.IsKeyDown(Keys.Right))
                                    Rotation += .1f;
                                if (TInput.IsKeyDown(Keys.Left))
                                    Rotation -= .1f;


                                if (TInput.IsKeyDown(Keys.Up))
                                {
                                    Vector2 desVel = AngleToVector(Rotation) * .1f;

                                    Velocity += desVel;
                                }

                                if (TInput.KeyPressed(Keys.Enter))
                                {
                                    Vector2 vel = AngleToVector(Rotation) * 5;
                                    bulletList.Add(new Bullet(Position, vel));
                                }

                                if (Velocity.X > 6)
                                    Velocity.X = 6;
                                if (Velocity.X < -6)
                                    Velocity.X = -6;
                                if (Velocity.Y > 6)
                                    Velocity.Y = 6;
                                if (Velocity.Y < -6)
                                    Velocity.Y = -6;
                            }
                            else
                            {
                                if (TInput.KeyPressed(Keys.Enter))
                                {
                                    if (Lives > 0)
                                    {
                                        Alive = true;
                                        Lives--;
                                        Position = new Vector2(400, 300);
                                        Velocity = new Vector2(0, 0);
                                    }
                                }
                            }
                        }
                        else if (systemState == 4)
                        {
                            if (TInput.KeyPressed(Keys.Enter))
                            {
                                NewGame();
                            }
                        }
                        else if (systemState == 6)
                        {
                            if (TInput.KeyPressed(Keys.Enter))
                            {
                                systemState = 7;
                                letters = new string[] { "A", "A", "A" };
                                letIndex = 0;
                            }
                        }
                        else if (systemState == 7)
                        {
                            #region entering letters up down
                            if (TInput.KeyPressed(Keys.Up))
                            {
                                if (letters[letIndex] == "-")
                                    letters[letIndex] = "A";
                                else if (letters[letIndex] == "A")
                                    letters[letIndex] = "B";
                                else if (letters[letIndex] == "B")
                                    letters[letIndex] = "C";
                                else if (letters[letIndex] == "C")
                                    letters[letIndex] = "D";
                                else if (letters[letIndex] == "D")
                                    letters[letIndex] = "E";
                                else if (letters[letIndex] == "E")
                                    letters[letIndex] = "F";
                                else if (letters[letIndex] == "F")
                                    letters[letIndex] = "G";
                                else if (letters[letIndex] == "G")
                                    letters[letIndex] = "H";
                                else if (letters[letIndex] == "H")
                                    letters[letIndex] = "I";
                                else if (letters[letIndex] == "I")
                                    letters[letIndex] = "J";
                                else if (letters[letIndex] == "J")
                                    letters[letIndex] = "K";
                                else if (letters[letIndex] == "K")
                                    letters[letIndex] = "L";
                                else if (letters[letIndex] == "L")
                                    letters[letIndex] = "M";
                                else if (letters[letIndex] == "M")
                                    letters[letIndex] = "N";
                                else if (letters[letIndex] == "N")
                                    letters[letIndex] = "O";
                                else if (letters[letIndex] == "O")
                                    letters[letIndex] = "P";
                                else if (letters[letIndex] == "P")
                                    letters[letIndex] = "Q";
                                else if (letters[letIndex] == "Q")
                                    letters[letIndex] = "R";
                                else if (letters[letIndex] == "R")
                                    letters[letIndex] = "S";
                                else if (letters[letIndex] == "S")
                                    letters[letIndex] = "T";
                                else if (letters[letIndex] == "T")
                                    letters[letIndex] = "U";
                                else if (letters[letIndex] == "U")
                                    letters[letIndex] = "V";
                                else if (letters[letIndex] == "V")
                                    letters[letIndex] = "W";
                                else if (letters[letIndex] == "W")
                                    letters[letIndex] = "X";
                                else if (letters[letIndex] == "X")
                                    letters[letIndex] = "Y";
                                else if (letters[letIndex] == "Y")
                                    letters[letIndex] = "Z";
                                else if (letters[letIndex] == "Z")
                                    letters[letIndex] = "-";
                            }
                            if (TInput.KeyPressed(Keys.Down))
                            {
                                if (letters[letIndex] == "-")
                                    letters[letIndex] = "Z";
                                else if (letters[letIndex] == "A")
                                    letters[letIndex] = "-";
                                else if (letters[letIndex] == "B")
                                    letters[letIndex] = "A";
                                else if (letters[letIndex] == "C")
                                    letters[letIndex] = "B";
                                else if (letters[letIndex] == "D")
                                    letters[letIndex] = "C";
                                else if (letters[letIndex] == "E")
                                    letters[letIndex] = "D";
                                else if (letters[letIndex] == "F")
                                    letters[letIndex] = "E";
                                else if (letters[letIndex] == "G")
                                    letters[letIndex] = "F";
                                else if (letters[letIndex] == "H")
                                    letters[letIndex] = "G";
                                else if (letters[letIndex] == "I")
                                    letters[letIndex] = "H";
                                else if (letters[letIndex] == "J")
                                    letters[letIndex] = "I";
                                else if (letters[letIndex] == "K")
                                    letters[letIndex] = "J";
                                else if (letters[letIndex] == "L")
                                    letters[letIndex] = "K";
                                else if (letters[letIndex] == "M")
                                    letters[letIndex] = "L";
                                else if (letters[letIndex] == "N")
                                    letters[letIndex] = "M";
                                else if (letters[letIndex] == "O")
                                    letters[letIndex] = "N";
                                else if (letters[letIndex] == "P")
                                    letters[letIndex] = "O";
                                else if (letters[letIndex] == "Q")
                                    letters[letIndex] = "P";
                                else if (letters[letIndex] == "R")
                                    letters[letIndex] = "Q";
                                else if (letters[letIndex] == "S")
                                    letters[letIndex] = "R";
                                else if (letters[letIndex] == "T")
                                    letters[letIndex] = "S";
                                else if (letters[letIndex] == "U")
                                    letters[letIndex] = "T";
                                else if (letters[letIndex] == "V")
                                    letters[letIndex] = "U";
                                else if (letters[letIndex] == "W")
                                    letters[letIndex] = "V";
                                else if (letters[letIndex] == "X")
                                    letters[letIndex] = "W";
                                else if (letters[letIndex] == "Y")
                                    letters[letIndex] = "X";
                                else if (letters[letIndex] == "Z")
                                    letters[letIndex] = "Y";
                            }
                            #endregion

                            if (TInput.KeyPressed(Keys.Enter))
                            {
                                if (letIndex < 2)
                                    letIndex++;
                                else
                                {
                                    NewGame();
                                }
                            }
                        }




                        for (int i = 0; i < bulletList.Count; i++)
                        {
                            for (int y = 0; y < 2; y++)
                            {
                                bulletList[i].Update(gameTime);

                                if (!bulletList[i].Alive)
                                    break;
							
                                for (int p = 0; p < asteroidList.Count; p++)
                                {
                                    float distanceaway = Vector2.Distance(asteroidList[p].Position, bulletList[i].Position);

                                    if (distanceaway < 6 || (distanceaway < 12 && asteroidList[p].Size > 0) || (distanceaway < 30 && asteroidList[p].Size > 1))
                                    {
                                        bulletList[i].Alive = false;
                                        asteroidList[p].Hit();

                                        for (int u = 0; u < AdventureGame.RandomNumber.Next(8, 15); u++)
                                        {
                                            particleList.Add(new Particle(asteroidList[p].Position, AngleToVector(AdventureGame.RandomFloat(-314, 314, 100)) * AdventureGame.RandomFloat(10, 30, 10)));
                                        }
                                    }
                                }
                            }

                            if (!bulletList[i].Alive)
                            {
                                bulletList.RemoveAt(i);
                                i--;
                            }
                        }



                        for (int i = 0; i < asteroidList.Count; i++)
                        {
                            asteroidList[i].Update(gameTime);

                            if (Alive)
                            {
                                float distanceaway = Vector2.Distance(asteroidList[i].Position, Position);

                                if (distanceaway < 6 || (distanceaway < 12 && asteroidList[i].Size > 0) || (distanceaway < 30 && asteroidList[i].Size > 1))
                                {
                                    Alive = false;

                                    for (int u = 0; u < 3; u++)
                                    {
                                        particleList.Add(new Particle(asteroidList[i].Position, AngleToVector(AdventureGame.RandomFloat(-314, 314, 100)) * AdventureGame.RandomFloat(10, 30, 50), 1 + u));
                                    }

                                    if (Lives == 0)
                                    {
                                        systemState = 6;
                                    }
                                }
                            }

                            if (!asteroidList[i].Alive)
                            {
                                asteroidList.RemoveAt(i);
                                i--;
                            }
                        }





                        for (int i = 0; i < particleList.Count; i++)
                        {
                            particleList[i].Update(gameTime);
                            if (!particleList[i].Alive)
                            {
                                particleList.RemoveAt(i);
                                i--;
                            }
                        }



                    }
                    #endregion

                    #region breakout logic
                    else if (gameSelection == 1)
                    {
                        if(Alive)
                        {
                            
                            if (TInput.IsKeyDown(Keys.Right))
                            {
                                paddlePosition.X += 10;
                                if(paddlePosition.X > 600 - 80) paddlePosition.X = 600 - 80;
                            }
                            if (TInput.IsKeyDown(Keys.Left))
                            {
                                paddlePosition.X -= 10;
                                if(paddlePosition.X < 0) paddlePosition.X = 0;
                            }

                            if (TInput.KeyPressed(Keys.Enter))
                            {
                                if (ballPositon.X == -1 && ballPositon.Y == -1)
                                {
                                    ballVelocity = new Vector2(4, -4);

                                    ballPositon = new Vector2(paddlePosition.X + 32, paddlePosition.Y - 15);
                                }
                            }



                            if (ballPositon.X == -1 && ballPositon.Y == -1){}
                            else
                            {
                                Rectangle paddleRect = new Rectangle((int)paddlePosition.X, (int)paddlePosition.Y, 80, 15);
                                Rectangle ballRect = new Rectangle((int)ballPositon.X, (int)ballPositon.Y, 15, 15);

                              

                                ballPositon.X += ballVelocity.X;
                                if(ignoreCollision < 1)
                                {

                                    paddleRect = new Rectangle((int)paddlePosition.X, (int)paddlePosition.Y, 80, 15);
                                    ballRect = new Rectangle((int)ballPositon.X, (int)ballPositon.Y, 15, 15);

                                    if(paddleRect.Intersects(ballRect))
                                    {
                                        ignoreCollision = 1000;

                                        if(ballPositon.X + 7 < paddlePosition.X + 20)
                                        {

                                        }

                                        ballVelocity.Y *= -1;
                                        ballVelocity.X *= -1;
                                        if(ballPositon.X < paddlePosition.X)
                                            ballPositon.X = paddlePosition.X - 15;
                                        else ballPositon.X = paddleRect.X + 80;
                                    }
                                }
                                else ignoreCollision -= gameTime.ElapsedGameTime.Milliseconds;

                                for(int i = 0; i < blockList.Count;i++)
                                {
                                    paddleRect = new Rectangle((int)blockList[i].position.X, (int)blockList[i].position.Y, 30, 18);
                                    if(paddleRect.Intersects(ballRect))
                                    {
                                        ballVelocity.X *= -1;
                                        blockList.RemoveAt(i);
                                        Score+= 1;
                                        break;
                                    }
                                }





                                ballPositon.Y += ballVelocity.Y;
                                if(ignoreCollision < 1)
                                {

                                    paddleRect = new Rectangle((int)paddlePosition.X, (int)paddlePosition.Y, 80, 15);
                                    ballRect = new Rectangle((int)ballPositon.X, (int)ballPositon.Y, 15, 15);

                                    if(paddleRect.Intersects(ballRect))
                                    {

//                                        if(ballPositon.X + 7 < paddlePosition.X + 20)
//                                        {
//                                            if(Velocity.X > 0) Velocity.X *= -1;
//                                        }
//                                        if(ballPositon.X + 7 > paddlePosition.X + 60)
//                                        {
//                                            if(Velocity.X < 0) Velocity.X *= -1;
//                                        }

                                        ignoreCollision = 1000;
                                        ballVelocity.Y *= -1;
                                        ballPositon.Y = paddlePosition.Y - 15;
                                    }
                                }
                                else ignoreCollision -= gameTime.ElapsedGameTime.Milliseconds;


                                for(int i = 0; i < blockList.Count;i++)
                                {
                                    paddleRect = new Rectangle((int)blockList[i].position.X, (int)blockList[i].position.Y, 30, 18);
                                    if(paddleRect.Intersects(ballRect))
                                    {
                                        ballVelocity.Y *= -1;
                                        blockList.RemoveAt(i);
                                        Score+= 1;
                                        break;
                                    }
                                }




                                if(ballPositon.X > 600 - 15)
                                {
                                    ballPositon.X = 600 - 15;
                                    ballVelocity.X *= -1;
                                }
                                if(ballPositon.X < 0)
                                {
                                    ballPositon.X = 0;
                                    ballVelocity.X *= -1;
                                }
                                if(ballPositon.Y > 500 - 15)
                                {
                                    ballPositon.Y = 500 - 15;
                                    Alive = false;
                                }
                                if(ballPositon.Y < 0)
                                {
                                    ballPositon.Y = 0;
                                    ballVelocity.Y *= -1;
                                }
                            }

                        }
                        else
                        {

                            if (TInput.KeyPressed(Keys.Enter))
                            {
                                if(Lives > 0)
                                {
                                    Lives--;
                                    Alive = true;
                                    ballPositon = new Vector2(-1, -1);
                                    ballVelocity = new Vector2(0,0);
                                }
                                else
                                {
                                    NewGame();
                                }
                            }
                        }
                    }
                    #endregion



                    if (TInput.KeyPressed(Keys.Escape))
                    {
                        Running2 = false;
                        systemState = 1;
                        Timer = 1000;
                    }
                }

			} 


			#region closing tv
			else {
				
				if (systemState == 0) {

					if(Timer > 0)
					{
						Timer -=  gameTime.ElapsedGameTime.Milliseconds;
						if(Timer < 1)
						{
							systemState = -1;
                            Running = false;
						}
					}
					else
					{
						if (Size.X > 1) {
							Size.X = MathHelper.Lerp (Size.X, 0, .4f);
							Size.Y = MathHelper.Lerp (Size.Y, 0, .2f);
						} else {
							Size.X = 0;
							Size.Y = 0;
							Timer = 200;
						}
					}

				} else if (systemState == 1) {

					if (Size.Y > 41) {
						Size.Y = MathHelper.Lerp (Size.Y, 40, .4f);
					} else {
						Size.Y = 40;
						systemState = 0;
						Timer = 0;
					}

				}

			}
			#endregion
		}


		public void StartGame(int game) {
			Running = true;
			Running2 = true;
			Timer = 200;
			if (systemState < 0)
				systemState = 0;
			gameSelection = game;
		}

        public void NewGame()
        {
            Score = 0;
            if (gameSelection == 1)
            {
                blockList.Clear();
                for (int x = 0; x < 20; x++)
                {
                    for (int y = 0; y < 6; y++)
                    {
                        Color col = Color.Pink;
                        if (y == 1) col = Color.LightSalmon;
                        else if (y == 2) col = Color.Orange;
                        else if (y == 3) col = Color.Yellow;
                        else if (y == 4) col = Color.LightGreen;
                        else if (y == 5) col = Color.LightBlue;

                        blockList.Add(new Block(new Point(x * 30, 80 + y * 18), col));
                    }
                }

                Lives = 3;
                Alive = true;
                ballPositon = new Vector2(-1, -1);
                ballVelocity = new Vector2(0, 0);
                paddlePosition = new Vector2(300 - 40, 500 - 50);
            }
            else
            {
                Alive = true;
                Position = new Vector2(400, 300);
                Velocity = new Vector2(0, 0);
                systemState = 5;
                Lives = 3;

                asteroidList.Clear();
                asteroidList.Add(new Asteroid(new Vector2(AdventureGame.RandomFloat(0, 800, 1), AdventureGame.RandomFloat(0, 600, 1)), new Vector2(AdventureGame.RandomFloat(-30, 30, 10), AdventureGame.RandomFloat(-30, 30, 10)), 2));
                asteroidList.Add(new Asteroid(new Vector2(AdventureGame.RandomFloat(0, 800, 1), AdventureGame.RandomFloat(0, 600, 1)), new Vector2(AdventureGame.RandomFloat(-30, 30, 10), AdventureGame.RandomFloat(-30, 30, 10)), 2));
                asteroidList.Add(new Asteroid(new Vector2(AdventureGame.RandomFloat(0, 800, 1), AdventureGame.RandomFloat(0, 600, 1)), new Vector2(AdventureGame.RandomFloat(-30, 30, 10), AdventureGame.RandomFloat(-30, 30, 10)), 2));
            
            }
        }

		public void Draw(SpriteBatch spriteBatch)
		{
			if (systemState > -1) 
			spriteBatch.Draw(AdventureGame.tylerSquare, new Vector2(640 + 50, 360 + 35), new Rectangle(0, 0, 800, 650), Color.DarkGray, 0, new Vector2(900, 700) / 2, 1, SpriteEffects.None, 1);



			spriteBatch.Draw (AdventureGame.tylerSquare, new Vector2(640, 360), new Rectangle (0, 0, (int)Size.X, (int)Size.Y), Color.Black, 0, new Vector2((int)Size.X, (int)Size.Y) / 2, 1, SpriteEffects.None, 1);

			if(systemState > 2 || (systemState == 2 && titleAlpha > 2))
				spriteBatch.DrawString (AdventureGame.tylerFont, "ROBOTHAUS!", new Vector2 (640, 360), Color.White * titleAlpha, 0, AdventureGame.tylerFont.MeasureString ("ROBOTHAUS") / 2, 2, SpriteEffects.None, 1);
			else
				spriteBatch.DrawString (AdventureGame.tylerFont, "ROBOTHAUS", new Vector2 (640, 360), Color.White * titleAlpha, 0, AdventureGame.tylerFont.MeasureString ("ROBOTHAUS") / 2, 2, SpriteEffects.None, 1);


            #region asteroids draw
            if (gameSelection == 0)
            {
                if (systemState > 3)
                {

                    for (int i = 0; i < bulletList.Count; i++)
                        bulletList[i].Draw(spriteBatch);
                    for (int i = 0; i < asteroidList.Count; i++)
                        asteroidList[i].Draw(spriteBatch);
                    for (int i = 0; i < particleList.Count; i++)
                        particleList[i].Draw(spriteBatch);

                    if (systemState == 5)
                    {
                        if (Alive)
                        {
                            int frame = 0;
                            if (TInput.IsKeyDown(Keys.Up))
                                frame = 1;
                            spriteBatch.Draw(AdventureGame.tylerSheet, leftCorner + Position, new Rectangle(frame * 16, 208, 16, 24), Color.White, Rotation, new Vector2(8, 8), 1, SpriteEffects.None, 1);
                        }
                    }


                    // score
                    if (Score == 0)
                        spriteBatch.DrawString(AdventureGame.tylerFont, "0" + Score.ToString(), leftCorner + new Vector2(120, 60), Color.White, 0, AdventureGame.tylerFont.MeasureString("0" + Score.ToString()) / 1, 1.5f, SpriteEffects.None, 1);
                    else
                        spriteBatch.DrawString(AdventureGame.tylerFont, Score.ToString(), leftCorner + new Vector2(120, 60), Color.White, 0, AdventureGame.tylerFont.MeasureString(Score.ToString()) / 1, 1.5f, SpriteEffects.None, 1);
                    // lives
                    for (int i = 0; i < Lives; i++)
                        spriteBatch.Draw(AdventureGame.tylerSheet, leftCorner + new Vector2(100 - i * 17, 80), new Rectangle(0, 208, 16, 16), Color.White, 0, new Vector2(8, 8), 1, SpriteEffects.None, 1);



                    // top score
                    if (TopScore == 0)
                        spriteBatch.DrawString(AdventureGame.tylerFont, "0" + TopScore.ToString(), leftCorner + new Vector2(400, 60), Color.White * .5f, 0, AdventureGame.tylerFont.MeasureString("0" + TopScore.ToString()) / 1, 1f, SpriteEffects.None, 1);
                    else
                        spriteBatch.DrawString(AdventureGame.tylerFont, TopScore.ToString(), leftCorner + new Vector2(400, 60), Color.White * .5f, 0, AdventureGame.tylerFont.MeasureString(TopScore.ToString()) / 1, 1f, SpriteEffects.None, 1);

                    // 2016 corp
                    spriteBatch.DrawString(AdventureGame.tylerFont, "2016 ROBOTHAUS CORP", leftCorner + new Vector2(400, 550), Color.White * .5f, 0, AdventureGame.tylerFont.MeasureString("2016 ROBOTHAUS CORP") / 2, 1f, SpriteEffects.None, 1);

                }


                if (systemState == 4)
                {
				
                    spriteBatch.DrawString(AdventureGame.tylerFont, "PRESS ENTER", leftCorner + new Vector2(400, 160), Color.White, 0, AdventureGame.tylerFont.MeasureString("PRESS START") / 2, 1.5f, SpriteEffects.None, 1);

                }
                else if (systemState == 6)
                {
				
                    spriteBatch.DrawString(AdventureGame.tylerFont, "GAME OVER", leftCorner + new Vector2(400, 160), Color.White, 0, AdventureGame.tylerFont.MeasureString("GAME OVER") / 2, 1.5f, SpriteEffects.None, 1);

                }
                else if (systemState == 7)
                {
				
                    spriteBatch.DrawString(AdventureGame.tylerFont, "YOUR SCORE IS ONE OF THE TEN BEST", leftCorner + new Vector2(100, 200), Color.White, 0, AdventureGame.tylerFont.MeasureString("") / 2, 1.5f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(AdventureGame.tylerFont, "PLEASE ENTER YOUR INITIALS", leftCorner + new Vector2(100, 200 + 40), Color.White, 0, AdventureGame.tylerFont.MeasureString("") / 2, 1.5f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(AdventureGame.tylerFont, "PRESS UP/DOWN TO CHANGE LETTER", leftCorner + new Vector2(100, 200 + 40 + 40), Color.White, 0, AdventureGame.tylerFont.MeasureString("") / 2, 1.5f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(AdventureGame.tylerFont, "PRESS X WHEN LETTER IS CORECT", leftCorner + new Vector2(100, 200 + 40 + 40 + 40), Color.White, 0, AdventureGame.tylerFont.MeasureString("") / 2, 1.5f, SpriteEffects.None, 1);


                    spriteBatch.DrawString(AdventureGame.tylerFont, letters[0], leftCorner + new Vector2(400 - 60, 500), Color.White, 0, AdventureGame.tylerFont.MeasureString("") / 2, 1.5f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(AdventureGame.tylerFont, letters[1], leftCorner + new Vector2(400 - 20, 500), Color.White, 0, AdventureGame.tylerFont.MeasureString("") / 2, 1.5f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(AdventureGame.tylerFont, letters[2], leftCorner + new Vector2(400 + 20, 500), Color.White, 0, AdventureGame.tylerFont.MeasureString("") / 2, 1.5f, SpriteEffects.None, 1);




                }


            }
            #endregion

            #region breakout draw
            else if (gameSelection == 1)
            {
                if (systemState > 3)
                {

                    spriteBatch.Draw (AdventureGame.tylerSquare, new Vector2(640, 360 + 25), new Rectangle (0, 0, (int)Size.X - 100, (int)Size.Y - 50), Color.Gray, 0, new Vector2((int)Size.X - 100, (int)Size.Y - 50) / 2, 1, SpriteEffects.None, 1);

                    spriteBatch.Draw (AdventureGame.tylerSquare, new Vector2(640 - 350,  360 + 300 - 30), new Rectangle (0, 0, 100, 30), Color.Green, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);
                    spriteBatch.Draw (AdventureGame.tylerSquare, new Vector2(640 + 250,  360 + 300 - 30), new Rectangle (0, 0, 100, 30), Color.Pink, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

                    spriteBatch.Draw (AdventureGame.tylerSquare, new Vector2(640, 360 + 50), new Rectangle (0, 0, (int)Size.X - 200, (int)Size.Y - 100), Color.Black, 0, new Vector2((int)Size.X - 200, (int)Size.Y - 100) / 2, 1, SpriteEffects.None, 1);

                    string score = Score.ToString();
                    if(score.Length < 3) score = "0" + score;
                    if(score.Length < 3) score = "0" + score;
                    spriteBatch.DrawString (AdventureGame.tylerFont3, score, new Vector2 (640 - 50, 360 - 300), Color.White, 0, new Vector2(AdventureGame.tylerFont.MeasureString (score).X / 2,0), 3, SpriteEffects.None, 1);
                    spriteBatch.DrawString (AdventureGame.tylerFont3, Lives.ToString(), new Vector2 (640 + 50, 360 - 300), Color.White, 0, AdventureGame.tylerFont.MeasureString ("") / 2, 3, SpriteEffects.None, 1);
                    spriteBatch.DrawString (AdventureGame.tylerFont3, "1", new Vector2 (640 + 180, 360 - 300), Color.White, 0, AdventureGame.tylerFont.MeasureString ("") / 2, 3, SpriteEffects.None, 1);



                    // play screen = 600, 500

                    for(int i = 0; i < blockList.Count;i++)
                    {
                        spriteBatch.Draw (AdventureGame.tylerSquare, new Vector2( 640 - 300 + blockList[i].position.X, 360 - 200 + blockList[i].position.Y), new Rectangle (0, 0, 30, 18), blockList[i].colour, 0, 
                            new Vector2(0, 0), 1, SpriteEffects.None, 1);
                    }

                    if (ballPositon.X == -1 && ballPositon.Y == -1)
                        spriteBatch.Draw (AdventureGame.tylerSquare, new Vector2(640 - 300 + paddlePosition.X + 32, 360 - 200 + paddlePosition.Y - 15), new Rectangle (0, 0, 15, 15), 
                            Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);
                    else
                        spriteBatch.Draw (AdventureGame.tylerSquare, new Vector2(640 - 300 + ballPositon.X, 360 - 200 + ballPositon.Y), new Rectangle (0, 0, 15, 15), 
                            Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);
                    
                    spriteBatch.Draw (AdventureGame.tylerSquare, new Vector2(640 - 300 + paddlePosition.X, 360 - 200 + paddlePosition.Y), new Rectangle (0, 0, 80, 15), 
                        Color.Pink, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

                    if(!Alive && Lives == 0)
                    {

                    }
                }
            }
            #endregion







            // tv border
            if (systemState > -1)
                spriteBatch.Draw(AdventureGame.tylerSheet, new Vector2(640 - 2, 360 - 3), new Rectangle(0, 324, 1030, 700), Color.White, 0, new Vector2(900, 700) / 2, 1, SpriteEffects.None, 1);
		}






		public Vector2 AngleToVector(float angle)
		{
			return new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle));
		}
		public float VectorToAngle(Vector2 vector)
		{
			return (float)Math.Atan2(vector.X, -vector.Y);
		}
	}
}

