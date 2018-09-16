using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class CollisionObject
    {
        //Data---------------------------------------------
        public BoxSprite pColSprite;
        public CollisionRect poColRect;

        public CollisionObject(ProxySprite pProxySprite)
        {
            Debug.Assert(pProxySprite != null);

            // Create Collision Rect
            // Use the reference sprite to set size and shape
            // need to refactor if you want it different
            GameSprite pSprite = pProxySprite.pSprite;
            Debug.Assert(pSprite != null);

            // Origin is in the UPPER RIGHT 
            this.poColRect = new CollisionRect(pSprite.GetScreenRect());
            Debug.Assert(this.poColRect != null);

            // Create the sprite
            this.pColSprite = BoxSpriteManager.Add(BoxSprite.Name.Box, this.poColRect);
            Debug.Assert(this.pColSprite != null);
            this.pColSprite.SetLineColor(1.0f, 1.0f, 0.0f);

        }

        public void UpdatePos(float x, float y)
        {
            this.poColRect.x = x;
            this.poColRect.y = y;

            this.pColSprite.x = this.poColRect.x;
            this.pColSprite.y = this.poColRect.y;
        }


    }
}