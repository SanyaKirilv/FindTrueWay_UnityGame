using System.Collections.Generic;
using UnityEngine;

namespace FTW.Game.Fill
{
    [CreateAssetMenu(fileName = "Level", menuName = "Fill-Level")]
    public class Level : BaseLevel
    {
        public int Row;
        public int Col;
        public List<int> Data;
    }
}
