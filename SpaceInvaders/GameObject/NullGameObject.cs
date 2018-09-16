using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    /* Null Object Pattern
     *      - NullObjects are for safety;
     *      - they inherit from objects that may or may not be instantiated
     *          and allows for a more streamlined architecture
     *      - lets you avoid developing special cases
     *
     */
    public class NullGameObject : GameObject
    {
        public NullGameObject()
            : base(GameObject.Name.NullObject, GameSprite.Name.NullObject, 0)
        {

        }
        ~NullGameObject()
        {
#if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~NullGameObject():{0}", this.GetHashCode());
#endif
        }
        public override void Update()
        {
            // do nothing - its a null object
        }
    }
}
