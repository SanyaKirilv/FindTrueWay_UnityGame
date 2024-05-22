using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FTW.Core;
using FTW.Data.Enums;

namespace FTW.UI
{
    public class LanguageElement : MonoBehaviour
    {
        [SerializeField] private List<string> translates;

        private void OnEnable()
        {
            SettingsController.OnLanguageChanged += Change;
        }

        private void OnDisable()
        {
            SettingsController.OnLanguageChanged -= Change;
        }

        private void Change(Language language)
        {
            GetComponent<Text>().text = translates[(int)language];
        }
    }
}
