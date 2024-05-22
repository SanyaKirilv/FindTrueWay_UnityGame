using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTW.Game.NumberLink
{
    public class GameManager : BaseGame
    {
        public static GameManager Instance;

        [HideInInspector] public bool hasGameFinished;

        public float EdgeSize => cellGap + cellSize;

        [SerializeField] private Cell cellPrefab;
        [SerializeField] private SpriteRenderer bgSprite;
        [SerializeField] private SpriteRenderer highlightSprite;
        [SerializeField] private Vector2 highlightSize;
        [SerializeField] private Level level;
        [SerializeField] private float cellGap;
        [SerializeField] private float cellSize;
        [SerializeField] private float levelGap;

        private int[,] levelGrid;
        private Cell[,] cellGrid;
        private Cell startCell;
        private Vector2 startPos;

        private List<Vector2Int> Directions = new List<Vector2Int>()
        { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

        public override void SetLevel(ScriptableObject level)
        {
            this.level = (Level)level;
            StartGame();
        }

        private void StartGame()
        {
            Instance = this;
            hasGameFinished = false;
            highlightSprite.gameObject.SetActive(false);
            levelGrid = new int[level.row, level.col];
            cellGrid = new Cell[level.row, level.col];
            for (int i = 0; i < level.row; i++)
            {
                for (int j = 0; j < level.col; j++)
                {
                    levelGrid[i, j] = level.data[i * level.row + j];
                }
            }

            SpawnLevel();
        }

        private void SpawnLevel()
        {
            float width = (cellSize + cellGap) * level.col - cellGap + levelGap;
            float height = (cellSize + cellGap) * level.row - cellGap + levelGap;
            bgSprite.size = new Vector2(width, height);
            Vector3 bgPos = new Vector3(
                width / 2f - cellSize / 2f - levelGap / 2f,
                height / 2f - cellSize / 2f - levelGap / 2f, 0);
            bgSprite.transform.position = bgPos;

            Camera.main.orthographicSize = width * 1.2f;
            Camera.main.transform.position = new Vector3(bgPos.x, bgPos.y, -10f);

            Vector3 startPos = Vector3.zero;
            Vector3 rightOffset = Vector3.right * (cellSize + cellGap);
            Vector3 topOffset = Vector3.up * (cellSize + cellGap);

            for (int i = 0; i < level.row; i++)
            {
                for (int j = 0; j < level.col; j++)
                {
                    Vector3 spawnPos = startPos + j * rightOffset + i * topOffset;
                    Cell tempCell = Instantiate(cellPrefab, spawnPos, Quaternion.identity);
                    tempCell.transform.SetParent(transform);
                    tempCell.Init(i, j, levelGrid[i, j]);
                    cellGrid[i, j] = tempCell;
                    if (tempCell.Number == 0)
                    {
                        Destroy(tempCell.gameObject);
                        cellGrid[i, j] = null;
                    }
                }
            }

            for (int i = 0; i < level.row; i++)
            {
                for (int j = 0; j < level.col; j++)
                {
                    if (cellGrid[i, j] != null)
                    {
                        cellGrid[i, j].Init();
                    }
                }
            }

            OnLevelStarted?.Invoke();
        }

        private void Update()
        {
            if (hasGameFinished) return;

            if (Input.GetMouseButtonDown(0))
            {
                startCell = null;
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                startPos = mousePos2D;
                if (hit && hit.collider.TryGetComponent(out startCell))
                {
                    highlightSprite.gameObject.SetActive(true);
                    highlightSprite.size = highlightSize;
                    highlightSprite.transform.position = startCell.transform.position;
                }
                else
                {
                    hit = Physics2D.Raycast(mousePos, Vector2.left);
                    if (hit && hit.collider.TryGetComponent(out startCell))
                    {
                        startCell.RemoveEdge(0);
                    }
                    hit = Physics2D.Raycast(mousePos, Vector2.down);
                    if (hit && hit.collider.TryGetComponent(out startCell))
                    {
                        startCell.RemoveEdge(1);
                    }
                    hit = Physics2D.Raycast(mousePos, Vector2.right);
                    if (hit && hit.collider.TryGetComponent(out startCell))
                    {
                        startCell.RemoveEdge(2);
                    }
                    hit = Physics2D.Raycast(mousePos, Vector2.up);
                    if (hit && hit.collider.TryGetComponent(out startCell))
                    {
                        startCell.RemoveEdge(3);
                    }
                    startCell = null;
                }

            }
            else if (Input.GetMouseButton(0))
            {
                if (startCell == null) return;

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                Vector2 offset = mousePos2D - startPos;
                Vector2Int offsetDirection = GetDirection(offset);
                float offsetValue = GetOffset(offset, offsetDirection);
                int directionIndex = GetDirectionIndex(offsetDirection);
                Vector3 angle = new Vector3(0, 0, 90f * (directionIndex - 1));
                highlightSprite.size = new Vector2(highlightSize.x, offsetValue);
                highlightSprite.transform.eulerAngles = angle;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (startCell == null) return;

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit && hit.collider.TryGetComponent(out Cell endCell))
                {
                    if (endCell == startCell)
                    {
                        startCell.RemoveAllEdges();
                        for (int i = 0; i < 4; i++)
                        {
                            var adjacentCell = GetAdjacentCell(startCell.Row, startCell.Column, i);
                            if (adjacentCell != null)
                            {
                                int adjacentDirection = (i + 2) % 4;
                                adjacentCell.RemoveEdge(adjacentDirection);
                                adjacentCell.RemoveEdge(adjacentDirection);
                                OnAudioPlay?.Invoke(0);
                            }
                        }
                    }
                    else
                    {
                        Vector2 offset = mousePos2D - startPos;
                        Vector2Int offsetDirection = GetDirection(offset);
                        int directionIndex = GetDirectionIndex(offsetDirection);
                        if (startCell.IsValidCell(endCell, directionIndex))
                        {
                            startCell.AddEdge(directionIndex);
                            endCell.AddEdge((directionIndex + 2) % 4);
                            OnAudioPlay?.Invoke(0);
                        }
                    }
                }
                startCell = null;
                highlightSprite.gameObject.SetActive(false);
                CheckWin();
            }
        }

        private void CheckWin()
        {
            for (int i = 0; i < level.row; i++)
            {
                for (int j = 0; j < level.col; j++)
                {
                    if (cellGrid[i, j] != null && cellGrid[i, j].Number != 0) return;
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

        private int GetDirectionIndex(Vector2Int offsetDirection)
        {
            int result = 0;
            if (offsetDirection == Vector2Int.right)
            {
                result = 0;
            }
            if (offsetDirection == Vector2Int.left)
            {
                result = 2;
            }
            if (offsetDirection == Vector2Int.up)
            {
                result = 1;
            }
            if (offsetDirection == Vector2Int.down)
            {
                result = 3;
            }
            return result;
        }

        private float GetOffset(Vector2 offset, Vector2Int offsetDirection)
        {
            float result = 0;
            if (offsetDirection == Vector2Int.left || offsetDirection == Vector2Int.right)
            {
                result = Mathf.Abs(offset.x);
            }
            if (offsetDirection == Vector2Int.up || offsetDirection == Vector2Int.down)
            {
                result = Mathf.Abs(offset.y);
            }
            return result;
        }

        private Vector2Int GetDirection(Vector2 offset)
        {
            Vector2Int result = Vector2Int.zero;

            if (Mathf.Abs(offset.y) > Mathf.Abs(offset.x) && offset.y > 0)
            {
                result = Vector2Int.up;
            }
            if (Mathf.Abs(offset.y) > Mathf.Abs(offset.x) && offset.y < 0)
            {
                result = Vector2Int.down;
            }
            if (Mathf.Abs(offset.y) < Mathf.Abs(offset.x) && offset.x > 0)
            {
                result = Vector2Int.right;
            }
            if (Mathf.Abs(offset.y) < Mathf.Abs(offset.x) && offset.x < 0)
            {
                result = Vector2Int.left;
            }

            return result;
        }

        public Cell GetAdjacentCell(int row, int col, int direction)
        {
            Vector2Int currentDirection = Directions[direction];
            Vector2Int startPos = new Vector2Int(row, col);
            Vector2Int checkPos = startPos + currentDirection;
            while (IsValid(checkPos) && cellGrid[checkPos.x, checkPos.y] == null)
            {
                checkPos += currentDirection;
            }
            return IsValid(checkPos) ? cellGrid[checkPos.x, checkPos.y] : null;
        }

        public bool IsValid(Vector2Int pos)
        {
            return pos.x >= 0 && pos.y >= 0 && pos.x < level.row && pos.y < level.col;
        }
    }
}