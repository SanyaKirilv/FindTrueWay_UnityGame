using System;
using System.Collections.Generic;
using UnityEngine;
using FTW.UI;


namespace FTW.Core
{
    public class GameListController : MonoBehaviour
    {
        public static Action OnLevelSelected;

        [Header("View's")]
        [SerializeField] private ListView gameListView;
        [SerializeField] private ListView levelListView;

        [Header("Level's")]
        public List<LevelView> levels;

        private int selectedGame;

        private GameController GameController => FindAnyObjectByType<GameController>();
        private GameManager GameManager => FindAnyObjectByType<GameManager>();
        private Data.Resources Resources { get => GameManager.Save.Resources; set => Resources = value; }
        private List<Data.Game> Games { get => GameManager.Save.Games; set => Games = value; }

        private void OnEnable()
        {
            PageController.OnShopCalled += Close;
            PageController.OnGameListCalled += UpdateView;
            PageController.OnSettingsCalled += Close;

            LevelView.OnLevelSelected += SelectLevel;
            GameController.OnLevelQuit += SelectGame;
        }

        private void OnDisable()
        {
            PageController.OnShopCalled -= Close;
            PageController.OnGameListCalled -= UpdateView;
            PageController.OnSettingsCalled -= Close;

            LevelView.OnLevelSelected -= SelectLevel;
            GameController.OnLevelQuit -= SelectGame;
        }

        public void UpdateView()
        {
            gameListView.Enable();
            levelListView.Disable();
            selectedGame = -1;
        }

        public void SelectGame(string game)
        {
            selectedGame = (int)Enum.Parse(typeof(Enums.Game), game);

            gameListView.Disable();
            levelListView.Enable();

            UpdateLevels();
        }

        public void SelectLevel(int level)
        {
            GameController.UpdateView(selectedGame, level);
            OnLevelSelected?.Invoke();
            Close();
        }

        public void Back()
        {
            gameListView.Enable();
            levelListView.Disable();
        }

        private void Close()
        {
            gameListView.Disable();
            levelListView.Disable();
        }

        private void UpdateLevels()
        {
            bool isAvailable = Resources.Energy > 0;
            for (int i = 0; i < levels.Count; i++)
            {
                if (i < Games[selectedGame].Levels.Count)
                {
                    levels[i].SetUpLevel(i, Games[selectedGame].Levels[i].State, Games[selectedGame].Levels[i].Stars, isAvailable);
                }
                else
                {
                    levels[i].SetUpLevel(i, "Blocked", 0, isAvailable);
                }
            }
        }
    }
}
