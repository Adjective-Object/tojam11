using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Adventure
{
	public class PhoneGUI
	{
		public PhoneGUI ()
		{
            newsList.Add("Toronto up 43-31 \nover Miama at \nhalf-time.");
            newsList.Add("Car stolen in the \narea of Franklin \npark.");
            newsList.Add("FBI agents seen \nin town.");


            GotNewText("Donna", "Why aren't you\nat the party yet?\nStill coming?");
            GotNewText("Jim", "No problem. The \nanswer to q 3 on \nthe biology hw is \nprotoplasm.");
            GotNewText("Mom", "You left your \nsweater at home\nhoney.");
            GotNewText("Cameron", "I heard Sam was \ngoing to be at \nthe party \ntonight!!");

        }


		public bool Running = false;
		public Vector2 Position = new Vector2 (60, 740);


        int MainSelectorInt = 0;
        int SecondSelectorInt = 0;

        int MenuLevel = 0;
        bool RunningGame = false;
        bool GameOver = false;

        List<Point> snakePositions = new List<Point>();
        int snakeTimer = 0;
        int snakeTimerWait = 160;
        Point snakeVelocity = new Point(0, 0);
        int snakeLength = 1;

        Point newP = new Point(AdventureGame.RandomNumber.Next(26) * 10, AdventureGame.RandomNumber.Next(5, 38) * 10);

        Vector2 handOffset = new Vector2(0, 0);

        List<string> newsList = new List<string>();
        List<TextMessage> textMessages = new List<TextMessage>();


        public void GotNewText(string name, string message)
        {
            int allreadymessaged = -1;

            for (int i = 0; i < textMessages.Count; i++)
            {
                if (textMessages[i].name == name)
                {
                    allreadymessaged = i;
                    break;
                }
            }

            if (allreadymessaged == -1)
            {
                textMessages.Insert(0, new TextMessage(name, message));
            }
            else
            {
                textMessages[allreadymessaged].messages.Insert(0, message);
            }
        }


		public void Update(GameTime gameTime)
		{
			if (Running) {
				if (Position.Y > 90) {
					Position.Y = MathHelper.Lerp (Position.Y, 89, .1f);
				} else {
					Position.Y = 90;

				}

				if (TInput.KeyPressed (Keys.P))
					Running = false;




                if (Position.Y < 91)
                {
                    #region cat paw stuff
                    if (TInput.IsKeyDown(Keys.Up))
                    {
                        if (handOffset.Y > -40)
                            handOffset.Y -= 10; 
                    }
                    else if (TInput.IsKeyDown(Keys.Down))
                    {
                        if (handOffset.Y < 40)
                            handOffset.Y += 10; 
                    }
                    else if (TInput.IsKeyDown(Keys.Left))
                    {
                        if (handOffset.X > -40)
                            handOffset.X -= 10; 
                    }
                    else if (TInput.IsKeyDown(Keys.Right))
                    {
                        if (handOffset.X < 40)
                            handOffset.X += 10; 
                    }
                    else if (TInput.IsKeyDown(Keys.Z))
                    {
                        if (handOffset.Y > -10)
                            handOffset.Y -= 10; 
                    }
                    else if (TInput.IsKeyDown(Keys.X))
                    {
                        if (handOffset.Y < 10)
                            handOffset.Y += 10; 
                    }
                    else
                    {
                        if (handOffset.Y > 0)
                            handOffset.Y -= 5;
                        if (handOffset.Y < 0)
                            handOffset.Y += 5;
                        if (handOffset.X > 0)
                            handOffset.X -= 5;
                        if (handOffset.X < 0)
                            handOffset.X += 5;
                    }
                    #endregion
                }

                #region snake game
                if (RunningGame)
                {
                    if (GameOver)
                    {
                        if (TInput.KeyPressed(Keys.Z))
                        {
                            GameOver = false;
                            newP = new Point(AdventureGame.RandomNumber.Next(26) * 10, AdventureGame.RandomNumber.Next(5, 38) * 10);
                            snakePositions.Clear();
                            snakePositions.Add(new Point(100, 100));
                            snakeTimerWait = 160;
                            snakeTimer = 0;
                            snakeLength = 1;
                            snakeVelocity = new Point(0, 0);
                        }
                    }
                    else
                    {
                        if (TInput.KeyPressed(Keys.Up) && snakeVelocity.Y != 10)
                        {
                            snakeVelocity = new Point(0, -10);
                        }
                        else if (TInput.KeyPressed(Keys.Down) && snakeVelocity.Y != -10)
                        {
                            snakeVelocity = new Point(0, 10);
                        }
                        else if (TInput.KeyPressed(Keys.Right) && snakeVelocity.X != -10)
                        {
                            snakeVelocity = new Point(10, 0);
                        }
                        else if (TInput.KeyPressed(Keys.Left) && snakeVelocity.X != 10)
                        {
                            snakeVelocity = new Point(-10, 0);
                        }


                        if (snakeTimer > 0)
                            snakeTimer -= gameTime.ElapsedGameTime.Milliseconds;
                        else
                        {
                            snakeTimer = snakeTimerWait;

                            if (snakeVelocity != new Point(0, 0))
                            {
                                Point newpos = snakePositions[0] + snakeVelocity;

                                if (snakePositions.Contains(newpos) || newpos.X < 0 || newpos.Y < 40 || newpos.X > 250 || newpos.Y > 370)
                                {
                                    GameOver = true;
                                }
                                else
                                {
                                    snakePositions.Insert(0, newpos);

                                    if (snakePositions[0] == newP)
                                    {
                                        snakeLength++;
                                        snakeTimerWait -= 3;

                                        while (snakePositions.Contains(newP))
                                        {
                                            newP = new Point(AdventureGame.RandomNumber.Next(20) * 10, AdventureGame.RandomNumber.Next(5, 25) * 10);
                                        }
                                    }
                                
                                    if (snakePositions.Count > snakeLength)
                                        snakePositions.RemoveAt(snakePositions.Count - 1);
                                }
                            }
                        }
                    }

                    if (TInput.KeyPressed(Keys.X))
                    {
                        RunningGame = false;
                        MenuLevel = 1;
                    }
                }
                #endregion


                else
                {
                    if (Position.Y < 91)
                    {
                    
                        #region main menu
                        if (MenuLevel == 0)
                        {
                            if (TInput.KeyPressed(Keys.Up))
                            {
                                MainSelectorInt--;
                                if (MainSelectorInt < 0)
                                    MainSelectorInt = 2;
                            }
                            else if (TInput.KeyPressed(Keys.Down))
                            {
                                MainSelectorInt++;
                                if (MainSelectorInt > 2)
                                    MainSelectorInt = 0;
                            }
                            else if (TInput.KeyPressed(Keys.Z))
                            {
                                MenuLevel++;
                                SecondSelectorInt = 0;
                            }
                            else if (TInput.KeyPressed(Keys.X))
                            {

                            }
                        }
                        #endregion

                        #region menu level 1
                        else if (MenuLevel == 1)
                        {
                            if (MainSelectorInt == 0)
                            {

                                if (TInput.KeyPressed(Keys.Up))
                                {
                                    SecondSelectorInt--;
                                    if (SecondSelectorInt < 0)
                                        SecondSelectorInt = 3;
                                }
                                else if (TInput.KeyPressed(Keys.Down))
                                {
                                    SecondSelectorInt++;
                                    if (SecondSelectorInt > 3)
                                        SecondSelectorInt = 0;
                                }
                                else if (TInput.KeyPressed(Keys.Z))
                                {
                                    MenuLevel++;
                                }

                            }
                            else if (MainSelectorInt == 1)
                            {
                                if (TInput.KeyPressed(Keys.Up))
                                {
                                    SecondSelectorInt--;
                                    if (SecondSelectorInt < 0)
                                        SecondSelectorInt = 2;
                                }
                                else if (TInput.KeyPressed(Keys.Down))
                                {
                                    SecondSelectorInt++;
                                    if (SecondSelectorInt > 2)
                                        SecondSelectorInt = 0;
                                }
                                else if (TInput.KeyPressed(Keys.Z))
                                {
                                    MenuLevel++;
                                }

                            }
                            else if (MainSelectorInt == 2)
                            {
                                if (TInput.KeyPressed(Keys.Up))
                                {
                                    SecondSelectorInt--;
                                    if (SecondSelectorInt < 0)
                                        SecondSelectorInt = 1;
                                }
                                else if (TInput.KeyPressed(Keys.Down))
                                {
                                    SecondSelectorInt++;
                                    if (SecondSelectorInt > 1)
                                        SecondSelectorInt = 0;
                                }

                                if (TInput.KeyPressed(Keys.Z))
                                {
                                    RunningGame = true;

                                    GameOver = false;
                                    newP = new Point(AdventureGame.RandomNumber.Next(25) * 10, AdventureGame.RandomNumber.Next(5, 35) * 10);
                                    snakePositions.Clear();
                                    snakePositions.Add(new Point(100, 100));
                                    snakeTimerWait = 160;
                                    snakeTimer = 0;
                                    snakeLength = 1;
                                    snakeVelocity = new Point(0, 0);
                                }
                            }

                            if (TInput.KeyPressed(Keys.X))
                            {
                                MenuLevel--;
                            }
                        }
                        #endregion

                        else if (MenuLevel == 2)
                        {
                           
                            if (TInput.KeyPressed(Keys.X))
                            {
                                MenuLevel--;
                            }
                        }

                    }
                }





			}

			#region closing phone
			else {
				if (Position.Y < 760) {
					Position.Y = MathHelper.Lerp (Position.Y, 761, .1f);
				} else {
					Position.Y = 760;

				}

				if (TInput.KeyPressed (Keys.P))
					Running = true;
			}
			#endregion
		}

        public void Draw(SpriteBatch spriteBatch)
        {
            Color lightGreen = new Color(116, 155, 106);

            // back cat paw
            spriteBatch.Draw (AdventureGame.tylerSheet, Position + new Vector2(-240, 200), new Rectangle (1248, 0, 416, 704), Color.White, 0, new Vector2 (0, 0), 1, SpriteEffects.FlipHorizontally, 1);

            // phone image
            spriteBatch.Draw (AdventureGame.tylerSheet, Position - new Vector2(37, 84), new Rectangle (912, 0, 336, 704), lightGreen, 0, new Vector2 (0, 0), 1, SpriteEffects.None, 1);

            // green bg
            spriteBatch.Draw (AdventureGame.tylerSquare, Position, new Rectangle (0, 0, 260, 380), lightGreen, 0, new Vector2 (0, 0), 1, SpriteEffects.None, 1);

            // black title bar
            spriteBatch.Draw (AdventureGame.tylerSquare, Position + new Vector2(5, 5), new Rectangle (0, 0, 250, 34), Color.Black, 0, new Vector2 (0, 0), 1, SpriteEffects.None, 1);


            #region actual snake game
            if (RunningGame)
            {
                // title name
                spriteBatch.DrawString (AdventureGame.tylerFont2, "SNAKE", Position + new Vector2(130, 24), lightGreen, 0, AdventureGame.tylerFont2.MeasureString ("SNAKE") / 2, 2f, SpriteEffects.None, 1);

                if(GameOver)
                {
                    spriteBatch.DrawString (AdventureGame.tylerFont2, "GAME OVER", Position + new Vector2(130, 124), Color.Black, 0, AdventureGame.tylerFont2.MeasureString ("GAME OVER") / 2, 2f, SpriteEffects.None, 1);
                    spriteBatch.DrawString (AdventureGame.tylerFont2, snakeLength.ToString(), Position + new Vector2(130, 174), Color.Black, 0, AdventureGame.tylerFont2.MeasureString (snakeLength.ToString()) / 2, 2f, SpriteEffects.None, 1);
                }

                spriteBatch.Draw (AdventureGame.tylerSquare, Position + new Vector2(newP.X, newP.Y), new Rectangle (0, 0, 9, 9), Color.Black * .5f, 0, new Vector2 (0, 0), 1, SpriteEffects.None, 1);

                for (int i = 0; i < snakePositions.Count; i++)
                {
                    spriteBatch.Draw (AdventureGame.tylerSquare, Position + new Vector2(snakePositions[i].X, snakePositions[i].Y), new Rectangle (0, 0, 9, 9), Color.Black, 0, new Vector2 (0, 0), 1, SpriteEffects.None, 1);

                }
            }
            #endregion

            else
            {
                #region main menu 0
                if (MenuLevel == 0)
                {
                    // title name
                    spriteBatch.DrawString (AdventureGame.tylerFont2, "HOME", Position + new Vector2(130, 24), lightGreen, 0, AdventureGame.tylerFont2.MeasureString ("HOME") / 2, 2f, SpriteEffects.None, 1);

                    spriteBatch.Draw(AdventureGame.tylerSquare, Position + new Vector2(5, 3 + 50 + MainSelectorInt * 50), new Rectangle(0, 0, 250, 40), Color.Black, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);
                    spriteBatch.Draw(AdventureGame.tylerSquare, Position + new Vector2(7, 5 + 50 + MainSelectorInt * 50), new Rectangle(0, 0, 246, 36), lightGreen, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

                    spriteBatch.DrawString(AdventureGame.tylerFont2, "Messages", Position + new Vector2(15, 5 + 50), Color.Black, 0, AdventureGame.tylerFont2.MeasureString("") / 2, 2f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(AdventureGame.tylerFont2, "Top News", Position + new Vector2(15, 5 + 100), Color.Black, 0, AdventureGame.tylerFont2.MeasureString("") / 2, 2f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(AdventureGame.tylerFont2, "Games", Position + new Vector2(15, 5 + 150), Color.Black, 0, AdventureGame.tylerFont2.MeasureString("") / 2, 2f, SpriteEffects.None, 1);
                }
                #endregion

                #region menulevel 1
                if (MenuLevel == 1 && MainSelectorInt == 0)
                {
                    // title name
                    spriteBatch.DrawString (AdventureGame.tylerFont2, "MESSAGES", Position + new Vector2(130, 24), lightGreen, 0, AdventureGame.tylerFont2.MeasureString ("MESSAGES") / 2, 2f, SpriteEffects.None, 1);

                    spriteBatch.Draw(AdventureGame.tylerSquare, Position + new Vector2(5, 3 + 50 + SecondSelectorInt * 50), new Rectangle(0, 0, 250, 40), Color.Black, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);
                    spriteBatch.Draw(AdventureGame.tylerSquare, Position + new Vector2(7, 5 + 50 + SecondSelectorInt * 50), new Rectangle(0, 0, 246, 36), lightGreen, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

                    spriteBatch.DrawString(AdventureGame.tylerFont2, "Donna", Position + new Vector2(15, 5 + 50), Color.Black, 0, AdventureGame.tylerFont2.MeasureString("") / 2, 2f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(AdventureGame.tylerFont2, "Jim", Position + new Vector2(15, 5 + 100), Color.Black, 0, AdventureGame.tylerFont2.MeasureString("") / 2, 2f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(AdventureGame.tylerFont2, "Mom", Position + new Vector2(15, 5 + 150), Color.Black, 0, AdventureGame.tylerFont2.MeasureString("") / 2, 2f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(AdventureGame.tylerFont2, "Cameron", Position + new Vector2(15, 5 + 200), Color.Black, 0, AdventureGame.tylerFont2.MeasureString("") / 2, 2f, SpriteEffects.None, 1);
                }
                if (MenuLevel == 1 && MainSelectorInt == 1)
                {
                    // title name
                    spriteBatch.DrawString(AdventureGame.tylerFont2, "TOP NEWS", Position + new Vector2(130, 24), lightGreen, 0, AdventureGame.tylerFont2.MeasureString("TOP NEWS") / 2, 2f, SpriteEffects.None, 1);

                    spriteBatch.Draw(AdventureGame.tylerSquare, Position + new Vector2(5, 3 + 50 + SecondSelectorInt * 50), new Rectangle(0, 0, 250, 40), Color.Black, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);
                    spriteBatch.Draw(AdventureGame.tylerSquare, Position + new Vector2(7, 5 + 50 + SecondSelectorInt * 50), new Rectangle(0, 0, 246, 36), lightGreen, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);


                    for (int i = 0; i < newsList.Count; i++)
                        spriteBatch.DrawString(AdventureGame.tylerFont2, newsList[i].Substring(0, 13) + "...", 
                            Position + new Vector2(15, 5 + 50 + i * 50), Color.Black, 0, AdventureGame.tylerFont2.MeasureString("") / 2, 2f, SpriteEffects.None, 1);
                
                }
                if (MenuLevel == 1 && MainSelectorInt == 2)
                {
                    // title name
                    spriteBatch.DrawString (AdventureGame.tylerFont2, "GAMES", Position + new Vector2(130, 24), lightGreen, 0, AdventureGame.tylerFont2.MeasureString ("GAMES") / 2, 2f, SpriteEffects.None, 1);

                    spriteBatch.Draw(AdventureGame.tylerSquare, Position + new Vector2(5, 3 + 50 + SecondSelectorInt * 50), new Rectangle(0, 0, 250, 40), Color.Black, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);
                    spriteBatch.Draw(AdventureGame.tylerSquare, Position + new Vector2(7, 5 + 50 + SecondSelectorInt * 50), new Rectangle(0, 0, 246, 36), lightGreen, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

                    spriteBatch.DrawString(AdventureGame.tylerFont2, "Snake", Position + new Vector2(15, 5 + 50), Color.Black, 0, AdventureGame.tylerFont2.MeasureString("") / 2, 2f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(AdventureGame.tylerFont2, "Match 3", Position + new Vector2(15, 5 + 100), Color.Black, 0, AdventureGame.tylerFont2.MeasureString("") / 2, 2f, SpriteEffects.None, 1);
                }
                #endregion


                if (MenuLevel == 2 && MainSelectorInt == 0)
                {
                    // title name
                    spriteBatch.DrawString (AdventureGame.tylerFont2, "MESSAGES", Position + new Vector2(130, 24), lightGreen, 0, AdventureGame.tylerFont2.MeasureString ("MESSAGES") / 2, 2f, SpriteEffects.None, 1);

                    if (SecondSelectorInt == 0)
                    {
                        spriteBatch.DrawString(AdventureGame.tylerFont2, "Donna: \nWhy aren't you\nat the party yet?\nStill coming?", Position + new Vector2(15, 5 + 50), Color.Black, 0, AdventureGame.tylerFont2.MeasureString("") / 2, 2f, SpriteEffects.None, 1);
                    }
                    if (SecondSelectorInt == 1)
                    {
                        spriteBatch.DrawString(AdventureGame.tylerFont2, "Jim: \nNo problem. The \nanswer to q 3 on \nthe biology hw is \nprotoplasm.", Position + new Vector2(15, 5 + 50), Color.Black, 0, AdventureGame.tylerFont2.MeasureString("") / 2, 2f, SpriteEffects.None, 1);
                    }
                    if (SecondSelectorInt == 2)
                    {
                        spriteBatch.DrawString(AdventureGame.tylerFont2, "Mom: \nYou left your \nsweater at home\nhoney.", Position + new Vector2(15, 5 + 50), Color.Black, 0, AdventureGame.tylerFont2.MeasureString("") / 2, 2f, SpriteEffects.None, 1);
                    }
                    if (SecondSelectorInt == 3)
                    {
                        spriteBatch.DrawString(AdventureGame.tylerFont2, "Cameron: \nI heard Sam was \ngoing to be at \nthe party \ntonight!!", Position + new Vector2(15, 5 + 50), Color.Black, 0, AdventureGame.tylerFont2.MeasureString("") / 2, 2f, SpriteEffects.None, 1);
                    }
                }

                if (MenuLevel == 2 && MainSelectorInt == 1)
                {
                    // title name
                    spriteBatch.DrawString(AdventureGame.tylerFont2, "TOP NEWS", Position + new Vector2(130, 24), lightGreen, 0, AdventureGame.tylerFont2.MeasureString("TOP NEWS") / 2, 2f, SpriteEffects.None, 1);

                    // actual new story
                    spriteBatch.DrawString(AdventureGame.tylerFont2, newsList[SecondSelectorInt], Position + new Vector2(15, 5 + 50), Color.Black, 0, AdventureGame.tylerFont2.MeasureString("") / 2, 2f, SpriteEffects.None, 1);
                }
            }




            // front cat paw
            spriteBatch.Draw (AdventureGame.tylerSheet, Position + new Vector2(60, 480) + handOffset, new Rectangle (1248, 0, 416, 704), Color.White, 0, new Vector2 (0, 0), 1, SpriteEffects.None, 1);

		}
	}
}

