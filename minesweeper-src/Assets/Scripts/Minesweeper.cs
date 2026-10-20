
using System.Collections.Generic;
using UnityEngine;

public class Minesweeper : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private int numMines;

    private Tile[,] grid;
    private MinesweeperTile[,] gameGrid;

    private bool gameOver;
    private bool gameWon;
    private int revealedSafeTiles;

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
        gameOver = false;
        gameWon = false;
        revealedSafeTiles = 0;

        grid = new Tile[gridWidth, gridHeight];
        gameGrid = new MinesweeperTile[gridWidth, gridHeight];
        for (int r = 0; r < gridWidth; r++)
        {
            for (int c = 0; c < gridHeight; c++)
            {
                grid[r, c] = new Tile();
                GameObject mt = Instantiate(tilePrefab, new Vector2(50 * r + (835/2.0f), 50 * c + (312/2.0f)), Quaternion.identity);
                mt.transform.SetParent(canvas.transform);
                gameGrid[r, c] = mt.GetComponent<MinesweeperTile>();
                gameGrid[r, c].Setup(this, r, c);
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

                int mineCount = 0;
                for (int i = 0; i < 8; i++)
                {
                    int nr = r + dr[i];
                    int nc = c + dc[i];
                    if (!IsInBounds(nr, nc) || !grid[nr, nc].isMine)
                    {
                        continue;
                    }

                    mineCount++;
                }
                grid[r, c].numAdjMines = mineCount;
            }
        }
    }

    private bool IsInBounds(int r, int c)
    {
        return r >= 0 && r < gridWidth && c >= 0 && c < gridHeight;
    }

    public void Reveal(int r, int c)
    {
        if (!IsInBounds(r, c) || gameOver)
        {
            return;
        }

        Tile t = grid[r, c];
        if (t.isRevealed)
        {
            return;
        }

        if (t.isMine)
        {
            t.isRevealed = true;
            gameOver = true;
            RevealAll();
            return;
        }

        FloodFill(r, c);


    }

    private void FloodFill(int r, int c)
    {
        Queue<int> rq = new Queue<int>();
        Queue<int> cq = new Queue<int>();

        rq.Enqueue(r);
        cq.Enqueue(c);

        while (rq.Count > 0)
        {
            r = rq.Dequeue();
            c = cq.Dequeue();

            Tile t = grid[r, c];

            t.isRevealed = true;
            UpdateTileLabel(r, c);
            revealedSafeTiles++;

            if (t.numAdjMines == 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    int nr = r + dr[i];
                    int nc = c + dc[i];
                    if (!IsInBounds(nr, nc) || grid[nr, nc].isRevealed || grid[nr, nc].isMine)
                    {
                        continue;
                    }

                    rq.Enqueue(nr);
                    cq.Enqueue(nc);
                }
            }
        }
    }

    private void RevealAll()
    {
        for (int r = 0; r < gridWidth; r++)
        {
            for (int c = 0; c < gridHeight; c++)
            {
                grid[r, c].isRevealed = true;

                UpdateTileLabel(r, c);
            }
        }
    }

    private void UpdateTileLabel(int r, int c)
    {
        if (grid[r, c].isMine)
        {
            gameGrid[r, c].SetLabel("*");
        }
        else if (grid[r, c].numAdjMines == 0)
        {
            gameGrid[r, c].SetLabel("");
        }
        else
        {
            char ch = (char)('0' + grid[r, c].numAdjMines);
            gameGrid[r, c].SetLabel("" + ch);
        }
    }
}
