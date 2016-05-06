using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Adventure
{
	public abstract class BaseEntity 
	{
		public Vector2 position = new Vector2 (0, 0);
		protected Boolean _alive = true;
		public Boolean alive {
			get { return _alive; }
		}

		public BaseEntity (Vector2 position) {
			this.position = position;
		}

		public abstract void Load(ContentManager content, SpriteBatch batch);
		public abstract void Update(GameTime time);
		public abstract void Draw(SpriteBatch batch, GameTime time);

	}
}

