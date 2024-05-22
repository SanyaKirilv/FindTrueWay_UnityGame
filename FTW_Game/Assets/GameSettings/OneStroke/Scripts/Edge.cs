using UnityEngine;

namespace FTW.Game.OneStroke
{
    public class Edge : MonoBehaviour
    {
        [HideInInspector] public bool Filled;

        [SerializeField] private LineRenderer line;
        [SerializeField] private Gradient startColor;
        [SerializeField] private Gradient activeColor;

        public void Init(Vector3 start, Vector3 end)
        {
            line.positionCount = 2;
            line.SetPosition(0, start);
            line.SetPosition(1, end);
            line.colorGradient = startColor;
            Filled = false;
        }

        public void Add()
        {
            Filled = true;
            line.colorGradient = activeColor;
        }
    }
}
