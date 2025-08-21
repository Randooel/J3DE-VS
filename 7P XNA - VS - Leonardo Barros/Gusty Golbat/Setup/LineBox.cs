using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gusty_Golbat.Setup
{
    public class LineBox
    {
        Matrix world;
        Vector3 position, scale;
        VertexPositionColor[] verts;
        VertexBuffer vBuffer;
        short[] indexes;
        IndexBuffer iBuffer;
        Color color;
        Game game;

        public LineBox(Game game, Vector3 position, Vector3 scale, Color color)
        {
            this.game = game;
            this.position = position;
            this.scale = scale;
            this.color = color;

            CreateMatrix();
            CreateVertex();
            CreateVBuffer();
            CreateIndexes();
            CreateIBuffer();
        }

        private void CreateMatrix()
        {
            world = Matrix.Identity;
            world *= Matrix.CreateScale(scale);
            world *= Matrix.CreateTranslation(position);
        }

        private void CreateVertex()
        {
            float v = 0.5f;

            verts = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(-v, v,-v), color),//0
                new VertexPositionColor(new Vector3( v, v,-v), color),//1
                new VertexPositionColor(new Vector3(-v, v, v), color),//2
                new VertexPositionColor(new Vector3( v, v, v), color),//3

                new VertexPositionColor(new Vector3(-v,-v,-v), color),//4
                new VertexPositionColor(new Vector3( v,-v,-v), color),//5
                new VertexPositionColor(new Vector3(-v,-v, v), color),//6
                new VertexPositionColor(new Vector3( v,-v, v), color),//7
            };
        }

        private void CreateVBuffer()
        {
            vBuffer = new VertexBuffer(game.GraphicsDevice,
                                            typeof(VertexPositionColor),
                                            verts.Length,
                                            BufferUsage.None);
            vBuffer.SetData(verts);
        }

        private void CreateIndexes()
        {
            indexes = new short[]
            {
                // cima
                0, 1,
                1, 3,
                3, 2,
                2, 0,

                // baixo
                4, 5,
                5, 7,
                7, 6,
                6, 4,

                // vertical
                0, 4,
                1, 5,
                2, 6,
                3, 7
            };
        }

        private void CreateIBuffer()
        {
            iBuffer = new IndexBuffer(game.GraphicsDevice,
                                           IndexElementSize.SixteenBits,
                                           indexes.Length,
                                           BufferUsage.None);
            iBuffer.SetData(indexes);
        }

        public void Draw(BasicEffect e)
        {
            e.World = world;
            e.VertexColorEnabled = true;

            game.GraphicsDevice.SetVertexBuffer(vBuffer);
            game.GraphicsDevice.Indices = iBuffer;

            foreach (EffectPass pass in e.CurrentTechnique.Passes)
            {
                pass.Apply();

                game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList,
                                          verts,
                                          0,
                                          verts.Length,
                                          indexes,
                                          0,
                                          indexes.Length / 2);
            }
            e.VertexColorEnabled = false;
        }

        public void SetPosition(Vector3 position)
        {
            this.position = position;
            CreateMatrix();
        }

        public void SetScale(Vector3 scale)
        {
            this.scale = scale;
            CreateMatrix();
        }

        public void SetColor(Color color)
        {
            this.color = color;

            for (int i = 0; i < verts.Length; i++)
            {
                verts[i].Color = this.color;
            }
        }
    }
}
