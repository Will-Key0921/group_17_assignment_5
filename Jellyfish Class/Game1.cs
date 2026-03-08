using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_17_assignment5;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Model _jellyfish;
    private Model _sphere;
    private float aspectRatio;
    private Matrix projection;
    private Matrix view;
    private Vector3 cameraPosition;
    private Vector3 cameraTarget;
    private float cameraSpeed = .1f;
    private Jellyfish Jellyfish;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        
        aspectRatio = GraphicsDevice.Viewport.AspectRatio;
        projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
            aspectRatio, 1.0f, 100.0f);
        view = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 45),
            Vector3.Zero, Vector3.Up);

        cameraPosition = new Vector3(0, 0, 45);
        cameraTarget = Vector3.Zero; 
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _jellyfish = Content.Load<Model>("meshes/uploads_files_5014703_Jellyfish_Quad");
        _sphere = Content.Load<Model>("meshes/sphere");
        
        Jellyfish = new Jellyfish(_jellyfish, _sphere, 
            new Vector3(-4, -4, 4), new Vector3(4, 4, -2), 
            4f);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        
        KeyboardState ks = Keyboard.GetState();

        if (ks.IsKeyDown(Keys.Left))
            cameraPosition.X -= cameraSpeed;
        if (ks.IsKeyDown(Keys.Right))
            cameraPosition.X += cameraSpeed;
        if (ks.IsKeyDown(Keys.Up))
            cameraPosition.Y += cameraSpeed;
        if (ks.IsKeyDown(Keys.Down))
            cameraPosition.Y -= cameraSpeed;
        
        view = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
        
        Jellyfish.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        Jellyfish.Draw(view, projection);

        base.Draw(gameTime);
    }
}
