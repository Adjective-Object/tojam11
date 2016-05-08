using System;

namespace Adventure
{
	public class Antenna : SpriteBehavior
	{
		Boolean encountered = false;

		public override void RespondToInteraction(Character player) {
			this.EmitSpeechOption(
				"Television Antenna",
				new SpeechText.Option[] {
					new SpeechText.Option("Take it", () => {
						switch(player.heldItem) {
						case ItemID.HAMMER:
							this.EmitSpeech("you smash the antenna off the TV with the hammer");
							LogEndMessage("Broke the TV");
							Inventory.Add(ItemID.ANTENNA);
							this.entity.Kill();
							break;
						case ItemID.SCREWDRIVER:
							this.EmitSpeech("you try to use the screwdriver, but the screws are flathead");
                            LogEndMessage("Tried to steal a TV antenna, but couldn't figure out the screws");
							break;
						case ItemID.NO_ITEM:
						default:
							this.EmitSpeech("you try to take the antenna, but it's screwed on to the TV");
							LogEndMessage("Failed to steal a TV antenna");
							break;
						}
					}),
					new SpeechText.Option("Don't take it", () => {
						this.EmitSpeech("you leave the antenna alone", 
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

