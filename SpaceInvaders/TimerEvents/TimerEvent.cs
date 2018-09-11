using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class TimerEvent : MLink
    {
        public enum Name
        {
            SpriteAnimation,

            Blank
        }

        // Data: ---------------
        private TimerEvent.Name name;
        public Command pCommand;
        public float triggerTime;
        public float deltaTime;

        public TimerEvent()
            : base()
        {
            this.name = Name.Blank;
            this.pCommand = null;
            this.triggerTime = 0.0f;
            this.deltaTime = 0.0f;
        }

        ~TimerEvent()
        {
#if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~TimeEvent():{0} ", this.GetHashCode());
#endif
            this.name = TimerEvent.Name.Blank;
            this.pCommand = null;
        }

        public void Wash()
        {
            this.name = Name.Blank;
            this.pCommand = null;
            this.triggerTime = 0.0f;
            this.deltaTime = 0.0f;
        }

        public void Set(Name eventName, Command pCommand, float deltaTimeToTrigger)
        {
            Debug.Assert(pCommand != null);

            this.name = eventName;
            this.pCommand = pCommand;
            this.deltaTime = deltaTimeToTrigger;

            // set the trigger time
            this.triggerTime = TimerEventManager.GetCurrTime() + deltaTimeToTrigger;
        }
        public TimerEvent.Name GetName()
        {
            return this.name;
        }
        public void SetName(Name inName)
        {
            this.name = inName;
        }
        public void Process()
        {
            // make sure the command is valid
            Debug.Assert(this.pCommand != null);
            // fire off command
            this.pCommand.Execute(deltaTime);
        }

        public void Dump()
        {
            //this.MLinkDump();

            // we are using HASH code as its unique identifier 
            Debug.WriteLine("   {0} ({1})", this.name, this.GetHashCode());
            if (this.pMNext == null)
            {
                Debug.WriteLine("      next: null");
            }
            else
            {
                TimerEvent pTmp = (TimerEvent)this.pMNext;
                Debug.WriteLine("      next: {0} ({1})", pTmp.GetName(), pTmp.GetHashCode());
            }

            if (this.pMPrev == null)
            {
                Debug.WriteLine("      prev: null");
            }
            else

            {
                TimerEvent pTmp = (TimerEvent)this.pMPrev;
                Debug.WriteLine("      prev: {0} ({1})", pTmp.GetName(), pTmp.GetHashCode());
            }

            // Data:
            Debug.WriteLine("      Command: {0}", this.pCommand);
            Debug.WriteLine("   Event Name: {0}", this.name);
            Debug.WriteLine(" Trigger Time: {0}", this.triggerTime);
            Debug.WriteLine("   Delta Time: {0}", this.deltaTime);

        }


        override protected float derivedCompareValue()
        {
            return this.triggerTime;
        }
    }
}
