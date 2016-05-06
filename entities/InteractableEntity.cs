using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Adventure
{
	public abstract class InteractableEntity : BaseEntity
	{
		public EntityBehavior behavior;
		public Boolean focused = false, highlighted = false;
		static Texture2D speechBubble;

		public InteractableEntity (Vector2 position, EntityBehavior behavior) : base (position)
		{
			this.behavior = behavior;
		}

		override public void Load(ContentManager content, SpriteBatch batch) {
			if (speechBubble == null) {
				speechBubble = content.Load<Texture2D> ("speechbubble");
			}
		}

		protected void DrawFocusIndicator(SpriteBatch batch, Vector2 offset) {
			if (this.highlighted) {
				Color tint = this.focused ? Color.White : new Color(200, 200, 200, 0.3f);
				batch.Draw (speechBubble, this.position + offset + new Vector2(-35, -50), tint);
			}
		}
	}
}

