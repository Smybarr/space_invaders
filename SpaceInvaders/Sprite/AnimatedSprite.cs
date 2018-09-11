using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class AnimationSprite : Command
    {
        // Data: ---------------
        private GameSprite pSprite;
        private SLink pCurrImage;
        private SLink poFirstImage;


        public AnimationSprite(GameSprite.Name spriteName)
        {
            // initialized the sprite animation is attached to
            this.pSprite = GameSpriteManager.Find(spriteName);
            Debug.Assert(this.pSprite != null);

            // initialize references
            this.pCurrImage = null;

            // list
            this.poFirstImage = null;
        }
        ~AnimationSprite()
        {
#if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~AnimationSprite():{0} ", this.GetHashCode());
#endif

            ImageHolder pNode = (ImageHolder)this.poFirstImage;
            while (pNode != null)
            {
                ImageHolder pNodeToKill = pNode;

                // squirrel away
                pNode = (ImageHolder)pNode.pSNext;

#if (TRACK_DESTRUCTOR)
                Debug.WriteLine("~AnimationSprite():  --->{0} ", pNodeToKill.GetHashCode());
#endif
                // kill the image inside the holder
                pNodeToKill.pSNext = null;
                pNodeToKill = null;
            }

            this.pSprite = null;
            this.pCurrImage = null;
            this.poFirstImage = null;
        }


        public void Attach(Image.Name imageName)
        {
            // Get the image
            Image pImage = ImageManager.Find(imageName);
            Debug.Assert(pImage != null);

            // Create a new holder
            ImageHolder pImageHolder = new ImageHolder(pImage);
            Debug.Assert(pImageHolder != null);

            // Attach it to the Animation Sprite ( Push to front )
            SLink.AddToFront(ref this.poFirstImage, pImageHolder);

            // Set the first one to this image
            this.pCurrImage = pImageHolder;
        }

        public override void Execute(float deltaTime)
        {
            // advance to next image 
            ImageHolder pImageHolder = (ImageHolder)this.pCurrImage.pSNext;

            // if at end of list, set to first
            if (pImageHolder == null)
            {
                pImageHolder = (ImageHolder)poFirstImage;
            }

            // squirrel away for next timer event
            this.pCurrImage = pImageHolder;

            // change image
            this.pSprite.ChangeImage(pImageHolder.pImage);

            // Add itself back to timer
            TimerEventManager.Add(TimerEvent.Name.SpriteAnimation, this, deltaTime);
        }


    }
}