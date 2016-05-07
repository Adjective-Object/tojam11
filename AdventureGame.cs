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
		public static AdventureGame instance;
		public GraphicsDeviceManager graphics;
		SpriteBatch entityBatch;
		Texture2D shadowTexture, houseTexture, collisionTexture;
        public Byte[,] collisionMap;
		Camera gameCamera;
		Character player;
		public static Character Player {
			get { return instance.player; }
		}

		SpriteFont defaultFont;
		public static SpriteFont DefaultFont {
			get { return AdventureGame.instance.defaultFont; }
		}

        GameStateDictionary gameStateDictionary;
		public static List<BaseEntity> Entities {
			get { return instance.entities; }
		}
		List<BaseEntity> entities;
		List<BaseEntity> toSpawn;

		public static Rectangle ScreenBounds {
			get { return instance.graphics.GraphicsDevice.PresentationParameters.Bounds; }
		}

        //Byte[,] collisionMap;

		public AdventureGame ()
		{
			// make adventureGame a singleton
			if (instance == null) {
				instance = this;
			} else {
				Exit ();
			}

            //collisionMap = new Byte[2135, 4048];

			// intitialize the;

			graphics = new GraphicsDeviceManager (this);
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			Content.RootDirectory = "Content";

            gameStateDictionary = new GameStateDictionary();
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

			// initialize the static Input class
			Input.Initialize();

			// add all the entities on the map
			this.InitEntities ();

			// initialize the camera
			gameCamera = new Camera(player, new int [] {600, 1000, 1500});
			this.entities.Add (gameCamera);

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
			entityBatch = new SpriteBatch (GraphicsDevice);
			foreach (BaseEntity e in this.entities) {
				e.Load (Content, entityBatch);
			}

			Item.LoadContent(Content);
			Inventory.LoadContent (Content, entityBatch);
			Inventory.Add (ItemID.BEER);
			Inventory.Add (ItemID.POISON);
			Inventory.Add (ItemID.KNIFE);
			Inventory.Add (ItemID.KNIFE_USED);

			catSounds.LoadContent (Content);

			//Tell font loader to load Monaco as defaulft font
			defaultFont = Content.Load<SpriteFont>("Monaco");
			SpeechText.LoadFont("Monaco", defaultFont);

			// load the shadow texture
			shadowTexture = Content.Load<Texture2D>("shadow");
			houseTexture = Content.Load<Texture2D> ("house");


            collisionTexture = Content.Load<Texture2D>("collision");
            Color[] collisionColors = new Color[collisionTexture.Width * collisionTexture.Height];
            collisionTexture.GetData<Color>(collisionColors);

            collisionMap = new Byte[collisionTexture.Height, collisionTexture.Width];
            for (int y = 0; y < collisionTexture.Height; y++)
            {
				for (int x = 0; x < collisionTexture.Width; x++) {
					Color c = collisionColors [x + y * collisionTexture.Width];
					if (c.G != c.R) {
						Console.WriteLine (c);
					}
					if (c.R == 0 && c.G == 0 && c.B == 0) {
						collisionMap [y, x] = 1;
					} else if (c.R == 255 && c.G == 0 && c.B == 0) {
						collisionMap [y, x] = 3;
					} else if (c.R == 0 && c.G ==255 && c.B ==0) {
	                    collisionMap[y, x] = 2;
					} else {
                        collisionMap[y, x] = 0;
					}
                }
            }

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

			Input.Update ();

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
			entities.Sort((BaseEntity e, BaseEntity f) => e.isUI ? 1 : e.position.Y.CompareTo(f.position.Y));

			Inventory.Update(gameTime);

			// update base
			base.Update (gameTime);
		}


		Random r = new Random ();
		Color oldBkg = new Color (0, 0, 0);

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			float k = 0.05f;
			Color newBkg = new Color(
				(int)(r.NextDouble() * 100) + 50, 
				(int)(r.NextDouble() * 100) + 50, 
				(int)(r.NextDouble() * 100) + 50);
			newBkg = new Color(
				(int)(newBkg.R * k + oldBkg.R * (1-k)),
				(int)(newBkg.G * k + oldBkg.G * (1-k)),
				(int)(newBkg.G * k + oldBkg.B * (1-k)));
			oldBkg = newBkg;
			graphics.GraphicsDevice.Clear (newBkg);
            
			entityBatch.Begin (SpriteSortMode.Deferred, null, null, null, null, null, gameCamera.Transform);

			// draw hosue
			entityBatch.Draw(houseTexture, new Vector2(0,0));
			// entityBatch.Draw(collisionTexture, new Vector2(0,0), new Color(255,255,255, 200));

			// draw entity shadows
			foreach (BaseEntity e in this.entities) {
				if (e.hasShadow) {
					entityBatch.Draw (shadowTexture, new Vector2 (e.position.X - 64, e.position.Y - 6));
				}
			}

			// draw the entities
			foreach (BaseEntity e in this.entities) {
				e.Draw(entityBatch, gameTime);
			}
			entityBatch.End ();

			entityBatch.Begin ();
			Inventory.Draw (entityBatch);
			entityBatch.End ();

			base.Draw (gameTime);
		}

		SoundFont catSounds;
		private void InitEntities() {
			catSounds = new SoundFont ("soundfonts/SWAR1685_TalkingEngM");

			player = new Character (new Vector2 (1000, 1200),
				"bunny", new Color[] { new Color (255, 255, 255), new Color (255, 200, 200) },
				"female_hipster", new Color[] { new Color (255, 255, 255), new Color (255, 255, 200) },
				new PlayerBehavior()
			);
			entities.Add (player);

			entities.Add (new Character (new Vector2 (1240, 730),
				"bunny", new Color[] { new Color (180, 190, 170), new Color (255, 200, 200) },
				"male", new Color[] { new Color (180, 190, 170), new Color (200, 100, 255), new Color (255, 200, 255) },
				new NPCCat(catSounds)
			));

			entities.Add (new Character (new Vector2 (1500, 740),
				"kitty", new Color[] { new Color (180, 190, 170), new Color (255, 200, 200), new Color (20, 250, 30) },
				"female_hipster", new Color[] { new Color (180, 190, 170), new Color (100, 60, 190), new Color (255, 200, 255) },
				new CharacterBehavior(catSounds)
			));

			entities.Add (new Character (new Vector2 (1450, 800),
				"kitty", new Color[] { new Color (180, 190, 170), new Color (255, 200, 200), new Color (250, 250, 100) },
				"male", new Color[] { new Color (180, 190, 170), new Color (120, 120, 30), new Color (255, 200, 255) },
				new CharacterBehavior(catSounds)
			));
		}

		// schedules an entity to be spawned after this update loop
		public static void SpawnEntity(BaseEntity e) {
			AdventureGame.instance.toSpawn.Add (e);
		}
	}
}

