using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Adventure
{
	public class Item
	{

		static Dictionary<ItemID, Item> items = new Dictionary<ItemID, Item> ();
		static Boolean initialized = false;

		public static void LoadContent(ContentManager content)
		{
			if (initialized) return;
			initialized = true;

			items.Add (ItemID.NO_ITEM, 		new Item (null, 		"No Item", 		"you put away your item"));
			items.Add (ItemID.KNIFE, 		new Item (content.Load<Texture2D>("items/knife"), 		"Knife", 		"A harmless kitchen knife"));
			items.Add (ItemID.KNIFE_USED, 	new Item (content.Load<Texture2D>("items/knife_used"), 	"Used Knife", 	"You should probably get rid of this"));
			items.Add (ItemID.BEER, 		new Item (content.Load<Texture2D>("items/beer_1"), 		"Beer", 		"A craft beer. The label says 'Mister Bone's Wild IPA'."));
			items.Add (ItemID.POISON, 		new Item (content.Load<Texture2D>("items/poison"), 		"Poison", 		"Rat poison"));
			items.Add (ItemID.ANTENNA, 		new Item (content.Load<Texture2D>("items/antenna"), 	"Antenna", 		"The antenna to the TV in the basement. Why did you take it?"));

			foreach (ItemID k in items.Keys) {
				items [k].type = k;
			}
		}

		public static Item Get(ItemID i) {
			return items [i];
		}

		public Texture2D texture;
		public ItemID type;
		public String name, description;
		public Item (Texture2D texture, String name, String description)
		{
			this.texture = texture;
			this.name = name;
			this.description = description;
		}

	}

	public enum ItemID {
		NO_ITEM,
		KNIFE, KNIFE_USED,
		BEER, POISON, ANTENNA
	}

}

