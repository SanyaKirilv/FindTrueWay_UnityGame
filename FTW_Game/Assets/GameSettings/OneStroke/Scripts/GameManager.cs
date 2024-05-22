using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTW.Game.OneStroke
{
    public class GameManager : BaseGame
    {
        [SerializeField] private Level level;
        [SerializeField] private Edge edgePrefab;
        [SerializeField] private Point pointPrefab;
        [SerializeField] private LineRenderer highlight;

        private Dictionary<int, Point> points;
        private Dictionary<Vector2Int, Edge> edges;
        private Point startPoint, endPoint;
        private int currentId;
        private bool hasGameFinished;

        public override void SetLevel(ScriptableObject level)
        {
            this.level = (Level)level;
            StartGame();
        }

        private void StartGame()
        {
            hasGameFinished = false;
            points = new Dictionary<int, Point>();
            edges = new Dictionary<Vector2Int, Edge>();
            highlight.gameObject.SetActive(false);
            currentId = -1;
            SpawnLevel();
        }

        private void SpawnLevel()
        {
            Vector3 camPos = Camera.main.transform.position;
            camPos.x = level.Col * 0.5f;
            camPos.y = (level.Row * 0.5f) + 0.5f;
            Camera.main.transform.position = camPos;
            Camera.main.orthographicSize = Mathf.Max(level.Col, level.Row) + 2f;

            for (int i = 0; i < level.Points.Count; i++)
            {
                Vector4 posData = level.Points[i];
                Vector3 spawnPos = new(posData.x, posData.y, posData.z);
                int id = (int)posData.w;
                points[id] = Instantiate(pointPrefab);
                points[id].transform.SetParent(this.transform);
                points[id].Init(spawnPos, id);
            }

            for (int i = 0; i < level.Edges.Count; i++)
            {
                Vector2Int normal = level.Edges[i];
                Vector2Int reversed = new(normal.y, normal.x);
                Edge spawnEdge = Instantiate(edgePrefab);
                spawnEdge.transform.SetParent(this.transform);
                edges[normal] = spawnEdge;
                edges[reversed] = spawnEdge;
                spawnEdge.Init(points[normal.x].Position, points[normal.y].Position);
            }

            OnLevelStarted?.Invoke();
        }

        private void Update()
        {
            if (hasGameFinished)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new(mousePos.x, mousePos.y);
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (!hit)
                {
                    return;
                }

                startPoint = hit.collider.gameObject.GetComponent<Point>();
                highlight.gameObject.SetActive(true);
                highlight.positionCount = 2;
                highlight.SetPosition(0, startPoint.Position);
                highlight.SetPosition(1, startPoint.Position);
                OnAudioPlay?.Invoke(0);
            }
            else if (Input.GetMouseButton(0) && startPoint != null)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new(mousePos.x, mousePos.y);
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit)
                {
                    endPoint = hit.collider.gameObject.GetComponent<Point>();
                }
                highlight.SetPosition(1, mousePos2D);
                if (startPoint == endPoint || endPoint == null)
                {
                    return;
                }
                OnAudioPlay?.Invoke(0);

                if (IsStartAdd())
                {
                    currentId = endPoint.Id;
                    edges[new Vector2Int(startPoint.Id, endPoint.Id)].Add();
                    startPoint = endPoint;
                    highlight.SetPosition(0, startPoint.Position);
                    highlight.SetPosition(1, startPoint.Position);
                    OnAudioPlay?.Invoke(0);
                }
                else if (IsEndAdd())
                {
                    currentId = endPoint.Id;
                    edges[new Vector2Int(startPoint.Id, endPoint.Id)].Add();
                    startPoint = endPoint;
                    highlight.SetPosition(0, startPoint.Position);
                    highlight.SetPosition(1, startPoint.Position);
                    OnAudioPlay?.Invoke(0);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                highlight.gameObject.SetActive(false);
                startPoint = null;
                endPoint = null;
                CheckWin();
            }
        }

        private bool IsStartAdd()
        {
            if (currentId != -1)
            {
                return false;
            }

            Vector2Int edge = new(startPoint.Id, endPoint.Id);
            if (!edges.ContainsKey(edge))
            {
                return false;
            }

            return true;
        }

        private bool IsEndAdd()
        {
            if (currentId != startPoint.Id)
            {
                return false;
            }

            Vector2Int edge = new(endPoint.Id, startPoint.Id);
            if (edges.TryGetValue(edge, out Edge result))
            {
                if (result == null || result.Filled)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        private void CheckWin()
        {
            foreach (KeyValuePair<Vector2Int, Edge> item in edges)
            {
                if (!item.Value.Filled)
                {
                    return;
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
