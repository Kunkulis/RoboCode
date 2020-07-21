using System;
using Robocode;
using System.Drawing;
using Robocode.Util;
using System.Collections.Generic;

namespace ARE
{
    public class Kunkulis : AdvancedRobot
    {
        double previousEnemyEnergy = 100;
        double energyChange = 0.0;
        double oldEnemyHeading;
        int _direction = 1;

        public override void Run()
        {
            SetColors(Color.DarkOrange, Color.Black, Color.DimGray, Color.Orange, Color.WhiteSmoke);
            while (true)
            {
                TurnRadarRight(Double.PositiveInfinity);
            }
        }

        public override void OnScannedRobot(ScannedRobotEvent e)
        {
            if (e.Distance <= 250)
            {
                Raming(e);                
            }
            else
            {
                LinearTargeting(e);
            }
        }

        public override void OnHitWall(HitWallEvent e)
        {
            SetTurnRight(e.Bearing + 135);
            SetAhead(200);
        }

        public void Raming(ScannedRobotEvent e)
        {
            energyChange = previousEnemyEnergy - e.Energy;
            previousEnemyEnergy = e.Energy;            
            double absBearing = e.BearingRadians + HeadingRadians;
            double turn = absBearing + Math.PI / 2;
            Random r = new Random();
            double ran = r.NextDouble();            
            SetColors(Color.Red, Color.Red, Color.Red, Color.Red, Color.Red);
            double bulletPower = 3;
            double bulletSpeed = 20 - 3 * bulletPower;
            double bulletDamage = 4 * bulletPower;
            turn -= Math.Max(0.5, (1 / e.Distance) * 100) * _direction;
            
            SetTurnRightRadians(Utils.NormalRelativeAngle(turn - HeadingRadians));

            //This block of code detects when an opponents energy drops.
            //Console.WriteLine("prevEnergy={0} e.Energy={1}", previousEnemyEnergy, e.Energy);
            if (energyChange > 0 && energyChange <= 3)
            {
                Console.WriteLine("Enemy energy drop Ram");
                //We use 300/e.getDistance() to decide if we want to change directions.
                //This means that we will be less likely to reverse right as we are about to ram the enemy robot.
                if (ran > 300 / e.Distance)
                {
                    Console.WriteLine("ran={0} division={1}", turn, (300 / e.Distance));
                    _direction = -_direction;
                }
            }

            //This line makes us slow down when we need to turn sharply.

            MaxVelocity = (400 / TurnRemaining);

            SetAhead(100 * _direction);

            //Finding the heading and heading change.
            double enemyHeading = e.HeadingRadians;
            double enemyHeadingChange = enemyHeading - oldEnemyHeading;
            oldEnemyHeading = enemyHeading;

            /*This method of targeting is know as circular targeting; you assume your enemy will
             *keep moving with the same speed and turn rate that he is using at fire time.The 
             *base code comes from the wiki.
            */
            double deltaTime = 0;
            double predictedX = X + e.Distance * Math.Sin(absBearing);
            double predictedY = Y + e.Distance * Math.Cos(absBearing);

            while ((++deltaTime) * bulletSpeed < Math.Sqrt(Math.Pow((predictedX - X), 2) + Math.Pow((predictedY - Y), 2)))
            { 
                //Add the movement we think our enemy will make to our enemy's current X and Y
                predictedX += Math.Sin(enemyHeading) * e.Velocity;
                predictedY += Math.Cos(enemyHeading) * e.Velocity;
                
                //Find our enemy's heading changes.
                enemyHeading += enemyHeadingChange;

                //If our predicted coordinates are outside the walls, put them 18 distance units away from the walls as we know 
                //that that is the closest they can get to the wall (Bots are non-rotating 36*36 squares).
                predictedX = Math.Max(Math.Min(predictedX, BattleFieldWidth - 18), 18);
                predictedY = Math.Max(Math.Min(predictedY, BattleFieldHeight - 18), 18);
            }            
            //Find the bearing of our predicted coordinates from us.
            double aim = Utils.NormalAbsoluteAngle(Math.Atan2(predictedX - X, predictedY - Y));
            
            //Aim and fire.
            SetTurnGunRightRadians(Utils.NormalRelativeAngle(aim - GunHeadingRadians));
            SetFire(bulletPower);

            SetTurnRadarRightRadians(Utils.NormalRelativeAngle(absBearing - RadarHeadingRadians) * 2);
        }

        public void LinearTargeting(ScannedRobotEvent e)
        {
            SetColors(Color.Green, Color.Green, Color.Green, Color.Green, Color.Green);
            SetTurnRight(e.Bearing + 90 - 30 * _direction);
            double bulletPower = Math.Min((400.00 / e.Distance), 3.00);
            double myX = X;
            double myY = Y;
            double absBearing = e.BearingRadians + HeadingRadians;
            double eX = X + e.Distance * Math.Sin(absBearing); //Enemy future X
            double eY = Y + e.Distance * Math.Cos(absBearing); //Enemy future Y
            double enemyHeading = e.HeadingRadians;
            double enemyVelocity = e.Velocity;
            double deltaTime = 0;
            double battleFieldHeight = BattleFieldHeight,
                   battleFieldWidth = BattleFieldWidth;
            double enemyDistance = e.Distance;

            while ((deltaTime++) * (20.0 - 3.0 * bulletPower) < Math.Sqrt(Math.Pow((eX - myX), 2) + Math.Pow((eY - myY), 2)))
            {
                eX += Math.Sin(enemyHeading) * enemyVelocity;
                eY += Math.Cos(enemyHeading) * enemyVelocity;
                if (eX < 18.0 || eY < 18.0 || eX > battleFieldWidth - 18.0 || eY > battleFieldHeight - 18.0)
                {
                    eX = Math.Min(Math.Max(18.0, eX), battleFieldWidth - 18.0);
                    eY = Math.Min(Math.Max(18.0, eY), battleFieldHeight - 18.0);
                    break;
                }
            }
            double theta = Utils.NormalAbsoluteAngle(Math.Atan2(eX - X, eY - Y));
            SetTurnRadarRightRadians(Utils.NormalRelativeAngle(absBearing - RadarHeadingRadians));
            SetTurnGunRightRadians(Utils.NormalRelativeAngle(theta - GunHeadingRadians));

            Fire(bulletPower);
            if (energyChange > 0 && energyChange <= 3)
            {                
                _direction = (e.Distance <= 300 && _direction < 0) ? _direction : -_direction;
                SetAhead((e.Distance / 4 + 25) * _direction);
            }
        }        
    }
}
