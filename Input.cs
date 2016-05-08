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

		public static Boolean disabled;

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

		public const float deadZone = 0.1f;

		public static void Update() {
			// switch the last frame and current frame dictionaries
			Dictionary<Key, Boolean> tmp = pressedLastFrame;
			pressedLastFrame = pressedThisFrame;
			pressedThisFrame = tmp;

			GamePadState gps = GamePad.GetState(PlayerIndex.One);

			// re-evaluate the current frame dictionary
			KeyboardState state = Keyboard.GetState ();
			pressedThisFrame[Key.ENTER] = state.IsKeyDown (Keys.Enter) || gps.Buttons.A == ButtonState.Pressed;
			pressedThisFrame[Key.UP] 	= state.IsKeyDown (Keys.Up) || state.IsKeyDown(Keys.W) || gps.ThumbSticks.Left.Y < -deadZone;
			pressedThisFrame[Key.DOWN] 	= state.IsKeyDown (Keys.Down) || state.IsKeyDown(Keys.S) || gps.ThumbSticks.Left.Y > deadZone;
			pressedThisFrame[Key.LEFT] 	= state.IsKeyDown (Keys.Left) || state.IsKeyDown(Keys.A) || gps.ThumbSticks.Left.X > deadZone;
			pressedThisFrame[Key.RIGHT] = state.IsKeyDown (Keys.Right) || state.IsKeyDown(Keys.D) || gps.ThumbSticks.Left.X < -deadZone;
			pressedThisFrame[Key.TAB]   = state.IsKeyDown (Keys.Tab) || gps.Buttons.RightShoulder == ButtonState.Pressed;
			pressedThisFrame[Key.SHIFT] = state.IsKeyDown (Keys.LeftShift) || state.IsKeyDown (Keys.RightShift) || gps.Buttons.LeftShoulder == ButtonState.Pressed;
			pressedThisFrame[Key.I] 	= state.IsKeyDown (Keys.I) || gps.Buttons.X == ButtonState.Pressed;
			pressedThisFrame[Key.DEBUG] = state.IsKeyDown (Keys.Z);

		}

		public static Boolean KeyDown(Key k) {
			if (disabled) {
				return false;
			}
			return pressedThisFrame[k];
		}

		public static Boolean KeyPressed(Key k) {
			if (disabled) {
				return false;
			}
			return (!pressedLastFrame[k]) && pressedThisFrame[k];
		}
	}

	public enum Key {
		ENTER, UP, DOWN, LEFT, RIGHT, TAB, SHIFT, I, DEBUG
	}
}

