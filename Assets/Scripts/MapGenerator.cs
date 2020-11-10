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

    public GameObject windPrefab;
    public GameObject stenchPrefab;
    public GameObject glitterPrefab;

    public Tilemap levelMap;
    public Tilemap entitiesMap;
    public Tilemap playerMap;

    [Range(4, 10)]
    public static int mapSize = 4;

    private List<GameObject> prefabs = new List<GameObject>();


    // Start is called before the first frame update
    void Awake()
    {
        ClearWorld();
        GenerateWorld();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
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
                    continue;
                }

                Vector3Int pos = new Vector3Int(x, y, (int)levelMap.transform.position.z);
                levelMap.SetTile(pos, ground);

            }
        }

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                Vector3Int pos = new Vector3Int(x, y, (int)levelMap.transform.position.z);
                if (levelMap.GetTile(pos) == null)
                {
                    AddPrefabs(x, y, windPrefab);
                }
            }
        }

        // playerMap.SetTile(new Vector3Int(0, 0, (int)playerMap.transform.position.z), player);

        int posX, posY;

        (posX, posY) = SetRandomTileForEntity(wampus);
        if (posX != -1)
            AddPrefabs(posX, posY, stenchPrefab);

        (posX, posY) = SetRandomTileForEntity(gold);
        if (posX != -1)
        {
            Vector3 windPos = new Vector3(posX + .5f, posY + 0.5f, (int)playerMap.transform.position.z);
            prefabs.Add(Instantiate(glitterPrefab, windPos, Quaternion.identity));
        }

    }

    private void AddPrefabs(int x, int y, GameObject prefab)
    {
        if (x - 1 >= 0 && y >= 0
            && !(levelMap.GetTile(new Vector3Int(x - 1, y, (int)levelMap.transform.position.z)) == null))
        {
            Vector3 windPos = new Vector3(x - 0.5f, y + .5f, (int)playerMap.transform.position.z);
            prefabs.Add(Instantiate(prefab, windPos, Quaternion.identity));
        }
        if (x >= 0 && y - 1 >= 0
             && !(levelMap.GetTile(new Vector3Int(x, y - 1, (int)levelMap.transform.position.z)) == null))
        {
            Vector3 windPos = new Vector3(x + 0.5f, y - .5f, (int)playerMap.transform.position.z);
            prefabs.Add(Instantiate(prefab, windPos, Quaternion.identity));
        }
        if (x + 1 >= 0 && x + 1 < mapSize && y >= 0
            && !(levelMap.GetTile(new Vector3Int(x + 1, y, (int)levelMap.transform.position.z)) == null))
        {
            Vector3 windPos = new Vector3(x + 1.5f, y + .5f, (int)playerMap.transform.position.z);
            prefabs.Add(Instantiate(prefab, windPos, Quaternion.identity));
        }
        if (x >= 0 && y + 1 >= 0 && y + 1 < mapSize
            && !(levelMap.GetTile(new Vector3Int(x, y + 1, (int)levelMap.transform.position.z)) == null))
        {
            Vector3 windPos = new Vector3(x + 0.5f, y + 1.5f, (int)playerMap.transform.position.z);
            prefabs.Add(Instantiate(prefab, windPos, Quaternion.identity));
        }
    }

    private (int, int) SetRandomTileForEntity(TileBase tile)
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
            if (onLevelTile == null || onEntityTile == wampus || onEntityTile == gold)
            {
                continue;
            }

            entitiesMap.SetTile(entityPos, tile);
            return (x, y);
        }

        Debug.LogWarning("Cannot generate tile: " + tile);
        return (-1, -1);
    }

    private void ClearWorld()
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            Destroy(prefabs[i]);
            // prefabs.RemoveAt(i);
        }
        prefabs.RemoveAll(item => true);

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
