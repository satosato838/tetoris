using UnityEngine;

public class Osero
{
    private const int BoardSize = 8;
    public enum DiskState
    {
        None,
        SetDot,
        Black,
        White
    }

    public enum PlayerTurn
    {
        None,
        Black,
        White
    }
    public OseroDisk[,] BoardDisks { get; private set; } = new OseroDisk[8, 8];

    private PlayerTurn firstPlayerColor = PlayerTurn.Black;
    public PlayerTurn CurrentTurnDiskColor { get; private set; }
    public Osero()
    {
        StartGame();
    }

    private void StartGame()
    {
        CurrentTurnDiskColor = firstPlayerColor;
        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                BoardDisks[y, x] = new OseroDisk(DiskState.None, (y, x));
            }
        }
        BoardDisks[3, 3].SetState(DiskState.White);
        BoardDisks[4, 4].SetState(DiskState.White);
        BoardDisks[3, 4].SetState(DiskState.Black);
        BoardDisks[4, 3].SetState(DiskState.Black);
        RefreshBoard();
    }

    public void PlaceDisk((int, int) pos)
    {
        if (BoardDisks[pos.Item2, pos.Item1].DiskState != DiskState.None) return;

        BoardDisks[pos.Item2, pos.Item1].SetState(GetDiskState(CurrentTurnDiskColor));
        //CurrentTurn = CurrentTurn == DiskState.Black ? DiskState.White : DiskState.Black;
    }

    public void Reset()
    {
        StartGame();
    }

    public void TurnDisk()
    {
        throw new System.NotImplementedException();
    }

    private void RefreshBoard()
    {
        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                if (IsSetDisk((y, x)))
                {
                    BoardDisks[y, x].SetState(DiskState.SetDot);
                }
            }
        }
    }

    private bool IsSetDisk((int, int) yx)
    {
        var y = yx.Item1;
        var x = yx.Item2;
        if (y < 0 || y >= BoardSize || x < 0 || x >= BoardSize) return false; // 盤外
        if (BoardDisks[y, x].DiskState != DiskState.None) return false; // 空マスのみ

        for (int dY = -1; dY <= 1; dY++)
        {
            for (int dX = -1; dX <= 1; dX++)
            {
                if (dY == 0 && dX == 0) continue; // 方向なしはスキップ

                int rY = y + dY;
                int rX = x + dX;

                // まず隣が相手の石か
                if (rY < 0 || rY >= BoardSize || rX < 0 || rX >= BoardSize) continue;
                if (BoardDisks[rY, rX].DiskState != GetOpponentDiskColor()) continue;

                // さらにその先を調べる
                while (true)
                {
                    rY += dY;
                    rX += dX;
                    if (rY < 0 || rY >= BoardSize || rX < 0 || rX >= BoardSize) break;
                    if (BoardDisks[rY, rX].DiskState == GetOpponentDiskColor()) continue;
                    if (BoardDisks[rY, rX].DiskState == GetDiskState(CurrentTurnDiskColor)) return true;
                    break;
                }
            }
        }
        return false;
    }

    private DiskState GetDiskState(PlayerTurn playerTurn)
    {
        if (playerTurn == PlayerTurn.Black) return DiskState.Black;
        else return DiskState.White;
    }

    private DiskState GetOpponentDiskColor()
    {
        if (GetDiskState(CurrentTurnDiskColor) == DiskState.Black) return DiskState.White;
        else return DiskState.Black;
    }

    public bool IsGameEnd()
    {
        throw new System.NotImplementedException();
    }

    public void GameOver()
    {
        throw new System.NotImplementedException();
    }
}

public class OseroDisk
{
    public Osero.DiskState DiskState { get; private set; }
    public (int, int) Position { get; private set; }
    public OseroDisk(Osero.DiskState state, (int, int) pos)
    {
        SetState(state);
        Position = pos;
    }

    public void SetState(Osero.DiskState state)
    {
        DiskState = state;
    }
}

