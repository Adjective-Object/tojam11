﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Adventure
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class AdventureGame : Game
	{
		static AdventureGame instance;
		GraphicsDeviceManager graphics;
		SpriteBatch enitityBatch;
		Camera gameCamera;
		List<BaseEntity> entities; 
		List<BaseEntity> toSpawn; 

		public AdventureGame ()
		{
			// make adventureGame a singleton
			if (instance == null) {
				instance = this;
			} else {
				Exit ();
			}

			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// initialize my fields
			entities = new List<BaseEntity> ();
			toSpawn = new List<BaseEntity> ();

			// initialize the camera
			// gameCamera = new Camera ();

			// add all the entities on the map
			this.InitEntities ();

			// init game
			base.Initialize ();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			enitityBatch = new SpriteBatch (GraphicsDevice);
			foreach (BaseEntity e in this.entities) {
				e.Load (Content, enitityBatch);
			}

			// Tell font loader that arial existsT
			SpeechText.LoadFont(Content, "Monaco");
			SpeechText.Spawn ("Monaco", new Vector2 (50, 50), "emitting speech text sounds");
			SpeechText.Spawn ("Monaco", new Vector2 (50, 100), "Press Enter to dismiss this text", SpeechText.SpeechMode.PLAYER_CONTROLLED);
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
			#if !__IOS__ &&  !__TVOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState ().IsKeyDown (Keys.Escape))
				Exit ();
			#endif

			// update all entities in the entity list
			for (int i=0; i<entities.Count; i++) {
				entities[i].Update(gameTime);

				if (!entities [i].alive) {
					Console.WriteLine("removing dead entity " + entities[i].ToString());
					entities.RemoveAt (i);
					i--;
				}
			}

			// add newly spawned entities
			entities.AddRange(toSpawn);
			toSpawn.Clear ();

			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
            
			// Camera.apply ();

			// draw the entities
			enitityBatch.Begin ();
			foreach (BaseEntity e in this.entities) {
				e.Draw(enitityBatch, gameTime);
			}
			enitityBatch.End ();

            
			base.Draw (gameTime);
		}





		private void InitEntities() {
			entities.Add (new Character (new Vector2 (200, 300),
				"bunny", new Color[] { new Color (255, 255, 255), new Color (255, 200, 200) },
				"male", new Color[] { new Color (255, 255, 255), new Color (255, 255, 200) }
			));

			entities.Add (new Character (new Vector2 (350, 300),
				"bunny", new Color[] { new Color (180, 190, 170), new Color (255, 200, 200) },
				"female_hipster", new Color[] { new Color (180, 190, 170), new Color (200, 100, 255), new Color (255, 200, 255) }
			));

			entities.Add (new Character (new Vector2 (500, 300),
				"kitty", new Color[] { new Color (180, 190, 170), new Color (255, 200, 200), new Color (20, 250, 30) },
				"male", new Color[] { new Color (180, 190, 170), new Color (100, 60, 190), new Color (255, 200, 255) }
			));

			entities.Add (new Character (new Vector2 (650, 300),
				"kitty", new Color[] { new Color (180, 190, 170), new Color (255, 200, 200), new Color (250, 250, 100) },
				"female_hipster", new Color[] { new Color (180, 190, 170), new Color (120, 120, 30), new Color (255, 200, 255) }
			));
		}

		// schedules an entity to be spawned after this update loop
		public static void SpawnEntitiy(BaseEntity e) {
			AdventureGame.instance.toSpawn.Add (e);
		}
	}
}

