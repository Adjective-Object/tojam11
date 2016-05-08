using System;

namespace Adventure
{
	public class ChickenGangMember : CharacterBehavior
	{

		protected int interactionCount;
		static String[] sayings = {
			"cluck", "bwaack", "bwaaaaaacccck", "cluck, cluck"
		};
		Random r = new Random();

		public ChickenGangMember (SoundFont speech) : base (speech) {
		}

		override public void RespondToInteraction(Character player) {
			// turn to face player
			character.facingLeft = player.position.X < character.position.X;

			this.EmitSpeech(sayings[r.Next (0, sayings.Length)]);
		}
	}
}