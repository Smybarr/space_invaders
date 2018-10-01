using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class Octopus : AlienType
    {
        public Octopus(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY)
            : base(name, spriteName, index, AlienType.Type.Octopus)
        {
            this.x = posX;
            this.y = posY;


            //set octopus sprite to red color
            this.pProxySprite.pSprite.ChangeColor(1.0f, 0.0f, 0.0f);
        }

        ~Octopus()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~Octopus():{0}", this.GetHashCode());
            #endif
        }


        public override void Remove()
        {
            base.Remove();
        }

        //called from column ( (GameObject)this.pParent );
        public override void DropBomb()
        {
            float bomb_startPosX = this.x;
            float bomb_startPosY = this.y;

            //Bomb pBomb = new Bomb(GameObject.Name.Bomb, GameSprite.Name.CrossAlienBomb, new FallStraight(), 0, bomb_startPosX, bomb_startPosY);
            //Debug.Assert(pBomb != null);
            Random r = new Random();
            int randomBombType = r.Next(1, 4);

            Bomb pBomb = privCreateBomb(randomBombType, bomb_startPosX, bomb_startPosY);

            //get the root tree
            PCSTree rootGamObjTree = GameObjectManager.GetRootTree();
            Debug.Assert(rootGamObjTree != null);

            //find the bomb root
            GameObject pBombRoot = GameObjectManager.Find(GameObject.Name.BombRoot);
            Debug.Assert(pBombRoot != null);

            rootGamObjTree.Insert(pBomb, pBombRoot);

            //activate the sprites
            SpriteBatch pSB_GameSprites = SpriteBatchManager.Find(SpriteBatch.Name.GameSprites);
            SpriteBatch pSB_Boxes = SpriteBatchManager.Find(SpriteBatch.Name.SpriteBoxes);

            pBomb.ActivateGameSprite(pSB_GameSprites);
            pBomb.ActivateCollisionSprite(pSB_Boxes);
        }


        //todo repack this into a BombManager to avoid creating a new bomb each time one is dropped
        private Bomb privCreateBomb(int randomBombInt, float bomb_startPosX, float bomb_startPosY)
        {
            Bomb pBomb = null;

            switch (randomBombInt)
            {
                case 1:
                    pBomb = new Bomb(GameObject.Name.Bomb, GameSprite.Name.ZigZagAlienBomb, new DropStraight(), 0, bomb_startPosX, bomb_startPosY);
                    Debug.Assert(pBomb != null);
                    return pBomb;

                case 2:
                    pBomb = new Bomb(GameObject.Name.Bomb, GameSprite.Name.CrossAlienBomb, new DropStraight(), 0, bomb_startPosX, bomb_startPosY);
                    Debug.Assert(pBomb != null);
                    return pBomb;

                case 3:
                    pBomb = new Bomb(GameObject.Name.Bomb, GameSprite.Name.RollingAlienBomb, new DropStraight(), 0, bomb_startPosX, bomb_startPosY);
                    Debug.Assert(pBomb != null);
                    return pBomb;

                default:
                    pBomb = new Bomb(GameObject.Name.Bomb, GameSprite.Name.CrossAlienBomb, new DropStraight(), 0, bomb_startPosX, bomb_startPosY);
                    Debug.Assert(pBomb != null);
                    return pBomb;
            }
        }

        //--------------------------------------------------------------------------
        //collisions

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitOctopus(this);
        }


        public override void VisitMissileRoot(MissileRoot m)
        {
            // AlienOctopus vs MissileRoot
            //    Debug.WriteLine("collide: {0} with {1}", this, m);

            // AlienOctopus vs Missile
            ColPair.Collide((GameObject)m.pChild, this);
        }

        public override void VisitMissile(Missile m)
        {
            // AlienOctopus vs Missile
            ColPair collisionPair = ColPairManager.GetActiveColPair();
            collisionPair.SetCollision(m, this);
            collisionPair.NotifyListeners();
            //    Debug.WriteLine("collide: {0} with {1}", this, m);

            Debug.WriteLine("-------> Done  <--------");

            //m.hit = true;

        }



    }
}