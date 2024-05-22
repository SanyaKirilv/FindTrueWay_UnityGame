using System.Collections.Generic;
using UnityEngine;

namespace FTW.Game.OneStroke
{
    [CreateAssetMenu(fileName = "Level", menuName = "OneStroke-Level")]
    public class Level : BaseLevel
    {
        public int Row, Col;
        public List<Vector4> Points;
        public List<Vector2Int> Edges;
    }
}
