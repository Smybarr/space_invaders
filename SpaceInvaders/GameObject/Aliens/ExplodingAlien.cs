using System;
using System.Diagnostics;


namespace SpaceInvaders
{
    public class ExplodingAlien : AlienType
    {
        public ExplodingAlien(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY) 
            : base(name, spriteName, index, AlienType.Type.AlienExplosion)
        {
            this.x = posX;
            this.y = posY;

            //this exploding alien will always be marked for death
            this.markForDeath = true;

            //get the right sprite batch and activate the explosion sprite
            SpriteBatch pSB_GameSprites = SpriteBatchManager.Find(SpriteBatch.Name.GameSprites);
            SpriteBatch pSB_Boxes = SpriteBatchManager.Find(SpriteBatch.Name.SpriteBoxes);

            //activate the game and collision sprites
            this.ActivateGameSprite(pSB_GameSprites);
            this.ActivateCollisionSprite(pSB_Boxes);



            //set the color of the proxy sprite;
            this.pProxySprite.pSprite.ChangeColor(1.0f, 1.0f, 1.0f, 1.0f);
        }

        ~ExplodingAlien()
        {
            #if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~Crab():{0}", this.GetHashCode());
            #endif
        }

        public override void Accept(ColVisitor other)
        {
            //do nothing or have it register a collision if it freaks out;
            other.VisitExplodingAlien(this);
        }

        public override void DropBomb()
        {
            //do nothing
        }
    }
}
