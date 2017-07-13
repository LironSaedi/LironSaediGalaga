using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LironSaediGalaga
{
    class Enemies : Sprite
    {
        float SpeedX;
        float SpeedY;
        int score;
        List<Bullet> bullets;
        Texture2D enemyBulletTexture;
        float scale;
        float scaleRate;
        Vector2 origin;
        float bulletSpeed;
        float timer = 0;
        int minShotDelay = 1;
        int maxShotDelay = 3;

        Random random;
        float shotDelay;
        public List<Bullet> Bullets
        {
            get
            {
                return bullets;
            }
        }

        public int Score
        {
            get
            {
                return score;
            }
        }
        public float Scale
        {
            get
            {
                return scale;
            }
        }

        public Vector2 Origin
        {
            get
            {
                return origin;

            }
        }

        public override Rectangle Hitbox
        {
            get
            {
                Rectangle hitbox = new Rectangle((int)(position.X - (Origin.X * scale)),
                    (int)(position.Y - (Origin.Y * scale)),
                    (int)(texture.Width * scale), (int)(texture.Height * scale));
                return hitbox;
            }
        }

        public Enemies(Texture2D texture, Texture2D enemyBulletTexture, Vector2 position, int bulletSpeed, Color tint, float SpeedX, float SpeedY, int score, float scaleRate, Random random)
            : base(texture, position, tint)

        {
            bullets = new List<Bullet>();
            this.score = score;
            this.SpeedX = SpeedX;
            this.SpeedY = SpeedY;
            this.scaleRate = -scaleRate;
            this.enemyBulletTexture = enemyBulletTexture;
            this.bulletSpeed = bulletSpeed;
            origin = new Vector2(texture.Width, texture.Height) / 2;
            scale = 1;
            this.random = random;
            this.shotDelay = random.Next(minShotDelay, maxShotDelay);

        }
        public void Update(GameTime time)
        {
            if (Scale <= .1 || Scale > 1)
            {
                scaleRate *= -1;
            }
            scale += scaleRate;
            foreach (Bullet bullet in bullets)
            {
                bullet.Update();
            }

            timer += (float)time.ElapsedGameTime.TotalSeconds;
            if (timer > this.shotDelay)
            {
                this.Shoot();
                this.timer = 0;
                this.shotDelay = random.Next(minShotDelay, maxShotDelay);
            }
            // position += new Vector2(SpeedX, SpeedY);
        }

        public void Shoot()
        {
            bullets.Add(new Bullet(enemyBulletTexture, new Vector2(position.X - enemyBulletTexture.Width / 2, position.Y), Color.White, bulletSpeed));
        }

        public override void Draw(SpriteBatch batch, bool debug)
        {
            if (debug) { batch.Draw(Game1.hitboxSprite, this.Hitbox, Color.White); }
            batch.Draw(texture, position, null, tint, 0f, Origin, Scale, SpriteEffects.None, 0);

            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(batch, debug);
            }

        }
    }
}
