using System;
using Microsoft.Xna.Framework;

namespace Adventure
{
	public class CharacterBehavior : SpriteBehavior
	{
		public CharacterBehavior(SoundFont sf) : base(sf) {
		}

		override public void Update(GameTime elapsed) {
			// reset head if we are done speaking
			if (this.speechReference != null &&
			    this.speechReference.DoneEmitting) {
				((Character) entity).PlayAnimHead("idle");
			}
			base.Update (elapsed);
		}

        public bool MoveCharacter(Vector2 newPosition)
        {
            bool moved = false;
            if (character.position.X != newPosition.X && !CharacterCollidesWithMap(new Vector2(newPosition.X, character.position.Y)))
            {
                character.position.X = newPosition.X;
                moved = true;
            }
            if (character.position.Y != newPosition.Y && !CharacterCollidesWithMap(new Vector2(character.position.X, newPosition.Y)))
            {
                character.position.Y = newPosition.Y;
                moved = true;
            }
            return moved;
        }

        protected bool CharacterCollidesWithMap(Vector2 pos)
        {
            //Width and height of player collision
            int width = 15;
            int height = 15;
            for (int y = 0; y < height; y++)
            {
                for (int x = -width; x < width; x++)
                {
                    if (AdventureGame.instance.collisionMap[(int)pos.Y + y, (int)pos.X + x] == 1)
                        return true;
                }
            }
            return false;
        }

		public Character character;
		override public void BindToEntity(InteractableEntity entity) {
			base.BindToEntity(entity);
			character = (Character) entity;
		}

		override public void EmitSpeech(String text, SpeechText.SpeechMode mode = SpeechText.SpeechMode.PLAYER_CONTROLLED, Func<bool> walkAwayCallback = null, Action enterCallback = null) {
			base.EmitSpeech(text, mode, walkAwayCallback, enterCallback);
			((Character) entity).PlayAnimHead("talk");
		}

		override public void EmitSpeechOption(String text, SpeechText.Option[] options, Func<bool> walkAwayCallback = null) {
			base.EmitSpeechOption(text, options, walkAwayCallback);
			((Character) entity).PlayAnimHead ("talk");
		}

		override public void RespondToInteraction(Character player) {
			this.EmitSpeech("response not implemented");
			((Character) entity).facingLeft = player.position.X < entity.position.X;
		}


		protected int interactionCount;

		// misc helpers for stuff that gets used a lot
		protected void SpeakAndAdvance(Character player, String str) {
			EmitSpeech (
				str,
				SpeechText.SpeechMode.PLAYER_CONTROLLED, 
				walkAway(entity, player, advanceConvo(player)),
				advanceConvo(player));
		}
		protected void resetConvo() {
			this.interactionCount = 0;
		}
		protected Action advanceConvo(Character player) {
			return () => {
				interactionCount++;
				this.RespondToInteraction (player);
			};
		}

		static Random r = new Random();
		protected void EmitRandom(String[] options, 
			SpeechText.SpeechMode mode = SpeechText.SpeechMode.PLAYER_CONTROLLED,
			Func<bool> walkAwayCallback = null, 
			Action enterAction = null) {

			this.EmitSpeech(options[r.Next (0, options.Length)]);
		}

	}
}

