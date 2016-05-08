using Microsoft.Xna.Framework;
using System;

namespace Adventure
{
	public class ChickenGangMember : CharacterBehavior
	{
        double speedx = 300;
        double speedy = 190;

        Vector2 targetPosition;
        Vector2 startPosition;
        bool firstUpdate = true;

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

        override public void Update(GameTime elapsed)
        {
            if (firstUpdate)
            {
                startPosition = character.position;
                firstUpdate = false;
            }
            
            if (targetPosition != Vector2.Zero && Vector2.Distance(targetPosition, character.position) > 5)
            {
                character.PlayAnimBody("walk");
                Vector2 movement = new Vector2(0, 0);

                if (Math.Abs(targetPosition.X - character.position.X) > 3)
                {
                    if (targetPosition.X > character.position.X)
                    {
                        movement.X += (float)(elapsed.ElapsedGameTime.TotalSeconds);
                    }
                    else
                    {
                        movement.X -= (float)(elapsed.ElapsedGameTime.TotalSeconds);
                    }
                }

                if (Math.Abs(targetPosition.Y - character.position.Y) > 3)
                {
                    if (targetPosition.Y > character.position.Y)
                    {
                        movement.Y += (float)(elapsed.ElapsedGameTime.TotalSeconds);
                    }
                    else
                    {
                        movement.Y -= (float)(elapsed.ElapsedGameTime.TotalSeconds);
                    }
                }

                if (movement.X < 0)
                    character.facingLeft = true;
                else if (movement.X > 0)
                    character.facingLeft = false;

                movement.Normalize();
                movement = movement * (float)elapsed.ElapsedGameTime.TotalSeconds;
                movement.X *= (float)speedx;
                movement.Y *= (float)speedy;
                Vector2 newPosition = character.position + movement;

                if (!MoveCharacter(newPosition))
                {
                    targetPosition = character.position;
                }
            }
            else
            {
                character.PlayAnimBody("idle");
                if (AdventureGame.RandomNumber.Next(500) == 1)
                    targetPosition = startPosition + new Vector2(AdventureGame.RandomNumber.Next(-100, 100), AdventureGame.RandomNumber.Next(-100, 100));
            }
            base.Update(elapsed);
        }
	}
}