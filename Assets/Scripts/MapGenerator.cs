using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{

    public TileBase ground;
    public TileBase wall;
    public TileBase gold;
    public TileBase wampus;
    public TileBase player;

    public Tilemap levelMap;
    public Tilemap entitiesMap;
    public Tilemap playerMap;

    [Range(4, 10)]
    public int mapSize;


    // Start is called before the first frame update
    void Start()
    {
        GenerateWorld();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) {
            ClearWorld();
            GenerateWorld();
        }
    }

    private void GenerateWorld()
    {
        for (int y = -1; y < mapSize + 1; y++)
        {
            for (int x = -1; x < mapSize + 1; x++)
            {
                if (x < 0 || x >= mapSize || y < 0 || y >= mapSize)
                {
                    Vector3Int wallPos = new Vector3Int(x, y, (int)levelMap.transform.position.z);
                    levelMap.SetTile(wallPos, wall);
                    continue;
                }

                if (Random.value <= 0.2 && !(x == 0 && y == 0))
                {
                    // TODO add wind
                    continue;
                }

                Vector3Int pos = new Vector3Int(x, y, (int)levelMap.transform.position.z);
                levelMap.SetTile(pos, ground);

            }
        }

        playerMap.SetTile(new Vector3Int(0, 0, (int)playerMap.transform.position.z), player);

        SetRandomTileForEntity(wampus);
        SetRandomTileForEntity(gold);

    }

    private void SetRandomTileForEntity(TileBase tile)
    {
        int tries = 100;
        while (--tries > 0)
        {
            int pos = Random.Range(0, mapSize * mapSize);
            int x = pos / mapSize;
            int y = pos % mapSize;

            if (x == 0 && y == 0)
                continue;

            Vector3Int levelPos = new Vector3Int(x, y, (int)levelMap.transform.position.z);
            Vector3Int entityPos = new Vector3Int(x, y, (int)entitiesMap.transform.position.z);
 
            // pit or wampus or gold
            TileBase onLevelTile = levelMap.GetTile(levelPos);
            TileBase onEntityTile = entitiesMap.GetTile(entityPos);
            if (onLevelTile == null || onEntityTile == wampus || onEntityTile == gold) {
                Debug.Log("Pos: " + entityPos + "; Level: " + onLevelTile + "; Entity: " + onEntityTile);
                continue;
            }

            entitiesMap.SetTile(entityPos, tile);
            return;
        }

        Debug.LogWarning("Cannot generate tile: " + tile);
    }

    private void ClearWorld(){
        for (int y = -1; y < mapSize + 1; y++)
        {
            for (int x = -1; x < mapSize + 1; x++)
            {
                Vector3Int levelPos = new Vector3Int(x, y, (int)levelMap.transform.position.z);
                levelMap.SetTile(levelPos, null);

                Vector3Int entityPos = new Vector3Int(x, y, (int)entitiesMap.transform.position.z);
                entitiesMap.SetTile(entityPos, null);

                Vector3Int playerPos = new Vector3Int(x, y, (int)playerMap.transform.position.z);
                playerMap.SetTile(playerPos, null);
            }
        }
    }
}
