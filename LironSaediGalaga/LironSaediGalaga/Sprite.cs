using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LironSaediGalaga
{
    class Sprite
    {
        protected Texture2D texture;
        protected Vector2 position;
        protected Color tint;
        
        public Color Tint
        {
            get
            {
                return tint;
            }
            set
            {
                tint = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public virtual Rectangle Hitbox
        {
            get
            {
                Rectangle hitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
                return hitbox;
            }
        }





        public Sprite(Texture2D texture, Vector2 position,Color tint)
        {
            this.texture = texture;
            this.position = position;
            this.tint = tint;
        }

        public virtual void Draw(SpriteBatch batch, bool debug)
        {
            if (debug) { batch.Draw(Game1.hitboxSprite, Hitbox, Color.White); }
            batch.Draw(texture,position,tint);
        }
        public void Update()
        {

        }
    }
}
