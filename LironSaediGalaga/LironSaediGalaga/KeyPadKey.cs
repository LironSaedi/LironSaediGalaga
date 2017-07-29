using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LironSaediGalaga
{
    class KeyPadKey : Sprite
    {
        string letter;
        Color textColor;
        SpriteFont letterFont;

        Vector2 textOffset;

        public KeyPadKey(Texture2D keyTex, Vector2 position, Color textColor, string letter, SpriteFont letterFont)
            : base(keyTex, position, Color.White)
        {
            this.letter = letter;
            this.textColor = textColor;
            this.letterFont = letterFont;

            float x = keyTex.Width / 2;
            float y = keyTex.Height * 0.4f;
            Vector2 letterDims = letterFont.MeasureString(letter);
            x -= letterDims.X / 2;
            y -= letterDims.Y / 2;
            textOffset = new Vector2(x, y);
        }

        public override void Draw(SpriteBatch batch, bool debug)
        {
            base.Draw(batch, debug);

            batch.DrawString(letterFont, letter, position + textOffset, textColor);
        }

        public override string ToString()
        {
            return this.letter;
        }
    }
}
