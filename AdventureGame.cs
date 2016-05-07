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
		Texture2D shadowTexture, houseTexture, collisionTexture;
        public Byte[,] collisionMap;
		Camera gameCamera;
		Character player;
		public static Character Player {
			get { return instance.player; }
		}

		SpriteFont defaultFont;
		SoundFont defaultSoundFont;
		public static SpriteFont DefaultFont {
			get { return AdventureGame.instance.defaultFont; }
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

        int currentSelectionType = 0;
        int currentHeadSprite = 0;
        public List<String> headSprites;
        int currentBodySprite = 0;
        public List<String> bodySprites;

        public enum GameState
        {
            StartGame,
            Game,
            EndGame
        }
        GameState currentState;

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

            headSprites = new List<string>();
            headSprites.Add("bunny");
            headSprites.Add("kitty");
            headSprites.Add("bird");
            headSprites.Add("mouse");

            bodySprites = new List<string>();
            bodySprites.Add("male");
            bodySprites.Add("female_hipster");

            //Initialize player class
            player = new Character(new Vector2(500, 500),
                headSprites[currentHeadSprite], new Color[] { new Color(255, 255, 255), new Color(255, 200, 200) },
                bodySprites[currentBodySprite], new Color[] { new Color(255, 255, 255), new Color(255, 255, 200) },
                new PlayerBehavior()
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
			Inventory.Add (ItemID.BEER);
			Inventory.Add (ItemID.POISON);
			Inventory.Add (ItemID.KNIFE);
			Inventory.Add (ItemID.KNIFE_USED);

			catSounds = new SoundFont("soundfonts/SWAR1685_TalkingEngM", 94);
			catSounds.LoadContent (Content);
			defaultSoundFont = new SoundFont ("soundfonts/machine", 12);
			defaultSoundFont.LoadContent (Content);

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
            }
            else if (currentState == GameState.StartGame)
            {
                bool selectionChanged = false;
                if (Input.KeyPressed(Key.ENTER))
                {
                    currentState = GameState.Game;
                    initGame();
                }
                else if (Input.KeyPressed(Key.LEFT))
                {
                    if (currentSelectionType == 0)
                        currentHeadSprite++;
                    else
                        currentBodySprite++;
                    selectionChanged = true;
                }
                else if (Input.KeyPressed(Key.RIGHT))
                {
                    if (currentSelectionType == 0)
                        currentHeadSprite--;
                    else
                        currentBodySprite--;
                    selectionChanged = true;
                }
                else if (Input.KeyPressed(Key.UP))
                {
                    currentSelectionType--;
                    currentSelectionType = Math.Max(0, currentSelectionType);
                }
                else if (Input.KeyPressed(Key.DOWN))
                {
                    currentSelectionType++;
                    currentSelectionType = Math.Min(currentSelectionType, 1);
                }

                if (selectionChanged)
                {
                    if (currentHeadSprite >= headSprites.Count)
                        currentHeadSprite = 0;
                    if (currentHeadSprite < 0)
                        currentHeadSprite = headSprites.Count - 1;

                    if (currentBodySprite >= bodySprites.Count)
                        currentBodySprite = 0;
                    if (currentBodySprite < 0)
                        currentBodySprite = bodySprites.Count - 1;

                    player.SetCharacterSprites(headSprites[currentHeadSprite], bodySprites[currentBodySprite]);
                    player.Load(Content, entityBatch);
                }
            }
            else if (currentState == GameState.EndGame)
            {

            }

			// update base
			base.Update (gameTime);
		}

        public void initGame()
        {
            // Move player to start position
            player.position = new Vector2(1500, 1500);
            //player.position = new Vector2(825, 963);

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
            }

            else if (currentState == GameState.StartGame)
            {
                graphics.GraphicsDevice.Clear(Color.Black);

                
                entityBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, null);
                Player.Draw(entityBatch, gameTime);
                entityBatch.End();
            }
			base.Draw (gameTime);
		}

		SoundFont catSounds;
		private void InitEntities() {
			entities.Add (player);

			entities.Add (new Character (new Vector2 (1240, 730),
				"bunny", new Color[] { new Color (180, 190, 170), new Color (255, 200, 200) },
				"male", new Color[] { new Color (180, 190, 170), new Color (200, 100, 255), new Color (255, 200, 255) },
				new CatNPC(catSounds)
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

			entities.Add (new StaticEntity (
				"environment/antenna",
				new Vector2 (1600, 1400),
				new Antenna (),
				new Vector2(-20, -175),
				new Vector2(0, -150)
			));

		}

		// schedules an entity to be spawned after this update loop
		public static void SpawnEntity(BaseEntity e) {
			AdventureGame.instance.toSpawn.Add (e);
		}
	}
}

