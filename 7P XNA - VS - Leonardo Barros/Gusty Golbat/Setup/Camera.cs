using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gusty_Golbat.Setup
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
            position = new Vector3(3, 2, 5);
            // ponto que a câmera mira
            target = new Vector3(0, 1, 0);
            // qual vetor é o "para cima"
            up = Vector3.Up;
            SetupView(position, target, up);

            SetupProjection();
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

            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                  screen.GetWidth() / (float)screen.GetHeight(),
                                                                  0.1f,
                                                                  1000);
        }

        public Matrix GetView()
        {
            return view;
        }

        public Matrix GetProjection()
        {
            return projection;
        }

        public void Update(GameTime gameTime)
        {
            this.Rotation(gameTime);
            //this.Translation(gameTime);

            Movement(gameTime);

            view = Matrix.Identity;
            view *= Matrix.CreateRotationY(MathHelper.ToRadians(angleY));
            view *= Matrix.CreateRotationX(MathHelper.ToRadians(angleX));
            view *= Matrix.CreateTranslation(position);
            view = Matrix.Invert(view);
        }

        private void Movement(GameTime gameTime)
        {
            position.X -= (float)Math.Sin(MathHelper.ToRadians(angleY - 90)) * gameTime.ElapsedGameTime.Milliseconds * 0.0001f * speed;
            position.Z -= (float)Math.Cos(MathHelper.ToRadians(angleY - 90)) * gameTime.ElapsedGameTime.Milliseconds * 0.0001f * speed;
        }

        private void Rotation(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                angleY += speedY * gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                angleY -= speedY * gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            }
            /*
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                // Como quero que rotacione na mesma velocidade que o eixo Y, reaproveitei a variável speedY
                angleX -= speedY * gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                angleX += speedY * gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            }
            */
        }

        private void Translation(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                position.X -= (float)Math.Sin(MathHelper.ToRadians(angleY)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * speed;
                position.Z -= (float)Math.Cos(MathHelper.ToRadians(angleY)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                position.X += (float)Math.Sin(MathHelper.ToRadians(angleY)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * speed;
                position.Z += (float)Math.Cos(MathHelper.ToRadians(angleY)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                position.X += (float)Math.Sin(MathHelper.ToRadians(angleY + 90)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * speed;
                position.Z += (float)Math.Cos(MathHelper.ToRadians(angleY + 90)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                position.X += (float)Math.Sin(MathHelper.ToRadians(angleY - 90)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * speed;
                position.Z += (float)Math.Cos(MathHelper.ToRadians(angleY - 90)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                position.Y -= (float)Math.Sin(MathHelper.ToRadians(angleY - 90)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                position.Y += (float)Math.Sin(MathHelper.ToRadians(angleY - 90)) * gameTime.ElapsedGameTime.Milliseconds * 0.001f * speed;
            }
        }
    }
}