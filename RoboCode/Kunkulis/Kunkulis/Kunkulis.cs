using System;
using Robocode;
using System.Drawing;
using Robocode.Util;

namespace ARE
{
    public class Kunkulis : AdvancedRobot
    {
        Random rnd = new Random();
        double previousEnemyEnergy = 100;
        double energyChange = 0.0;
        int _direction = 1;        

        public override void Run()
        {
            SetColors(Color.DarkOrange, Color.Black, Color.DimGray, Color.Firebrick, Color.WhiteSmoke);

            double height = this.BattleFieldHeight;
            double width = this.BattleFieldWidth;

            while (true)
            {
                TurnRadarRight(Double.PositiveInfinity);


                //int turn = rnd.Next(0, 120);
                //double forward = rnd.Next(100, 500);
                //double revers = rnd.Next(100, 500);

                //int direction = rnd.Next(1, 3);
                //int trn = rnd.Next(1, 3);



                //if (direction == 2)
                //{
                //    SetAhead(forward);
                //}
                //else
                //{
                //    SetBack(revers);
                //}

                //if (trn == 2)
                //{
                //    SetTurnLeft(turn);
                //}
                //else
                //{
                //    SetTurnRight(turn);
                //}

                //Execute();
                //while (DistanceRemaining > 0 && TurnRemaining > 0)
                //{
                //    Execute();
                //}

            }
        }


        public override void OnScannedRobot(ScannedRobotEvent e)
        {
            double firePower = Math.Min(400 / e.Distance, 3);
            double bulletSpeed = 20 - firePower * 3;
            long time = (long)(e.Distance / bulletSpeed);

            double absBearing = e.BearingRadians + HeadingRadians;
            double laterVelocity = e.Velocity * Math.Sin(e.HeadingRadians - absBearing);
            double gunTurnAmt;            

            Console.WriteLine("absBearing=" + absBearing + " laterVel=" + laterVelocity);

            SetTurnRadarRightRadians(Utils.NormalRelativeAngle(e.BearingRadians + HeadingRadians - RadarHeadingRadians));
            //SetTurnRadarLeftRadians(RadarTurnRemainingRadians);
            Console.WriteLine("RadarTurnRemainging=" + RadarTurnRemainingRadians);
            SetTurnGunRightRadians(Utils.NormalRelativeAngle(e.BearingRadians + HeadingRadians - GunHeadingRadians));

            SetTurnRight(e.Bearing + 90 - 30 * _direction);

            energyChange = previousEnemyEnergy - e.Energy;
            previousEnemyEnergy = e.Energy;

            //if (e.Distance > 150)
            //{
            //    Console.WriteLine("Distance=" + e.Distance);
            //    gunTurnAmt = Utils.NormalRelativeAngle(absBearing - GunHeadingRadians + laterVelocity / 22);//amount to turn our gun, lead just a little bit
            //    Console.WriteLine("gunTurnAmt=" + gunTurnAmt);
            //    TurnGunRightRadians(gunTurnAmt);
            //    TurnRightRadians(Utils.NormalRelativeAngle(absBearing - GunHeadingRadians + laterVelocity / Velocity));//drive towards the enemies predicted future location
            //    Console.WriteLine("TurnRightRadians=" + Utils.NormalRelativeAngle(absBearing - GunHeadingRadians + laterVelocity / Velocity));
            //    SetAhead((e.Distance - 140) * _direction);
            //    Console.WriteLine("Ahead=" + (e.Distance - 140) * _direction);
            //    SetFire(firePower);
            //}
            //else
            //{
            //    Console.WriteLine("Distance=" + e.Distance);
            //    gunTurnAmt = Utils.NormalRelativeAngle(absBearing - GunHeadingRadians + laterVelocity / 15);//amount to turn our gun, lead just a little bit
            //    Console.WriteLine("gunTurnAmt=" + gunTurnAmt);
            //    TurnGunRightRadians(gunTurnAmt);
            //    TurnLeft(-90 - e.Bearing);//turn perpendicular to the enemy
            //    Console.WriteLine("TurnLeft=" + (-90 - e.Bearing));
            //    SetAhead((e.Distance - 140) * _direction);
            //    Console.WriteLine("Ahead=" + (e.Distance - 140) * _direction);
            //    SetFire(firePower);
            //}

            Console.WriteLine("Andrejs part");

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
            if (GunHeat == 0 && Math.Abs(GunTurnRemaining) < 10)
            {                 
                SetFire(firePower);
            }
        }

        public override void OnHitWall(HitWallEvent e)
        {
            SetTurnRight(e.Bearing + 135);
            SetAhead(200);
        }
        public override void OnHitByBullet(HitByBulletEvent e)
        {
            SetTurnRight(e.Bearing + 135);
            SetAhead(300);
        }

    }
}
