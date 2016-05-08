using System;

namespace Adventure
{
    public class BroNPC : CharacterBehavior
    {
        public BroNPC(SoundFont speech)
            : base(speech)
        {
        }

        override public void RespondToInteraction(Character player)
        {
            // turn to face player
            character.facingLeft = player.position.X < character.position.X;

            // do conversation

            if (GameStateDictionary.instance.getState("bro_asshole").Equals("yes"))
            {
                EmitSpeech("Don't know who invited you bro, but fuck off!");
                return;
            }

            if (GameStateDictionary.instance.getState("bro_final").Equals("yes"))
            {
                EmitSpeech("Hey brah! Fun party, I'll call you some time!");
                return;
            }

            if(GameStateDictionary.instance.getState("bro_friendly").Equals("yes"))
            {
                EmitSpeechOption("Man that sports team, they really did it this time",
                    new SpeechText.Option[] {
					                    new SpeechText.Option("Yeah they really screwed up this time.", () => GameStateDictionary.instance.setState("bro_friendly", "no")),
					                    new SpeechText.Option("Yeah can't believe they did so well", () => GameStateDictionary.instance.setState("bro_final", "yes")),
					                },
                    walkAway(
                        player, character,
                        () =>
                        {
                            GameStateDictionary.instance.setState("bro_asshole", "yes");
                            EmitSpeech("???OK CYA BRAH");
                        })
                );
            }
            else if(GameStateDictionary.instance.getState("bro_friendly").Equals("no"))
            {
                if(!GameStateDictionary.instance.getState("cat_favorite").Equals(""))
                {
                    EmitSpeech("Someone mentioned you really like " + GameStateDictionary.instance.getState("cat_favorite") + ", you're a ball of excitement...");
                }
                else
                {
                    EmitSpeech("... Busy bro, trying to score here... ");
                }
            }
            else
            {
                EmitSpeechOption("Hey Brah",
                    new SpeechText.Option[] {
						                new SpeechText.Option("... hey ...", () => GameStateDictionary.instance.setState("bro_friendly", "no")),
						                new SpeechText.Option("yoooo", () => GameStateDictionary.instance.setState("bro_friendly", "yes")),
					                },
                    walkAway(
                        player, character,
                        () =>
                        {
                            GameStateDictionary.instance.setState("bro_asshole", "yes");
                            EmitSpeech("COOL BRO, COOOOL");
                        })
                );
            }
 
        }
    }
}

