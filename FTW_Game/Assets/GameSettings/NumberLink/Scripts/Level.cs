using System.Collections.Generic;
using UnityEngine;

namespace FTW.Game.NumberLink
{
    [CreateAssetMenu(fileName = "Level", menuName = "NumberLink-Level")]

    public class Level : BaseLevel
    {
        public int row, col;
        public List<int> data;
    }
}