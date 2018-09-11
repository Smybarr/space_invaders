using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class Command
    {
        ~Command()
        {
#if (TRACK_DESTRUCTOR)
            Debug.WriteLine("      ~Command():{0}", this.GetHashCode());
#endif
        }
        
        // define this in concrete 
        public abstract void Execute(float deltaTime);
    }
}
