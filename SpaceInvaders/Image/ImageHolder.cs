using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ImageHolder : SLink
    {
        // Data: ---------------
        public Image pImage;

        public ImageHolder(Image image)
            : base()
        {
            this.pImage = image;
        }

    }
}