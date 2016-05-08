using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Adventure
{
	public class DissapearingWall : BaseEntity
	{
		
		Texture2D texture;
		String assetPath;
		Easing<float> opacity;
		Rectangle container;
		BaseEntity following;
		Boolean behind = false;

		public DissapearingWall (String assetPath, Vector2 position, BaseEntity tracking) : base(position)
		{
			this.assetPath = assetPath;
			this.opacity = new Easing<float> (1, 0, 5);
			this.following = tracking;
		}

		public override void Load(ContentManager content, SpriteBatch batch) {
			this.texture = content.Load<Texture2D> (assetPath);
			this.container = new Rectangle (
				(int) this.position.X,
				(int)this.position.Y - this.texture.Bounds.Height,
				this.texture.Bounds.Width,
				this.texture.Bounds.Height
			);
		}

		public override void Update(GameTime time) {
			behind = this.container.Contains (this.following.position);
			this.opacity.Update (time, behind);
		}

		public override void Draw(SpriteBatch batch, GameTime time) {
			batch.Draw (
				this.texture,
				this.position + new Vector2(0, - this.container.Height),
				Color.White * this.opacity.current);
		}

		public override int SortingLayer {
			get {
				if (this.container.Contains (this.following.position)) {
					return this.following.SortingLayer - 1;
				} else {
					return 0;
				}
			}
		}
			 
	}
}

