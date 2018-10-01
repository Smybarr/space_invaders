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
        private bool privKeyPrev_Space;
        private bool privKeyPrev_C;
        private bool privKeyPrev_T;


        ////left/right keys shouldn't keep history - allows for long key press
        //private bool privLetKeyPrev;
        //private bool privRightKeyPrev;


        private InputSubject pSubjectKey_Space;
        private InputSubject pSubjectKey_ArrowLeft;
        private InputSubject pSubjectKey_ArrowRight;
        private InputSubject pSubjectKey_C;

        //test key (for testing certain actions, usually observer actions)
        private InputSubject pSubjectKey_T;



        private InputManager()
        {
            this.pSubjectKey_ArrowLeft = new InputSubject();
            this.pSubjectKey_ArrowRight = new InputSubject();
            this.pSubjectKey_Space = new InputSubject();

            this.pSubjectKey_C = new InputSubject();
            this.pSubjectKey_T = new InputSubject();
                     

            this.privKeyPrev_Space = false;
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


            //T Key - TestObserver allows debugging certain game actions by pressing the 't' key
            //call any functions within the TestObserver class
            //example: press T key to test pulling a new ship after old one is destroyed and/or triggering a game over;
            inputSubject = InputManager.GetTKeySubject();
            TestInputObserver pTestObserverAction = new TestInputObserver();
            inputSubject.Attach(pTestObserverAction);
            DeathManager.Attach(pTestObserverAction);

        }


        public static InputSubject GetArrowRightSubject()
        {
            InputManager pMan = InputManager.privGetInstance();
            Debug.Assert(pMan != null);

            return pMan.pSubjectKey_ArrowRight;
        }
        public static InputSubject GetArrowLeftSubject()
        {
            InputManager pMan = InputManager.privGetInstance();
            Debug.Assert(pMan != null);

            return pMan.pSubjectKey_ArrowLeft;
        }
        public static InputSubject GetSpaceSubject()
        {
            InputManager pMan = InputManager.privGetInstance();
            Debug.Assert(pMan != null);

            return pMan.pSubjectKey_Space;
        }
        public static InputSubject GetCKeySubject()
        {
            InputManager pMan = InputManager.privGetInstance();
            Debug.Assert(pMan != null);

            return pMan.pSubjectKey_C;
        }
        public static InputSubject GetTKeySubject()
        {
            InputManager pMan = InputManager.privGetInstance();
            Debug.Assert(pMan != null);

            return pMan.pSubjectKey_T;
        }




        public static void Update()
        {
            InputManager pMan = InputManager.privGetInstance();
            Debug.Assert(pMan != null);

            // SpaceKey: (Fire Hero Missile) (with key history) -----------------------------------------------------------
            bool spaceKeyCurr = Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_SPACE);
            if (spaceKeyCurr == true && pMan.privKeyPrev_Space == false)
            {
                pMan.pSubjectKey_Space.Notify();
            }
            pMan.privKeyPrev_Space = spaceKeyCurr;

            // C Key (Toggle Collision Box Render): (with key history) -----------------------------------------------------------
            bool cKeyCurr = Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_C);
            if (cKeyCurr == true && pMan.privKeyPrev_C == false)
            {
                pMan.pSubjectKey_C.Notify();
            }
            pMan.privKeyPrev_C = cKeyCurr;


            // C Key (Test Various Game Actions): (with key history) -----------------------------------------------------------
            bool tKeyCurr = Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_T);
            if (tKeyCurr == true && pMan.privKeyPrev_T == false)
            {
                pMan.pSubjectKey_T.Notify();
            }
            pMan.privKeyPrev_T = tKeyCurr;





            //Note that having no key input history allows for continuous movement
            //by holding down the left/right key arrows

            // RightKey: (no history) -----------------------------------------------------------
            if (Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_ARROW_RIGHT) == true)
            {
                pMan.pSubjectKey_ArrowRight.Notify();
            }

            // RightKey: (no history) -----------------------------------------------------------
            if (Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_ARROW_LEFT) == true)
            {
                pMan.pSubjectKey_ArrowLeft.Notify();
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