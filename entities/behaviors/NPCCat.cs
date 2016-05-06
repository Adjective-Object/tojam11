using System;

namespace Adventure
{
	public class NPCCat : CharacterBehavior
	{
		int interactionCount = 0;
		override public void RespondToInteraction() {
			switch (interactionCount) {
			case 0:
				EmitSpeech ("this party sucks..");
				break;
			case 1:
				EmitSpeech ("so who dragged you here?");
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

