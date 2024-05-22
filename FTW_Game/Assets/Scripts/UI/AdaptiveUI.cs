using UnityEngine;

namespace FTW.UI
{
    public class AdaptiveUI : MonoBehaviour
    {
        [Header("Adaptive modes")]
        public Vector2 min;
        public Vector2 max;

        private void Awake()
        {
            UpdateArea();
        }

        private void UpdateArea()
        {
            Rect safeArea = Screen.safeArea;
            RectTransform myRectTransform = GetComponent<RectTransform>();

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x = min.x == 1 ? anchorMin.x /= Screen.width : 0;
            anchorMin.y = min.y == 1 ? anchorMin.y /= Screen.height : 0;
            anchorMax.x = max.x == 1 ? anchorMax.x /= Screen.width : 1;
            anchorMax.y = max.y == 1 ? anchorMax.y /= Screen.height : 1;

            myRectTransform.anchorMin = anchorMin;
            myRectTransform.anchorMax = anchorMax;
        }
    }
}
