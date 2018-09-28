using System;
using System.ComponentModel;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class InputManager
    {
        // Data: ----------------------------------------------
        private static InputManager pInstance = null;

        //for tracking key history
        private bool privSpaceKeyPrev;
        private bool privCKeyPrev;
        ////left/right keys shouldn't keep history - allows for long key press
        //private bool privLetKeyPrev;
        //private bool privRightKeyPrev;


        private InputSubject pSubjectSpace;
        private InputSubject pSubjectArrowLeft;
        private InputSubject pSubjectArrowRight;
        private InputSubject pSubjectCKey;




        private InputManager()
        {
            this.pSubjectArrowLeft = new InputSubject();
            this.pSubjectArrowRight = new InputSubject();
            this.pSubjectSpace = new InputSubject();

            this.pSubjectCKey = new InputSubject();
            

            this.privSpaceKeyPrev = false;
        }
        private static InputManager privGetInstance()
        {
            if (pInstance == null)
            {
                pInstance = new InputManager();
            }
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        public static void LoadKeyInputObservers()
        {
            InputSubject inputSubject;

            //CollisionBoxToggle;
            //inputSubject = InputManager.Get_C_KeySubject();
            //inputSubject.Attach(new ToggleCollisionBoxObserver());

            //left arrow key;
            inputSubject = InputManager.GetArrowRightSubject();
            MoveRightObserver pMR_Observer = new MoveRightObserver();
            inputSubject.Attach(pMR_Observer);
            DeathManager.Attach(pMR_Observer);

            //right arrow key;
            inputSubject = InputManager.GetArrowLeftSubject();
            MoveLeftObserver pML_Observer = new MoveLeftObserver();
            inputSubject.Attach(pML_Observer);
            DeathManager.Attach(pML_Observer);

            //Spacebar;
            inputSubject = InputManager.GetSpaceSubject();
            ShootMissileObserver pShoot_Observer = new ShootMissileObserver();
            inputSubject.Attach(pShoot_Observer);
            DeathManager.Attach(pShoot_Observer);


            //C Key;
            inputSubject = InputManager.GetCKeySubject();
            ToggleBoxSpriteObserver pToggleBoxSprites = new ToggleBoxSpriteObserver();
            inputSubject.Attach(pToggleBoxSprites);
            DeathManager.Attach(pToggleBoxSprites);

        }


        public static InputSubject GetArrowRightSubject()
        {
            InputManager pMan = InputManager.privGetInstance();
            Debug.Assert(pMan != null);

            return pMan.pSubjectArrowRight;
        }
        public static InputSubject GetArrowLeftSubject()
        {
            InputManager pMan = InputManager.privGetInstance();
            Debug.Assert(pMan != null);

            return pMan.pSubjectArrowLeft;
        }
        public static InputSubject GetSpaceSubject()
        {
            InputManager pMan = InputManager.privGetInstance();
            Debug.Assert(pMan != null);

            return pMan.pSubjectSpace;
        }
        public static InputSubject GetCKeySubject()
        {
            InputManager pMan = InputManager.privGetInstance();
            Debug.Assert(pMan != null);

            return pMan.pSubjectCKey;
        }





        public static void Update()
        {
            InputManager pMan = InputManager.privGetInstance();
            Debug.Assert(pMan != null);

            // SpaceKey: (Fire Hero Missile) (with key history) -----------------------------------------------------------
            bool spaceKeyCurr = Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_SPACE);
            if (spaceKeyCurr == true && pMan.privSpaceKeyPrev == false)
            {
                pMan.pSubjectSpace.Notify();
            }
            pMan.privSpaceKeyPrev = spaceKeyCurr;

            // C Key (Toggle Collision Box Render): (with key history) -----------------------------------------------------------
            bool cKeyCurr = Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_C);
            if (cKeyCurr == true && pMan.privCKeyPrev == false)
            {
                pMan.pSubjectCKey.Notify();
            }
            pMan.privCKeyPrev = cKeyCurr;


            //Note that having no key input history allows for continuous movement
            //by holding down the left/right key arrows

            // RightKey: (no history) -----------------------------------------------------------
            if (Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_ARROW_RIGHT) == true)
            {
                pMan.pSubjectArrowRight.Notify();
            }

            // RightKey: (no history) -----------------------------------------------------------
            if (Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_ARROW_LEFT) == true)
            {
                pMan.pSubjectArrowLeft.Notify();
            }


            //// LeftKey: (Move Hero Ship Left) (with key history) -----------------------------------------------------------
            //bool leftKeyCurr = Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_ARROW_LEFT);
            //if (leftKeyCurr == true && pMan.privLeftKeyPrev == false)
            //{
            //    pMan.pSubjectArrowLeft.Notify();
            //}
            //pMan.privLeftKeyPrev = leftKeyCurr;

            //// RightKey (Move Hero Ship Right): (with key history) -----------------------------------------------------------
            //bool rightKeyCurr = Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_ARROW_RIGHT);
            //if (rightKeyCurr == true && pMan.privRightKeyPrev == false)
            //{
            //    pMan.pSubjectArrowRight.Notify();
            //}
            //pMan.privRightKeyPrev = rightKeyCurr;






        }

 
    }
}