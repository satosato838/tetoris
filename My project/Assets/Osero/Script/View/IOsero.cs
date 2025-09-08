
public interface IOsero
{
    public OseroDisk[,] BoardDisks { get; }

    public void GameStart();
    public void PlaceDisk((int, int) pos);
    public void TurnDisk();
    public bool IsGameEnd();
    public void GameOver();
}
