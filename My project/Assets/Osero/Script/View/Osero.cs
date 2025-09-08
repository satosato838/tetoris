using UnityEngine;

public class Osero
{

    public enum DiskState
    {
        None,
        Black,
        White
    }
    public OseroDisk[,] BoardDisks { get; private set; } = new OseroDisk[8, 8];
    public DiskState CurrentTurn { get; private set; } = DiskState.Black;
    public Osero()
    {
        StartGame();
    }

    private void StartGame()
    {
        CurrentTurn = DiskState.Black;
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                BoardDisks[y, x] = new OseroDisk(DiskState.None, (y, x));
            }
        }
        BoardDisks[3, 3].SetState(DiskState.White);
        BoardDisks[4, 4].SetState(DiskState.White);
        BoardDisks[3, 4].SetState(DiskState.Black);
        BoardDisks[4, 3].SetState(DiskState.Black);
    }

    public void PlaceDisk((int, int) pos)
    {
        if (BoardDisks[pos.Item2, pos.Item1].DiskState != DiskState.None) return;

        BoardDisks[pos.Item2, pos.Item1].SetState(CurrentTurn);
        CurrentTurn = CurrentTurn == DiskState.Black ? DiskState.White : DiskState.Black;
    }

    public void Reset()
    {
        StartGame();
    }

    public void TurnDisk()
    {
        throw new System.NotImplementedException();
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

