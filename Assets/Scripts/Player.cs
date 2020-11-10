using System.Collections;
using System.Collections.Generic;
using Logic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 1;

    private Vector3Int positionToMove;
    private Brain brain;

    private List<Symbol> percepted;

    private void Create()
    {
        positionToMove = new Vector3Int(0, 0, (int)transform.position.z);
        brain = new Brain();
        percepted = new List<Symbol>();

        CellSymbol cellSymbol = new CellSymbol((int)transform.position.x, (int)transform.position.y, Symbol.Wind);
        brain.Tell(new Not(cellSymbol));
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


    private void FixedUpdate(){
        if (positionToMove == transform.position)
            StartCoroutine(ProccedMove());
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


    private IEnumerator ProccedMove(){
        yield return null;
    }
}
