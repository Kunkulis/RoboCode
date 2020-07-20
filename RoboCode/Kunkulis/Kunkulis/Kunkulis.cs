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
            //---Linear Targeting---            
            double bulletPower = Math.Min((400 / e.Distance), 3);
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
            //---Linear Targeting---


            double bulletSpeed = 20 - bulletPower * 3;
            long time = (long)(e.Distance / bulletSpeed);


            //SetTurnRadarRightRadians(Utils.NormalRelativeAngle(e.BearingRadians + HeadingRadians - RadarHeadingRadians));

            //SetTurnGunRightRadians(Utils.NormalRelativeAngle(e.BearingRadians + HeadingRadians - GunHeadingRadians));

            SetTurnRight(e.Bearing + 90 - 30 * _direction);

            energyChange = previousEnemyEnergy - e.Energy;
            previousEnemyEnergy = e.Energy;

            if (e.Distance <= 300)
            {
                SetTurnRight(e.Bearing - 90);
                SetBack(100);
            }

            if (energyChange > 0 && energyChange <= 3)
            {
                _direction = (e.Distance <= 300 && _direction < 0) ? _direction : -_direction;
                SetAhead((e.Distance / 4 + 25) * _direction);
            }
            //if (GunHeat == 0 && Math.Abs(GunTurnRemaining) < 10)
            //{
            //    SetFire(bulletPower);
            //}
        }

        public override void OnHitWall(HitWallEvent e)
        {
            SetTurnRight(e.Bearing + 135);
            SetAhead(200);
        }
    }
   
}
