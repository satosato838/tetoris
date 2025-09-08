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
    public PlayerTurn CurrentTurnDiskColor { get; private set; } = PlayerTurn.Black;
    public Osero()
    {
        StartGame();
    }

    private void StartGame()
    {
        CurrentTurnDiskColor = PlayerTurn.Black;
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
        if (y < 1 || BoardSize < y || x < 1 || BoardSize < x) { return false; }  //盤外には打てない
        if (BoardDisks[y, x].DiskState != DiskState.None) { return false; }  //空マスでなければ打てない

        //注目するマスの周囲8方向に対し、石を打てるか調べる
        int rY, rX;  //調べるマスを移動させるのに使う変数
        for (int dY = -1; dY <= 1; dY++)
        {  //横方向
            for (int dX = -1; dX <= 1; dX++)
            {  //縦方向
                rY = y + dY; rX = y + dX;  //調べるマスの初期値

                //調べるマスが「相手の石」ならループ
                while (BoardDisks[rY, rX].DiskState == GetOpponentDiskColor())
                {
                    rY += dY; rX += dX;  //次のマスに移動

                    //同色の石に出会った(打てると分かった)時
                    if (BoardDisks[rY, rX].DiskState == GetDiskState(CurrentTurnDiskColor)) { return true; }  //打てると判定
                }
            }
        }

        return false;  //打てないと判定
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

