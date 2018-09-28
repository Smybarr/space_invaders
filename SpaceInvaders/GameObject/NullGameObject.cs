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

        //TODO GameObjectName = Root or NullObject? NullGameObjects are usually used as the root node...
        public NullGameObject()
            : base(GameObject.Name.Root, GameSprite.Name.NullObject, 0)
        {

        }
        ~NullGameObject()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~NullGameObject():{0}", this.GetHashCode());
            #endif
        }

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitNullGameObject(this);

        }
        public override void Update()
        {
            // do nothing - its a null object
        }
    }
}
