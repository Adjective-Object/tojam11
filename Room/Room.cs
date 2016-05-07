using System;
using Microsoft.Xna.Framework;

namespace Adventure
{
	public interface Room
	{
		void Update (GameTime gameTime);
		void Initialize ();
		void LoadContent ();
		void Draw (GameTime gameTime);
	}
}

