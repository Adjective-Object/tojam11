using System;

namespace Adventure
{
	public class Atari : SpriteBehavior
	{
		public override void RespondToInteraction(Character player)
		{
			this.EmitSpeechOption(
				"ROBOTHAUS 2200 GAMING CONSOLE",
				new SpeechText.Option[] {
					new SpeechText.Option("Play Asteroids", () => {
						AdventureGame.atari.StartGame(0);
					}),
					new SpeechText.Option("Play Breakout", () => {
						AdventureGame.atari.StartGame(1);
					}),
				}
			);
			((PlayerBehavior) player.behavior).triggeredText = this.speechReference;
		}
	}
}

