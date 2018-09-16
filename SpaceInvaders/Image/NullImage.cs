﻿using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class NullImage : Image
    {
        public NullImage(Image.Name name)
            : base()
        {
            this.name = name;
        }
    }
}