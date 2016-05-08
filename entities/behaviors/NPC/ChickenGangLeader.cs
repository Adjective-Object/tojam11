using System;

namespace Adventure
{
	public class ChickenGangLeader : CharacterBehavior
	{

		protected int interactionCount;

		public ChickenGangLeader (SoundFont speech) : base (speech) {
		}

		override public void RespondToInteraction(Character player) {
			// turn to face player
			character.facingLeft = player.position.X < character.position.X;

			// do conversation
			switch (interactionCount) {
			case 0:
				SpeakAndAdvance (player, "We're the chicken gang");
				break;
			default:
				EmitSpeech ("I think we're pretty cool");
				break;
			}
			interactionCount++;
		}

		private void SpeakAndAdvance(Character player, String str) {
			EmitSpeech (
				str,
				SpeechText.SpeechMode.PLAYER_CONTROLLED, 
				walkAway(entity, player, advanceConvo(player)),
				advanceConvo(player));
		}

		private void resetConvo() {
			this.interactionCount = 0;
		}
		private Action advanceConvo(Character player) {
			return () => {
				interactionCount++;
				this.RespondToInteraction (player);
			};
		}

	}
}

