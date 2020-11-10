using System;
using System.Collections;
using System.Collections.Generic;
using Logic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Logic.Utils;

public class Player : MonoBehaviour
{

    public float speed = 1;

    private Vector3Int positionToMove;
    private Brain brain;

    private List<Symbol> percepted;
    private bool isThinking = false;

    private MapGenerator world;

    private Queue<Cell> toVisit;

    private bool isGameEnd = false;

    private void Start()
    {
        positionToMove = new Vector3Int(0, 0, (int)transform.position.z);
        brain = new Brain();
        percepted = new List<Symbol>();

        world = GameObject.Find("GameManager").GetComponent<MapGenerator>();

        toVisit = new Queue<Cell>();
        // toVisit.Enqueue(new Cell(0, 0));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            positionToMove += Vector3Int.up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            positionToMove += Vector3Int.down;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            positionToMove += Vector3Int.right;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            positionToMove += Vector3Int.left;
        }

        transform.position = Vector3.MoveTowards(transform.position, positionToMove, Time.deltaTime * speed);
    }


    private void FixedUpdate()
    {
        if (!isGameEnd && positionToMove == transform.position && !isThinking)
        {
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
            isGameEnd = true;
        }


        brain.Tell(new Not(new CellSymbol(x, y, Symbol.Pit)));
        brain.Tell(new Not(new CellSymbol(x, y, Symbol.Wampus)));

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
            // TODO End game;
            Debug.Log("WIN!");
            isGameEnd = true;
        }


        AddReachable(x, y);
        if (x - 1 >= 0 && y >= 0)
        {
            // Left
            AddReachable(x - 1, y);
            toVisit.Enqueue(new Cell(x - 1, y));
        }
        if (x >= 0 && y - 1 >= 0)
        {
            // Down
            AddReachable(x, y - 1);
            toVisit.Enqueue(new Cell(x, y - 1));
        }
        if (x + 1 >= 0 && x + 1 < MapGenerator.mapSize && y >= 0)
        {
            // Right
            AddReachable(x + 1, y);
            toVisit.Enqueue(new Cell(x + 1, y));
        }
        if (x >= 0 && y + 1 >= 0 && y + 1 < MapGenerator.mapSize)
        {
            // Up
            AddReachable(x, y + 1);
            toVisit.Enqueue(new Cell(x, y + 1));
        }

        bool findNextCell = false;
        while (toVisit.Count != 0)
        {
            Cell next = toVisit.Dequeue();

            Debug.Log("Next move check: " + next);
            if (brain.Ask(new Not(HasWampus(next.x, next.y))) &&
                brain.Ask(new Not(HasPit(next.x, next.y))))
            {
                Debug.Log("Next move!");
                positionToMove = new Vector3Int(next.x, next.y, (int)transform.position.z);
                findNextCell = true;
            }
        }

        if (!findNextCell){
            Debug.Log("End of game");
            isGameEnd = true;
        }

        isThinking = false;
        yield return new WaitForSeconds(1);
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

        public override string ToString(){
            return ("(" + x + "; " + y + ")");
        }
    }
}
