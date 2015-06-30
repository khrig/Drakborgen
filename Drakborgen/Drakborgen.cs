using Drakborgen.States;
using Gengine;
using Microsoft.Xna.Framework;

namespace Drakborgen {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Drakborgen : Game {
        GraphicsDeviceManager _graphics;
        private HGengine _engine;

        public Drakborgen() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            _engine = new HGengine(this, _graphics);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent(){
            _engine.Initialize(640, 360, 1280, 720);
            _engine.AddState("mainmenu", new MainMenu());
            _engine.AddState("game", new GameState());
            _engine.StartWith("mainmenu");
            _engine.AddFont("text", "Fonts/04b_03");
            _engine.AddTexture("tiles32.png", "Sprites/tiles32");
            _engine.AddTexture("player", "Sprites/player32_full");
            _engine.AddTexture("dungeon", "Sprites/tilemap32");

            _engine.SetDebugDraw(false); // disabled is default
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent(){
            _engine.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime){
            _engine.Update((float) gameTime.ElapsedGameTime.TotalMilliseconds);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            _engine.Draw();
            base.Draw(gameTime);
        }
    }
}
