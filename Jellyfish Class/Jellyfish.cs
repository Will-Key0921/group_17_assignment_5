using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_17_assignment5;

public class Jellyfish
{
    //Position Variables
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _currentPosition;

    //Model Variables
    private Model _jellyfishModel;
    private Model _sphereModel;
    
    //Animation Numbers
    private float _rotation;
    private float _time;
    private float _orbitAngle;
    private float _orbitRadius;
    private float _animationLength = 10f;
    
    //Rotation Matrix
    private Matrix _worldRotation;
    
    //Constructor
    public Jellyfish(Model tentacles, Model sphere, Vector3 startPos, Vector3 endPos, float orbitRadius)
    {
        _jellyfishModel = tentacles;
        _sphereModel = sphere;
        _startPosition = startPos;
        _endPosition = endPos;
        _orbitRadius = orbitRadius;
    }

    //Update adjusts current position with lerp, rotation, and orbit angle
    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _time += dt;
        if (_time > _animationLength)
        {
            _time = 0f;
        }
        
        float t = MathHelper.Clamp(_time / _animationLength, 0f, 1f);
        
        _currentPosition = Vector3.Lerp(_startPosition, _endPosition, t);
        
        
        _rotation += gameTime.ElapsedGameTime.Milliseconds * 0.005f;
        _worldRotation = Matrix.CreateRotationY(_rotation);

        _orbitAngle += dt;
    }

    public void Draw(Matrix view, Matrix projection)
    {
        //Applying Transformations to the parent
        Matrix rootWorld = _worldRotation * 
                           Matrix.CreateTranslation(_currentPosition) * 
                           Matrix.CreateScale(10);

        DrawMesh(_jellyfishModel, rootWorld, view, projection);

        float x = (float)Math.Cos(_orbitAngle) * _orbitRadius;
        float z = (float)Math.Sin(_orbitAngle) * _orbitRadius;
        
        //Applying transformations to the child
        Matrix childWorld = Matrix.CreateTranslation(new Vector3(x, 1.5f, z)) *
                            Matrix.CreateScale(.2f) * 
                            rootWorld;
        
        DrawMesh(_sphereModel, childWorld, view, projection);
    }
    
    private void DrawMesh(Model m, Matrix world, Matrix view, Matrix projection)
    {
        Matrix[] transforms = new Matrix[m.Bones.Count];
        m.CopyAbsoluteBoneTransformsTo(transforms);
        
        foreach (ModelMesh mesh in m.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.EnableDefaultLighting();
                effect.View = view;
                effect.Projection = projection;
                effect.World = world * transforms[mesh.ParentBone.Index];
            }

            mesh.Draw();
        }
    }
}