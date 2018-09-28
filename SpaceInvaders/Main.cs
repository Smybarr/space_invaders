using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using SpaceInvaders.Test.SpaceInvaders;

namespace SpaceInvaders
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //test render switch
            bool isTest = false;


            if (isTest)
            {
                TestGameTwo testGame = new TestGameTwo();
                Debug.Assert(testGame != null);

                testGame.Run();

            }
            else
            {
                // Create the instance
                SpaceInvaders game = new SpaceInvaders();
                Debug.Assert(game != null);

                // Start the game
                game.Run();
            }
        }
    }
}

