using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    
    public const int empty = 0;
    public const int outerCorner = 1;
    public const int outerWall = 2;
    public const int innerCorner = 3;
    public const int innerWall = 4;
    public const int pellet = 5;
    public const int powerPellet = 6;
    public const int tJunction = 7;
    public const int ghostDoor = 8;

    private readonly int [,] grid =
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,8},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };

    private int[,] fullGrid;
    private bool[,] playerWalkableGrid;
    private bool[,] ghostWalkableGrid;

    private int width;
    private int height;

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public void Awake()
    {
        buildFullGrid();
        floodFillPlayerWalkable(new Vector2Int(1, 1));
        floodFillGhostWalkable(new Vector2Int(1, 1));
        
    }

    public bool inBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }

    public int getTile(Vector2Int pos)
    {
        if (!inBounds(pos))
        {
            return outerWall;   
        }
        return fullGrid[pos.y, pos.x];
    }

    public bool isPlayerWalkable(int tile)
    {
        if (tile == empty || tile == pellet || tile == powerPellet)
        {
            return true;
        }
        return false;
    }

    public bool isGhostWalkable(int tile)
    {
        if (tile == empty || tile == pellet || tile == powerPellet || tile == ghostDoor)
        {
            return true;
        }
        return false;
    }

    public Vector2Int worldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(-worldPos.y));
    }

    public Vector3 gridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x, -gridPos.y, 0);
    }

   
    public void buildFullGrid()
    {
        int quadRows = grid.GetLength(0); 
        int quadCols = grid.GetLength(1);   

        width = quadCols * 2;               
        height = quadRows * 2 - 1;          

   
        fullGrid = new int[height, width];
        playerWalkableGrid = new bool[height, width];
        ghostWalkableGrid = new bool[height, width];

        for (int y = 0; y < quadRows; y++)
        {
            for (int x = 0; x < quadCols; x++)
            {
                int tile = grid[y, x];

                // top left quadrant
                fullGrid[y, x] = tile;
                
                // top right quadrant
                fullGrid[y, width - 1 - x] = tile;

                // bottom left quadrant
                fullGrid[height - 1 - y, x] = tile;

                // bottom right quadrant
                fullGrid[height - 1 - y, width - 1 - x] = tile;

            }
        }
    }

    public bool findSpecialTile(int tileType, out Vector2Int pos)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (fullGrid[y, x] == tileType)
                {
                    pos = new Vector2Int(x, y);
                    return true;
                }
            }
        }
        pos = new Vector2Int(-1, -1);
        return false;
    }

    void floodFillPlayerWalkable(Vector2Int pos)
    {
        if (!inBounds(pos))
        {
            return;
        }

        if (playerWalkableGrid[pos.y, pos.x])
        {
            return;
        }

        int tile = getTile(pos);
        if (!isPlayerWalkable(tile))
        {
            return;
        }

        playerWalkableGrid[pos.y, pos.x] = true;

        floodFillPlayerWalkable(new Vector2Int(pos.x + 1, pos.y));
        floodFillPlayerWalkable(new Vector2Int(pos.x - 1, pos.y));
        floodFillPlayerWalkable(new Vector2Int(pos.x, pos.y + 1));
        floodFillPlayerWalkable(new Vector2Int(pos.x, pos.y - 1));
    }

    void floodFillGhostWalkable(Vector2Int pos)
    {
        if (!inBounds(pos))
        {
            return;
        }

        if (ghostWalkableGrid[pos.y, pos.x])
        {
            return;
        }

        int tile = getTile(pos);
        if (!isGhostWalkable(tile))
        {
            return;
        }

        ghostWalkableGrid[pos.y, pos.x] = true;

        floodFillGhostWalkable(new Vector2Int(pos.x + 1, pos.y));
        floodFillGhostWalkable(new Vector2Int(pos.x - 1, pos.y));
        floodFillGhostWalkable(new Vector2Int(pos.x, pos.y + 1));
        floodFillGhostWalkable(new Vector2Int(pos.x, pos.y - 1));
    }

    public bool canEnterPlayer(Vector2Int pos)
    {
        if (!inBounds(pos))
        {
            return false;
        }
        return playerWalkableGrid[pos.y, pos.x];
    }

    public bool canEnterGhost(Vector2Int pos)
    {
        if (!inBounds(pos))
        {
            return false;
        }
        return ghostWalkableGrid[pos.y, pos.x];
    }

    
    
}
