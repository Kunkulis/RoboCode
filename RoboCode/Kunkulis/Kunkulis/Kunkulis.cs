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
        int gunDirection = 1;



        public override void Run()
        {
            SetColors(Color.LimeGreen, Color.Black, Color.DimGray, Color.Firebrick, Color.WhiteSmoke);

            double height = this.BattleFieldHeight;
            double width = this.BattleFieldWidth;

            while (true)
            {

                //int turn = rnd.Next(0, 120);
                //double forward = rnd.Next(100, 500);
                //double revers = rnd.Next(100, 500);

                //int direction = rnd.Next(1, 3);
                //int trn = rnd.Next(1, 3);

                TurnRadarRight(Double.PositiveInfinity);

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
            SetTurnRadarRightRadians(Utils.NormalRelativeAngle(e.BearingRadians + HeadingRadians - RadarHeadingRadians));
            SetTurnGunRightRadians(Utils.NormalRelativeAngle(e.BearingRadians + HeadingRadians - GunHeadingRadians));

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


            if (Energy >= 75 || e.Distance <= 50)
            {
                Fire(3);
            }
            else if ((Energy < 75 && Energy > 50) || (e.Distance > 50 && e.Distance < 75))
            {
                Fire(2);
            }
            else
            {
                Fire(1);
            }
        }

        public override void OnHitWall(HitWallEvent e)
        {
            TurnRight(e.Bearing + 90);
            SetAhead(20);
        }
    }
}
