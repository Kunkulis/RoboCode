using System;
using Robocode;
using System.Drawing;

namespace ARE
{
    public class Kunkulis : AdvancedRobot
    {
        Random rnd = new Random();
        double previousEnergy = 100;
        int movementDirection = 1;
        int gunDirection = 1;



        public override void Run()
        {
            SetColors(Color.LimeGreen, Color.Black, Color.DimGray);

            double height = this.BattleFieldHeight;
            double width = this.BattleFieldWidth;

            while (true)
            {

                int turn = rnd.Next(0, 120);
                double forward = rnd.Next(100, 500);
                double revers = rnd.Next(100, 500);

                int direction = rnd.Next(1, 3);
                int trn = rnd.Next(1, 3);

                if (direction == 2)
                {
                    SetAhead(forward);
                }
                else
                {
                    SetBack(revers);
                }

                if (trn == 2)
                {
                    SetTurnLeft(turn);
                }
                else
                {
                    SetTurnRight(turn);
                }

                Execute();
                while (DistanceRemaining > 0 && TurnRemaining > 0)
                {
                    Execute();
                }

            }
        }

        public override void OnScannedRobot(ScannedRobotEvent e)
        {
            SetTurnRadarRight(Heading - RadarHeading + e.Bearing);
            //TurnRight(e.Bearing + 90 - 30 * movementDirection);

            //double changeInEnergy = previousEnergy - e.Energy;
            //if (changeInEnergy > 0 && changeInEnergy <= 3)
            //{
            //    movementDirection = -movementDirection;
            //    Ahead((e.Distance / 4 + 25) * movementDirection);
            //}

            //gunDirection = -gunDirection;
            //TurnGunRight(99999 * gunDirection);
            //if (Energy => 75)
            //{

            Fire(2);

            //previousEnergy = e.Energy;

            //}
        }

        public override void OnHitWall(HitWallEvent e)
        {
            SetBack(100);
            SetTurnLeft(70);
            Execute();
            while (DistanceRemaining > 0 && TurnRemaining > 0)
            {
                Execute();
            }
        }
    }
}
