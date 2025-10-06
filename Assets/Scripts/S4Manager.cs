using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class S4Manager : MonoBehaviour
{
    private int maxPoints;
    private List< (int, int) > pointPositions;
    private const float minPos = 1f;
    private const float maxPos = 100f;
    private const int maxDistance = -1; // -1 means no limit
    [SerializeField] private GameObject pointPrefab;
    private int [ , ] distanceArray;
    [SerializeField] private LineRenderer line;
    private List<GameObject> pointObjects;
    private int startPoint = 0;
    private Dictionary<(int, int), LineRenderer> lineDict = new Dictionary<(int, int), LineRenderer>();


    private void Start()
    {
        InitArray(10);
        Dijkstra(startPoint);
        int [] prev = Dijkstra(startPoint);
        HighlightShortestPaths(startPoint,prev);

    }

    private void InitArray(int number)
    {
        MakeListPoint(number);
        CheckHasConnection();
        DrawLine();
        HighlightPoint(startPoint);

    }

    private void MakeListPoint(int maxPoints)
    {
        this.maxPoints = maxPoints;
        pointPositions = new List< (int, int) >();
        pointObjects = new List<GameObject>();

        HashSet<(int, int)> usedPoints = new HashSet<(int, int)>();

        for (int i = 0; i < maxPoints; i++)
        {
            bool check = true;
            while (check)
            {
                int x = Random.Range((int)minPos, (int)maxPos);
                int y = Random.Range((int)minPos, (int)maxPos);
                if (!usedPoints.Contains((x, y)))
                {
                    pointPositions.Add((x, y));
                    usedPoints.Add((x, y));
                    check = false;
                    GameObject obj =  Instantiate(pointPrefab, new Vector3(x, 0, y), Quaternion.identity);
                    pointObjects.Add(obj);
                }
            }
        }

        distanceArray = new int[maxPoints, maxPoints];

        for (int i = 0; i < maxPoints; i++)
        {
            for (int j = 0; j < maxPoints; j++)
            {
                if (i == j)
                {
                    distanceArray[i, j] = 0;
                }
                else
                {
                    int canGo = Random.Range(1, 4); // 1, 2, 3
                    if (canGo == 1)
                    {
                        var p1 = pointPositions[i];
                        var p2 = pointPositions[j];
                        float dist = Mathf.Sqrt((p1.Item1 - p2.Item1) * (p1.Item1 - p2.Item1) + (p1.Item2 - p2.Item2) * (p1.Item2 - p2.Item2));
                        distanceArray[i, j] = Mathf.RoundToInt(dist);
                    }
                    else
                    {
                        distanceArray[i, j] = maxDistance;
                    }
                }
            }
        }
    }

    private void CheckHasConnection()
    {
        for (int i = 0; i < maxPoints; i++)
        {
            bool hasConnection = false;
            for (int j = 0; j < maxPoints; j++)
            {
                if (i != j && distanceArray[i, j] != maxDistance)
                {
                    hasConnection = true;
                    break;
                }
            }
            if (!hasConnection)
            {
                int j;
                do
                {
                    j = Random.Range(0, maxPoints);
                } while (j == i);

                var p1 = pointPositions[i];
                var p2 = pointPositions[j];
                float dist = Mathf.Sqrt((p1.Item1 - p2.Item1) * (p1.Item1 - p2.Item1) + (p1.Item2 - p2.Item2) * (p1.Item2 - p2.Item2));
                distanceArray[i, j] = Mathf.RoundToInt(dist);
            }
        }
    }

    private void DrawLine()
    {
        lineDict = new Dictionary<(int, int), LineRenderer>();
        for (int i = 0; i < maxPoints; i++)
        {
            for (int j = i + 1; j < maxPoints; j++)
            {
                if (distanceArray[i, j] != maxDistance && i != j)
                {
                    var p1 = pointPositions[i];
                    var p2 = pointPositions[j];

                    LineRenderer lr = Instantiate(line, Vector3.zero, Quaternion.identity);
                    lr.positionCount = 2;
                    lr.SetPosition(0, new Vector3(p1.Item1, 0.1f, p1.Item2));
                    lr.SetPosition(1, new Vector3(p2.Item1, 0.1f, p2.Item2));
                    lineDict[(i, j)] = lr;
                    lineDict[(j, i)] = lr; 
                }
            }
        }
    }

    private void HighlightPoint(int i)
    {
        GameObject obj = pointObjects[i];
        foreach (var rend in obj.GetComponentsInChildren<Renderer>())
        {
            rend.material.color = Color.green;
        }
        float scale = 1.5f;
        obj.transform.localScale = new Vector3(scale, scale, scale);
    }


    private int[] Dijkstra(int start)
    {
        bool [] visited = new bool[maxPoints];
        int[] prev = new int[maxPoints];
        float [] distFromStart = new float[maxPoints];
        
        for (int i = 0; i < maxPoints; i++)
        {
            visited[i] = false;
            prev[i] = -1;
            distFromStart[i] = maxDistance;
        }
        distFromStart[start] = 0;

        for (int i = 0; i < maxPoints - 1; i++)
        {
            float minDist = maxDistance;
            int minIndex = -1;
            for (int j = 0; j < maxPoints; j++)
            {
                if (!visited[j] && distFromStart[j] <= minDist)
                {
                    minDist = distFromStart[j];
                    minIndex = j;
                }
            }
            visited[minIndex] = true;
            for (int k = 0; k < maxPoints; k++)
            {
                if (!visited[k] && distanceArray[minIndex, k] != maxDistance && distFromStart[minIndex] != maxDistance && (distFromStart[minIndex] + distanceArray[minIndex, k] < distFromStart[k]))
                {
                    distFromStart[k] = distFromStart[minIndex] + distanceArray[minIndex, k];
                    prev[k] = minIndex;
                }
            }
        }

        return prev;

    }

    private void HighlightShortestPaths(int start, int[] prev)
    {
        for (int i = 0; i < maxPoints; i++)
        {
            if (i == start || prev[i] == -1)
                continue;

            int current = i;
            while (prev[current] != -1)
            {
                int from = prev[current];
                int to = current;
                if (lineDict.TryGetValue((from, to), out LineRenderer lr))
                {
                    lr.startColor = Color.green;
                    lr.endColor = Color.green;
                }
                current = prev[current];
                if (current == start) break;
            }
        }
    }

}
