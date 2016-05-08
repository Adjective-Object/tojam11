using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Adventure
{
    public class TextMessage
    {
        public TextMessage(string na, string me)
        {
            name = na;
            messages.Add(me);
        }

        public string name;
        public List<string> messages = new List<string>();
    }
}

