using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 1;

    private Vector3Int position;

    private void Create()
    {
        position = new Vector3Int(0, 0, (int)transform.position.z);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            position += Vector3Int.up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            position += Vector3Int.down;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            position += Vector3Int.right;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            position += Vector3Int.left;
        }

        transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name + "; " + other.tag);
    }
}
