using System.Collections.Generic;
using UnityEngine;

public class OseroView : MonoBehaviour
{
    public const int BoardSize = 8;
    [SerializeField] private OseroCellView OseroCellPrefab;
    [SerializeField] private Transform OseroCellParent;
    private Osero _osero;
    private Color ClearColor => new Color(1, 1, 1, 0);
    private List<List<OseroCellView>> _AllCells = new List<List<OseroCellView>>();
    void Start()
    {
        for (int y = 0; y < BoardSize; y++)
        {
            var cells = new List<OseroCellView>();
            for (int x = 0; x < BoardSize; x++)
            {
                var cell = Instantiate(OseroCellPrefab, OseroCellParent);
                cell.name = $"Cell({y},{x})";
                cell.Init((y, x), (yx) =>
                {
                    Debug.Log($"Click {yx.Item1 + 1},{yx.Item2 + 1}");
                    PlaceDisk(yx);
                });
                cells.Add(cell);
            }
            _AllCells.Add(cells);
        }
        GameStart();
    }

    public void GameStart()
    {
        _osero = new Osero();
        Refresh();
    }
    public void PlaceDisk((int, int) pos)
    {
        _osero.PlaceDisk(pos);
        Refresh();
    }

    public void Refresh()
    {
        var disks = _osero.BoardDisks;
        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                if (disks[y, x].IsDot)
                {
                    _AllCells[y][x].SetDot(_osero.CurrentTurnDiskColor == Osero.PlayerTurn.Black ? Color.black : Color.white);
                }
                else
                {
                    _AllCells[y][x].SetDot(ClearColor);
                }
                _AllCells[y][x].SetDisk(disks[y, x].DiskState switch
                {
                    Osero.DiskState.None => ClearColor,
                    Osero.DiskState.Black => Color.black,
                    Osero.DiskState.White => Color.white,
                    _ => ClearColor
                });
                // if (disks[y, x].DiskState == Osero.DiskState.Black || disks[y, x].DiskState == Osero.DiskState.White)
                //     Debug.Log($"disks[{y}][{x}]:" + disks[y, x].DiskState);
            }
        }
    }
}
