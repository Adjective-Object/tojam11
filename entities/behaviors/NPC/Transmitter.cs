using System;

namespace Adventure
{
	public class Transmitter : SpriteBehavior
	{
		public Transmitter (SoundFont speech = null) : base (speech) {
		}

		int currentState = 0;
		// 0 -> empty
		// 1 -> microwave
		// 2 -> microwave + wire
		// 3 -> microwave + wire + antenna
		override public void RespondToInteraction(Character player) {

			switch (this.currentState) {
			case 0:
				if (player.heldItem == ItemID.MICROWAVE) {
					Inventory.RemoveCurrent ();
					EmitSpeech ("The first step is complete...");
					currentState++;
					((StaticEntity)this.entity).SwitchSprite ("microwave");
				} else {
					EmitSpeech ("There's a weird gap in the grass here.");
				}
				break;
			case 1:
				if (player.heldItem == ItemID.STRINGS) {
					Inventory.RemoveCurrent ();
					EmitSpeech ("The device is almost complete...");
					currentState++;
					((StaticEntity)this.entity).SwitchSprite ("wire");
				} else {
					EmitSpeech ("I need wire. Where can I get wire?");
				}
				break;
			case 2:
				if (player.heldItem == ItemID.ANTENNA) {
					Inventory.RemoveCurrent ();
					EmitSpeech ("THE TIME OF AWAKENING COMETH");
					currentState++;
					((StaticEntity)this.entity).SwitchSprite ("antenna");
				} else {
					EmitSpeech ("...Antenna...");
				}
				break;
			case 3:
				EmitSpeechOption(
					"activate the device?",
					new  SpeechText.Option[] {
						new SpeechText.Option("yes", ()=>{
							GameStateDictionary.SetFlag ("alien", true);
							LogEndMessage("ACTIVATED THE ALIEN TRANSMITTER");
							AdventureGame.instance.SetEndGame();
						}),
						new SpeechText.Option("no",  ()=>{})
					});
				break;
			}

			((PlayerBehavior) player.behavior).triggeredText = this.speechReference;
		}
	}
}

