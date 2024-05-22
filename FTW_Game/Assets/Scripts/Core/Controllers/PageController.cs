using System;
using UnityEngine;
using FTW.UI;


namespace FTW.Core
{
    public class PageController : MonoBehaviour
    {
        public static Action OnShopCalled;
        public static Action OnGameListCalled;
        public static Action OnSettingsCalled;

        public MoveElement Background;

        public MoveElement Move => GetComponent<MoveElement>();

        private void Awake()
        {
            Background.Enable();
        }

        private void OnEnable()
        {
            GameManager.OnAuthCalled += Move.Disable;
            SettingsController.OnChangeCalled += Move.Disable;
            GameListController.OnLevelSelected += ShowGame;
            AuthController.OnAuthCompleted += Show;
            GameController.OnLevelQuit += HideGame;
        }

        private void OnDisable()
        {
            GameManager.OnAuthCalled -= Move.Disable;
            SettingsController.OnChangeCalled -= Move.Disable;
            GameListController.OnLevelSelected -= ShowGame;
            AuthController.OnAuthCompleted -= Show;
            GameController.OnLevelQuit -= HideGame;
        }

        public void Shop()
        {
            OnShopCalled?.Invoke();
        }

        public void Games()
        {
            OnGameListCalled?.Invoke();
        }

        public void Settings()
        {
            OnSettingsCalled?.Invoke();
        }

        private void ShowGame()
        {
            Background.Disable();
            Move.Disable();
        }

        private void HideGame(string name)
        {
            Background.Enable();
            Move.Enable();
        }

        private void Show()
        {
            Move.Enable();
            Games();
        }
    }
}
