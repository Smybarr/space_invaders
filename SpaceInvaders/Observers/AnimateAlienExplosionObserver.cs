using System;
using System.Diagnostics;
using System.Dynamic;

namespace SpaceInvaders
{
    public class AnimateAlienExplosionObserver : ColObserver
    {
        //data
        private GameObject pAlienExplosionObj;
        //for tracking coordinates of subject (alien being destroyed);
        private float pos_x;
        private float pos_y;

        public AnimateAlienExplosionObserver()
        {
            this.pAlienExplosionObj = null;
            this.pos_x = 0.0f;
            this.pos_y = 0.0f;
        }

        public AnimateAlienExplosionObserver(AnimateAlienExplosionObserver m)
        {
            //transfer data here;
            this.pAlienExplosionObj = m.pAlienExplosionObj;
            this.pos_x = m.pos_x;
            this.pos_y = m.pos_y;
        }


        //this observer triggers the explosion animation before removing an alien;
        public override void Notify()
        {
            //todo - create an explosion manager that can add and remove explosion objects
            //can't simply swap image of the alien object due to proxy pattern - stupid


            //get the coordinates of the destroyed subject;
            this.pos_x = this.pSubject.pObjB.x;
            this.pos_y = this.pSubject.pObjB.y;

            //create a game object to temporarily render in the position of the destroyed subject
            //game sprites and sprite batch stuff is taken care of in the constructor;
            this.pAlienExplosionObj = new ExplodingAlien(GameObject.Name.ExplodingAlien, GameSprite.Name.AlienExplosion, 0, this.pos_x, this.pos_y);

            this.pAlienExplosionObj.x = this.pos_x;
            this.pAlienExplosionObj.y = this.pos_y;

            GameObjectManager.AttachTree(this.pAlienExplosionObj);


            this.pAlienExplosionObj.Update();


            //pass to delay manager
            AnimateAlienExplosionObserver pObserver = new AnimateAlienExplosionObserver(this);
            DelayedObjectManager.Attach(pObserver);
        }

        public override void Execute()
        {
            ////pExplosionSprite.pProxySprite.Draw();
            //Debug.Assert(this.pAlienExplosionObj != null);
            ////remove the game object

            //this.pAlienExplosionObj.pProxySprite = null;
            //this.pAlienExplosionObj.index = 0;
            //this.pAlienExplosionObj.x = 0.0f;
            //this.pAlienExplosionObj.y = 0.0f;


            //this.pAlienExplosionObj = null;
            
            this.pAlienExplosionObj.Remove();


        }



    }
}
