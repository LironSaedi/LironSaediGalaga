using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LironSaediGalaga
{
    class ArWing : Sprite
    {
        Vector2 speed;
        Texture2D bulletTexture;
        Texture2D bombTexture;
        List<Bullet> bullets;
        SoundEffect laserSound;
        float bulletSpeed = 5;
        KeyboardState lastKeyboardState;
        public bool stop = false;
        //get for bullets
        //public objectType Bullets
        public List<Bullet> Bullets
        {
            get
            {
                return bullets;
            }
        }

        public ArWing(Texture2D texture, Texture2D bombTexture, Texture2D bulletTexture, Vector2 position, Color tint, Vector2 speed, SoundEffect laserSound)
            : base(texture, position, tint)
        {
            this.speed = speed;
            this.bombTexture = bombTexture;
            this.bulletTexture = bulletTexture;
            this.laserSound = laserSound;
            bullets = new List<Bullet>();
            //Shoot();
        }

        public void Update(KeyboardState ks, Viewport viewport, Random random)
        {
            if (ks.IsKeyDown(Keys.Right))
            {
                if (viewport.Width > Position.X + Hitbox.Width)
                {
                    position.X += speed.X;
                }

            }
            if (ks.IsKeyDown(Keys.Left))
            {
                if (position.X > 0)
                {
                    position.X -= speed.X;
                }

            }
            if (ks.IsKeyDown(Keys.Space) && !lastKeyboardState.IsKeyDown(Keys.Space) && stop == false)
            {

                laserSound.Play();
                Shoot(random);

            }
            foreach (Bullet bullet in bullets)
            {
                bullet.Update();
            }
            lastKeyboardState = ks;
        }

        public new void Draw(SpriteBatch batch, bool debug)
        {

            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(batch, debug);
            }

            base.Draw(batch, debug);
        }

        public void Shoot(Random random)
        {
            int rnd = random.Next(0, 10);

            if (rnd == 0)
            {
                bullets.Add(new Bullet(bombTexture, new Vector2(position.X + (texture.Width - bombTexture.Width) / 2, position.Y), Color.White, -bulletSpeed, 4f));
            }
            else
            {
                bullets.Add(new Bullet(bulletTexture, new Vector2(position.X + (texture.Width - bulletTexture.Width) / 2, position.Y), Color.White, -bulletSpeed));
            }
        }
    }
}
