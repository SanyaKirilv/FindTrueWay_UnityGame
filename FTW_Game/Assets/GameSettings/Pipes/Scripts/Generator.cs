using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace FTW.Game.Pipes
{
    public class Generator : MonoBehaviour
    {
        public static Generator Instance;

        [SerializeField] private Level level;
        [SerializeField] private SpawnCell cellPrefab;

        [SerializeField] private int row, col;

        private bool hasGameFinished;
        private SpawnCell[,] pipes;
        private List<SpawnCell> startPipes;

        private void Awake()
        {
            Instance = this;
            hasGameFinished = false;
            CreateLevelData();
            SpawnLevel();
        }

        private void CreateLevelData()
        {
            if (level.Column == col && level.Row == row)
            {
                return;
            }

            level.Row = row;
            level.Column = col;
            level.Data = new List<int>();

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    level.Data.Add(0);
                }
            }
        }

        private void SpawnLevel()
        {
            pipes = new SpawnCell[level.Row, level.Column];
            startPipes = new List<SpawnCell>();

            for (int i = 0; i < level.Row; i++)
            {
                for (int j = 0; j < level.Column; j++)
                {
                    Vector2 spawnPos = new(j + 0.5f, i + 0.5f);
                    SpawnCell tempPipe = Instantiate(cellPrefab);
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
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                pipes[row, col].Init(0);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                pipes[row, col].Init(1);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                pipes[row, col].Init(2);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                pipes[row, col].Init(3);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                pipes[row, col].Init(4);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                pipes[row, col].Init(5);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                pipes[row, col].Init(6);
            }

            _ = StartCoroutine(ShowHint());
        }

        private IEnumerator ShowHint()
        {
            yield return new WaitForSeconds(0.1f);
            ResetStartPipe();
            CheckFill();
            CheckWin();
            SaveData();
        }

        private void CheckFill()
        {
            for (int i = 0; i < level.Row; i++)
            {
                for (int j = 0; j < level.Column; j++)
                {
                    SpawnCell tempPipe = pipes[i, j];
                    if (tempPipe.PipeType != 0)
                    {
                        tempPipe.IsFilled = false;
                    }
                }
            }

            Queue<SpawnCell> check = new();
            HashSet<SpawnCell> finished = new();
            foreach (SpawnCell pipe in startPipes)
            {
                check.Enqueue(pipe);
            }

            while (check.Count > 0)
            {
                SpawnCell pipe = check.Dequeue();
                _ = finished.Add(pipe);
                List<SpawnCell> connected = pipe.ConnectedPipes();
                foreach (SpawnCell connectedPipe in connected)
                {
                    if (!finished.Contains(connectedPipe))
                    {
                        check.Enqueue(connectedPipe);
                    }
                }
            }

            foreach (SpawnCell filled in finished)
            {
                filled.IsFilled = true;
            }

            for (int i = 0; i < level.Row; i++)
            {
                for (int j = 0; j < level.Column; j++)
                {
                    SpawnCell tempPipe = pipes[i, j];
                    tempPipe.UpdateFilled();
                }
            }

        }

        private void ResetStartPipe()
        {
            startPipes = new List<SpawnCell>();

            for (int i = 0; i < level.Row; i++)
            {
                for (int j = 0; j < level.Column; j++)
                {
                    if (pipes[i, j].PipeType == 1)
                    {
                        startPipes.Add(pipes[i, j]);
                    }
                }
            }
        }

        private void SaveData()
        {
            for (int i = 0; i < level.Row; i++)
            {
                for (int j = 0; j < level.Column; j++)
                {
                    level.Data[(i * level.Column) + j] = pipes[i, j].PipeData;
                }
            }

            EditorUtility.SetDirty(level);
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
        }
    }
}
#endif
