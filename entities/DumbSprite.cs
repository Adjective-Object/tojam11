using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Adventure
{
	public class DumbSprite : BaseEntity
	{
		Vector2 spriteOffset, bakedSpriteOffset;
		Texture2D sprite;
		String spritePath;

		public DumbSprite (
			String spritePath, 
			Vector2 position, 
			Vector2? spriteOffset = null
		) : base(position)
		{
			this.spritePath = spritePath;
			this.spriteOffset = spriteOffset.GetValueOrDefault(new Vector2(0,0));
		}

		override public void Load(ContentManager content, SpriteBatch batch) {
			if (this.spritePath != null)
			{
				this.sprite = content.Load<Texture2D>(this.spritePath);
				this.bakedSpriteOffset = new Vector2(-this.sprite.Bounds.Width / 2, -this.sprite.Bounds.Height);
			}
		}

		override public void Update(GameTime time) {
		}

		override public void Draw(SpriteBatch batch, GameTime elapsed) {
			if (this.spritePath != null)
			{
				batch.Draw(this.sprite,
					this.position +
					this.bakedSpriteOffset +
					this.spriteOffset);
			}
		}
	}
}

