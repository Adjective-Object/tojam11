using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Adventure
{
	// class that handles assembling and animating sprites
	public class Character : BaseEntity
	{

		Dictionary <String, Texture2D> headTextures, bodyTextures;
		string headName, bodyName;
		Texture2D head, body;

		Color [] headColors, bodyColors;
		static Color [] referenceColors = {
			new Color(255,0,0),
			new Color(0,255,0),
			new Color(0,0,255),
			new Color(255,255,0),
			new Color(255,0,255),
			new Color(0,255,255),
		};

		public Character (
			Vector2 position,
			String headName, Color[] headColors,
			String bodyName, Color[] bodyColors) : base(position)
		{
			this.headName = headName;
			this.bodyName = bodyName;
			this.headColors = headColors;
			this.bodyColors = bodyColors;
		}

		// load and set the colors of the sprites
		override public void Load(ContentManager content, SpriteBatch batch) {
			// initialize either dictionary by rendering the things
			Console.WriteLine("npc_parts/" + headName + "_idle.png");
			headTextures = new Dictionary<String, Texture2D>() {
				{"idle", ApplyPallette(content.Load<Texture2D>("npc_parts/head_" + headName + "_idle"), headColors)}
			};

			bodyTextures = new Dictionary<String, Texture2D>() {
				{"idle", ApplyPallette(content.Load<Texture2D>("npc_parts/body_" + bodyName + "_idle"), bodyColors)}
			};

		}

		// apply some color pallette
		private Texture2D ApplyPallette(Texture2D texture, Color [] palette) {
			Texture2D newTexture = new Texture2D(
				texture.GraphicsDevice, texture.Width, texture.Height);

			Color[] data = new Color[texture.Width * texture.Height];
			texture.GetData(data);

			for (int i=0; i< palette.Length ; i++) {
				for (int j = 0; j < data.Length; j++) {
					if (data [j] == referenceColors [i]) {
						data [j] = palette [i];
					}
				}
			}

			newTexture.SetData (data);
			return newTexture;
		}
			

		override public void Update(GameTime time) {
			head = headTextures["idle"];
			body = bodyTextures["idle"];
		}

		override public void Draw(SpriteBatch batch, GameTime time) {
			batch.Draw(body, new Vector2 (position.X - 64, position.Y - 105));
			batch.Draw(head, new Vector2 (position.X - 64, position.Y - 175));
		}

	}

}

