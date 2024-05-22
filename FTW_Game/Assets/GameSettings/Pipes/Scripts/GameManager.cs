using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTW.Game.Pipes
{
    public class GameManager : BaseGame
    {
        public static GameManager Instance;

        [SerializeField] private Level level;
        [SerializeField] private Pipe cellPrefab;

        private bool hasGameFinished;
        private Pipe[,] pipes;
        private List<Pipe> startPipes;

        public override void SetLevel(ScriptableObject level)
        {
            this.level = (Level)level;
            StartGame();
        }

        private void StartGame()
        {
            Instance = this;
            hasGameFinished = false;
            SpawnLevel();
        }

        private void SpawnLevel()
        {
            pipes = new Pipe[level.Row, level.Column];
            startPipes = new List<Pipe>();

            for (int i = 0; i < level.Row; i++)
            {
                for (int j = 0; j < level.Column; j++)
                {
                    Vector2 spawnPos = new(j + 0.5f, i + 0.5f);
                    Pipe tempPipe = Instantiate(cellPrefab);
                    tempPipe.transform.SetParent(transform);
                    tempPipe.transform.position = spawnPos;
                    tempPipe.Init(level.Data[(i * level.Column) + j]);
                    pipes[i, j] = tempPipe;
                    if (tempPipe.PipeType == 1)
                    {
                        startPipes.Add(tempPipe);
                    }
                }
            }

            Camera.main.orthographicSize = Mathf.Max(level.Row, level.Column) + 2f;
            Vector3 cameraPos = Camera.main.transform.position;
            cameraPos.x = level.Column * 0.5f;
            cameraPos.y = level.Row * 0.5f;
            Camera.main.transform.position = cameraPos;

            _ = StartCoroutine(ShowHint());
            OnLevelStarted?.Invoke();
        }

        private void Update()
        {
            if (hasGameFinished)
            {
                return;
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int row = Mathf.FloorToInt(mousePos.y);
            int col = Mathf.FloorToInt(mousePos.x);
            if (row < 0 || col < 0)
            {
                return;
            }

            if (row >= level.Row)
            {
                return;
            }

            if (col >= level.Column)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                pipes[row, col].UpdateInput();
                OnAudioPlay?.Invoke(0);
                OnAudioPlay?.Invoke(1);
                _ = StartCoroutine(ShowHint());
            }
        }

        private IEnumerator ShowHint()
        {
            yield return new WaitForSeconds(0.1f);
            CheckFill();
            CheckWin();
        }

        private void CheckFill()
        {
            for (int i = 0; i < level.Row; i++)
            {
                for (int j = 0; j < level.Column; j++)
                {
                    Pipe tempPipe = pipes[i, j];
                    if (tempPipe.PipeType != 0)
                    {
                        tempPipe.IsFilled = false;
                    }
                }
            }

            Queue<Pipe> check = new();
            HashSet<Pipe> finished = new();
            foreach (Pipe pipe in startPipes)
            {
                check.Enqueue(pipe);
            }

            while (check.Count > 0)
            {
                Pipe pipe = check.Dequeue();
                _ = finished.Add(pipe);
                List<Pipe> connected = pipe.ConnectedPipes();
                foreach (Pipe connectedPipe in connected)
                {
                    if (!finished.Contains(connectedPipe))
                    {
                        check.Enqueue(connectedPipe);
                    }
                }
            }

            foreach (Pipe filled in finished)
            {
                filled.IsFilled = true;
            }

            for (int i = 0; i < level.Row; i++)
            {
                for (int j = 0; j < level.Column; j++)
                {
                    Pipe tempPipe = pipes[i, j];
                    tempPipe.UpdateFilled();
                }
            }

        }

        private void CheckWin()
        {
            for (int i = 0; i < level.Row; i++)
            {
                for (int j = 0; j < level.Column; j++)
                {
                    if (!pipes[i, j].IsFilled)
                    {
                        return;
                    }
                }
            }

            hasGameFinished = true;
            _ = StartCoroutine(GameFinished());
        }

        private IEnumerator GameFinished()
        {
            yield return new WaitForSeconds(.5f);
            CoinAdd?.Invoke(level.Cubes);
            OnLevelCompleted?.Invoke();
            OnAudioPlay?.Invoke(2);
        }
    }
}
