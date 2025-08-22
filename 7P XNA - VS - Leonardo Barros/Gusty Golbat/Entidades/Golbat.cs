using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gusty_Golbat.Geometria;
using Gusty_Golbat.Setup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gusty_Golbat.Entidades
{
    public class Golbat : Collider
    {
        Game game;
        Matrix world;
        Vector3 oldPosition;
        Vector3 rotation, scale;
        float moveSpeed;

        // Substituir para uma geometria do Golbat, se der tempo
        CubeDrawer[] cubes;
        public Texture2D texture;

        public State currentState;
        public enum State
        {
            Idle,
            Flying,
            Damaged
        }

        public Golbat(Game game, Vector3 position, Vector3 rot, Vector3 sca, float speed, Texture2D texture, Vector3 dimension, Color color, bool visible = true)
            : base(game, position, dimension, color, visible)
        {
            this.game = game;

            scale = sca;
            rotation = rot;
            this.position = position;

            moveSpeed = speed;

            this.texture = texture;

            world = Matrix.Identity;

            world = Matrix.CreateScale(scale)
                * Matrix.CreateRotationX(rotation.X)
                * Matrix.CreateRotationY(rotation.Y)
                * Matrix.CreateRotationZ(rotation.Z)
                * Matrix.CreateTranslation(this.position);

            SetVisible(true);

            Initialize();
        }

        public void Initialize()
        {
            currentState = State.Idle;

            cubes = new CubeDrawer[]
            {
                new CubeDrawer(game, new Vector3(0f, 0f, 0f), Vector3.Zero, new Vector3(1f,1f,1f), world, texture)
            };
        }

        public void Update(GameTime gameTime)
        {
            

            Translation(gameTime);

        }

        public void Draw(Camera camera)
        {
            foreach (var cube in cubes)
            {
                cube.Draw(camera, texture);
            }

            BasicEffect effect = new BasicEffect(game.GraphicsDevice)
            {
                View = camera.GetView(),
                Projection = camera.GetProjection(),
                VertexColorEnabled = true
            };

            Draw(effect);
        }

        // FUNÇÕES DE ESTADOS

        public void SwitchState(State nextState)
        {
            currentState = nextState;

            switch (currentState)
            {
                case State.Idle:
                    HandleIdle();
                    break;
                case State.Flying:
                    HandleFly();
                    break;
                case State.Damaged:
                    HandleDamage();
                    break;
            }
        }
        private void HandleIdle()
        {

        }

        private void HandleFly()
        {

        }

        private void HandleDamage()
        {

        }

        // FUNÇÕES DE AÇÃO
        private void Translation(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            oldPosition = position;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                position.Y += moveSpeed * deltaTime;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                position.Y -= moveSpeed * deltaTime;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                position.X -= moveSpeed * deltaTime;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                position.X += moveSpeed * deltaTime;
            }

            world = Matrix.CreateScale(scale)
            * Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X))
            * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
            * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z))
            * Matrix.CreateTranslation(position);

            foreach (var cube in cubes)
            {
                cube.UpdateMatrix(world);
            }

            SetPosition(position);
        }

        public void RestorePosition()
        {
            position = oldPosition;
        }
    }
}
