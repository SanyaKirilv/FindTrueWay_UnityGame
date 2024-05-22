using System.Collections.Generic;
using UnityEngine;


namespace FTW.Game.Pipes
{
    [CreateAssetMenu(fileName = "Level", menuName = "Pipes-Level")]
    public class Level : BaseLevel
    {
        public int Row;
        public int Column;
        public List<int> Data;
    }
}
