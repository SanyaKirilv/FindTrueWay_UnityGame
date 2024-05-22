using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTW.Game;
using FTW.UI;

namespace FTW.Core
{
    public class GameController : MonoBehaviour
    {
        public static Action<string> OnLevelQuit;
        public static Action<int> OnAudioPlay;
        public static Action OnAdsLaunch;
        public static Action UpdateThemeElements;

        [Header("View's")]
        [SerializeField] private GameView gameView;
        [SerializeField] private MoveElement menuView;
        [SerializeField] private CompleteView completeView;

        [Header("Game's data")]
        public List<GameData> GameDatas;

        private GameObject game;
        private int currentGame;
        private int currentLevel;
        private int time;

        private GameData CurrentGame => GameDatas[currentGame];
        private BaseLevel CurrentLevel => CurrentGame.Levels[currentLevel];

        private GameManager GameManager => FindAnyObjectByType<GameManager>();
        private bool Ads => GameManager.Save.Settings.Ads;
        private List<Data.Game> Games { get => GameManager.Save.Games; set => Games = value; }

        private void OnEnable()
        {
            BaseGame.OnLevelStarted += LevelStart;
            BaseGame.OnLevelCompleted += Complete;
        }

        private void OnDisable()
        {
            BaseGame.OnLevelStarted -= LevelStart;
            BaseGame.OnLevelCompleted -= Complete;
        }

        public void UpdateView(int game, int level)
        {
            currentGame = game;
            currentLevel = level;
            gameView.Enable();
            SetupGame();
        }

        public void Complete()
        {
            int stars = time <= CurrentLevel.TimeThreeStar ? 3 : time <= CurrentLevel.TimeTwoStar ? 2 : 1;
            if (Games[currentGame].Levels[currentLevel].Stars < stars)
            {
                Games[currentGame].Levels[currentLevel].Complete(time, stars);
            }
            else
            {
                Games[currentGame].Levels[currentLevel].Complete(time, Games[currentGame].Levels[currentLevel].Stars);
            }

            if (currentLevel + 1 < Games[currentGame].Levels.Count)
            {
                Games[currentGame].Levels[currentLevel + 1].Open();
            }
            StopCoroutine(nameof(Timer));

            completeView.Enable();
            OnAudioPlay?.Invoke(1);

            for (int i = 0; i < completeView.Stars.Count; i++)
            {
                completeView.Stars[i].SetActive(stars - 1 == i);
            }
        }

        public void Leave()
        {
            StopCoroutine(nameof(Timer));
            Exit();
        }

        public void Continue()
        {
            menuView.Disable();
            OnAudioPlay?.Invoke(1);
        }

        public void Menu()
        {
            menuView.Enable();
            OnAudioPlay?.Invoke(1);
        }

        public void ClearLevel()
        {
            SetupGame();
            OnAudioPlay?.Invoke(0);
            OnAudioPlay?.Invoke(1);
        }

        public void Exit()
        {
            time = 0;
            Destroy(game);

            gameView.Disable();
            menuView.Disable();
            completeView.Disable();

            OnLevelQuit?.Invoke(CurrentGame.name);
            if (Ads)
            {
                OnAdsLaunch?.Invoke();
            }
            UpdateThemeElements?.Invoke();
        }

        private void LevelStart()
        {
            if (time != 0)
            {
                return;
            }
            _ = StartCoroutine(nameof(Timer));
        }

        private void SetupGame()
        {
            if (game != null)
            {
                Destroy(game);
            }

            game = Instantiate(CurrentGame.Prefab);
            game.GetComponent<BaseGame>().SetLevel(CurrentLevel);

            gameView.Reqs[0].UpdateView(CurrentLevel.TimeOneStar);
            gameView.Reqs[1].UpdateView(CurrentLevel.TimeTwoStar);
            gameView.Reqs[2].UpdateView(CurrentLevel.TimeThreeStar);
        }

        private IEnumerator Timer()
        {
            while (true)
            {
                time++;
                gameView.UpdateView(time);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
