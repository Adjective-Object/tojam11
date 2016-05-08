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

		public Character character;
		override public void BindToEntity(BaseEntity entity) {
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
	}
}

