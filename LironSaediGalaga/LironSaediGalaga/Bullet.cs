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
        float speedY;

        public Bullet(Texture2D texture, Vector2 position, Color tint, float speedY) 
            : base(texture, position, tint)
        {
            this.speedY = speedY;
        }

        public new void Update()
        {
            position += new Vector2 (0, speedY);
        }
    }
}
