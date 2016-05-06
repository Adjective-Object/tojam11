using System;
using Microsoft.Xna.Framework;

namespace Adventure
{
	public class CharacterBehavior : EntityBehavior
	{
		protected Character character;
		protected SpeechText speechReference;

		public void BindToCharacter(Character c) {
			this.character = c;
		}

		public virtual void Update(GameTime elapsed) {
			// reset head if we are done speaking
			if (this.speechReference != null &&
			    this.speechReference.doneEmitting) {

				this.speechReference = null;
				character.PlayAnimHead("idle");
			}
		}

		
		public virtual void EmitSpeech(String text, SpeechText.SpeechMode mode = SpeechText.SpeechMode.AMBIENT) {
			this.speechReference = SpeechText.Spawn (
				"Monaco", character.position + new Vector2(0, -200), text, mode);
			character.PlayAnimHead("talk");
		}
			
		public virtual void RespondToInteraction() {
			this.EmitSpeech("character response not implemented");
		}
	}
}

