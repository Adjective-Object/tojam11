using System;
using Microsoft.Xna.Framework;

namespace Adventure
{
	public class CharacterBehavior
	{
		Character character;
		SpeechText speechReference;

		public void BindToCharacter(Character c) {
			this.character = c;
		}

		public void Update(GameTime elapsed) {
			// reset head if we are done speaking
			if (this.speechReference != null &&
			    this.speechReference.doneEmitting) {

				this.speechReference = null;
				character.PlayAnimHead("idle");
			}
		}

		
		public void EmitSpeech(String text, SpeechText.SpeechMode mode = SpeechText.SpeechMode.AMBIENT) {
			this.speechReference = SpeechText.Spawn (
				"Monaco", character.position + new Vector2(0, -200), text, mode);
			character.PlayAnimHead("talk");
		}
			
		public void RespondToInteraction() {
			this.EmitSpeech("character response not implemented");
		}
	}
}

