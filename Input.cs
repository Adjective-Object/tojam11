using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Adventure
{
	public class Input
	{

		protected static Input instance = null;

		protected static Dictionary<Key, Boolean> pressedThisFrame;
		protected static Dictionary<Key, Boolean> pressedLastFrame;

		public static void Initialize ()
		{
			pressedThisFrame = new Dictionary<Key, Boolean> ();
			pressedLastFrame = new Dictionary<Key, Boolean> ();

			// initialize all entries to false
			foreach (Key k in Enum.GetValues(typeof(Key))) {
				pressedThisFrame.Add (k, false);
				pressedLastFrame.Add (k, false);
			}
		}

		public static void Update() {
			// switch the last frame and current frame dictionaries
			Dictionary<Key, Boolean> tmp = pressedLastFrame;
			pressedLastFrame = pressedThisFrame;
			pressedThisFrame = tmp;

			// re-evaluate the current frame dictionary
			KeyboardState state = Keyboard.GetState ();
			pressedThisFrame[Key.ENTER] = state.IsKeyDown (Keys.Enter);
			pressedThisFrame[Key.UP] 	= state.IsKeyDown (Keys.Up);
			pressedThisFrame[Key.DOWN] 	= state.IsKeyDown (Keys.Down);
			pressedThisFrame[Key.LEFT] 	= state.IsKeyDown (Keys.Left);
			pressedThisFrame[Key.RIGHT] = state.IsKeyDown (Keys.Right);
			pressedThisFrame[Key.TAB]   = state.IsKeyDown (Keys.Tab);
			pressedThisFrame[Key.SHIFT] = state.IsKeyDown (Keys.LeftShift) || state.IsKeyDown (Keys.RightShift);

		}

		public static Boolean KeyDown(Key k) {
			return pressedThisFrame[k];
		}

		public static Boolean KeyPressed(Key k) {
			return (!pressedLastFrame[k]) && pressedThisFrame[k];
		}
	}

	public enum Key {
		ENTER, UP, DOWN, LEFT, RIGHT, TAB, SHIFT
	}
}

