﻿using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gusty_Golbat
{
    public class Camera
    {
        private Matrix view;
        private Matrix projection;

        private Vector3 position;
        private Vector3 target;
        private Vector3 up;

        float speed = 10;

        float angleY = 0;
        float angleX = 0;
        float speedY = 100;

        public Camera()
        {
            // posição inicial
            this.position = new Vector3(3, 2, 5);
            // ponto que a câmera mira
            this.target = new Vector3(0, 1, 0);
            // qual vetor é o "para cima"
            this.up = Vector3.Up;
            this.SetupView(this.position, this.target, this.up);

            this.SetupProjection();
        }

        public void SetupView(Vector3 position, Vector3 target, Vector3 up)
        {
            //this.view = Matrix.CreateLookAt(position, target, up);
            this.position = position;
            this.target = target;
            this.up = up;
        }

        public void SetupProjection()
        {
            Screen screen = Screen.GetInstance();

            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                  screen.GetWidth() / (float)screen.GetHeight(),
                                                                  0.1f,
                                                                  1000);
        }

        public Matrix GetView()
        {
            return this.view;
        }

        public Matrix GetProjection()
        {
            return this.projection;
        }

        public void Update(GameTime gameTime)
        {
            //this.Rotation(gameTime);
            //this.Translation(gameTime);

            Movement(gameTime);

            this.view = Matrix.Identity;
            this.view *= Matrix.CreateRotationY(MathHelper.ToRadians(this.angleY));
            this.view *= Matrix.CreateRotationX(MathHelper.ToRadians(this.angleX));
            this.view *= Matrix.CreateTranslation(this.position);
            this.view = Matrix.Invert(this.view);
        }

        private void Movement(GameTime gameTime)
        {
            this.position.X -= (float)Math.Sin(MathHelper.ToRadians(this.angleY - 90)) * gameTime.ElapsedGameTime.Milliseconds * 0.0001f * this.speed;
            this.position.Z -= (float)Math.Cos(MathHelper.ToRadians(this.angleY - 90)) * gameTime.ElapsedGameTime.Milliseconds * 0.0001f * this.speed;
        }

        private void Rotation(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                this.angleY += this.speedY * gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                this.angleY -= this.speedY * gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                // Como quero que rotacione na mesma velocidade que o eixo Y, reaproveitei a variável speedY
                this.angleX -= this.speedY * gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                this.angleX += this.speedY * gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            }
        }

        private void Translation(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                this.position.X -= (float)Math.Sin(MathHelper.ToRadians(this.angleY)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * this.speed;
                this.position.Z -= (float)Math.Cos(MathHelper.ToRadians(this.angleY)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * this.speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                this.position.X += (float)Math.Sin(MathHelper.ToRadians(this.angleY)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * this.speed;
                this.position.Z += (float)Math.Cos(MathHelper.ToRadians(this.angleY)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * this.speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                this.position.X += (float)Math.Sin(MathHelper.ToRadians(this.angleY + 90)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * this.speed;
                this.position.Z += (float)Math.Cos(MathHelper.ToRadians(this.angleY + 90)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * this.speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                this.position.X += (float)Math.Sin(MathHelper.ToRadians(this.angleY - 90)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * this.speed;
                this.position.Z += (float)Math.Cos(MathHelper.ToRadians(this.angleY - 90)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * this.speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                this.position.Y -= (float)Math.Sin(MathHelper.ToRadians(this.angleY - 90)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * this.speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                this.position.Y += (float)Math.Sin(MathHelper.ToRadians(this.angleY - 90)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * this.speed;
            }
        }
    }
}