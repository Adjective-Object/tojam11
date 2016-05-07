﻿using System;
using Microsoft.Xna.Framework;

namespace Adventure
{
	public class CharacterBehavior : EntityBehavior
	{
		protected Character character;
		protected SpeechText speechReference;
		protected SoundFont sounds;

		public CharacterBehavior(SoundFont sounds) {
			this.sounds= sounds;
		}

		public void BindToCharacter(Character c) {
			this.character = c;
		}

		public virtual void Update(GameTime elapsed) {
			// reset head if we are done speaking
			if (this.speechReference != null &&
			    this.speechReference.DoneEmitting) {

				character.PlayAnimHead("idle");
			}

			if (this.speechReference != null && 
				!this.speechReference.alive) {
				this.speechReference = null;
			}
		}

		
		public virtual void EmitSpeech(String text, SpeechText.SpeechMode mode = SpeechText.SpeechMode.AMBIENT, Func<bool> walkAwayCallback = null, Action enterCallback = null) {
			walkAwayCallback =  (walkAwayCallback == null) ? walkAway(AdventureGame.Player, this.character, () => {}) : walkAwayCallback;
			this.speechReference = SpeechText.Spawn (
				"Monaco", character.position + new Vector2 (0, -200), text, mode);
			this.speechReference.AttachWalkawayCallback (walkAwayCallback);
			this.speechReference.AttachEnterCallback (enterCallback);
			this.speechReference.AttachSoundFont (this.sounds);

			character.PlayAnimHead("talk");
		}

		public virtual void EmitSpeechOption(String text, SpeechText.Option[] options, Func<bool> walkAwayCallback = null) {
			this.speechReference = SpeechText.Spawn (
				"Monaco", character.position + new Vector2(0, -200), text,  options);
			this.speechReference.AttachWalkawayCallback (walkAwayCallback);
			this.speechReference.AttachSoundFont (this.sounds);

			character.PlayAnimHead ("talk");
		}

		static double WALK_AWAY_RADIUS = 100;
		protected Func<Boolean> walkAway(BaseEntity e1, BaseEntity e2, Action callback) {
			return () => {
				if ((e1.position - e2.position).Length () > WALK_AWAY_RADIUS) {
					callback ();
					return true;
				}
				return false;
			};
		}
			
		public virtual void RespondToInteraction(Character player) {
			this.EmitSpeech("character response not implemented");
			character.facingLeft = player.position.X < character.position.X;
		}

		public Boolean IsDisabled() {
			return this.speechReference != null;
		}
	}
}

