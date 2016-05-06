using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Adventure
{
	public abstract class BaseEntity
	{
		protected Vector2 position = new Vector2 (0, 0);

		public BaseEntity (Vector2 position) {
			this.position = position;
		}

		public abstract void Load(ContentManager content, SpriteBatch batch);
		public abstract void Update(GameTime time);
		public abstract void Draw(SpriteBatch batch, GameTime time);
	}
}

