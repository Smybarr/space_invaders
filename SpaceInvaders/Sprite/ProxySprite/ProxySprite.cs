using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ProxySprite : SpriteBase
    {
        public enum Name
        {
            Proxy,

            NullObject,
            Blank
        }


        // Static Data: ------------------------------------

        // Data: -----------------------------------

        public Name name;
        public float x;
        public float y;
        public GameSprite pSprite;

        public override Enum GetSpriteName()
        {
            return this.name;
        }

        // Create a single sprite and all dynamic objects ONCE and ONLY ONCE (OOO- tm)
        public ProxySprite()
            : base()
        {
            this.name = ProxySprite.Name.Blank;

            this.x = 0.0f;
            this.y = 0.0f;

            this.pSprite = null;
        }
        public ProxySprite(GameSprite.Name name)
        {
            this.name = ProxySprite.Name.Proxy;

            this.x = 0.0f;
            this.y = 0.0f;

            this.pSprite = GameSpriteManager.Find(name);
            Debug.Assert(this.pSprite != null);
        }

        ~ProxySprite()
        {
            #if (TRACK_DESTRUCTOR)   
            Debug.WriteLine("~ProxySprite():{0} ", this.GetHashCode());
            #endif
            this.pSprite = null;
            this.name = ProxySprite.Name.Blank;
        }

        public void Set(GameSprite.Name name)
        {
            this.name = ProxySprite.Name.Proxy;

            this.x = 0.0f;
            this.y = 0.0f;

            this.pSprite = GameSpriteManager.Find(name);

            ////COMMENT THIS OUT AND IT STILL RUNS
            //// See 
            Debug.Assert(this.pSprite != null);
        }
        public void Wash()
        {

        }

        public override void Update()
        {
            //// push the data from proxy to Real GameSprite
            //this.privPushToReal();
            //this.pSprite.Update();

            //Due to proxy sprite, do push in render
        }

        private void privPushToReal()
        {
            // push the data from proxy to Real GameSprite
            Debug.Assert(this.pSprite != null);

            this.pSprite.x = this.x;
            this.pSprite.y = this.y;
        }

        public override void Draw()
        {
            // move the values over to Real GameSprite
            this.privPushToReal();

            // update and draw real sprite 
            // Seems redundant - Real Sprite might be stale
            this.pSprite.Update();
            this.pSprite.Draw();
        }


        public void SetName(Name inName)
        {
            this.name = inName;
        }
        public Name GetName()
        {
            return this.name;
        }
        public void DumpNodeData()
        {


        }

    }
}