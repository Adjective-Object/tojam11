using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Adventure
{
	public class SpeechText : BaseEntity
	{
		//
		// Singleton 'spawner' interface
		//

		protected static Dictionary<String, SpriteFont> fonts = new Dictionary<String, SpriteFont>();

		public static void LoadFont(ContentManager content, String fontName) {
			if (fonts.Count == 0) SpeechText.fonts ["default"] = content.Load<SpriteFont> (fontName);

			SpeechText.fonts [fontName] = content.Load<SpriteFont> (fontName);
		}
		public static SpeechText Spawn(String fontName, Vector2 position, String text, SpeechMode mode = SpeechMode.AMBIENT) {
			SpeechText e = new SpeechText (fonts [fontName], position, text, mode);
			AdventureGame.SpawnEntity(e);
			return e;
		}

		//
		// Instance Methods and Properties
		//
		static Vector2 STARTING_OFFSET = new Vector2(0,10);
		static Vector2 LETTER_OFFSET = new Vector2(10,0);
		static Vector2 GRADUAL_RISE = new Vector2(0,-10);
		static double ANIMATION_TIME = 0.4;
		static double LETTER_SPAWN_DELAY = 0.03;
		static double AMBIENT_SPEECH_LIFETIME = 3.0;

		override public Boolean isUI {
			get { return true; }
		}
		public Boolean doneEmitting {
			get { return this.ages[this.ages.Length - 1] > ANIMATION_TIME; }
		}

		SpriteFont font;
		SpeechMode mode;
		double [] ages;
		double dismissedTime;
		string text;
		double timeScale = 1;
		protected SpeechText(SpriteFont font, Vector2 position, String text, SpeechMode mode) : base(position){
			this.text = text;
			this.font = font;
			this.mode = mode;
			this.ages = new double[text.Length];
			this.dismissedTime = -1;
			for (int i = 0; i < text.Length; i++) {
				this.ages [i] = i * -LETTER_SPAWN_DELAY;
			}
		}

		public override void Load(ContentManager content, SpriteBatch batch) {}
		public override void Update(GameTime time) {
			// update the ages of each letter
			for (int i = 0; i < this.ages.Length; i++) {
				this.ages [i] = this.ages[i] + time.ElapsedGameTime.TotalSeconds * timeScale;
			}

			// update depending on mode
			switch (mode) {
			case SpeechMode.AMBIENT:
				if (ages [ages.Length - 1] > AMBIENT_SPEECH_LIFETIME) {
					this.dismissedTime = AMBIENT_SPEECH_LIFETIME;
				}
				break;
			case SpeechMode.PLAYER_CONTROLLED:
				if (Input.KeyPressed(Key.ENTER)) {
					// if it's still animating in and the enter button is pressed, speed up the animation
					if (ages [ages.Length - 1] < ANIMATION_TIME) {
						this.timeScale = 3.0;
					} else if (dismissedTime < 0){
						this.dismissedTime = ages[ages.Length - 1];
					}
				}
				break;
			}

			if (dismissedTime > 0 && ages [ages.Length - 1] > dismissedTime + ANIMATION_TIME) {
				this._alive = false;	
			}
		}

		public override void Draw(SpriteBatch batch, GameTime time) {
			for  (int i = 0; i < this.ages.Length; i++) {
				// calculate the animation time for this letter
				double age = this.ages [i];

				Vector2 offset;
				float opacity;
				// text is appearing still
				if (dismissedTime < 0) {
					float t = (float)Math.Max (0, Math.Min (1, age / ANIMATION_TIME));
					offset = 
						LETTER_OFFSET * i +
						STARTING_OFFSET * (float)Math.Pow ((1 - t), 3);
					opacity = t;
				}

				// text has been dismissed
				else {
					float t = Math.Min(1, (float) ((this.ages[this.ages.Length - 1] - dismissedTime) / ANIMATION_TIME));
					offset =
						LETTER_OFFSET * i +
						STARTING_OFFSET * (float) Math.Pow(t, 3);
					opacity = (1 - t);
				}

				offset = offset - LETTER_OFFSET * text.Length / 2;
				offset = offset + GRADUAL_RISE * (float) ages [0];

				// apply the calculated opacity and offset
				batch.DrawString (
					font, 
					this.text [i].ToString (), 
					this.position + offset,
					Color.White * opacity);
			}
		}

		public enum SpeechMode {
			AMBIENT, PLAYER_CONTROLLED
		}

	}
}

