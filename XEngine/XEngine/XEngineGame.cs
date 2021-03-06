using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XEngine {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class XEngineGame : Microsoft.Xna.Framework.Game {

        protected GraphicsDeviceManager m_graphics;

        public XEngineGame() {
            m_graphics = new GraphicsDeviceManager( this );
            Content.RootDirectory = "Content";
            m_graphics.PreferredBackBufferWidth = 1280;
            m_graphics.PreferredBackBufferHeight = 720;
            ServiceLocator.Initialize( this );
        }

        virtual protected void ConfigureGameComponents() { }

        public void BindGameComponent(IGameComponent gameComponent, Type serviceInterface) {
            this.BindGameComponent( gameComponent );
            this.Services.AddService( serviceInterface, gameComponent );
        }

        public void BindGameComponent( IGameComponent gameComponent ) {
            this.Components.Add( gameComponent );
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            ServiceLocator.Graphics = this.GraphicsDevice;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load all of your content.
        /// </summary>
        protected override void LoadContent() {

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload all content.
        /// </summary>
        protected override void UnloadContent() {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }

        static public void RunGame() {
            using (XEngineGame game = new XEngineGame()) {
                game.Run();
            }
        }

        public GameTime gameTime {
            get {return this.gameTime;}
        }
    }
}
