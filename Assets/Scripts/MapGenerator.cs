using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] int x=256;
    [SerializeField] int y=256;
    [SerializeField] Tilemap[] Tilemaps;
    [SerializeField] TileBase tile;
    // Start is called before the first frame update
    void Start()
    {
        int startingPoint = 5;
        //foreach(Tilemap tilemap in Tilemaps)
        //{
        //    int[,] cords = new int[x, y];
        //    int[,] map = RandomWalkTopSmoothed(cords, Random.Range(1.0f, 1000.0f), Random.Range(1, 3),startingPoint);
        //    RenderMap(map, tilemap, tile);
        //    UpdateMap(map, tilemap);
        //    startingPoint = getLastHeight(map);
        //}

        int[,] cords = new int[x, y];
        int[,] map = RandomPerlinNoise(cords, 0.15f);

        RenderMap(map, Tilemaps[0], tile);
        UpdateMap(map, Tilemaps[0]);
    }

    private int getLastHeight(int [,] lastrow)
    {
        for (int y = lastrow.GetUpperBound(1); y >= 0; y--)
        {
            if (lastrow[x - 1, y] == 1)
                return y;
        }
        return 0;
    }

    public static void RenderMap(int[,] map, Tilemap tilemap, TileBase tile)
    {
        //Clear the map (ensures we dont overlap)
        tilemap.ClearAllTiles();
        //Loop through the width of the map
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                // 1 = tile, 0 = no tile
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }
    }
    public static void UpdateMap(int[,] map, Tilemap tilemap) //Takes in our map and tilemap, setting null tiles where needed
    {
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                //We are only going to update the map, rather than rendering again
                //This is because it uses less resources to update tiles to null
                //As opposed to re-drawing every single tile (and collision data)
                if (map[x, y] == 0)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
        }
    }

    public static int [,] RandomPerlinNoise(int [,] map, float scale)
    {
        int newPoint;
        float modifier = Random.Range(0f,10f);
        for (int i = 0; i < map.GetUpperBound(0); i++)
        {
            for (int j = 0; j < map.GetUpperBound(1); j++)
            {
                float x = (j + modifier) * scale;
                float y = (i + modifier) * scale;
                newPoint = Mathf.RoundToInt(Mathf.PerlinNoise(x, y));
                map[i, j] = newPoint;
            }
        }
        return map;
    }

    public static int[,] RandomWalkTopSmoothed(int[,] map, float seed, int minSectionWidth, int startingHeight)
    {
    //Seed our random
        System.Random rand = new System.Random(seed.GetHashCode());

    //Determine the start position
        int lastHeight = startingHeight;
        int currentHeight = startingHeight;

    //Used to determine which direction to go
        int nextMove = 0;
    //Used to keep track of the current sections width
        int sectionWidth = 0;

    //Work through the array width
        for (int x = 0; x <= map.GetUpperBound(0); x+= minSectionWidth)
        {
            //Determine the next move
            nextMove = rand.Next(3);

            if (nextMove == 0 && lastHeight > 0)
                currentHeight = --lastHeight;
            else if (nextMove == 1 && lastHeight < map.GetUpperBound(1))
                currentHeight = ++lastHeight;
            else if (nextMove == 2 && currentHeight != -1)
                currentHeight = -1;
            ////sectionWidth++;

            for (int currentX = 0; currentX <= minSectionWidth; currentX++)
            {
                if (x + currentX <= map.GetUpperBound(0))
                {
                    for (int y = currentHeight; y >= 0; y--)
                    {
                        map[x + currentX, y] = 1;
                    }
                }
            }
            //Work our way from the height down to 0
            
        }

        //Return the modified map
        return map;
    }
}
