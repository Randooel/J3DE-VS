using Gusty_Golbat.Geometria;
using Gusty_Golbat.Setup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Net.Mime;

namespace Gusty_Golbat.Entidades
{
    public class Heart : Collider
    {
        Game game;
        Vector3 rotation, scale;
        float moveSpeed;
        PlaneDrawer _plane;
        public Texture2D texture;
        Matrix world;

        public Heart(Game game, Vector3 position, Vector3 dimension, Color color, bool visible = true)
            : base(game, position, dimension, color, visible)
        {
            this.game = game;
            this.position = position;
            this.rotation = new Vector3(174.4f, 0f, 0f);
            this.scale = new Vector3(0.1f, 0.1f, 0.1f);
            moveSpeed = new Random().Next(3, 15);

            world = Matrix.Identity;
            world *= Matrix.CreateScale(this.scale);
            world *= Matrix.CreateRotationX(MathHelper.ToRadians(this.rotation.X));
            world *= Matrix.CreateTranslation(this.position);

            Initialize();
        }

        public void Initialize()
        {
            _plane = new PlaneDrawer(game.GraphicsDevice);
            _plane.SetPlaneInitialTransform(this.position, this.rotation, this.scale);

            SetPosition(this.position);
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var maxScale = new Vector3(1f, 1f, 1f);

            position.X -= moveSpeed * deltaTime * 0.5f;

            SetPosition(this.position);

            world = Matrix.CreateScale(this.scale) * Matrix.CreateRotationX(this.rotation.X) * Matrix.CreateTranslation(this.position);

            _plane.SetWorld(world);

            // DEBUG DE ROTAÇÃO DO CORAÇÃO
            /*
            var rotationSpeed = 1f;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                this.rotation.X += rotationSpeed * deltaTime;
                game.Window.Title = this.rotation.X.ToString();
            }
            if(Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                this.rotation.X -= rotationSpeed * deltaTime;
                game.Window.Title = this.rotation.X.ToString();
            }
            */
        }

        public void Draw(Camera camera, BasicEffect effect)
        {
            _plane.Draw(effect, texture);
        }
    }
}