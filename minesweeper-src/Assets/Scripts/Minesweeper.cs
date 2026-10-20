using UnityEngine;

public class Minesweeper : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private int numMines;

    private Tile[,] grid;

    private int[] dr = { 0, -1, 0, 1, -1, -1, 1, 1 };
    private int[] dc = { -1, 0, 1, 0, -1, 1, -1, 1 };

    private class Tile
    {
        public bool isMine { get; set; }
        public bool isRevealed { get; set; }
        public int numAdjMines { get; set; }

        public Tile()
        {
            isMine = false;
            isRevealed = false;
            numAdjMines = 0;
        }
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        
    }

    private void NewGame()
    {
        grid = new Tile[gridWidth, gridHeight];
        for (int r = 0; r < gridWidth; r++)
        {
            for (int c = 0; c < gridHeight; c++)
            {
                grid[r, c] = new Tile();
            }
        }

        int placedMines = 0;
        while (placedMines < numMines)
        {
            int r = Random.Range(0, gridWidth);
            int c = Random.Range(0, gridHeight);

            if (grid[r, c].isMine)
            {
                continue;
            }

            grid[r, c].isMine = true;
            placedMines++;
        }

        for (int r = 0; r < gridWidth; r++)
        {
            for (int c = 0; c < gridHeight; c++)
            {
                if (grid[r, c].isMine)
                {
                    continue;
                }

                for (int i = 0; i < 8; i++)
                {
                    int nr = r + dr[i];
                    int nc = c + dc[i];
                    if (!IsInBounds(nr, nc) || grid[r, c].isMine)
                    {
                        continue;
                    }

                    grid[r, c].numAdjMines++;
                }
            }
        }
    }

    private bool IsInBounds(int r, int c)
    {
        return r >= 0 && r < gridWidth && c >= 0 && c < gridHeight;
    }

    public void Reveal(int r, int c)
    {

    }
}
