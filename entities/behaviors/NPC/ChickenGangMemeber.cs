using System;

namespace Adventure
{
	public class ChickenGangMember : CharacterBehavior
	{

		static String[] sayings = {
			"cluck", "bwaack", "bwaaaaaacccck", "cluck, cluck"
		};

		public ChickenGangMember (SoundFont speech) : base (speech) {
		}

		override public void RespondToInteraction(Character player) {
			// turn to face player
			character.facingLeft = player.position.X < character.position.X;
			EmitRandom (sayings);
		}
	}
}