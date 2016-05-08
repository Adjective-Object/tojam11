using System;
using Microsoft.Xna.Framework;

namespace Adventure
{
	public interface EntityBehavior
	{
		Boolean IsDisabled();
		void BindToEntity(InteractableEntity entity);
		void Update(GameTime elapsed);
		void RespondToInteraction(Character player);
	}
}

