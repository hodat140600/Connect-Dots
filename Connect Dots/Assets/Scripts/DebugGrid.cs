using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType { 
    Empty = 0,
    Red,
    Green,
    Blue
}

public class DebugGrid : MonoBehaviour
{
    private const int SIZE = 9;

    //public event Action<int[,]> OnNewWave;

    [Header("Spheres Prefabs")]
    [SerializeField]
    private GameObject redSphere;

    [SerializeField]
    private GameObject blueSphere;

    [SerializeField]
    private GameObject greenSphere;

    private CellType[,] cells;

    private int[,] wave;

    public Vector3 ToWorldCoords(Vector2Int coord) 
    {
        return new Vector3(coord.y, 0.45f, coord.x);
    }

    private Vector2Int ToLocalCoords(Vector3 coord)
    {
        return new Vector2Int(Mathf.RoundToInt(coord.z), Mathf.RoundToInt(coord.x));
    }

    private bool InBounds(int x, int y)
    {
        return (x >= 0) && (x < SIZE) && (y >= 0) && (y < SIZE);
    }

    private List<Vector2Int> GetPath(Vector2Int from, Vector2Int to, int[,] wave)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        if(wave[from.x, from.y] == -1)
        {
            return path;
        }

        Vector2Int[] dxdy = new Vector2Int[4]
        {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1)
        };

        Vector2Int current = from;

        path.Add(current);

        while (current != to)
        {
            bool stop = true;

            for(int k = 0; k < dxdy.Length; k++)
            {
                int x = current.x + dxdy[k].x;
                int y = current.y + dxdy[k].y;

                if(InBounds(x,y) && (wave[x,y] == wave[current.x, current.y] - 1))
                {
                    current = new Vector2Int(x, y);

                    path.Insert(0, current);

                    stop = false;
                }
            }

            if (stop)
            {
                break;
            }
        }

        if(path.Count > 0)
        {
            if(path[0] != to)
            {
                path.Clear();
            }
        }

        return path;
    }

    private int[,] Wave(Vector2Int from, Vector2Int to)
    {
        int[,] wave = new int[SIZE, SIZE];

        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                wave[j, i] = cells[j, i] == CellType.Empty ? 0 : -1;
                
            }
        }

        Vector2Int[] dxdy = new Vector2Int[4]
        {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1)
        };

        int d = 1;

        wave[from.x, from.y] = d;
        
        while (true)
        {
            bool stop = true;
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if(wave[j,i] == d)
                    {
                        for(int k = 0; k < dxdy.Length; k++)
                        {
                            int x = j + dxdy[k].x;
                            int y = i + dxdy[k].y;

                            if (InBounds(x, y) && wave[x,y] == 0)
                            {
                                wave[x, y] = d + 1;

                                stop = false;
                            }
                        }
                    }
                }
            }
            //OnNewWave?.Invoke(wave);

            //yield return new WaitForSeconds(0.25f);

            d++;

            if(wave[to.x,to.y] != 0)
            {
                break;
            }
            if (stop)
            {
                break;
            }
        }
        return wave;
    }

    private List<Vector2Int> GetEmptyCoords() 
    {
        var emptyCoords = new List<Vector2Int>();

        for (int i = 0; i < SIZE; i++)
        { 
            for(int j = 0; j < SIZE; j++)
            {
                if(cells[j, i] == CellType.Empty)
                {
                    emptyCoords.Add(new Vector2Int(j,i));
                }
            }
        }

        return emptyCoords;
    }

    public void Generate(int x, int y, CellType cellType)
    {
        cells[x, y] = cellType;

        GameObject prefab = null;
        switch (cellType)
        {
            case CellType.Red:
                prefab = redSphere;
                break;
            case CellType.Green:
                prefab = greenSphere;
                break;
            case CellType.Blue:
                prefab = blueSphere;
                break;

        }
        Vector3 pos = ToWorldCoords(new Vector2Int(x,y));
        Instantiate(prefab, pos, Quaternion.identity);
    }

    public int Generate(int count) 
    {
        var emptyCoords = GetEmptyCoords();

        if (emptyCoords.Count == 0) return 0;

        count = Mathf.Min(count, emptyCoords.Count);

        var cellTypes = Enum.GetValues(typeof(CellType)).Cast<CellType>();
        int max = (int)cellTypes.Max() + 1;
        int min = (int)cellTypes.Min() + 1;

        for(int i = 0; i < count; i++)
        {
            CellType cellType = (CellType)UnityEngine.Random.Range(min, max);
            int index = UnityEngine.Random.Range(0, emptyCoords.Count);
            cells[emptyCoords[index].x, emptyCoords[index].y] = cellType;

            GameObject prefab = null;
            switch (cellType)
            {
                case CellType.Red:
                    prefab = redSphere;
                    break;
                case CellType.Green:
                    prefab = greenSphere;
                    break;
                case CellType.Blue:
                    prefab = blueSphere;
                    break;

            }
            Vector3 pos = ToWorldCoords(emptyCoords[index]);
            Instantiate(prefab, pos, Quaternion.identity);

            emptyCoords.RemoveAt(index);
        }

        return count;
    }
    
    public List<Vector3> GetPath(Vector3 from, Vector3 to)
    {
        Vector2Int _from = ToLocalCoords(from);
        Vector2Int _to = ToLocalCoords(to);

        var wave = Wave(_from, _to);
        var path = GetPath(_to, _from, wave);

        var result = path.Select(v => ToWorldCoords(v)).ToList();

        return result;
    }

    public void Move(Vector3 from, Vector3 to)
    {
        Vector2Int _from = ToLocalCoords(from);
        Vector2Int _to = ToLocalCoords(to);

        cells[_to.x, _to.y] = cells[_from.x, _from.y];
        cells[_from.x, _from.y] = CellType.Empty;
    }

    private HashSet<Vector2Int> CheckLine(Vector2Int pos, int dx, int dy)
    {
        HashSet<Vector2Int> line = new HashSet<Vector2Int>();

        for(int i = 0; i < SIZE; i++)
        {
            int x = pos.x + i * dx;
            int y = pos.y + i * dy;

            if (InBounds(x, y))
            {
                if(cells[x,y] == cells[pos.x, pos.y])
                {
                    line.Add(new Vector2Int(x, y));
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        return line;
    }

    public int DestroyLines(Vector3 pos)
    {
        Vector2Int _pos = ToLocalCoords(pos);

        HashSet<Vector2Int> destroyed = new HashSet<Vector2Int>();
        HashSet<Vector2Int> line = new HashSet<Vector2Int>();

        List<Tuple<Vector2Int, Vector2Int>> dxdy = new List<Tuple<Vector2Int, Vector2Int>>()
        {
            new Tuple<Vector2Int, Vector2Int> (new Vector2Int(1,0), new Vector2Int(-1,0)),
            new Tuple<Vector2Int, Vector2Int> (new Vector2Int(0,1), new Vector2Int(0,-1)),
            new Tuple<Vector2Int, Vector2Int> (new Vector2Int(1,1), new Vector2Int(-1,-1)),
            new Tuple<Vector2Int, Vector2Int> (new Vector2Int(1,-1), new Vector2Int(-1,1))
        };

        for(int i = 0; i < dxdy.Count; i++)
        {
            line.UnionWith(CheckLine(_pos, dxdy[i].Item1.x, dxdy[i].Item1.y));
            line.UnionWith(CheckLine(_pos, dxdy[i].Item2.x, dxdy[i].Item2.y));

            if (line.Count >= 5)
            {
                destroyed.UnionWith(line);
            }

            line.Clear();
        }

        //line.UnionWith( CheckLine(_pos, 1, 0));
        //line.UnionWith( CheckLine(_pos, -1, 0));

        //if(line.Count >= 5)
        //{
        //    destroyed.UnionWith(line);
        //}

        //line.Clear();

        //line.UnionWith(CheckLine(_pos, 0, 1));
        //line.UnionWith(CheckLine(_pos, 0, -1));

        //if (line.Count >= 5)
        //{
        //    destroyed.UnionWith(line);
        //}

        //line.Clear();

        //line.UnionWith(CheckLine(_pos, 1, 1));
        //line.UnionWith(CheckLine(_pos, -1, -1));

        //if (line.Count >= 5)
        //{
        //    destroyed.UnionWith(line);
        //}

        //line.Clear();

        //line.UnionWith(CheckLine(_pos, 1, -1));
        //line.UnionWith(CheckLine(_pos, -1, 1));

        //if (line.Count >= 5)
        //{
        //    destroyed.UnionWith(line);
        //}

        //line.Clear();

        List<Sphere> spheres = FindObjectsOfType<Sphere>().ToList();

        foreach(var sphere in spheres)
        {
            Vector2Int spherePos = ToLocalCoords(sphere.transform.position);

            if (destroyed.Contains(spherePos))
            {
                GameObject.Destroy(sphere.gameObject);

                cells[spherePos.x, spherePos.y] = CellType.Empty;
            }
        }
        return destroyed.Count;
    }

    public void Clear()
    {
        cells = new CellType[SIZE, SIZE];
    }
    void Start()
    {
        
    }

}
