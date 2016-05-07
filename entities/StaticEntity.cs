using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Adventure
{
	public class StaticEntity : InteractableEntity
	{
		Vector2 spriteOffset, speechOffset, bakedSpriteOffset;
		Texture2D sprite;
		String spritePath;

		public StaticEntity (
			String spritePath, 
			Vector2 position, 
			EntityBehavior behavior,
			Vector2? spriteOffset = null,
			Vector2? speechOffset = null
			) : base(position, behavior)
		{

			this.spritePath = spritePath;
			this.spriteOffset = spriteOffset.GetValueOrDefault(new Vector2(0,0));
			this.speechOffset = speechOffset.GetValueOrDefault(new Vector2(0,0));
		}

		override public void Load(ContentManager content, SpriteBatch batch) {
			this.sprite = content.Load<Texture2D> (this.spritePath);
			this.bakedSpriteOffset = new Vector2 (- this.sprite.Bounds.Width / 2, - this.sprite.Bounds.Height);
		}

		override public void Draw(SpriteBatch batch, GameTime elapsed) {
			batch.Draw (this.sprite, 
				this.position +
					this.bakedSpriteOffset +
					this.spriteOffset);
			base.DrawFocusIndicator (batch, this.speechOffset);
		}


	}
}

