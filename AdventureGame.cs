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
		public SpriteBatch entityBatch;
		Texture2D shadowTexture, houseTexture, collisionTexture;
        public Byte[,] collisionMap;
		Camera gameCamera;
		Character player;
		public static Character Player {
			get { return instance.player; }
		}

		SpriteFont defaultFont;
        SpriteFont titleFont;

		SoundFont defaultSoundFont;

		public static SpriteFont DefaultFont {
			get { return AdventureGame.instance.defaultFont; }
		}
		public static SpriteFont TitleFont {
			get { return AdventureGame.instance.titleFont; }
		}
		public static SoundFont DefaultSoundFont {
			get { return AdventureGame.instance.defaultSoundFont; }
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


        public enum GameState
        {
            StartGame,
            Game,
            EndGame
        }
        GameState currentState;
		Room CharSelect = new CharacterSelect();

        bool endGame = false;

		public AdventureGame ()
		{
			// make adventureGame a singleton
			if (instance == null) {
				instance = this;
			} else {
				Exit ();
			}

			graphics = new GraphicsDeviceManager (this);
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			Content.RootDirectory = "Content";

            gameStateDictionary = new GameStateDictionary();
		}

		public List<String> headSprites;
		public List<String> bodySprites;

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
            currentState = GameState.StartGame;

			// initialize my fields
			entities = new List<BaseEntity> ();
			toSpawn = new List<BaseEntity> ();

			// initialize the static Input class
			Input.Initialize();

			CharSelect.Initialize ();

			headSprites = new List<string>();
			headSprites.Add("bunny");
			headSprites.Add("kitty");
			headSprites.Add("bird");
			headSprites.Add("mouse");
			headSprites.Add("raccoon");
			headSprites.Add("fish");
			headSprites.Add("beaver");
            headSprites.Add("goat");
            headSprites.Add("frog");
            headSprites.Add("fox");
            headSprites.Add("dog");
            headSprites.Add("rooster");
            headSprites.Add("chicken");

			bodySprites = new List<string>();
			bodySprites.Add("male");
			bodySprites.Add("female_hipster");
            bodySprites.Add("jacket");
            bodySprites.Add("jock");
            bodySprites.Add("male_hipster");

            //Initialize player class
            player = new Character(new Vector2(1280/2, 500),
                headSprites[0], new Color[] { new Color(255, 255, 255), new Color(255, 200, 200) },
                bodySprites[0], new Color[] { new Color(255, 255, 255), new Color(255, 255, 200) },
				new PlayerBehavior(catSounds)
            );
            player.Load(Content, entityBatch);
			
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

			Item.LoadContent(Content);
			Inventory.LoadContent (Content, entityBatch);

			catSounds = new SoundFont("soundfonts/SWAR1685_TalkingEngM", 94);
			catSounds.LoadContent (Content);
			defaultSoundFont = new SoundFont ("soundfonts/machine", 9);
			defaultSoundFont.LoadContent (Content);

			//Tell font loader to load Monaco as defaulft font
			defaultFont = Content.Load<SpriteFont>("Monaco");
            titleFont = Content.Load<SpriteFont>("MonacoTitle");
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
			if (currentState == GameState.StartGame)
				CharSelect.Update (gameTime);
			
            if (currentState == GameState.Game)
            {
                // update all entities in the entity list
                for (int i = 0; i < entities.Count; i++)
                {
                    entities[i].Update(gameTime);

                    if (!entities[i].alive)
                    {
                        entities.RemoveAt(i);
                        i--;
                    }
                }

                // add newly spawned entities
                entities.AddRange(toSpawn);
                toSpawn.Clear();
                entities.Sort((BaseEntity e, BaseEntity f) => e.isUI ? 1 : e.position.Y.CompareTo(f.position.Y));

                Inventory.Update(gameTime);

                if (endGame)
                    GameOver();
            }
            
            if (currentState == GameState.EndGame)
            {
                if (Input.KeyPressed(Key.ENTER))
                {
                    currentState = GameState.StartGame;
                }
            }

			// update base
			base.Update (gameTime);
		}




        /**
         * Call to cause end of game.
         */
        public void SetEndGame()
        {
            endGame = true;
        }

		public void StartGame() {
			currentState = GameState.Game;
			initGame ();
		}

        public void GameOver()
        {
            endGame = false;
            currentState = GameState.EndGame;
            entities.Clear();
            player.position = new Vector2(1280/2, 500);
        }

        public void initGame()
        {
            // Move player to start position
            player.position = new Vector2(3000, 1000);
            // player.position = new Vector2(825, 963);

            // add all the entities on the map
            this.InitEntities();

            this.entities.Add(gameCamera);

            foreach (BaseEntity e in this.entities)
            {
                e.Load(Content, entityBatch);
            }
        }






		Random r = new Random ();
		Color oldBkg = new Color (0, 0, 0);

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
            if (currentState == GameState.Game)
            {
                float k = 0.05f;
                Color newBkg = new Color(
                    (int)(r.NextDouble() * 100) + 50,
                    (int)(r.NextDouble() * 100) + 50,
                    (int)(r.NextDouble() * 100) + 50);
                newBkg = new Color(
                    (int)(newBkg.R * k + oldBkg.R * (1 - k)),
                    (int)(newBkg.G * k + oldBkg.G * (1 - k)),
                    (int)(newBkg.G * k + oldBkg.B * (1 - k)));
                oldBkg = newBkg;
                graphics.GraphicsDevice.Clear(newBkg);

                entityBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, gameCamera.Transform);

                // draw hosue
                entityBatch.Draw(houseTexture, new Vector2(0, 0));

                // draw entity shadows
                foreach (BaseEntity e in this.entities)
                {
                    if (e.hasShadow)
                    {
                        entityBatch.Draw(shadowTexture, new Vector2(e.position.X - 64, e.position.Y - 6));
                    }
                }

                // draw the entities
                foreach (BaseEntity e in this.entities)
                {
                    e.Draw(entityBatch, gameTime);
                }
                entityBatch.End();

                entityBatch.Begin();
                Inventory.Draw(entityBatch);
                entityBatch.End();

            }
            else if (currentState == GameState.EndGame)
            {
                graphics.GraphicsDevice.Clear(Color.Black);
                entityBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, null);
                entityBatch.DrawString(titleFont, "Well that party is over...", new Vector2(1280 / 2 - titleFont.MeasureString("Well that party is over...").X / 2, 100), Color.White);
                entityBatch.End();
            }
            else if (currentState == GameState.StartGame)
            {
				CharSelect.Draw (gameTime);
            }
			base.Draw (gameTime);
		}






		SoundFont catSounds;
		private void InitEntities() {
			entities.Add (player);

			entities.Add (new Character (new Vector2 (1240, 730),
				headSprites[r.Next(0, headSprites.Count)], new Color[] { new Color (180, 190, 170), new Color (255, 200, 200) },
                bodySprites[r.Next(0, bodySprites.Count)], new Color[] { new Color(180, 190, 170), new Color(200, 100, 255), new Color(255, 200, 255) },
				new CatNPC(catSounds)
			));

			entities.Add (new Character (new Vector2 (1500, 740),
                headSprites[r.Next(0, headSprites.Count)], new Color[] { new Color(180, 190, 170), new Color(255, 200, 200), new Color(20, 250, 30) },
                bodySprites[r.Next(0, bodySprites.Count)], new Color[] { new Color(180, 190, 170), new Color(100, 60, 190), new Color(255, 200, 255) },
				new BroNPC(catSounds)
			));

			entities.Add (new Character (new Vector2 (1450, 800),
                headSprites[r.Next(0, headSprites.Count)], new Color[] { new Color(180, 190, 170), new Color(255, 200, 200), new Color(250, 250, 100) },
                bodySprites[r.Next(0, bodySprites.Count)], new Color[] { new Color(180, 190, 170), new Color(120, 120, 30), new Color(255, 200, 255) },
				new CharacterBehavior(catSounds)
			));


			entities.Add (new StaticEntity (
				"environment/antenna",
				new Vector2 (1600, 1400),
				new Antenna (),
				new Vector2(0, -97),
				new Vector2(0, -117)
			));

			// TODO position these flavor items

			entities.Add (new StaticEntity (
				"environment/guitar",
				new Vector2 (1100, 1100),
				new GenericItem("it's a guitar.")
			));

			entities.Add (new StaticEntity (
				"environment/catnip",
				new Vector2 (1500, 1100),
				new GenericItem("Mmm, deliious catnip...", "Too bad you're not a cat.")
			));

			entities.Add (new StaticEntity (
				"environment/saw",
				new Vector2 (3258, 878),
				new GenericItem("I see a saw. What do you see?"),
				new Vector2(0, -30), new Vector2(0, -90)
			));

			entities.Add (new StaticEntity (
				"environment/hammer",
				new Vector2 (3196, 878),
				new GenericItem(ItemID.BEER, "I sure wish I could pick up this hammer."),
				new Vector2(0, -30), new Vector2(0, -90)
			));

			entities.Add (new StaticEntity (
				"environment/soap",
				new Vector2 (2000, 1100),
				new GenericItem("SlipperyCo(tm) scrubbing soap.", "The label says \"Don't Drop it\".")
			));

			entities.Add (new StaticEntity (
				"environment/microwave",
				new Vector2 (1600, 1100),
				new GenericItem("A microwave.", "It's called that because you need a magnifying glass to see the waves.")
			));

			entities.Add (new StaticEntity (
				"environment/bleach",
				new Vector2 (2100, 1100),
				new GenericItem ("Bleach", "\"Warning, do not drink\"")
			));

		}

		// schedules an entity to be spawned after this update loop
		public static void SpawnEntity(BaseEntity e) {
			AdventureGame.instance.toSpawn.Add (e);
		}
	}
}

