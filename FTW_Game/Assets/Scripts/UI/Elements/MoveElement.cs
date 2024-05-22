using System.Collections;
using UnityEngine;

namespace FTW.UI
{
    public class MoveElement : MonoBehaviour
    {
        public float MoveTime;
        public Vector2 PositionAddiction;

        private Vector2 target;
        private Vector2 enablePosition;
        private Vector2 disablePosition;

        private Vector2 AnchoredPosition
        {
            get => GetComponent<RectTransform>().anchoredPosition;
            set => GetComponent<RectTransform>().anchoredPosition = value;
        }

        private void Awake()
        {
            enablePosition = new Vector2(AnchoredPosition.x, AnchoredPosition.y);
            disablePosition = new Vector2(enablePosition.x + PositionAddiction.x,
                enablePosition.y + PositionAddiction.y);
            target = disablePosition;
            AnchoredPosition = target;
        }

        public void MoveX(float x)
        {
            Change(new(x, target.y));
        }

        public void MoveY(float y)
        {
            Change(new(target.x, y));
        }

        public void Disable()
        {
            Change(disablePosition);
        }

        public void Enable()
        {
            Change(enablePosition);
        }

        public void Toggle(bool state)
        {
            Change(state ? enablePosition : disablePosition);
        }

        private void Change(Vector2 nextPosition)
        {
            target = nextPosition;
            _ = StartCoroutine(ChangeCoroutine(MoveTime / 8, target, AnchoredPosition));
        }

        private IEnumerator ChangeCoroutine(float time, Vector2 targetPosition, Vector2 currentPosition)
        {
            float elapsedTime = 0;

            while (elapsedTime < time)
            {
                AnchoredPosition = Vector2.Lerp(currentPosition, targetPosition, elapsedTime / time);
                elapsedTime += Time.deltaTime;

                yield return null;
            }
            AnchoredPosition = target;
        }
    }
}
