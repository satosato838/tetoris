using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] private TitleView _titleView;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private NextView _nextView;
    [SerializeField] private Block blockPrefab;
    [SerializeField] private GridLayoutGroup _grid;



    [SerializeField] private GameObject _gameOverText;
    private TetorisLogic _tetorisLogic;

    private int width => _tetorisLogic.TetrisFieldWidth;
    private int height => _tetorisLogic.TetrisFieldHeight;

    List<List<Block>> _allblocks = new List<List<Block>>();

    private void Start()
    {
        _gamePanel.SetActive(false);
        _gameOverText.SetActive(false);
    }

    public void InitGame()
    {
        _tetorisLogic = new TetorisLogic(() => ShowGameOverResult());

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

        SoundManager.Instance.PlayBGM(BGMSoundData.BGM.main);
        _tetorisLogic.GameStart();
        _nextView.CreateNext(_tetorisLogic.GetNexTetriminoType());
    }

    private void ShowGameOverResult()
    {
        SoundManager.Instance.PlayBGM(BGMSoundData.BGM.gameover);
        _gameOverText.SetActive(true);
    }

    private void Update()
    {
        if (_tetorisLogic == null) return;
        if (_tetorisLogic.GetGameState() == TetorisLogic.GameState.Playing)
        {
            UpdatePlaying();
        }
        else if (_tetorisLogic.GetGameState() == TetorisLogic.GameState.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                InitGame();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _titleView.Show();
                _gamePanel.SetActive(false);
                _tetorisLogic.GameTitle();
            }
        }
    }

    private void UpdatePlaying()
    {
        var controlled = ControlTetorimino();
        var now = DateTime.UtcNow;
        if ((now - _tetorisLogic.LastFallTime).TotalSeconds < _tetorisLogic._fallInterval)
        {
            if (!controlled) return;
        }
        else
        {
            _tetorisLogic.UpdateLastFallTime(now);
            if (_tetorisLogic.CanMove(new Vector2Int(0, 1)))
            {
                _tetorisLogic.FallMoveTetrimino();
            }
            else
            {
                _tetorisLogic.FallEnd();
                _nextView.CreateNext(_tetorisLogic.GetNexTetriminoType());
            }
        }
        Draw();
    }
    private bool ControlTetorimino()
    {
        var now = DateTime.UtcNow;
        if ((now - _tetorisLogic.LastControlTime).TotalSeconds < _tetorisLogic._controlInterval)
        {
            return false;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            var move = new Vector2Int(-1, 0);
            if (_tetorisLogic.CanMove(move))
            {
                _tetorisLogic.UpdateLastFallTime(now);
                _tetorisLogic.MoveTetrimino(move);
                SoundManager.Instance.PlaySE(SESoundData.SE.move);
                return true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            var move = new Vector2Int(1, 0);
            if (_tetorisLogic.CanMove(move))
            {
                _tetorisLogic.UpdateLastFallTime(now);
                _tetorisLogic.MoveTetrimino(move);
                SoundManager.Instance.PlaySE(SESoundData.SE.move);
                return true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            var move = new Vector2Int(0, 1);
            if (_tetorisLogic.CanMove(move))
            {
                _tetorisLogic.UpdateLastFallTime(now);
                _tetorisLogic.MoveTetrimino(move);
                SoundManager.Instance.PlaySE(SESoundData.SE.move);
                return true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_tetorisLogic.CanRollTetorimino())
            {
                _tetorisLogic.UpdateLastControlTime(now);
                _tetorisLogic.RollTetrimino();
                SoundManager.Instance.PlaySE(SESoundData.SE.rotation);
                return true;
            }
        }
        return false;
    }

    private void Draw()
    {
        _tetorisLogic.UpdateLastFallTime(DateTime.UtcNow);
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var block = _allblocks[y][x];
                var type = _tetorisLogic._FieldTetriminos[y][x];
                block.SetColor(GetColor(type));
            }
        }
        foreach (var p in _tetorisLogic.CurrentTetoriminoPos)
        {
            var block = _allblocks[p.y][p.x];
            block.SetColor(GetColor(_tetorisLogic.CurrentTetriminoType));
        }
    }
    public Color GetColor(TetorisLogic.TetriminoType tetriminoType) => _tetorisLogic.GetColor(tetriminoType);


}
