using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Adventure
{
	public class Inventory
	{
		public static List<ItemID> contents = new List<ItemID>();
		public static int capacity = 5;
		public static int selector = 0;
		public static Boolean active;

		static double ACTIVATION_WINDOW = 1;
		static double lastActivated = -ACTIVATION_WINDOW;

		static Texture2D blackRect;

		public static Boolean IsFull {
			get { return contents.Count >= capacity; }
		}

		public static void Add(ItemID i) {
			contents.Add (i);
		}

		public static Item SelectedItem {
			get { return Item.Get (contents [selector]); }
		}

		public static ItemID SelectedItemID {
			get { return contents[selector]; }
		}

		static Easing<Vector2> offset = new Easing<Vector2>(
			new Vector2(-80, 0),
			new Vector2(0, 0),
			10
		);
		const int TEXT_MARGIN = 10;
		static Easing<Vector2> descriptionOffset = new Easing<Vector2> (
			                                           new Vector2 (0, TEXT_MARGIN),
			                                           new Vector2 (0, -64 - TEXT_MARGIN),
			                                           5
		                                           );

		static Vector2 OFFSET_VECTOR = new Vector2(0, 80);

		public static void LoadContent(ContentManager c, SpriteBatch b) {
			blackRect = new Texture2D(b.GraphicsDevice, 1, 1);
			blackRect.SetData(new Color[] { Color.Black });
		}

		public static void Update(GameTime time) {
			// update Inventory
			if (Input.KeyPressed (Key.I)) {
				int toNext = Input.KeyDown(Key.SHIFT) ? -1 : 1;
				selector = (selector + toNext + contents.Count) % contents.Count;
				lastActivated = time.TotalGameTime.TotalSeconds;
			}

			active = time.TotalGameTime.TotalSeconds - lastActivated < ACTIVATION_WINDOW;
			offset.Update (time, active);
			descriptionOffset.Update (time, active);
		}

		public static void Draw(SpriteBatch batch) {
			Vector2 origin = new Vector2(10, 10) + offset.current;
			for (int i = 0; i < contents.Count; i++) {
				if (i == selector) {
					batch.Draw (Item.Get (contents [i]).texture, origin + OFFSET_VECTOR * i);
				}
				else {
					batch.Draw (
						Item.Get (contents [i]).texture, 
						origin + OFFSET_VECTOR * i, 
						new Color(100,100,100, 100));
				}
			}

			Rectangle screenBounds = AdventureGame.ScreenBounds;
			Vector2 textLocation = new Vector2 (TEXT_MARGIN, screenBounds.Height) + descriptionOffset.current;
			batch.Draw (
				blackRect, null,
				new Rectangle(0, (int)(textLocation.Y - TEXT_MARGIN), screenBounds.Width, screenBounds.Height)
			);
			batch.DrawString (
				AdventureGame.DefaultFont,
				SelectedItem.description,
				textLocation,
				Color.White);
		}

	}
}

