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
        foreach(Tilemap tilemap in Tilemaps)
        {
            int[,] cords = new int[x, y];
            int[,] map = RandomWalkTopSmoothed(cords, Random.Range(1.0f, 1000.0f), Random.Range(1, 3));
            RenderMap(map, tilemap, tile);
            UpdateMap(map, tilemap);
        }
        

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

    public static int[,] RandomWalkTopSmoothed(int[,] map, float seed, int minSectionWidth)
    {
    //Seed our random
    System.Random rand = new System.Random(seed.GetHashCode());

    //Determine the start position
    int lastHeight = Random.Range(0, map.GetUpperBound(1));

    //Used to determine which direction to go
    int nextMove = 0;
    //Used to keep track of the current sections width
    int sectionWidth = 0;

    //Work through the array width
    for (int x = 0; x <= map.GetUpperBound(0); x++)
    {
        //Determine the next move
        nextMove = rand.Next(2);

        //Only change the height if we have used the current height more than the minimum required section width
        if (nextMove == 0 && lastHeight > 0 && sectionWidth > minSectionWidth)
        {
            lastHeight--;
            sectionWidth = 0;
        }
        else if (nextMove == 1 && lastHeight < map.GetUpperBound(1) && sectionWidth > minSectionWidth)
        {
            lastHeight++;
            sectionWidth = 0;
        }
        //Increment the section width
        sectionWidth++;

        //Work our way from the height down to 0
        for (int y = lastHeight; y >= 0; y--)
        {
            map[x, y] = 1;
        }
    }

    //Return the modified map
    return map;
    }
}
