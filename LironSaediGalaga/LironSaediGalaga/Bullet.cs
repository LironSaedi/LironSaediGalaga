using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LironSaediGalaga
{
    class Bullet : Sprite

    {
        protected float speedY;
        public float multiplier { get; protected set; }

        public Bullet(Texture2D texture, Vector2 position, Color tint, float speedY, float multiplier = 1f) 
            : base(texture, position, tint)
        {
            this.speedY = speedY;
            this.multiplier = multiplier;
        }

        public new void Update()
        {
            position += new Vector2 (0, speedY);
        }
    }
}
