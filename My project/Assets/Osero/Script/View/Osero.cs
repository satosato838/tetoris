using System.Collections.Generic;
using System.Linq;

public class Osero
{
    private const int BoardSize = 8;
    public enum DiskState
    {
        None,
        Black,
        White
    }

    public enum Result
    {
        None,
        Drow,
        BlackWin,
        WhiteWin
    }
    public enum PlayerTurn
    {
        None,
        Black,
        White
    }
    public List<List<OseroDisk>> BoardDisks { get; private set; } = new List<List<OseroDisk>>();

    private PlayerTurn firstPlayerColor = PlayerTurn.Black;
    public PlayerTurn CurrentTurnDiskColor { get; private set; }

    public Result GameResult { get; private set; }
    public Osero()
    {
        StartGame();
    }

    private void StartGame()
    {
        CurrentTurnDiskColor = firstPlayerColor;

        BoardDisks = new List<List<OseroDisk>>();
        for (int y = 0; y < BoardSize; y++)
        {
            List<OseroDisk> oseroDisks = new List<OseroDisk>();
            for (int x = 0; x < BoardSize; x++)
            {
                oseroDisks.Add(new OseroDisk(DiskState.None, (y, x)));
            }
            BoardDisks.Add(oseroDisks);
        }
        BoardDisks[3][3].SetState(DiskState.White);
        BoardDisks[4][4].SetState(DiskState.White);
        BoardDisks[3][4].SetState(DiskState.Black);
        BoardDisks[4][3].SetState(DiskState.Black);
        GameResult = Result.None;
        RefreshBoard();
    }

    public void PlaceDisk((int, int) yx)
    {
        if (BoardDisks[yx.Item1][yx.Item2].IsDot)
        {
            BoardDisks[yx.Item1][yx.Item2].SetState(GetDiskState(CurrentTurnDiskColor));
            ReverseDisk(yx);
            CurrentTurnDiskColor = CurrentTurnDiskColor == PlayerTurn.Black ? PlayerTurn.White : PlayerTurn.Black;
            RefreshBoard();
        }
    }

    public void Reset()
    {
        StartGame();
    }


    private void RefreshBoard()
    {
        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                BoardDisks[y][x].RefreshDot(IsSetDisk((y, x)));
            }
        }
        if (IsGameEnd())
        {
            GameOver();
        }
    }

    private void ReverseDisk((int, int) yx)
    {
        var y = yx.Item1;
        var x = yx.Item2;
        for (int dY = -1; dY <= 1; dY++)
        {
            for (int dX = -1; dX <= 1; dX++)
            {
                if (dY == 0 && dX == 0) continue;

                int rY = y + dY;
                int rX = x + dX;

                if (rY < 0 || rY >= BoardSize || rX < 0 || rX >= BoardSize) continue;
                if (BoardDisks[rY][rX].DiskState != GetOpponentDiskColor()) continue;

                var reverseList = new List<(int, int)> { (rY, rX) };

                while (true)
                {
                    rY += dY;
                    rX += dX;
                    if (rY < 0 || rY >= BoardSize || rX < 0 || rX >= BoardSize) break;
                    if (BoardDisks[rY][rX].DiskState == GetOpponentDiskColor())
                    {
                        reverseList.Add((rY, rX));
                        continue;
                    }
                    if (BoardDisks[rY][rX].DiskState == GetDiskState(CurrentTurnDiskColor))
                    {
                        foreach (var pos in reverseList)
                        {
                            BoardDisks[pos.Item1][pos.Item2].SetState(GetDiskState(CurrentTurnDiskColor));
                        }
                    }
                    break;
                }
            }
        }
    }

    private bool IsSetDisk((int, int) yx)
    {
        var y = yx.Item1;
        var x = yx.Item2;
        if (y < 0 || y >= BoardSize || x < 0 || x >= BoardSize) return false;
        if (BoardDisks[y][x].DiskState != DiskState.None) return false;

        for (int dY = -1; dY <= 1; dY++)
        {
            for (int dX = -1; dX <= 1; dX++)
            {
                if (dY == 0 && dX == 0) continue;

                int rY = y + dY;
                int rX = x + dX;

                if (rY < 0 || rY >= BoardSize || rX < 0 || rX >= BoardSize) continue;
                if (BoardDisks[rY][rX].DiskState != GetOpponentDiskColor()) continue;

                while (true)
                {
                    rY += dY;
                    rX += dX;
                    if (rY < 0 || rY >= BoardSize || rX < 0 || rX >= BoardSize) break;
                    if (BoardDisks[rY][rX].DiskState == GetOpponentDiskColor()) continue;
                    if (BoardDisks[rY][rX].DiskState == GetDiskState(CurrentTurnDiskColor)) return true;
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
        var allDisks = BoardDisks.SelectMany((disks, i) => disks.Select(disk => disk.DiskState));
        //すべてのDiskがNoneでないか黒か白のみになったらゲーム終了
        return allDisks.All(v => v != DiskState.None) || allDisks.All(v => v == DiskState.Black) || allDisks.All(v => v == DiskState.White);
    }

    public void GameOver()
    {
        var allDisks = BoardDisks.SelectMany((disks, i) => disks.Select(disk => disk.DiskState));
        var blackCount = allDisks.Count(v => v == DiskState.Black);
        var whiteCount = allDisks.Count(v => v == DiskState.White);
        if (blackCount == whiteCount)
        {
            GameResult = Result.Drow;
        }
        else
        {
            GameResult = blackCount > whiteCount ? Result.BlackWin : Result.WhiteWin;
        }
    }
}

public class OseroDisk
{
    public Osero.DiskState DiskState { get; private set; }
    public (int, int) Position { get; private set; }
    public bool IsDot { get; private set; }
    public OseroDisk(Osero.DiskState state, (int, int) pos)
    {
        SetState(state);
        Position = pos;
    }

    public void SetState(Osero.DiskState state)
    {
        DiskState = state;
    }

    public void RefreshDot(bool isDot)
    {
        IsDot = isDot;
    }
}