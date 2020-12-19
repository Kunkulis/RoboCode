# RoboCode
https://robowiki.net/wiki/CirclingBot
https://robowiki.net/wiki/SuperCrazy
https://www.ibm.com/developerworks/library/j-dodge/index.html
https://robowiki.net/wiki/Robocode/My_First_Robot
https://robowiki.net/wiki/GuessFactor_Targeting_Tutorial
https://robowiki.net/wiki/SuperRamFire
http://mark.random-article.com/weber/java/robocode/lesson4.html#anatomy
https://www.problemsphysics.com/mechanics/projectile/projectile_equation.html
https://www.khanacademy.org/science/physics/two-dimensional-motion/two-dimensional-projectile-mot/a/what-are-velocity-components

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    static class Program
    {
        static void Main(string[] args)
        {
            Input input = new Input();
            List<string> data = input.raw;
            List<char[]> layout = new List<char[]>();
            foreach (var item in data)
            {
                var row = item.ToArray();
                layout.Add(row);
            }
            List<char[]> newLayout = new List<char[]>();

            foreach (var item in layout)
            {
                var row = item.ToArray();
                newLayout.Add(row);
            }

            List<(int r, int c)> direction = new List<(int r, int c)>()
            {
                (-1,-1),(-1,0),(-1,1),
                (0,-1),(0,1),
                (1,-1),(1,0),(1,1)
            };
            bool doMatch = GetMatch(newLayout, layout);

            int loops = 0;
            do
            {
                for (int i = 0; i < layout.Count; i++)
                {
                    for (int j = 0; j < layout[i].Count(); j++)
                    {
                        var curCord = (i, j);
                        char currentSeat = layout[i][j];
                        List<char> aroundSeat = new List<char>();
                        foreach (var item in direction)
                        {
                            var newCord = (curCord.i + item.r, curCord.j + item.c);
                            if (newCord.Item1 < 0 || newCord.Item2 < 0 || newCord.Item1 == layout.Count || newCord.Item2 == layout[i].Count())
                            {
                                continue;
                            }
                            aroundSeat.Add(layout[newCord.Item1][newCord.Item2]);
                        }
                        if (currentSeat.Equals('L'))
                        {
                            if (!aroundSeat.Contains('#'))
                            {
                                newLayout[i][j] = '#';
                            }
                        }
                        else if (currentSeat.Equals('#'))
                        {
                            if (aroundSeat.Count(x => x == '#') >= 4)
                            {
                                newLayout[i][j] = 'L';
                            }
                        }
                    }
                }
                doMatch = GetMatch(newLayout, layout);
                if (!doMatch)
                {
                    loops++;
                    layout.Clear();
                    layout = newLayout.GetClone();

                }

            } while (!doMatch);

            int takenSeats = 0;
            for (int i = 0; i < newLayout.Count; i++)
            {
                for (int j = 0; j < newLayout[i].Count(); j++)
                {
                    if (newLayout[i][j] == '#')
                    {
                        takenSeats++;
                    }
                }
            }
            Console.WriteLine(takenSeats);
            Console.ReadKey();

        }

        private static bool GetMatch(List<char[]> newLayout, List<char[]> layout)
        {
            int notMatching = 0;
            for (int i = 0; i < newLayout.Count; i++)
            {
                for (int j = 0; j < newLayout[i].Count(); j++)
                {
                    if (newLayout[i][j] != layout[i][j])
                    {
                        notMatching++;
                    }
                }
            }
            if (notMatching > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static List<char[]> GetClone(this List<char[]> newLayout)
        {
            return newLayout.Select(item => (char[])item.Clone())
                    .ToList();
        }
    }
}

