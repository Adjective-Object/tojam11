using System;

namespace Adventure
{
	public class HipsterNPC : CharacterBehavior
	{
		int interactionCount = 0;
		int greeting;

		public HipsterNPC (SoundFont speech) : base (speech) {
		}

		override public void RespondToInteraction(Character player) {
			// turn to face player
			character.facingLeft = player.position.X < character.position.X;

			// do conversation
			switch (interactionCount) {
			case 0:
				EmitSpeechOption ("... hey. What's up?",
					new SpeechText.Option[] {
						new SpeechText.Option ("not bad, how about you?", () => {
							interactionCount++;
							greeting = 0;
							this.RespondToInteraction (player);
						}),
						new SpeechText.Option ("all right", () => {
							interactionCount++;
							greeting = 1;
							this.RespondToInteraction (player);
						}),
						new SpeechText.Option ("decent", () => {
							interactionCount++;
							greeting = 2;
							this.RespondToInteraction (player);
						}),
					},
					walkAway (
						player, character,
						() => {
							EmitSpeech ("...");
						})
				);
				return;
			case 1:
			case 2:
				EmitSpeech ("..");
				break;
			default:
				break;
			}
			interactionCount++;
		}
	}
}

