using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Adventure
{
	public class CharacterSelect : Room
	{

		// stuff for the opening menu
		int currentSelectionType = 0;
		int currentHeadSprite = 0;
		int currentBodySprite = 0;
		int currentBodyColor = 0;
		public List<Color[]> bodyColors;
		Easing<float> indicatorHeight;
		Jitter<float> titleJitterRotation;
		Jitter<Vector2> titleJitter;

		public void OnEnter() {
			Character player = AdventureGame.Player;
			player.position.X = AdventureGame.ScreenBounds.Width / 3;
			player.SetCharacterColor(bodyColors[currentBodyColor]);
		}

		public void Initialize () {
			bodyColors = new List<Color[]>();

			bodyColors.Add (new Color[] { new Color (214, 195, 189), 	new Color (172, 179, 190), 	new Color (141, 0, 3) });
			bodyColors.Add (new Color[] { new Color (227, 205, 164), 	new Color (199, 121, 102), 	new Color (112, 48, 48) });
			bodyColors.Add (new Color[] { new Color (255, 255, 157), 	new Color (190, 235, 159), 	new Color (0, 163, 136) });
			bodyColors.Add (new Color[] { new Color (178,  88,  25), 	new Color (237,  53,  25), 	new Color (154,  38,  94) });
			bodyColors.Add (new Color[] { new Color (  0,  64,  62), 	new Color (187,  53,  25), 	new Color (154,  38,  94) });
			bodyColors.Add (new Color[] { new Color (255, 255, 255), 	new Color (255, 200, 200), 	new Color (180, 100, 100) });
			bodyColors.Add (new Color[] { new Color (255, 255, 255), 	new Color (25, 135, 0), 	new Color (0, 20, 200) });
			bodyColors.Add (new Color[] { new Color (180, 190, 170), 	new Color (255, 200, 200),	new Color (250, 250, 100) });
			bodyColors.Add (new Color[] { new Color (100, 50, 15), 		new Color (100, 0, 100), 	new Color (250, 250, 100) });

			indicatorHeight = new Easing<float> (410f, 460f, 10);
			titleJitterRotation = new Jitter<float> (-0.02f, 0.02f);
			titleJitter = new Jitter<Vector2> (new Vector2(0, -3), new Vector2(0, 3));
		}
			
		public void Update (GameTime gameTime) {
			indicatorHeight.Update(gameTime, currentSelectionType != 0);
			titleJitter.Update (gameTime);
			titleJitterRotation.Update (gameTime);

			List<String> headSprites = AdventureGame.instance.headSprites;
			List<String> bodySprites = AdventureGame.instance.bodySprites;

			bool selectionChanged = false;
			if (Input.KeyPressed(Key.ENTER))
			{
				AdventureGame.instance.StartGame(gameTime);
                return;
			}
			else if (Input.KeyPressed(Key.LEFT))
			{
				if (currentSelectionType == 0)
					currentHeadSprite++;
				else if(currentSelectionType == 1)
					currentBodySprite++;
				else
					currentBodyColor++;
				selectionChanged = true;
			}
			else if (Input.KeyPressed(Key.RIGHT))
			{
				if (currentSelectionType == 0)
					currentHeadSprite--;
				else if(currentSelectionType == 1)
					currentBodySprite--;
				else
					currentBodyColor--;
				selectionChanged = true;
			}
			else if (Input.KeyPressed(Key.UP))
			{
				currentSelectionType--;
				currentSelectionType = Math.Max(0, currentSelectionType);
			}
			else if (Input.KeyPressed(Key.DOWN))
			{
				currentSelectionType++;
				currentSelectionType = Math.Min(currentSelectionType, 2);
			}

			Character player = AdventureGame.Player;

			if (selectionChanged)
			{
				if (currentHeadSprite >= headSprites.Count)
					currentHeadSprite = 0;
				if (currentHeadSprite < 0)
					currentHeadSprite = headSprites.Count - 1;

				if (currentBodySprite >= bodySprites.Count)
					currentBodySprite = 0;
				if (currentBodySprite < 0)
					currentBodySprite = bodySprites.Count - 1;

				if (currentBodyColor >= bodyColors.Count)
					currentBodyColor = 0;
				if (currentBodyColor < 0)
					currentBodyColor = bodyColors.Count - 1;

				// a bit of a hack but whatever
				player.SetCharacterSprites(headSprites[currentHeadSprite], bodySprites[currentBodySprite]);
				player.SetCharacterColor(bodyColors[currentBodyColor]);
				player.Load(AdventureGame.instance.Content, AdventureGame.instance.entityBatch);
			}
		}

		public void LoadContent () {
		}

		public void Draw (GameTime gameTime) {
			GraphicsDeviceManager graphics = AdventureGame.instance.graphics;
			SpriteBatch entityBatch = AdventureGame.instance.entityBatch;
			SpriteFont defaultFont = AdventureGame.DefaultFont;
			SpriteFont titleFont = AdventureGame.TitleFont;

			graphics.GraphicsDevice.Clear(Color.Black);

			entityBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, null);

			int screenWidth = AdventureGame.ScreenBounds.Width;
			entityBatch.DrawString(defaultFont, "Lets get ready for the party...", new Vector2(screenWidth/2 - defaultFont.MeasureString("Lets get ready for the party...").X/2, 150), Color.White);
			AdventureGame.Player.Draw(entityBatch, gameTime);

			const int characterSpacing = 42;

			int characterOffset = screenWidth / 3;
			int controlsOffset = screenWidth * 2 / 3;

			if (currentSelectionType == 0)
			{
				entityBatch.DrawString(
					defaultFont, "Choose Your Head >", 
					new Vector2(characterOffset - characterSpacing - defaultFont.MeasureString("Choose Your Head >").X, (int)indicatorHeight.current), Color.White);
			}
			else if (currentSelectionType == 1)
			{
				entityBatch.DrawString(defaultFont, "Choose Your Outfit >", new Vector2(characterOffset - characterSpacing - defaultFont.MeasureString("Choose Your Outfit >").X, (int) indicatorHeight.current), Color.White);
			}
			else if (currentSelectionType == 2)
			{
				entityBatch.DrawString(defaultFont, "Choose Your Colors >", new Vector2(characterOffset - characterSpacing - defaultFont.MeasureString("Choose Your Colors >").X, (int) indicatorHeight.current), Color.White);
			}

			entityBatch.DrawString(defaultFont, "Press Enter To Start", new Vector2(characterOffset - defaultFont.MeasureString("Press Enter To Start").X/2, 650), Color.White);

			const String controlsString = "Controls:\nWASD/Arrows to move\nENTER to interact\nTAB to select options\nI to go through inventory\nP to use phone";
			entityBatch.DrawString (
				defaultFont, 
				controlsString,
				new Vector2(controlsOffset - defaultFont.MeasureString(controlsString).X / 2, 350),
				Color.White);

			entityBatch.End ();

			Matrix textPosition = Matrix.CreateTranslation (new Vector3(screenWidth / 2, 100, 0));
			Matrix textRotation = Matrix.CreateRotationZ (this.titleJitterRotation.current);
			entityBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, textRotation * textPosition);
			entityBatch.DrawString (
				titleFont, "Social Anxiety Party Simulator", 
				new Vector2(
					- titleFont.MeasureString("Social Anxiety Party Simulator").X/2,
					titleJitter.current.Y), Color.White);
			entityBatch.End ();
		}
	}
}

