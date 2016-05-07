using System;

namespace Adventure
{
	public class Antenna : SpriteBehavior
	{
		public override void RespondToInteraction(Character player) {
			this.EmitSpeechOption(
				"Television Antenna",
				new SpeechText.Option[] {
					new SpeechText.Option("Take it", () => {
						this.EmitSpeech("you take the antenna", 
							SpeechText.SpeechMode.PLAYER_CONTROLLED);
					}),
					new SpeechText.Option("Don't take it", () => {
						this.EmitSpeech("you don't take the antenna", 
							SpeechText.SpeechMode.PLAYER_CONTROLLED);
					}),
				},
				walkAway(player, entity, () => {
					// TODO	
				})
			);
		}
	}
}

