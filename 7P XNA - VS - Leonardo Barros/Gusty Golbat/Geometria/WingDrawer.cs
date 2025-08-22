using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gusty_Golbat.Geometria
{
    public class WingDrawer
    {
        private GraphicsDevice _graphicsDevice;
        private VertexBuffer _vertexBuffer;
        private VertexPositionTexture[] verts;

        // Transforms
        public Matrix world = Matrix.Identity;
        Vector3 position, rotation, scale;

        public void SetWorld(Matrix world)
        {
            this.world = world;
        }

        public void SetPlaneInitialTransform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            this.scale = scale;
            this.position = position;
            this.rotation = rotation;

            this.world = Matrix.Identity;
            this.world = Matrix.CreateScale(this.scale);
            this.world *= Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X));
            this.world *= Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y));
            this.world *= Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z));
            this.world *= Matrix.CreateTranslation(this.position);
        }

        public WingDrawer(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            CreateVertices();
            CreateBuffer();
        }

        private void CreateVertices()
        {
            verts = new VertexPositionTexture[6];
            // IPC: Caso algum triângulo esteja com a normal errada pra consertar é muito
            // simples - basta trocar a posição e cor do primeiro e último vértice e...... (<- qtd de pontos pra indicar o tamanho da tela divida)
            // VOILÀ

            //FRENTE DO CUBO

            verts[0] = new VertexPositionTexture(new Vector3(10, -1, -10), new Vector2(1, 1));
            verts[1] = new VertexPositionTexture(new Vector3(10, -1, 10), new Vector2(1, 0));
            verts[2] = new VertexPositionTexture(new Vector3(-10, -1, 10), new Vector2(0, 0));

            verts[3] = new VertexPositionTexture(new Vector3(-10, -1, -10), new Vector2(0, 1));
            verts[4] = new VertexPositionTexture(new Vector3(10, -1, -10), new Vector2(1, 1));
            verts[5] = new VertexPositionTexture(new Vector3(-10, -1, 10), new Vector2(0, 0));
        }

        private void CreateBuffer()
        {
            _vertexBuffer = new VertexBuffer(
                _graphicsDevice,
                typeof(VertexPositionTexture),
                verts.Length,
                BufferUsage.None
            );
            _vertexBuffer.SetData(verts);
        }

        public void Update(GameTime gameTime, Matrix _base)
        {
            this.rotation.Y += gameTime.ElapsedGameTime.Milliseconds * 0.001f;

            this.world = Matrix.Identity;
            this.world *= Matrix.CreateScale(this.scale);
            this.world *= Matrix.CreateRotationY(this.rotation.Y);
            this.world *= Matrix.CreateTranslation(this.position);
            this.world *= _base;
        }

        public void Draw(BasicEffect effect, Texture2D texture)
        {
            // DESENHO DOS VÉRTICES ARMAZENADOS NO BUFFER USANDO EFEITOS
            _graphicsDevice.SetVertexBuffer(_vertexBuffer);

            effect.TextureEnabled = true;
            effect.Texture = texture;
            effect.World = world;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawPrimitives(
                    PrimitiveType.TriangleList, 0, verts.Length / 3
                );
            }
        }
    }
}
