using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Logic.Utils;

public class Player : MonoBehaviour
{

    public float speed = 1;


    public bool IsGameEnd = false;
    public bool IsWin = false;
    public bool IsLoose = false;
    public int StepCount = 0;



    private Vector3Int positionToMove;
    private Queue<Cell> path;
    private Brain brain;

    private List<Symbol> percepted;
    private bool isThinking = false;

    private MapGenerator world;

    private HashSet<Cell> visited;
    private HashSet<Cell> safeCells;
    private HashSet<Cell> dangerousCells;

    private Animator animator;

    private void Start()
    {
        positionToMove = new Vector3Int(0, 0, (int)transform.position.z);
        path = new Queue<Cell>();

        brain = new Brain();
        percepted = new List<Symbol>();

        world = GameObject.Find("GameManager").GetComponent<MapGenerator>();

        visited = new HashSet<Cell>();
        safeCells = new HashSet<Cell>();
        dangerousCells = new HashSet<Cell>();

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, positionToMove, Time.deltaTime * speed);
    }


    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reset");
            IsGameEnd = true;
        }
        if (!IsGameEnd && positionToMove == transform.position && !isThinking)
        {
            if (path.Count > 0)
            {
                Cell nextCell = path.Dequeue();
                Debug.Log("Move to next safe cell: " + nextCell);
                positionToMove = new Vector3Int(nextCell.x, nextCell.y, (int)transform.position.z);
                percepted.Clear();
                ++StepCount;
                return;
            }
            StartCoroutine(ProccedMove());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "wind")
            percepted.Add(Symbol.Wind);
        if (other.tag == "glitter")
            percepted.Add(Symbol.Glitter);
        if (other.tag == "stench")
            percepted.Add(Symbol.Stench);
    }


    public void ResetPlayer()
    {
        transform.position = new Vector3(0, 0, transform.position.z);

        IsGameEnd = false;
        IsWin = false;
        IsLoose = false;
        isThinking = false;
        StepCount = 0;

        positionToMove = new Vector3Int(0, 0, (int)transform.position.z);
        brain = new Brain();
        percepted.Clear();

        visited.Clear();
        safeCells.Clear();
        dangerousCells.Clear();
    }

    private IEnumerator ProccedMove()
    {
        isThinking = true;
        Debug.Log("Starting thing");

        yield return new WaitForSeconds(1);

        int x = (int)transform.position.x;
        int y = (int)transform.position.y;

        if (world.levelMap.GetTile(new Vector3Int(x, y, (int)world.levelMap.transform.position.z)) == null ||
            world.entitiesMap.GetTile(new Vector3Int(x, y, (int)world.entitiesMap.transform.position.z)) == world.wampus)
        {
            Debug.LogError("Failure");
            IsGameEnd = true;
            IsLoose = true;
        }

        Debug.Log("Percepted: " + String.Join(", ", percepted));

        CellSymbol wind = new CellSymbol(x, y, Symbol.Wind);
        if (percepted.Contains(Symbol.Wind))
        {
            brain.Tell(wind);
        }
        else
        {
            brain.Tell(new Not(wind));
        }

        CellSymbol stench = new CellSymbol(x, y, Symbol.Stench);
        if (percepted.Contains(Symbol.Stench))
        {
            brain.Tell(stench);
        }
        else
        {
            brain.Tell(new Not(stench));
        }

        CellSymbol glitter = new CellSymbol(x, y, Symbol.Glitter);
        if (percepted.Contains(Symbol.Glitter))
        {
            brain.Tell(glitter);
            Debug.Log("WIN!");
            IsGameEnd = true;
            IsWin = true;
        }

        // add visited cells
        AddReachable(x, y);
        Cell currCell = new Cell(x, y);
        visited.Add(currCell);

        // if nothing in this cell, then neighbours is safe
        if (percepted.Count == 0)
        {
            foreach (Cell cell in getValuableNeighbours(x, y))
            {
                if (!visited.Contains(cell))
                    safeCells.Add(cell);
            }
        }
        else // add all neighbours, which not in visited or safe
        {
            foreach (Cell cell in getValuableNeighbours(x, y))
            {
                if (!visited.Contains(cell) && !safeCells.Contains(cell))
                    dangerousCells.Add(cell);
            }
        }
        percepted.Clear();

        int z = (int)transform.position.z;

        if (!IsGameEnd)
        {
            // Find next cell in safe cells else think about dangerous
            if (safeCells.Count > 0)
            {
                Cell nextCell = null;
                foreach (Cell safeCell in safeCells)
                {
                    nextCell = safeCell;
                    break;
                }
                path = pathTo(nextCell, currCell);
                if (path == null)
                {
                    Debug.LogError("Empty path for safe cell. From: " + currCell + "; to: " + nextCell);
                    path = new Queue<Cell>();
                }
                path.Dequeue();
                safeCells.Remove(nextCell);
                isThinking = false;
            }
            else
            {
                Task task = new Task(() =>
                    {
                        Cell nextCell = tryToFindNextCellInDangerous();
                        if (nextCell != null)
                        {
                            positionToMove = new Vector3Int(nextCell.x, nextCell.y, z);
                            Debug.Log("Next move! " + positionToMove);
                            ++StepCount;
                            isThinking = false;
                        }
                        else
                        {
                            Debug.Log("End of game");
                            IsGameEnd = true;
                        }
                    });
                task.Start();
            }
        }
    }


    private Cell tryToFindNextCellInDangerous()
    {
        Vector3Int res = Vector3Int.zero;

        Cell[] dangerousCellsArray = new Cell[dangerousCells.Count];
        dangerousCells.CopyTo(dangerousCellsArray);

        for (int i = 0; i < dangerousCellsArray.Length; i++)
        {
            Cell nextCell = dangerousCellsArray[i];

            Debug.Log("Next move check: " + nextCell);
            if (brain.Ask(new Not(HasWampus(nextCell.x, nextCell.y))) &&
                brain.Ask(new Not(HasPit(nextCell.x, nextCell.y))))
            {
                dangerousCells.Remove(nextCell);
                return nextCell;
            }
        }

        return null;
    }


    private Queue<Cell> pathTo(Cell destCell, Cell currCell)
    {
        Queue<Cell> path = new Queue<Cell>();
        path.Enqueue(currCell);
        path = pathTo(currCell, destCell, path);
        return path;
    }


    private Queue<Cell> pathTo(Cell currCell, Cell destCell, Queue<Cell> path)
    {
        if (currCell == destCell)
            return path;

        List<Cell> visitedNeighbours =
            getValuableNeighbours(currCell.x, currCell.y)
                .Where(c => (visited.Contains(c) || c == destCell) && !path.Contains(c))
                .ToList();

        foreach (Cell visitedNeighbour in visitedNeighbours)
        {
            path.Enqueue(visitedNeighbour);
            Queue<Cell> newPath = pathTo(visitedNeighbour, destCell, path);
            if (newPath != null)
                return newPath;
            else
                path.Dequeue();
        }

        return null;
    }


    private List<Cell> getValuableNeighbours(int x, int y)
    {
        List<Cell> neigbours = new List<Cell>();

        if (x + 1 >= 0 && x + 1 < MapGenerator.mapSize && y >= 0)
        {
            // Right
            neigbours.Add(new Cell(x + 1, y));
        }
        if (x >= 0 && y + 1 >= 0 && y + 1 < MapGenerator.mapSize)
        {
            // Up
            neigbours.Add(new Cell(x, y + 1));
        }
        if (x - 1 >= 0 && y >= 0)
        {
            // Left
            neigbours.Add(new Cell(x - 1, y));
        }
        if (x >= 0 && y - 1 >= 0)
        {
            // Down
            neigbours.Add(new Cell(x, y - 1));
        }

        return neigbours;
    }


    private class Cell
    {
        public int x { get; set; }
        public int y { get; set; }

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return ("(" + (x + 1) + "; " + (y + 1) + ")");
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Cell cell = (Cell)obj;
                return (x == cell.x) && (y == cell.y);
            }
        }

        public static bool operator ==(Cell c1, Cell c2)
        {
            // Check for null on left side.
            if (System.Object.ReferenceEquals(c1, null))
            {
                if (System.Object.ReferenceEquals(c2, null))
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return c1.Equals(c2);
        }

        public static bool operator !=(Cell lhs, Cell rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return (x << 2) ^ y;
        }
    }
}