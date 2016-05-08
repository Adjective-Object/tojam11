using System;

namespace Adventure
{
	public class CatNPC : CharacterBehavior
	{
		int interactionCount = 0;

		public CatNPC (SoundFont speech) : base (speech) {
		}

		override public void RespondToInteraction(Character player) {
			// turn to face player
			character.facingLeft = player.position.X < character.position.X;

			// do conversation
			switch (interactionCount) {
			case 0:
				EmitSpeech ("I'm tired.", SpeechText.SpeechMode.PLAYER_CONTROLLED, null, () => {this.RespondToInteraction(player);});
				break;
			case 1:
				EmitSpeechOption ("what's your favorite?",
					new SpeechText.Option[] {
						new SpeechText.Option("chips", () => GameStateDictionary.instance.setState("cat_favorite", "chips")),
						new SpeechText.Option("dip", () => GameStateDictionary.instance.setState("cat_favorite", "dip")),
						new SpeechText.Option("salsa", () => GameStateDictionary.instance.setState("cat_favorite", "salsa")),
					},
					walkAway(
						player, character,
						() => {
							GameStateDictionary.instance.setState("cat_favorite", "being an asshole"); 
							EmitSpeech("where are you going?");
						})
				);
				break;
			case 2:
                EmitSpeech("so you like " + GameStateDictionary.instance.getState("cat_favorite") + "?");
				break;
			case 3:
				EmitSpeech ("I was told there would be catnip.");
				break;
            case 4:
                AdventureGame.instance.AddEndGameMessage("Talked to the cat");
                AdventureGame.instance.SetEndGame();
                break;
			default:
				EmitSpeech ("I'm out of things to say to you");
				break;
			}
			interactionCount++;
		}
	}
}

