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
            
            TurnRight(-Heading);            
            SetColors(Color.DarkOrange, Color.Black, Color.DimGray, Color.Orange, Color.WhiteSmoke);
            while (true)
            {
                TurnRadarRight(Double.PositiveInfinity);
            }
        }

        public override void OnScannedRobot(ScannedRobotEvent e)
        {
            energyChange = previousEnemyEnergy - e.Energy;
            previousEnemyEnergy = e.Energy;

            //if (e.Distance <= 250)
            //{
                Raming(e);
            //}
            //else if (e.Distance > 250)
            //{
            //    LinearTargeting(e);
            //}
        }

        public override void OnHitWall(HitWallEvent e)
        {
            SetTurnRight(e.Bearing + 90);
            SetAhead(50);
            _direction = -_direction;
        }

        public void Raming(ScannedRobotEvent e)
        {
            double absBearing = e.BearingRadians + HeadingRadians;
            Console.WriteLine("e.BR={0} HR={1}", e.BearingRadians, HeadingRadians);
            double turn = absBearing + Math.PI / 2;
            //Random r = new Random();
            //double ran = r.NextDouble();
            SetColors(Color.Red, Color.Black, Color.DimGray, Color.Orange, Color.WhiteSmoke);
            double bulletPower = 3;
            double bulletSpeed = 20 - 3 * bulletPower;
            double bulletDamage = 4 * bulletPower;
            turn -= Math.Max(0.5, (1 / e.Distance) * 100) * _direction;
            Console.WriteLine("e.D={0}", e.Distance);

            SetTurnRightRadians(Utils.NormalRelativeAngle(turn - HeadingRadians));

            //This line makes us slow down when we need to turn sharply.

            MaxVelocity = (400 / TurnRemaining);

            SetAhead(100 * _direction);

            //Finding the heading and heading change.
            double enemyHeading = e.HeadingRadians;
            double enemyHeadingChange = enemyHeading - oldEnemyHeading;
            oldEnemyHeading = enemyHeading;
            Console.WriteLine("enemyHeading={0} enHeadChang={1} oldEnHead={2}", e.HeadingRadians, enemyHeadingChange, oldEnemyHeading);

            /*This method of targeting is know as circular targeting; you assume your enemy will
             *keep moving with the same speed and turn rate that he is using at fire time.The 
             *base code comes from the wiki.
            */
            double deltaTime = 0;
            Console.WriteLine("deltaTime={0}", deltaTime);
            double predictedX = X + e.Distance * Math.Sin(absBearing);
            Console.WriteLine("predictedX={0} X={1} + e.Distance={2} * Math.Sin(absBearing)={3} | absBearing={4}", predictedX, X, e.Distance, Math.Sin(absBearing), absBearing);
            double predictedY = Y + e.Distance * Math.Cos(absBearing);
            Console.WriteLine("predictedY={0} Y={1} + e.Distance={2} * Math.Cos(absBearing)={3} | absBearing={4}", predictedY, Y, e.Distance, Math.Cos(absBearing), absBearing);
            Console.WriteLine("e.Vel={0}",e.Velocity);

            int i = 0;
            while ((++deltaTime) * bulletSpeed < Math.Sqrt(Math.Pow((predictedX - X), 2) + Math.Pow((predictedY - Y), 2)))
            {
                Console.WriteLine("Nr={0}", i);
                //Add the movement we think our enemy will make to our enemy's current X and Y
                Console.WriteLine("deltaTime={0}*bulletSpeed={1}", deltaTime, bulletSpeed);
                Console.WriteLine("Math.Sqrt(Math.Pow((predictedX - X), 2)={0} + Math.Pow((predictedY - Y), 2))={1} | X={2} Y={3}", Math.Pow((predictedX - X), 2), Math.Pow((predictedY - Y), 2), X, Y);
                predictedX += Math.Sin(enemyHeading) * e.Velocity;
                predictedY += Math.Cos(enemyHeading) * e.Velocity;
                Console.WriteLine("preX={0} preY={1}", predictedX, predictedY);

                //Find our enemy's heading changes.
                enemyHeading += enemyHeadingChange;
                Console.WriteLine("enemyHeading={0}", enemyHeading);

                //If our predicted coordinates are outside the walls, put them 18 distance units away from the walls as we know 
                //that that is the closest they can get to the wall (Bots are non-rotating 36*36 squares).
                predictedX = Math.Max(Math.Min(predictedX, BattleFieldWidth - 18), 18);
                predictedY = Math.Max(Math.Min(predictedY, BattleFieldHeight - 18), 18);
                ++i;
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
            SetColors(Color.Green, Color.Black, Color.DimGray, Color.Orange, Color.WhiteSmoke);
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
            if (energyChange > 0 && energyChange <= 3)
            {
                _direction = (e.Distance <= 300 && _direction < 0) ? _direction : -_direction;
                SetAhead((e.Distance / 4 + 25) * _direction);
            }

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
        }

        public override void OnWin(WinEvent e)
        {
            for (int i = 0; i < 50; i++)
            {
                TurnRight(30);
                TurnLeft(30);
            }
        }

    }
}
