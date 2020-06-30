using System;
using Robocode;

namespace ARE
{
    public class Kunkulis : Robot
    {
        Random rnd = new Random();
        public override void Run()
        {
            double height = this.BattleFieldHeight;
            double width = this.BattleFieldWidth;



            while (true)
            {
                
                int turn = rnd.Next(10, 181);
                double horiz = rnd.Next(0, Convert.ToInt32(width));
                double vertic = rnd.Next(0, Convert.ToInt32(height));
                
                int direction = rnd.Next(1, 3);

                if (direction == 1)
                {
                    Ahead(horiz);
                    TurnRight(turn);
                }
                else
                {
                    Ahead(vertic);
                    TurnLeft(turn);
                }

                // Turn the robot 90 degrees


                // Our robot will move along the borders of the battle field
                // by repeating the above two statements.
            }
        }

        public override void OnScannedRobot(ScannedRobotEvent e)
        {            
            if (Energy => 75)
            {
                Fire(1);

            }
        }



    }
}
