using System;

namespace Adventure
{
	public class GenericItem : SpriteBehavior
	{
		String[] description;
		int index = 0;
		public GenericItem (params String[] description) 
		{
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
				this.index = 0;
			}
		}

	}
}

