using System;

namespace Adventure
{
    public class Door : SpriteBehavior
    {
        public override void RespondToInteraction(Character player)
        {
            this.EmitSpeechOption(
                "Door",
                new SpeechText.Option[] {
					new SpeechText.Option("Leave the Party", () => {
                        AdventureGame.instance.SetEndGame();
					}),
					new SpeechText.Option("Stay a while longer", () => {
					}),
				},
                walkAway(player, entity, () =>
                {
                })
            );
        }
    }
}

