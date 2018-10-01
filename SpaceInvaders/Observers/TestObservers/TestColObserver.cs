using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class TestColObserver : ColObserver
    {
        //Data-------------------


        public TestColObserver()
        {
            //initialize any data here;

        }

        public TestColObserver(TestColObserver self)
        {
            //used for passing observer data from Notify into Execute via DelayManager...

            //example: this.x = self.x
        }


        public override void Notify()
        {


            //may need to create a new observer and pass to DelayManager...

        }


        public override void Execute()
        {
            //make sure an observer instance was passed to the delay manager if calling execute...
        }

    }
}