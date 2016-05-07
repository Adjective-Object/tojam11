using System;
using Microsoft.Xna.Framework;

namespace Adventure
{
	public class SpriteBehavior : EntityBehavior {

		protected BaseEntity entity;
		protected SpeechText speechReference;
		protected SoundFont sounds;

		public SpriteBehavior(SoundFont sounds = null) {
			this.sounds = (sounds == null) ? AdventureGame.DefaultSoundFont : sounds;
		}

		public virtual void BindToEntity(BaseEntity entity) {
			this.entity = entity;
		}

		public virtual void Update(GameTime elapsed) {
			if (this.speechReference != null && 
				!this.speechReference.alive) {
				this.speechReference = null;
			}
		}

		public virtual void EmitSpeech(String text, SpeechText.SpeechMode mode = SpeechText.SpeechMode.AMBIENT, Func<bool> walkAwayCallback = null, Action enterCallback = null) {
			walkAwayCallback = (walkAwayCallback == null) 
				? walkAway(AdventureGame.Player, this.entity, () => {}) 
				: walkAwayCallback;
			this.speechReference = SpeechText.Spawn (
					"Monaco", entity.position + new Vector2 (0, -200), text, mode);
			this.speechReference.AttachWalkawayCallback (walkAwayCallback);
			this.speechReference.AttachEnterCallback (enterCallback);
			this.speechReference.AttachSoundFont (this.sounds);
		}

		public virtual void EmitSpeechOption(String text, SpeechText.Option[] options, Func<bool> walkAwayCallback = null) {
			this.speechReference = SpeechText.Spawn (
					"Monaco", entity.position + new Vector2(0, -200), text,  options);
			if (walkAwayCallback == null) walkAwayCallback = walkAway(
				AdventureGame.Player, this.entity, () => {}
			);
			this.speechReference.AttachWalkawayCallback (walkAwayCallback);
			this.speechReference.AttachSoundFont (this.sounds);
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
		}

		public Boolean IsDisabled() {
			return this.speechReference != null;
		}
	}
}

	