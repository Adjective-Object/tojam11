using System;

namespace Adventure
{
	public class Guitar : SpriteBehavior
	{
		Boolean hasStrings = true;
		public override void RespondToInteraction(Character player)
		{
			if (this.hasStrings) {
				switch (player.heldItem) {
				case ItemID.SCREWDRIVER:
					EmitSpeech ("Surprisingly, that doesn't work");
					break;
				case ItemID.KNIFE:
					if (Inventory.Add (ItemID.STRINGS)) {
						hasStrings = false;
						LogEndMessage ("Ruined a Guitar");
						EmitSpeech ("You cut the strings off the guitar");
						//((StaticEntity)this.entity).SwitchSprite ("broken");
					} else {
						EmitSpeech ("Your inventory is too full of junk");
					}
					break;
				default:
					EmitSpeech ("It's a guitar");
					break;
				}
			} else {
				EmitSpeech ("It's a guitar. You cut the strings off for some reason");
			}
		}

	}
}

