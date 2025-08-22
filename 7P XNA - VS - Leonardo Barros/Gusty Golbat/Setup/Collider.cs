using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gusty_Golbat.Setup
{
    public class Collider
    {
        public Vector3 position, dimension;
        BoundingBox bb;
        protected LineBox lineBox;
        bool visible;

        public Collider(Game game, Vector3 position, Vector3 dimension, Color color, bool visible = true)
        {
            this.position = position;
            this.dimension = dimension;

            UpdateBoundingBox();

            this.visible = visible;
            lineBox = new LineBox(game, position, dimension, color);
        }

        protected void UpdateBoundingBox()
        {
            bb = new BoundingBox(position - dimension / 2f,
                                      position + dimension / 2f);
        }

        public void Draw(BasicEffect e)
        {
            if (visible)
                lineBox.Draw(e);
        }

        public void SetPosition(Vector3 position)
        {
            this.position = position;
            lineBox.SetPosition(this.position);
            UpdateBoundingBox();
        }

        public BoundingBox GetBoundingBox()
        {
            return bb;
        }

        public bool IsColliding(BoundingBox box)
        {
            return bb.Intersects(box);
        }

        public bool GetVisible()
        {
            return visible;
        }

        public void SetVisible(bool value)
        {
            visible = value;
        }

        public LineBox GetLineBox()
        {
            return lineBox;
        }
    }
}
