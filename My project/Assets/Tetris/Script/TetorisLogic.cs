
using System;
using System.Collections.Generic;
using UnityEngine;

public class TetorisLogic
{
    public int TetrisFieldWidth => 10;
    public int TetrisFieldHeight => 20;
    public float _fallInterval { get; private set; } = 0.3f;
    public float _controlInterval { get; private set; } = 0.1f;
    private TetriminoType nextTetriminoType;
    public DateTime LastFallTime { get; private set; }
    public DateTime LastControlTime { get; private set; }
    private GameState _gameState = GameState.None;
    private List<List<TetriminoType>> _fieldTetriminos = new List<List<TetriminoType>>();
    public List<List<TetriminoType>> _FieldTetriminos => _fieldTetriminos;
    private Tetrimino _currentTetrimino = new Tetrimino();
    public Vector2Int[] CurrentTetoriminoPos => _currentTetrimino.GetPos();
    public TetriminoType CurrentTetriminoType => _currentTetrimino.tetriminoType;
    Action _gameOverAction;

    public enum TetriminoType
    {
        None = 0, Imino, Omino, Tmino, Smino, Zmino, Jmino, Lmino
    }

    public enum GameState
    {
        None,
        Playing,
        GameOver
    }

    public GameState GetGameState()
    {
        return _gameState;
    }
    public TetriminoType GetNexTetriminoType()
    {
        return nextTetriminoType;
    }
    public void UpdateLastFallTime(DateTime time)
    {
        LastFallTime = time;
    }

    public void UpdateLastControlTime(DateTime time)
    {
        LastControlTime = time;
    }
    private TetriminoType GetRandomTetriminoType()
    {
        var values = Enum.GetValues(typeof(TetriminoType));
        return (TetriminoType)values.GetValue(new System.Random().Next(1, values.Length));
    }
    public TetriminoType CreateTetriminoType()
    {
        return GetRandomTetriminoType();
    }

    public void CreateNextTetriminoType()
    {
        nextTetriminoType = GetRandomTetriminoType();
    }

    public TetorisLogic(Action onGameOver)
    {
        GameTitle();
        _gameOverAction = onGameOver;
    }

    public void GameTitle()
    {
        _gameState = GameState.None;
    }
    public void GameStart()
    {
        _gameState = GameState.Playing;
        CreateFieldTetriMino();
        DropTetorimino(CreateTetriminoType());
    }

    public void GameOver()
    {
        _gameState = GameState.GameOver;
        _gameOverAction?.Invoke();
    }


    public void DropTetorimino(TetriminoType tetriminoType)
    {
        _currentTetrimino.Init(tetriminoType);
        LastFallTime = DateTime.UtcNow;
        LastControlTime = DateTime.UtcNow;
        CreateNextTetriminoType();
        if (!CanMove(new Vector2Int(0, 0)))
        {
            GameOver();
        }
    }
    public void FallMoveTetrimino()
    {
        MoveTetrimino(new Vector2Int(0, 1));
    }
    public void MoveTetrimino(Vector2Int pos)
    {
        _currentTetrimino.Move(pos);
    }
    public bool CanMove(Vector2Int move)
    {
        var blockPos = _currentTetrimino.GetPos();
        foreach (var p in blockPos)
        {
            var x = p.x + move.x;
            var y = p.y + move.y;
            if (x < 0 || x >= TetrisFieldWidth)
            {
                return false;
            }
            if (y < 0 || y >= TetrisFieldHeight)
            {
                return false;
            }
            if (_fieldTetriminos[y][x] != TetriminoType.None)
            {
                return false;
            }

        }
        return true;
    }

    public void RollTetrimino()
    {
        _currentTetrimino.Roll();
    }
    public bool CanRollTetorimino()
    {
        var blockPos = _currentTetrimino.GetRolledBlocksPositions();
        foreach (var p in blockPos)
        {
            var x = p.x;
            var y = p.y;
            if (x < 0 || x >= TetrisFieldWidth)
            {
                return false;
            }
            if (y < 0 || y >= TetrisFieldHeight)
            {
                return false;
            }
            if (_FieldTetriminos[y][x] != TetriminoType.None)
            {
                return false;
            }

        }
        return true;
    }

    public void CreateFieldTetriMino()
    {
        _fieldTetriminos = new List<List<TetriminoType>>();
        for (var y = 0; y < TetrisFieldHeight; y++)
        {
            List<TetriminoType> row = new List<TetriminoType>();
            for (var x = 0; x < TetrisFieldWidth; x++)
            {
                row.Add(TetriminoType.None);
            }
            _fieldTetriminos.Add(row);
        }
    }

    public void FallEnd()
    {
        var pos = _currentTetrimino.GetPos();
        SoundManager.Instance.PlaySE(SESoundData.SE.tyakuti);
        foreach (var p in pos)
        {
            _fieldTetriminos[p.y][p.x] = _currentTetrimino.tetriminoType;
        }

        DeleteLines();
        DropTetorimino(GetNexTetriminoType());
    }

    public void DeleteLines()
    {
        for (var y = TetrisFieldHeight - 1; y >= 0;)
        {
            var hasBlank = false;
            for (var x = 0; x < TetrisFieldWidth; x++)
            {
                if (_fieldTetriminos[y][x] == TetriminoType.None)
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
                for (var x = 0; x < TetrisFieldWidth; x++)
                {
                    _fieldTetriminos[downY][x] = _fieldTetriminos[downY - 1][x];
                }
            }
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
