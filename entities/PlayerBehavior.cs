using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Adventure
{
	public class PlayerBehavior : CharacterBehavior
	{
		double speedx = 300;
		double speedy = 190;

		public PlayerBehavior ()
		{
		}

		public override void Update(GameTime time) {
			Vector2 movement = new Vector2 (0, 0);
			if (Input.KeyDown(Key.LEFT)) {
				movement.X -= (float) (time.ElapsedGameTime.TotalSeconds);
				character.facingLeft = true;
			}
			if (Input.KeyDown (Key.RIGHT)) {
				movement.X += (float) (time.ElapsedGameTime.TotalSeconds);	
				character.facingLeft = false;
			}
			if (Input.KeyDown (Key.UP)) {
				movement.Y -= (float) (time.ElapsedGameTime.TotalSeconds);
			}
			if (Input.KeyDown (Key.DOWN)) {
				movement.Y += (float) (time.ElapsedGameTime.TotalSeconds);
			}

			if (movement.LengthSquared() != 0) {
				
				movement.Normalize ();
				movement = movement * (float) time.ElapsedGameTime.TotalSeconds;
				movement.X *= (float) speedx;
				movement.Y *= (float) speedy;
				character.position += movement;
		
			}
			base.Update(time);
		}
	}
}

