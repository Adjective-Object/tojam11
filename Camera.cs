using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Adventure
{
	public class Camera : BaseEntity
	{
		int [] snapHeights;
		BaseEntity following;
		Vector2 destination;
		Boolean firstUpdate = true;

		public Camera (BaseEntity following, int[] snapHeights) 
			: base (following.position)
		{
			this.following = following;
			this.snapHeights = snapHeights;
		}

		double easeSpeed = 8.0;

		override public void Update(GameTime time) {
			// figure out where we should be aiming for
			Rectangle viewBounds = AdventureGame.instance.graphics.GraphicsDevice.Viewport.Bounds;
			destination.X = following.position.X - viewBounds.Width / 2;
			destination.Y = snapHeights [0];

			float followY = following.position.Y - viewBounds.Height / 2;;
			foreach (int height in snapHeights) {
				if (Math.Abs (height - followY) < Math.Abs (destination.Y - followY)) {
					destination.Y = height;
				}
			}

			// ease to that location
			if (firstUpdate) {
				this.position = destination;
				this.firstUpdate = false;
			} else {
				this.position += (destination - this.position) * (float)(time.ElapsedGameTime.TotalSeconds * easeSpeed);
			}
		}

		override public void Load(ContentManager content, SpriteBatch batch) {}
		override public void Draw(SpriteBatch batch, GameTime time) {}

		public Matrix Transform {
			get {
				return Matrix.CreateTranslation (new Vector3 (-this.position.X, -this.position.Y, 0));
			}
		}
	}
}

