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

		static Vector2 hiddenOffset = new Vector2(0, 20), highlightOffset = new Vector2(0,10), focusOffset = new Vector2(0, 0);
		static float hiddenAlpha = 0, highlightAlpha = 0.3f, focusAlpha = 1.0f;

		float animatedAlpha = hiddenAlpha;
		Vector2 animatedOffset;

		public InteractableEntity (Vector2 position, EntityBehavior behavior) : base (position)
		{
			this.behavior = behavior;
			this.behavior.BindToEntity (this);
		}

		override public void Load(ContentManager content, SpriteBatch batch) {
			if (speechBubble == null) {
				speechBubble = content.Load<Texture2D> ("speechbubble");
			}
		}

		static float animSpeed = 10.0f;

		public override void Update(GameTime time) {
			Boolean effectiveHighlighting = this.highlighted && !this.behavior.IsDisabled ();
			Vector2 goalOffset = effectiveHighlighting ? this.focused ? focusOffset : highlightOffset : hiddenOffset;
			float goalAlpha = effectiveHighlighting ? this.focused ? focusAlpha : highlightAlpha : hiddenAlpha;

			animatedAlpha = animatedAlpha + (goalAlpha - animatedAlpha) * (float)time.ElapsedGameTime.TotalSeconds * animSpeed;
			animatedOffset = animatedOffset + (goalOffset - animatedOffset) * (float)time.ElapsedGameTime.TotalSeconds * animSpeed;

			this.behavior.Update (time);
		}

		protected void DrawFocusIndicator(SpriteBatch batch, Vector2 givenOffset) {
			Color tint = new Color (animatedAlpha, animatedAlpha, animatedAlpha, animatedAlpha);
			batch.Draw (speechBubble, this.position + givenOffset + new Vector2(-35, -50) + animatedOffset, tint);
		}
	}
}

