using System;

namespace Adventure
{
	public class NPCCat : CharacterBehavior
	{
		int interactionCount = 0;
		string favorite;

		override public void RespondToInteraction(Character player) {
			// turn to face player
			character.facingLeft = player.position.X < character.position.X;

			// do conversation
			switch (interactionCount) {
			case 0:
				EmitSpeechOption ("what's your favorite?",
					new SpeechText.Option[] {
						new SpeechText.Option("chips", () => this.favorite = "chips"),
						new SpeechText.Option("dip", () => this.favorite = "dip"),
						new SpeechText.Option("salsa", () => this.favorite = "salsa"),
					},
					walkAway(
						player, character,
						() => {
							this.favorite = "being an asshole"; 
							EmitSpeech("where are you going?");
						})
				);
				break;
			case 1:
				EmitSpeech ("so you like " + favorite + "?");
				break;
			case 2:
				EmitSpeech ("I'm tired.");
				break;
			case 3:
				EmitSpeech ("I was told there would be catnip.");
				break;
			default:
				EmitSpeech ("I'm out of things to say to you");
				break;
			}
			interactionCount++;
		}
	}
}

