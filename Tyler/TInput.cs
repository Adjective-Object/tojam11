using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Adventure
{
    public static class TInput
    {
        private static MouseState mouse = Mouse.GetState();
        private static MouseState mousePrev = Mouse.GetState();
        public static KeyboardState kbs = Keyboard.GetState();
        public static KeyboardState pkbs = Keyboard.GetState();
        public static GamePadState gps = GamePad.GetState(PlayerIndex.One);
        public static GamePadState pgps = GamePad.GetState(PlayerIndex.One);

        public static void Update()
        {
            mousePrev = mouse;
            pkbs = kbs;
            pgps = gps;
            mouse = Mouse.GetState();
            kbs = Keyboard.GetState();
            gps = GamePad.GetState(PlayerIndex.One);
        }

        #region PRESSED GamePad Button
        public static bool PressedA()
        {
            if (gps.Buttons.A == ButtonState.Pressed && pgps.Buttons.A == ButtonState.Released) return true;
            else return false;
        }
        public static bool PressedB()
        {
            if (gps.Buttons.B == ButtonState.Pressed && pgps.Buttons.B == ButtonState.Released) return true;
            else return false;
        }
        public static bool PressedX()
        {
            if (gps.Buttons.X == ButtonState.Pressed && pgps.Buttons.X == ButtonState.Released) return true;
            else return false;
        }
        public static bool PressedY()
        {
            if (gps.Buttons.Y == ButtonState.Pressed && pgps.Buttons.Y == ButtonState.Released) return true;
            else return false;
        }
        public static bool PressedStart()
        {
            if (gps.Buttons.Start == ButtonState.Pressed && pgps.Buttons.Start == ButtonState.Released) return true;
            else return false;
        }
        public static bool PressedBack()
        {
            if (gps.Buttons.Back == ButtonState.Pressed && pgps.Buttons.Back == ButtonState.Released) return true;
            else return false;
        }
        public static bool PressedLeftBumper()
        {
            if (gps.Buttons.LeftShoulder == ButtonState.Pressed && pgps.Buttons.LeftShoulder == ButtonState.Released) return true;
            else return false;
        }
        public static bool PressedRightBumper()
        {
            if (gps.Buttons.RightShoulder == ButtonState.Pressed && pgps.Buttons.RightShoulder == ButtonState.Released) return true;
            else return false;
        }

        public static bool PressedDPadUp()
        {
            if (gps.DPad.Up == ButtonState.Pressed && pgps.DPad.Up == ButtonState.Released) return true;
            else return false;
        }
        public static bool PressedDPadDown()
        {
            if (gps.DPad.Down == ButtonState.Pressed && pgps.DPad.Down == ButtonState.Released) return true;
            else return false;
        }
        #endregion



        #region Keyboard Controls
        public static KeyboardState GetKeyboardState()
        {
            return TInput.kbs;
        }

        public static KeyboardState GetPreviousKeyboardState()
        {
            return TInput.pkbs;
        }

        public static bool IsKeyDown(Keys key)
        {
            return kbs.IsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key)
        {
            return kbs.IsKeyUp(key);
        }

        public static bool KeyPressed(Keys key)
        {
            return (kbs.IsKeyDown(key) && pkbs.IsKeyUp(key));
        }

        public static bool KeyReleased(Keys key)
        {
            return (pkbs.IsKeyDown(key) && kbs.IsKeyUp(key));
        }
        #endregion

        #region Mouse Controls
        public static bool MouseScrollDown
        {
            get { return (mouse.ScrollWheelValue < mousePrev.ScrollWheelValue); }
        }

        public static bool MouseScrollUp
        {
            get { return (mouse.ScrollWheelValue > mousePrev.ScrollWheelValue); }
        }

        public static bool MouseRightButtonPressed
        {
            get { return (mouse.RightButton == ButtonState.Pressed && mousePrev.RightButton == ButtonState.Released); }
        }

        public static bool MouseRightButtonTapped
        {
            get { return (mouse.RightButton == ButtonState.Pressed && mousePrev.RightButton == ButtonState.Released); }
        }

        public static bool MouseRightButtonReleased
        {
            get { return (mouse.RightButton == ButtonState.Released && mousePrev.RightButton == ButtonState.Pressed); }
        }

        public static bool MouseMiddleButtonTapped
        {
            get { return (mouse.MiddleButton == ButtonState.Pressed && mousePrev.MiddleButton == ButtonState.Released); }
        }

        public static bool MouseLeftButtonPressed
        {
            get
            {
#if __IOS__ 
				if (clicked) Console.WriteLine("Mouse Left Button Pressed");
				return clicked;
#else
                return (mouse.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released);
#endif
            }
        }

        public static bool MouseLeftButtonReleased
        {
            get
            {
#if __IOS__ 
				return released;
#else
                return (mousePrev.LeftButton == ButtonState.Pressed && mouse.LeftButton == ButtonState.Released);
#endif
            }
        }

        public static bool MouseLeftButtonDown
        {
            get
            {
#if __IOS__ 
				return touching;
#else
                return mouse.LeftButton == ButtonState.Pressed;
#endif
            }
        }

        public static bool MouseRightButtonDown
        {
            get { return mouse.RightButton == ButtonState.Pressed; }
        }

        public static bool MouseMiddleButtonDown
        {
            get { return mouse.MiddleButton == ButtonState.Pressed; }
        }

        public static Point MousePosition
        {
            get
            {
#if __IOS__ 

//				if(Game1.DeviceType == Game1.DeviceTypes.ipad2)
//				{
//					Console.WriteLine(new Point(touchPosition.X / 2, touchPosition.Y / 2));
//					return new Point(touchPosition.X / 2, touchPosition.Y / 2);
//				}
//				else if(Game1.DeviceType == Game1.DeviceTypes.iphone6p)
//				{
//					Console.WriteLine(new Point(touchPosition.X / 2, touchPosition.Y / 2));
//					return new Point(touchPosition.X / 2, touchPosition.Y / 2);
//				}
//				else
				{
					Console.WriteLine(touchPosition.ToString());
					return touchPosition;
				}

#else
                return new Point(mouse.X, mouse.Y);

#endif
            }
        }


        //public static Vector2 MousePosition_InWorld
        //{
        //    get
        //    {
        //        Vector2 mp = new Vector2(mouse.X, mouse.Y);
        //        Vector2 cursorPosition = new Vector2((int)Game1.Camera.Center.X - 640, (int)Game1.Camera.Center.Y - 360) + mp;

        //        if (Game1.Camera.Zoom != 1)
        //        {
        //            Vector2 c1 = new Vector2(mp.X - 640, mp.Y - 360) / Game1.Camera.Zoom;
        //            cursorPosition = new Vector2((int)Game1.Camera.Center.X, (int)Game1.Camera.Center.Y) + c1; 
        //        }

        //        return cursorPosition;
        //    }
        //}

        public static Point MousePrevPosition
        {
            get { return new Point(mousePrev.X, mousePrev.Y); }
        }
        #endregion



        internal static void ResetKeys()
        {
//            Right_Key = DEFAULT_Right_Key;
//            Left_Key = DEFAULT_Left_Key;
//            Up_Key = DEFAULT_Up_Key;
//            Down_Key = DEFAULT_Down_Key;
//
//            A_Key = DEFAULT_A_Key;
//            B_Key = DEFAULT_B_Key;
//            C_Key = DEFAULT_C_Key;
//
//            Pause_Key = DEFAULT_Pause_Key;
//            Map_Key = DEFAULT_Map_Key;
        }
    }
}