using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FTW.Core;
using FTW.Data.Enums;

namespace FTW.UI
{
    public class ThemeElement : MonoBehaviour
    {
        public float MoveTime;
        [SerializeField] private List<Color32> colors;

        private Color32 target;

        private void OnEnable()
        {
            SettingsController.OnThemeChanged += Change;
        }

        private void OnDisable()
        {
            SettingsController.OnThemeChanged -= Change;
        }

        private void Change(Theme theme)
        {
            target = colors[(int)theme];

            if (TryGetComponent(out Image image))
            {
                _ = StartCoroutine(ChangeCoroutine(MoveTime, target, image));
            }

            if (TryGetComponent(out Text text))
            {
                _ = StartCoroutine(ChangeCoroutine(MoveTime, target, text));
            }
        }


        private IEnumerator ChangeCoroutine(float time, Color32 target, Image image)
        {
            float elapsedTime = 0;

            while (elapsedTime < time)
            {
                image.color = Color32.LerpUnclamped(image.color, target, elapsedTime / time);
                elapsedTime += 0.125f;//Time.fixedDeltaTime;

                yield return null;
            }
            image.color = target;
        }

        private IEnumerator ChangeCoroutine(float time, Color32 target, Text text)
        {
            float elapsedTime = 0;

            while (elapsedTime < time)
            {
                text.color = Color32.LerpUnclamped(text.color, target, elapsedTime / time);
                elapsedTime += 0.125f;//Time.deltaTime;

                yield return null;
            }
            text.color = target;
        }
    }
}
