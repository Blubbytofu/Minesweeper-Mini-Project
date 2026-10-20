using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinesweeperTile : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text label;

    private Minesweeper gameManager;
    private int r;
    private int c;

    public void Setup(Minesweeper game, int nr, int nc)
    {
        gameManager = game;
        r = nr;
        c = nc;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(
            () => gameManager.Reveal(r, c)
        );

        label.text = "";
    }
}
