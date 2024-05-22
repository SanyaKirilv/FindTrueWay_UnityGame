using UnityEngine;

namespace FTW.Game.Fill
{
    public class Cell : MonoBehaviour
    {
        [HideInInspector] public bool Blocked;
        [HideInInspector] public bool Filled;

        [SerializeField] private Color blockedColor;
        [SerializeField] private Color emptyColor;
        [SerializeField] private Color filledColor;
        [SerializeField] private SpriteRenderer cellRenderer;

        public void Init(int fill)
        {
            Blocked = fill == 1;
            Filled = Blocked;
            cellRenderer.color = Blocked ? blockedColor : emptyColor;
        }

        public void Add()
        {
            Filled = true;
            cellRenderer.color = filledColor;
        }

        public void Remove()
        {
            Filled = false;
            cellRenderer.color = emptyColor;
        }

        public void ChangeState()
        {
            Blocked = !Blocked;
            Filled = Blocked;
            cellRenderer.color = Blocked ? blockedColor : emptyColor;
        }
    }
}
