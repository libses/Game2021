using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class IntedMap
    {
        public Block[,] intMap;
        public IntedMap(Block[,] Map, int splitting)
        {
            intMap = new Block[Map.GetLength(0) * splitting, Map.GetLength(1) * splitting];
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    for (int i = 0; i < splitting; i++)
                    {
                        for (int j = 0; j < splitting; j++)
                        {
                            intMap[x * splitting + i, y * splitting + j] = Map[x, y];
                        }
                    }
                }
            }
        }
    }
}
