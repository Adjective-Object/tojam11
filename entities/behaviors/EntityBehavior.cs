using System;

namespace Adventure
{
	public interface EntityBehavior
	{
		Boolean IsDisabled();
		void RespondToInteraction(Character player);
	}
}

