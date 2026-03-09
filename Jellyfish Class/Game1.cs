using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_17_assignment5;

public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //cam vars
        private Matrix _view;
        private Matrix _projection;
        private float _camAngle;
        private const float CamRadius = 80f;
        private const float CamHeight = 15f;
        private const float CamRotSpeed = 0.04f;
        //Jellyfish vars
        private Model _jellyfish;
        private Model _sphere;
        private Jellyfish Jellyfish;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            //window size
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //proj matrix
            _projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60f), GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //jellyfish
            _jellyfish = Content.Load<Model>("meshes/uploads_files_5014703_Jellyfish_Quad");
            _sphere = Content.Load<Model>("meshes/sphere");
            Jellyfish = new Jellyfish(_jellyfish, _sphere,
                new Vector3(-4, -4, 4), new Vector3(4, 2.8f, -2),
                4f);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //cameera
            _camAngle += CamRotSpeed * dt;
            float cx = (float)System.Math.Cos(_camAngle) * CamRadius;
            float cz = (float)System.Math.Sin(_camAngle) * CamRadius;
            _view = Matrix.CreateLookAt(
                new Vector3(cx, CamHeight, cz),
                Vector3.Zero,
                Vector3.Up);

            //jellyfish
            Jellyfish.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(0, 30, 75));
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.BlendState = BlendState.Opaque;
            //jellyfish
            Jellyfish.Draw(_view, _projection);

            base.Draw(gameTime);
        }
    }
