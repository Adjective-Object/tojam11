using System;
using Microsoft.Xna.Framework;

namespace Adventure
{
	public class Easing
	{
		public Vector2 current;
		Vector2 start;
		Vector2 finish;
		float speed;
		public Easing(Vector2 start, Vector2 finish, float speed)
		{
			this.start = start;
			this.finish = finish;
			this.speed = speed;
			this.current = start;
		}

		public Vector2 Update(GameTime t, bool condition) {
			Vector2 target = condition ? finish : start;
			this.current += (target - start) * (float) t.ElapsedGameTime.TotalSeconds * this.speed;
			return this.current;
		}
	}
}

