using System;

namespace Adventure
{
	public class HighGuy : CharacterBehavior
	{
		public HighGuy (SoundFont speech = null) : base(speech)
		{
		}

		Boolean expectingChip = false;

		override public void RespondToInteraction(Character player) {
			// turn to face player
			character.facingLeft = player.position.X < character.position.X;

			// do conversation
			if (!expectingChip) {
				if (player.heldItem == ItemID.CHIP) {
					SpeakAndAdvance(player, "Oh man, is that for me?");
					expectingChip = true;
				}
				else {
					// do normal conversation
					nonChipConversation(player);
				}
			} else {
				if (player.heldItem != ItemID.CHIP) {
					EmitSpeech("yo, where did that chip go?");
					expectingChip = false;
				} else {
					GameStateDictionary.Increment ("fed_chips");
					EmitSpeech("Thanks, that hit the spot");
					if (GameStateDictionary.GetNum("fed_chips") == 1) {
						LogEndMessage("Fed the high guy a chip");
					} else {
						LogEndMessage("Fed the high guy " + GameStateDictionary.GetNum("fed_chips") + " chips");
					}
					expectingChip = false;
					Inventory.RemoveCurrent();
				}
			}
		}

		String [] nonChipLines = {
			"Yo, this plant is super cool",
			"Do you ever look at the sky and think about how small we all are?",
			"You should listen to Eric Clapton",
			"What if your blue and my blue are totally different blues?",
			"I think I just saw a UFO"
		};
		protected void nonChipConversation(Character player) {
			EmitRandom (nonChipLines);
		}


	}
}

