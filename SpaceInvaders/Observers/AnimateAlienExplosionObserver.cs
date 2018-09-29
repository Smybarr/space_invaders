using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class AnimateAlienExplosionObserver : ColObserver
    {
        //this observer triggers the explosion animation before removing an alien;
        public override void Notify()
        {
            //todo create animation sprite manager or a command manager for object pooling here\

            //create alien explosion animation object
            AnimationSprite pAnim_AlienExplosion_Pop = new AnimationSprite(GameSprite.Name.AlienExplosion);

            //attach the explosion image for the animation;
            pAnim_AlienExplosion_Pop.Attach(Image.Name.AlienExplosionPop);

            //add to the timer event manager
            TimerEventManager.Add(TimerEvent.Name.AlienExplosionAnimation, pAnim_AlienExplosion_Pop, 0.75f);
        }
    }
}
