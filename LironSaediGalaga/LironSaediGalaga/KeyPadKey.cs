using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LironSaediGalaga
{
    class KeyPadKey
    {
        Rectangle Hitbox;
        string Letter;

        public KeyPadKey(Rectangle Hitbox,string Letter)
        {

            this.Hitbox = Hitbox;
            this.Letter = Letter;
        }
    }
}
