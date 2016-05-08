using System;

namespace Adventure
{
	public class GenericItem : SpriteBehavior
	{
		String[] description;
		int index = 0;
		ItemID itemId;
		public GenericItem (params String[] description) 
		{
			this.itemId = ItemID.NO_ITEM;
			this.description = description;
		}

		public GenericItem (ItemID item, params String[] description) {
			this.itemId = item;
			this.description = description;
		}

		public override void RespondToInteraction(Character player) {
			if (index < description.Length) {
				Action enterCallback = () => this.RespondToInteraction (player);

				this.EmitSpeech (
					description [this.index],
					SpeechText.SpeechMode.PLAYER_CONTROLLED,
					walkAway (player, this.entity, () => {
						this.index = 0;
					}),
					enterCallback
				);
				this.index++;
			} else {
				if (this.itemId != ItemID.NO_ITEM) {

					String itemName = Item.Get (this.itemId).name;
					this.EmitSpeechOption(
						itemName,
						new SpeechText.Option[] {
							new SpeechText.Option("Take it", () => {
								this.EmitSpeech("you take the " + itemName, 
									SpeechText.SpeechMode.PLAYER_CONTROLLED);
								Inventory.Add(this.itemId);
								this.entity.Kill();
							}),
							new SpeechText.Option("Don't take it", () => {
								this.EmitSpeech("you don't take the " + itemName, 
									SpeechText.SpeechMode.PLAYER_CONTROLLED);
							}),
						},
						walkAway(player, entity, () => {
						})
					);

				}
				this.index = 0;
			}

			((PlayerBehavior) player.behavior).triggeredText = this.speechReference;
		}

	}
}

