using System;
using System.Collections;
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
        WingDrawer[] wings;
        public Texture2D texture;
        public Texture2D wingTexture;

        private float damagedTime = 1f, damagedRotation = 0f;

        Vector3 wingPosition, wingRotation, wingScale;

        public State currentState;
        public enum State
        {
            Idle,
            Flying,
            Damaged
        }

        public Golbat(Game game, Vector3 position, Vector3 rot, Vector3 sca, float speed, Texture2D texture, Vector3 dimension, Color color, bool visible = false)
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

            // Body
            cubes = new CubeDrawer[]
            {
                new CubeDrawer(game, new Vector3(0f, 0f, 0f), Vector3.Zero, new Vector3(1f,1f,1f), world, texture)
            };

            // Wings
            wings = new WingDrawer[]
            {
                new WingDrawer(game.GraphicsDevice),
                new WingDrawer(game.GraphicsDevice),
            };

            wingScale = new Vector3(0.2f, 0.1f, 0.1f);
            wingRotation = new Vector3(90f, 0f, 0f);
            wingPosition = new Vector3(0f, 0f, 0f);

            // Left Wing
            wings[0].SetPlaneInitialTransform(wingPosition, wingRotation, wingScale);
            // Right Wing
            //wings[1].SetPlaneInitialTransform(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0.2f, 0.1f, 0.1f));

        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var wing in wings)
            {
                wing.Update(gameTime, this.world);
            }

            Translation(gameTime);

            if (currentState == State.Damaged)
            {
                if (damagedTime > 0f)
                {
                    damagedTime -= deltaTime;
                    damagedRotation += 10f * deltaTime;

                    cubes[0].world = Matrix.CreateRotationZ(damagedRotation) * Matrix.CreateTranslation(this.position);
                    foreach(var wing in wings)
                    {
                        wing.world = Matrix.CreateScale(this.wingScale) * Matrix.CreateRotationZ(damagedRotation) * Matrix.CreateTranslation(this.position);
                    }
                }
                else
                {
                    damagedTime = 1f;
                    damagedRotation = 0f;
                    SwitchState(State.Idle);
                }
            }

            // WINGS DEBUG
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                wingPosition.X += 1 * deltaTime;
            }
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

            foreach (var plane in wings)
            {
                plane.Draw(effect, wingTexture);
            }

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


            /*
            currentState = State.Idle;
            SwitchState(currentState);
            */
        }

        // FUNÇÕES DE AÇÃO
        private void Translation(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            oldPosition = position;

            if (currentState != State.Damaged)
            {
                currentState = State.Flying;

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