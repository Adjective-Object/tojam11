using System;
using Microsoft.Xna.Framework;

namespace Adventure
{
	public interface EntityBehavior
	{
		Boolean IsDisabled();
		void BindToEntity(BaseEntity entity);
		void Update(GameTime elapsed);
		void RespondToInteraction(Character player);
	}
}

