using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Adventure
{
	public class StaticEntity : InteractableEntity
	{
		Dictionary<string, string> alternateSpritePaths;
		Dictionary<string, Texture2D> alternateSprites;
		Vector2 spriteOffset, speechOffset, bakedSpriteOffset;
		Texture2D sprite;
		String spritePath;

		public StaticEntity (
			String spritePath, 
			Vector2 position, 
			EntityBehavior behavior,
			Vector2? spriteOffset = null,
			Vector2? speechOffset = null,
			Dictionary<string, string> alternateSprites = null
			) : base(position, behavior)
		{
			this.spritePath = spritePath;
			this.spriteOffset = spriteOffset.GetValueOrDefault(new Vector2(0,0));
			this.speechOffset = speechOffset.GetValueOrDefault(new Vector2(0,-70));
			this.alternateSpritePaths = alternateSprites != null ? alternateSprites : new Dictionary<String, String>();
		}

		public void SwitchSprite(String spriteName){
			if (this.alternateSprites.ContainsKey(spriteName)) {
				Console.WriteLine ("requested change to sprite " + sprite);
				this.sprite = this.alternateSprites [spriteName];
			} else {
				Console.WriteLine ("sprite entity tried to switch to non-existant sprite " + sprite);
			}
		}

		override public void Load(ContentManager content, SpriteBatch batch) {
            if (this.spritePath != null)
            {
                this.sprite = content.Load<Texture2D>(this.spritePath);
                this.bakedSpriteOffset = new Vector2(-this.sprite.Bounds.Width / 2, -this.sprite.Bounds.Height);
            }

			this.alternateSprites = new Dictionary<String, Texture2D> ();
			this.alternateSprites.Add ("default", sprite);
			foreach (String k in this.alternateSpritePaths.Keys) {
				this.alternateSprites.Add (k, content.Load<Texture2D>(alternateSpritePaths[k]));
			}
		}

		override public void Draw(SpriteBatch batch, GameTime elapsed) {
            if (this.spritePath != null)
            {
                batch.Draw(this.sprite,
                    this.position +
                        this.bakedSpriteOffset +
                        this.spriteOffset);
            }
            base.DrawFocusIndicator(batch, this.speechOffset);
            
		}


	}
}

