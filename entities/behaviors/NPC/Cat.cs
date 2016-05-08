using System;

namespace Adventure
{
	public class CatNPC : CharacterBehavior
	{
		public CatNPC (SoundFont speech) : base (speech) {
		}

		override public void RespondToInteraction(Character player) {
			// turn to face player
			character.facingLeft = player.position.X < character.position.X;


            
			// do conversation
            if (player.heldItem == ItemID.CHIP)
            {
                EmitSpeech("Eating chips eh?");
            }
            else if (player.heldItem == ItemID.SCREWDRIVER && GameStateDictionary.instance.getState("cat_screw").Equals(""))
            {
                GameStateDictionary.instance.setState("cat_screw", "yes");

                EmitSpeechOption("... what are you going to do with that screwdriver?",
                    new SpeechText.Option[] {
						                new SpeechText.Option("... i like to screw things", () => GameStateDictionary.instance.setState("cat_dtf", "yes")),
						                new SpeechText.Option("The antenna needs fixing", () => GameStateDictionary.instance.setState("cat_dtf", "no")),
					                },
                    null
                );
            }
            else if (GameStateDictionary.instance.getState("cat_dtf").Equals("yes"))
            {
                EmitSpeech("Call me after the party ;)");
                AdventureGame.instance.AddEndGameMessage("Got the cat's phone number.");
            }
            else
            {
                switch (interactionCount)
                {
                    case 0:
                        EmitSpeech("I'm tired.", SpeechText.SpeechMode.PLAYER_CONTROLLED, null, () => { this.RespondToInteraction(player); });
                        break;
                    case 1:
                        EmitSpeechOption("what's your favorite?",
                            new SpeechText.Option[] {
						new SpeechText.Option("chips", () => GameStateDictionary.instance.setState("cat_favorite", "chips")),
						new SpeechText.Option("dip", () => GameStateDictionary.instance.setState("cat_favorite", "dip")),
						new SpeechText.Option("salsa", () => GameStateDictionary.instance.setState("cat_favorite", "salsa")),
					},
                            walkAway(
                                player, character,
                                () =>
                                {
                                    GameStateDictionary.instance.setState("cat_favorite", "being an asshole");
                                    AdventureGame.instance.AddEndGameMessage("Pissed off the cat.");
                                    EmitSpeech("where are you going?");
                                })
                        );
                        break;
                    case 2:
                        EmitSpeech("so you like " + GameStateDictionary.instance.getState("cat_favorite") + "?",
                            SpeechText.SpeechMode.PLAYER_CONTROLLED,
                            null, () =>
                            {
                                this.RespondToInteraction(player);
                            });
                        break;
                    default:
                        EmitSpeech("I hate " + GameStateDictionary.instance.getState("cat_favorite") + ", we can't be friends");
                        break;
                }
                interactionCount++;
            }
		}
	}
}

