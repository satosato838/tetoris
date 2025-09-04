using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TitleManager _titleManager;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private NextManager _nextManager;
    [SerializeField] private Block blockPrefab;
    [SerializeField] private GridLayoutGroup _grid;

    [SerializeField] private GameObject _gameOverText;

    public const int width = 10;
    public const int height = 20;
    [SerializeField, Range(0.05f, 1f)] private float _fallInterval = 0.3f;
    const float controlInterval = 0.1f;
    DateTime lastFallTime;
    DateTime lastControlTime;
    private TetriminoType[,] _fieldTetrimino;
    TetriminoType nextTetriminoType;

    private Tetrimino _Tetrimino = new Tetrimino();

    List<List<Block>> _allblocks = new List<List<Block>>();

    private GameState _gameState = GameState.None;

    private enum GameState
    {
        None,
        Playing,
        GameOver
    }

    public enum TetriminoType
    {
        None = 0, Imino, Omino, Tmino, Smino, Zmino, Jmino, Lmino
    }
    private TetriminoType CreateTetriminoType()
    {
        return (TetriminoType)UnityEngine.Random.Range(1, 8);
    }
    private void Start()
    {
        _gamePanel.SetActive(false);
        _gameOverText.SetActive(false);
    }

    public void InitGame()
    {
        _gamePanel.SetActive(true);
        _gameOverText.SetActive(false);
        foreach (Transform child in _grid.transform)
        {
            Destroy(child.gameObject);
        }
        _allblocks = new List<List<Block>>();
        for (var y = 0; y < height; y++)
        {
            List<Block> blocks = new List<Block>();
            for (var x = 0; x < width; x++)
            {
                var block = Instantiate(blockPrefab, _grid.transform);
                block.name = $"Block_{y}_{x}";
                block.Init();
                blocks.Add(block);
            }
            _allblocks.Add(blocks);
        }

        _fieldTetrimino = new TetriminoType[height, width];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                _fieldTetrimino[y, x] = TetriminoType.None;
            }
        }
        SoundManager.Instance.PlayBGM(BGMSoundData.BGM.main);
        _gameState = GameState.Playing;
        DropTetorimino(CreateTetriminoType());
    }

    private void GameOver()
    {
        SoundManager.Instance.PlayBGM(BGMSoundData.BGM.gameover);
        _gameState = GameState.GameOver;
        _gameOverText.SetActive(true);

    }
    private void DropTetorimino(TetriminoType tetriminoType)
    {
        _Tetrimino.Init(tetriminoType);
        lastFallTime = DateTime.UtcNow;
        lastControlTime = DateTime.UtcNow;

        nextTetriminoType = CreateTetriminoType();
        _nextManager.CreateNext(nextTetriminoType);
        if (!CanMove(new Vector2Int(0, 0)))
        {
            GameOver();
        }
    }

    private void Update()
    {
        if (_gameState == GameState.Playing)
        {
            UpdatePlaying();
        }
        else if (_gameState == GameState.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                InitGame();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _titleManager.Show();
                _gamePanel.SetActive(false);
                _gameState = GameState.None;
            }
        }

    }

    private void UpdatePlaying()
    {
        var controlled = ControlTetorimino();
        var now = DateTime.UtcNow;
        if ((now - lastFallTime).TotalSeconds < _fallInterval)
        {
            if (!controlled) return;
        }
        else
        {
            lastFallTime = now;
            if (CanMove(new Vector2Int(0, 1)))
            {
                _Tetrimino.Move(new Vector2Int(0, 1));
            }
            else
            {
                var pos = _Tetrimino.GetPos();
                foreach (var p in pos)
                {
                    _fieldTetrimino[p.y, p.x] = _Tetrimino.tetriminoType;
                }
                SoundManager.Instance.PlaySE(SESoundData.SE.tyakuti);
                DeleteLines();
                DropTetorimino(nextTetriminoType);
            }
        }
        Draw();
    }

    private void DeleteLines()
    {
        for (var y = height - 1; y >= 0;)
        {
            var hasBlank = false;
            for (var x = 0; x < width; x++)
            {
                if (_fieldTetrimino[y, x] == TetriminoType.None)
                {
                    hasBlank = true;
                    break;
                }
            }
            if (hasBlank)
            {
                y--;
                continue;
            }
            for (var downY = y; downY > 0; downY--)
            {
                for (var x = 0; x < width; x++)
                {
                    _fieldTetrimino[downY, x] = _fieldTetrimino[downY - 1, x];
                }
            }
        }
    }

    private bool ControlTetorimino()
    {
        var now = DateTime.UtcNow;
        if ((now - lastControlTime).TotalSeconds < controlInterval)
        {
            return false;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            var move = new Vector2Int(-1, 0);
            if (CanMove(move))
            {
                lastControlTime = now;
                _Tetrimino.Move(move);
                SoundManager.Instance.PlaySE(SESoundData.SE.move);
                return true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            var move = new Vector2Int(1, 0);
            if (CanMove(move))
            {
                lastControlTime = now;
                _Tetrimino.Move(move);
                SoundManager.Instance.PlaySE(SESoundData.SE.move);
                return true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            var move = new Vector2Int(0, 1);
            if (CanMove(move))
            {
                lastControlTime = now;
                _Tetrimino.Move(move);
                SoundManager.Instance.PlaySE(SESoundData.SE.move);
                return true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanRollTetorimino())
            {
                lastControlTime = now;
                _Tetrimino.Roll();
                SoundManager.Instance.PlaySE(SESoundData.SE.rotation);
                return true;
            }


        }
        return false;
    }
    private bool CanRollTetorimino()
    {
        var blockPos = _Tetrimino.GetRolledBlocksPositions();
        foreach (var p in blockPos)
        {
            var x = p.x;
            var y = p.y;
            if (x < 0 || x >= width)
            {
                return false;
            }
            if (y < 0 || y >= height)
            {
                return false;
            }
            if (_fieldTetrimino[y, x] != TetriminoType.None)
            {
                return false;
            }

        }
        return true;
    }

    private bool CanMove(Vector2Int move)
    {
        var blockPos = _Tetrimino.GetPos();
        foreach (var p in blockPos)
        {
            var x = p.x + move.x;
            var y = p.y + move.y;
            if (x < 0 || x >= width)
            {
                return false;
            }
            if (y < 0 || y >= height)
            {
                return false;
            }
            if (_fieldTetrimino[y, x] != TetriminoType.None)
            {
                return false;
            }

        }
        return true;
    }

    private void Draw()
    {
        lastFallTime = DateTime.UtcNow;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var block = _allblocks[y][x];
                var type = _fieldTetrimino[y, x];
                block.SetColor(GetColor(type));
            }
        }
        var pos = _Tetrimino.GetPos();
        foreach (var p in pos)
        {
            var block = _allblocks[p.y][p.x];
            block.SetColor(GetColor(_Tetrimino.tetriminoType));
        }
    }

    public Color GetColor(TetriminoType type)
    {
        switch (type)
        {
            case TetriminoType.None:
                return Color.black;
            case TetriminoType.Imino:
                return Color.cyan;
            case TetriminoType.Omino:
                return Color.yellow;
            case TetriminoType.Tmino:
                return new Color(0.6f, 0f, 1f); // Purple
            case TetriminoType.Smino:
                return Color.green;
            case TetriminoType.Zmino:
                return Color.red;
            case TetriminoType.Jmino:
                return Color.blue;
            case TetriminoType.Lmino:
                return new Color(1f, 0.5f, 0f); // Orange
            default:
                return Color.white;
        }
    }
}
