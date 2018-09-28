using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ColObject
    {
        //Data---------------------------------------------
        public BoxSprite pColSprite;
        public ColRect poColRect;

        public ColObject(ProxySprite pProxySprite)
        {
            Debug.Assert(pProxySprite != null);

            // Create Collision Rect
            // Use the reference sprite to set size and shape
            // need to refactor if you want it different
            GameSprite pSprite = pProxySprite.pSprite;
            Debug.Assert(pSprite != null);

            // Origin is in the UPPER RIGHT 
            this.poColRect = new ColRect(pSprite.GetScreenRect());
            Debug.Assert(this.poColRect != null);

            // Create the box sprite

            //WORKING
            //this.pColSprite = BoxSpriteManager.Add(GameSprite.Name.Box, this.poColRect.x, this.poColRect.y, this.poColRect.width, this.poColRect.height);

            //TEST
            this.pColSprite = BoxSpriteManager.Add(pProxySprite.pSprite.GetName(), this.poColRect.x, this.poColRect.y, this.poColRect.width, this.poColRect.height);


            ////get the box name from the matching sprite name;
            //BoxSprite.Name colBoxName = (BoxSprite.Name) pProxySprite.pSprite.GetName();
            //Debug.Assert(colBoxName != null);
            //this.pColSprite = BoxSpriteManager.Find(colBoxName);
            //this.pColSprite.SetScreenRect(this.poColRect.x, this.poColRect.y, this.poColRect.width, this.poColRect.height);
            //Debug.Assert(this.pColSprite != null);

            this.pColSprite.SetLineColor(1.0f, 1.0f, 0.0f);
        }


        //Rename to PushPosition?
        public void UpdatePos(float x, float y)
        {
            this.poColRect.x = x;
            this.poColRect.y = y;

            this.pColSprite.x = this.poColRect.x;
            this.pColSprite.y = this.poColRect.y;

            this.pColSprite.SetScreenRect(this.poColRect.x, this.poColRect.y, this.poColRect.width, this.poColRect.height);
            this.pColSprite.Update();
        }


    }
}