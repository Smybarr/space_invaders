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

        ~ImageHolder()
        {
#if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~ImageHolder():{0}", this.GetHashCode());
#endif
            this.pImage = null;
        }


    }
}