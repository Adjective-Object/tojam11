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


		// TYLER
		public static PhoneGUI phoneGUI;
		public static AtariSystem atari;
		public static Texture2D tylerSquare, tylerSheet;
		public static SpriteFont tylerFont, tylerFont2, tylerFont3;

		public static Random RandomNumber = new Random();
		internal static float RandomFloat(int a, int b, float divisor)
		{
			float value = (float)RandomNumber.Next(a, b) / divisor;
			return value;
		}




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

		SpriteFont defaultFont, titleFont, tinyFont;

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


        TimeSpan startTime;
        TimeSpan gameEndTime;
        List<String> endGameMessages;
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
            headSprites.Add("penguin");
            headSprites.Add("rooster");
            headSprites.Add("chicken");

			bodySprites = new List<string>();
			bodySprites.Add("male");
			bodySprites.Add("female_hipster");
            bodySprites.Add("jacket");
            bodySprites.Add("penguin");
            bodySprites.Add("jock");
            bodySprites.Add("male_hipster");

            endGameMessages = new List<string>();

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
			tinyFont = Content.Load<SpriteFont>("MonacoTiny");
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


			// Init Tyler
			phoneGUI = new PhoneGUI ();
			atari = new AtariSystem ();
			tylerSquare = Content.Load<Texture2D> ("square");
			tylerSheet = Content.Load<Texture2D> ("tylerSheet");
            tylerFont = Content.Load<SpriteFont> ("Vectorb");
            tylerFont2 = Content.Load<SpriteFont> ("arial");
            tylerFont3 = Content.Load<SpriteFont> ("visitor");
		}


		Vector2 previousMousePos = new Vector2(0,0);



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
			Input.disabled = atari.Running || phoneGUI.Running;
			TInput.Update ();

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
				entities.Sort((BaseEntity e, BaseEntity f) => {
					if (e.SortingLayer == f.SortingLayer) {
						return e.SortingY.CompareTo(f.SortingY);
					} else {
						return e.SortingLayer.CompareTo(f.SortingLayer);
					}
				});

                Inventory.Update(gameTime);

                if (endGame)
                    GameOver(gameTime);

				phoneGUI.Update(gameTime);
				atari.Update(gameTime);

            }
            else if (currentState == GameState.EndGame)
            {
                if (Input.KeyPressed(Key.ENTER))
                {
                    currentState = GameState.StartGame;
                }
            }

			// move objects around w/ debug mode
			Point mousePos = Mouse.GetState ().Position;
			Vector2 mp = new Vector2(
				mousePos.X + gameCamera.position.X - ScreenBounds.Width/2,
				mousePos.Y + gameCamera.position.Y - ScreenBounds.Height / 2);

			if (Input.KeyDown (Key.DEBUG)) {
				this.IsMouseVisible = true;
				float dist = 10000000;
				BaseEntity closest = entities [0];
				foreach (BaseEntity e in this.entities) {
					if (e != gameCamera && (e.position - previousMousePos).Length () < dist) {
						dist = (e.position - previousMousePos).Length ();
						closest = e;
					}
				}

				if (Input.KeyDown (Key.SHIFT)) {
					closest.position += (mp - previousMousePos);
				}
			} else {
				this.IsMouseVisible = false;
			}
			previousMousePos = mp;

			// update base
			base.Update (gameTime);
		}


        public void AddEndGameMessage(String message)
        {
            endGameMessages.Add(message);
        }

        /**
         * Call to cause end of game.
         */
        public void SetEndGame()
        {
            endGame = true;
        }

		public void StartGame(GameTime gameTime) {
            startTime = gameTime.TotalGameTime;
			currentState = GameState.Game;
			initGame ();
		}

        public void GameOver(GameTime gameTime)
        {
            gameEndTime = gameTime.TotalGameTime;
            endGame = false;
            currentState = GameState.EndGame;
            entities.Clear();
            player.position = new Vector2(1280/2, 500);
        }

        public void initGame()
        {
            // Move player to start position
            // player.position = new Vector2(3000, 1000);
            player.position = new Vector2(825, 963);

            // add all the entities on the map
            this.InitEntities();

            this.entities.Add(gameCamera);

            foreach (BaseEntity e in this.entities)
            {
                e.Load(Content, entityBatch);
            }

            endGameMessages.Clear();
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
					if (Input.KeyDown(Key.DEBUG) && e != gameCamera) {
						entityBatch.DrawString (tinyFont, e.position.ToString (), e.position, Color.AliceBlue);
					}
                }
                entityBatch.End();

                entityBatch.Begin();
                Inventory.Draw(entityBatch);
                entityBatch.End();


				entityBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
					DepthStencilState.None, RasterizerState.CullCounterClockwise);

				phoneGUI.Draw (entityBatch);
				atari.Draw (entityBatch);

				entityBatch.End ();

            }
            else if (currentState == GameState.EndGame)
            {
                graphics.GraphicsDevice.Clear(Color.Black);
                entityBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, null);
                entityBatch.DrawString(titleFont, "Well that party is over...", new Vector2(1280 / 2 - titleFont.MeasureString("Well that party is over...").X / 2, 100), Color.White);

                String timeSpent = "You spent " + (int)(gameEndTime.TotalSeconds - startTime.TotalSeconds) + " seconds at the party";
                entityBatch.DrawString(defaultFont, timeSpent, new Vector2(1280 / 2 - defaultFont.MeasureString(timeSpent).X / 2, 190), Color.White);
                entityBatch.DrawString(defaultFont, "What You Accomplished:", new Vector2(1280 / 2 - defaultFont.MeasureString("What You Accomplished:").X / 2, 210), Color.White);

                if (endGameMessages.Count == 0)
                {
                    entityBatch.DrawString(defaultFont, "You did nothing... BETTER THAN TALKING TO NORMIES, REEEE", new Vector2(1280 / 2 - defaultFont.MeasureString("You did nothing... BETTER THAN TALKING TO NORMIES, REEEE").X / 2, 250), Color.White);
                }
                else
                {
                    int offsetY = 0;
                    foreach (String message in endGameMessages)
                    {
                        entityBatch.DrawString(defaultFont, message, new Vector2(1280 / 2 - defaultFont.MeasureString(message).X / 2, 250 + offsetY), Color.White);
                        offsetY += 23;
                    }
                }

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

			// people
			Color[] CatPalette = Character.MakeRandomPallete (3);
			entities.Add (new Character (new Vector2 (1555, 1005),
				"kitty", CatPalette,
				bodySprites[r.Next(0, bodySprites.Count)], CatPalette,
				new CatNPC(catSounds)
			));

			Color[] BroPalette = Character.MakeRandomPallete (3);
			entities.Add (new Character (new Vector2 (1751, 1149),
				headSprites[r.Next(0, headSprites.Count)], BroPalette,
				"jock", BroPalette,
				new BroNPC(catSounds)
			));

			Color[] HipsterPalette = Character.MakeRandomPallete (3);
			entities.Add (new Character (new Vector2 (1852, 975),
				headSprites[r.Next(0, headSprites.Count)], HipsterPalette,
				"male_hipster", HipsterPalette,
				new HipsterNPC(catSounds)
			));

			Color[] LinuxPalette = Character.MakeRandomPallete (3);
			entities.Add (new Character (new Vector2 (1765, 1682),
				"penguin", LinuxPalette,
				"penguin", LinuxPalette,
				new CatNPC(catSounds)
			));
			
			Color[] FoxPalette= Character.MakeRandomPallete (3);
			entities.Add (new Character (new Vector2 (2677, 921),
				"fox", FoxPalette,
				"jacket", FoxPalette,
				new HighGuy(catSounds)
			));


			Color[] ChickenPalette = new Color[]{ new Color(230, 230, 230), new Color(230, 0, 0), new Color(30, 30, 30)};
			entities.Add (new Character (new Vector2 (3042, 953),
				"chicken", ChickenPalette,
				bodySprites[r.Next(0, bodySprites.Count)], ChickenPalette,
				new ChickenGangMember(catSounds)
			));

			entities.Add (new Character (new Vector2 (3082, 1071),
				"chicken", ChickenPalette,
				bodySprites[r.Next(0, bodySprites.Count)], ChickenPalette,
				new ChickenGangMember(catSounds)
			));

			entities.Add (new Character (new Vector2 (2940, 1134),
				"chicken", ChickenPalette,
				bodySprites[r.Next(0, bodySprites.Count)], ChickenPalette,
				new ChickenGangMember(catSounds)
			));

			entities.Add (new Character (new Vector2 (2893, 1005),
				"rooster", ChickenPalette,
				bodySprites[r.Next(0, bodySprites.Count)], ChickenPalette,
				new ChickenGangLeader(catSounds)
			));


            // Exit door
            entities.Add(new StaticEntity(
                null,
                new Vector2(825, 863),
                new Door(),
                new Vector2(0, -97),
                new Vector2(0, -117)
            ));


			// basement
			entities.Add (new StaticEntity (
				"environment/antenna",
				new Vector2 (1600, 1400),
				new Antenna (),
				new Vector2(0, -97),
				new Vector2(0, -117)
			));

			// pointless flavor items
			entities.Add (new StaticEntity (
				"environment/guitar",
				new Vector2 (1162, 1057),
				new GenericItem("it's a guitar.")
			));

			entities.Add (new StaticEntity (
				"environment/catnip",
				new Vector2 (2571, 939),
				new GenericItem("Mmm, deliious catnip...", "Too bad you're not a cat.")
			));
			
			entities.Add (new StaticEntity (
				"environment/soap",
				new Vector2 (1143, 465),
				new GenericItem("SlipperyCo(tm) scrubbing soap.", "The label says \"Don't Drop it\".")
			));

			entities.Add (new StaticEntity (
				"environment/microwave",
				new Vector2 (2031, 922),
				new GenericItem("A microwave.", "It's called that because you need a magnifying glass to see the waves."),
				null,
				new Vector2(0, -70)
			));

			entities.Add (new StaticEntity (
				"environment/bleach",
				new Vector2 (2374, 956),
				new GenericItem ("Bleach", "\"Warning, do not drink\""),
				new Vector2(0, -50),
				new Vector2(0, -127)
			));



			//toolshed
			entities.Add(new DissapearingWall("environment/shed", new Vector2(3128,997), player));

			entities.Add (new StaticEntity (
				"environment/saw",
				new Vector2 (3277, 879),
				new GenericItem("I see a saw. What do you see?"),
				new Vector2(0, -30), new Vector2(0, -90)
			));

			entities.Add (new StaticEntity (
				"environment/screwdriver",
				new Vector2 (3226, 878),
				new GenericItem(ItemID.SCREWDRIVER, "It's a screwdriver"),
				new Vector2(0, -30), new Vector2(0, -90)
			));

			entities.Add (new StaticEntity (
				"environment/hammer",
				new Vector2 (3178, 877),
				new GenericItem(ItemID.HAMMER, "I sure wish I could pick up this hammer."),
				new Vector2(0, -30), new Vector2(0, -90)
			));





			// table and things on the table
			entities.Add (new DumbSprite (
				"environment/kitchen_table",
				new Vector2 (2316, 1137)));

			StaticEntity chips, dip, punch;
			chips = new StaticEntity (
				"environment/chip_bowl",
				new Vector2 (2251, 1080),
				new ChipBowl(),
				null, null,
				new Dictionary<String, String>() {
					{"empty", "environment/empty_bowl"}
				}
			);
			dip = new StaticEntity (
				"environment/salsa",
				new Vector2 (2388, 1091),
				new GenericItem ("Ooh, some salsa")
			);
			punch = new StaticEntity (
				"environment/punch_bowl",
				new Vector2 (2310, 1114),
				new GenericItem ("A bowl of punch.", "It smells like rum?"),
				null, new Vector2(0, -100)
			);

			chips.SortingY = 1138;
			punch.SortingY = 1138;
			dip.SortingY = 1138;
			entities.Add (chips);
			entities.Add (dip);
			entities.Add (punch);
			

		}

		// schedules an entity to be spawned after this update loop
		public static void SpawnEntity(BaseEntity e) {
			AdventureGame.instance.toSpawn.Add (e);
		}
	}
}

