using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Adventure
{
	public class Inventory
	{
		public static List<ItemID> contents = new List<ItemID>();
		public static int capacity = 5;
		public static Boolean active;

		public static Boolean IsFull {
			get { return contents.Count >= capacity; }
		}

		public static void Add(ItemID i) {
			contents.Add (i);
		}

		public static void Toggle(){
			active = !active;
		}



		static Easing offset = new Easing(
			new Vector2(0, 100),
			new Vector2(0, 0),
			10
		);

		static Vector2 OFFSET_VECTOR = new Vector2(0, 80);


		public static void Update(GameTime time) {
			offset.Update (time, active);
		}

		public static void Draw(SpriteBatch batch) {
			Vector2 origin = new Vector2(100, 100) + offset.current;
			for (int i = 0; i < contents.Count; i++) {
				batch.Draw(Item.Get(contents[i]).texture, origin + OFFSET_VECTOR * i);
			}
		}

	}
}

