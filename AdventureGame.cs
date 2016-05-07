using System;
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
		Texture2D shadowTexture, houseTexture;
        public Byte[,] collisionMap;
		Camera gameCamera;
		Character player;

        GameStateDictionary gameStateDictionary;
		public static List<BaseEntity> Entities {
			get { return instance.entities; }
		}
		List<BaseEntity> entities; 
		List<BaseEntity> toSpawn;

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
			gameCamera = new Camera(player, new int [] {100, 400, 800, 1200, 1600});
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

			// Tell font loader to load Monaco as defaulft font
			SpeechText.LoadFont(Content, "Monaco");

			// load the shadow texture
			shadowTexture = Content.Load<Texture2D>("shadow");
			houseTexture = Content.Load<Texture2D> ("house");


            Texture2D collisionTexture = Content.Load<Texture2D>("collision");
            Color[] collisionColors = new Color[collisionTexture.Width * collisionTexture.Height];
            collisionTexture.GetData<Color>(collisionColors);

            collisionMap = new Byte[collisionTexture.Height, collisionTexture.Width];
            for (int y = 0; y < collisionTexture.Height; y++)
            {
                for (int x = 0; x < collisionTexture.Width; x++)
                {
                    if (collisionColors[x + y * collisionTexture.Width].A != 255)
                        collisionMap[y, x] = 0;
                    else
                        collisionMap[y, x] = 1;
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

			// update base
			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
            
			entityBatch.Begin (SpriteSortMode.Deferred, null, null, null, null, null, gameCamera.Transform);

			// draw hosue
			entityBatch.Draw(houseTexture, new Vector2(0,0));

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
			base.Draw (gameTime);
		}





		private void InitEntities() {
			player = new Character (new Vector2 (1000, 700),
				"bunny", new Color[] { new Color (255, 255, 255), new Color (255, 200, 200) },
				"female_hipster", new Color[] { new Color (255, 255, 255), new Color (255, 255, 200) },
				new PlayerBehavior()
			);
			entities.Add (player);

			entities.Add (new Character (new Vector2 (1240, 730),
				"bunny", new Color[] { new Color (180, 190, 170), new Color (255, 200, 200) },
				"male", new Color[] { new Color (180, 190, 170), new Color (200, 100, 255), new Color (255, 200, 255) },
				new NPCCat()
			));

			entities.Add (new Character (new Vector2 (1500, 740),
				"kitty", new Color[] { new Color (180, 190, 170), new Color (255, 200, 200), new Color (20, 250, 30) },
				"female_hipster", new Color[] { new Color (180, 190, 170), new Color (100, 60, 190), new Color (255, 200, 255) },
				new CharacterBehavior()
			));

			entities.Add (new Character (new Vector2 (1450, 800),
				"kitty", new Color[] { new Color (180, 190, 170), new Color (255, 200, 200), new Color (250, 250, 100) },
				"male", new Color[] { new Color (180, 190, 170), new Color (120, 120, 30), new Color (255, 200, 255) },
				new CharacterBehavior()
			));
		}

		// schedules an entity to be spawned after this update loop
		public static void SpawnEntity(BaseEntity e) {
			AdventureGame.instance.toSpawn.Add (e);
		}
	}
}

