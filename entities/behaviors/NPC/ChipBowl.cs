using System;
using System.Linq;

namespace Adventure
{
	public class ChipBowl : SpriteBehavior
	{
		int interactionCount = 0;

		public ChipBowl (SoundFont speech = null) : base (speech) {
		}

		override public void RespondToInteraction(Character player) {
			Func<bool> resetWalk = walkAway (player, entity, resetConvo);

			if (GameStateDictionary.GetFlag ("chip_bowl_empty")) {
				EmitSpeech ("The bowl is empty...");
			}
			else {
				// do conversation
				switch (interactionCount) {
				case 0:
					EmitSpeech ("A bowl of chips", SpeechText.SpeechMode.PLAYER_CONTROLLED, resetWalk, advanceConvo(player));
					break;
				case 1:
					EmitSpeechOption ("take a chip?",
						new SpeechText.Option[] {
							new SpeechText.Option("yes", giveChip),
							new SpeechText.Option("no", noGiveChip),
						},
						resetWalk
					);
					break;
				}
			}

			((PlayerBehavior) player.behavior).triggeredText = this.speechReference;
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

		private void giveChip() {
			
			Boolean success = Inventory.Add (ItemID.CHIP);
			int numChips = Inventory.contents.Where ((x) => x.Equals (ItemID.CHIP)).Count();
			if (success) {
				GameStateDictionary.Increment ("chips_taken_count");
				switch (numChips) {
				case 1:
					EmitSpeech ("you take a chip");
					break;
				case 2:
					EmitSpeech ("you take another chip");
					break;
				case 3:
					EmitSpeech ("you take a third chip");
					break;
				case 4:
					EmitSpeech ("Your pockets are overflowing with chips");
					break;
				}
			} else {
				if (numChips > 2) {
					EmitSpeech ("You have too many chips in your pockets");
				} else {
					EmitSpeech ("Your pockets are too full");
				}
			}

			if (GameStateDictionary.GetNum ("chips_taken_count") > 6 && !GameStateDictionary.GetFlag("chip_bowl_empty")) {
				GameStateDictionary.SetFlag ("chip_bowl_empty", true);
				((StaticEntity)this.entity).SwitchSprite ("empty");
			}
		}

		private void noGiveChip() {
			int numChips = Inventory.contents.Where ((x) => x.Equals (ItemID.CHIP)).Count();
			if (numChips > 2) {
				EmitSpeech ("Yeah, you already have a lot of chips");
			} else {
				EmitSpeech ("You opt not to take a chip");
			}
		}
	}

}

