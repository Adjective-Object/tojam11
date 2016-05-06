using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Adventure
{
	public class SpeechText : BaseEntity
	{
		//
		// Singleton 'spawner' interface
		//

		protected static Dictionary<String, SpriteFont> fonts = new Dictionary<String, SpriteFont>();

		public static void LoadFont(ContentManager content, String fontName) {
			Console.WriteLine (fontName);
			if (fonts.Count == 0) SpeechText.fonts ["default"] = content.Load<SpriteFont> (fontName);

			SpeechText.fonts [fontName] = content.Load<SpriteFont> (fontName);
		}
		public static void Spawn(String fontName, Vector2 position, String text) {
			AdventureGame.SpawnEntitiy(new SpeechText(fonts[fontName], position, text));
		}

		//
		// Instance Methods and Properties
		//
		static Vector2 startingOffset = new Vector2(0,10);
		static Vector2 letterOffset = new Vector2(10,0);
		static double interpolationTime = 0.4;
		static double letterSpawnDelay = 0.03;

		SpriteFont font;
		double [] ages;
		string text;
		protected SpeechText(SpriteFont font, Vector2 position, String text) : base(position){
			this.text = text;
			this.font = font;
			this.ages = new double[text.Length];
			for (int i = 0; i < text.Length; i++) {
				this.ages [i] = i * -letterSpawnDelay;
			}
		}

		public override void Load(ContentManager content, SpriteBatch batch) {}
		public override void Update(GameTime time) {
			// update the ages of each letter
			for (int i = 0; i < this.ages.Length; i++) {
				this.ages [i] = this.ages[i] + time.ElapsedGameTime.TotalSeconds;
			}
		}
		public override void Draw(SpriteBatch batch, GameTime time) {
			for (int i=0; i<this.ages.Length; i++) {
				// calculate the animation time for this letter
				double age = this.ages [i];
				float t = (float) Math.Max(0, Math.Min(1, age / interpolationTime));

				Vector2 offset = 
					letterOffset * i + 
					startingOffset * (float) Math.Pow((1 - t), 3);
				batch.DrawString(
					font, 
					this.text[i].ToString(), 
					this.position + offset,
					Color.White * t);
			}

		}

	}
}

