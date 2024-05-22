using System;
using UnityEngine;
using FTW.UI;
using FTW.Data;
using FTW.Data.Enums;
using System.Collections.Generic;


namespace FTW.Core
{
    public class SettingsController : MonoBehaviour
    {
        public static Action<Theme> OnThemeChanged;
        public static Action<Language> OnLanguageChanged;
        public static Action<bool> OnAudioChanged;
        public static Action OnChangeCalled;

        [Header("View")]
        [SerializeField] private SettingsView settingsView;

        private int complitedCount;
        private int starsCount;
        private int levelsCount;

        private GameManager GameManager => FindAnyObjectByType<GameManager>();
        private Player Player { get => GameManager.Save.Player; set => Player = value; }
        private Settings Settings { get => GameManager.Save.Settings; set => Settings = value; }
        private List<Data.Game> Games { get => GameManager.Save.Games; set => Games = value; }
        private AuthController AuthController => GetComponent<AuthController>();
        private Sprite Photo => AuthController.DefaultPhotos[Player.Photo];

        private void ApplySettings()
        {
            ChangeTheme(Settings.Theme.ToString());
            ChangeLanguage(Settings.Language.ToString());
            ChangeAudio(Settings.Audio);
        }

        private void OnEnable()
        {
            GameManager.OnAuthCalled += ApplySettings;
            PageController.OnShopCalled += settingsView.Disable;
            PageController.OnGameListCalled += settingsView.Disable;
            PageController.OnSettingsCalled += UpdateView;
            GameController.UpdateThemeElements += ApplySettings;
            ShopController.OnOffAds += ChangeAds;
        }

        private void OnDisable()
        {
            GameManager.OnAuthCalled -= ApplySettings;
            PageController.OnShopCalled -= settingsView.Disable;
            PageController.OnGameListCalled -= settingsView.Disable;
            PageController.OnSettingsCalled -= UpdateView;
            GameController.UpdateThemeElements -= ApplySettings;
            ShopController.OnOffAds -= ChangeAds;
        }

        public void UpdateView()
        {
            settingsView.Enable();
            settingsView.Photo.sprite = Photo;
            settingsView.Name.text = Player.Name;

            settingsView.Registration.text = Player.RegistrationDate;

            GetGamesData();
            settingsView.Level.text = $"{complitedCount}/{levelsCount}";
            settingsView.Star.text = $"{starsCount}/{levelsCount * 3}";

            ChangeTheme(Settings.Theme.ToString());
            ChangeLanguage(Settings.Language.ToString());
            ChangeAudio(Settings.Audio);
        }

        public void ChangeData()
        {
            settingsView.Disable();
            OnChangeCalled?.Invoke();
        }

        public void ChangeTheme(string theme)
        {
            Settings.Theme = (Theme)Enum.Parse(typeof(Theme), theme);
            settingsView.MoveTheme.MoveX(8 + (96 * (int)Settings.Theme));
            OnThemeChanged?.Invoke(Settings.Theme);
        }

        public void ChangeLanguage(string language)
        {
            Settings.Language = (Language)Enum.Parse(typeof(Language), language);
            settingsView.MoveLanguage.MoveX(8 + (96 * (int)Settings.Language));
            OnLanguageChanged?.Invoke(Settings.Language);
        }

        public void ChangeAudio(bool audio)
        {
            Settings.Audio = audio;
            settingsView.MoveAudio.MoveX(8 + (96 * (audio ? 0 : 1)));
            OnAudioChanged?.Invoke(audio);
        }

        public void ChangeAds(bool ads)
        {
            Settings.Ads = ads;
        }

        private void GetGamesData()
        {
            complitedCount = 0;
            levelsCount = 0;
            starsCount = 0;
            foreach (Data.Game game in Games)
            {
                foreach (Level level in game.Levels)
                {
                    if (level.State == "Completed")
                    {
                        complitedCount++;
                        starsCount += level.Stars;
                    }
                    levelsCount++;
                }
            }
        }
    }
}
