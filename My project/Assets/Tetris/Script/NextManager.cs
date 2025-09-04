using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextManager : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _grid;
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private GameManager _blockManager;
    List<List<Block>> _allblocks = new List<List<Block>>();
    private GameManager.TetriminoType[,] _nextTetrimino;
    private Tetrimino _NextMino = new Tetrimino();

    public const int width = 4;
    public const int height = 4;
    private void Start()
    {
        Init();
    }

    private void Init()
    {
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
                var block = Instantiate(_blockPrefab, _grid.transform);
                block.name = $"Block_{y}_{x}";
                block.Init();
                blocks.Add(block);
            }
            _allblocks.Add(blocks);
        }
        Reset();
    }

    private void Reset()
    {
        foreach (var blocks in _allblocks)
        {
            foreach (var block in blocks)
            {
                block.Init();
            }
        }
    }

    public void CreateNext(GameManager.TetriminoType tetriminoType)
    {
        Reset();
        _nextTetrimino = new GameManager.TetriminoType[height, width];
        _NextMino.Init(tetriminoType);
        Draw();
    }

    private void Draw()
    {
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var block = _allblocks[y][x];
                var type = _nextTetrimino[y, x];
                block.SetColor(_blockManager.GetColor(type));
            }
        }
        var pos = _NextMino.GetPos();
        foreach (var p in pos)
        {
            var block = _allblocks[p.y][p.x];
            block.SetColor(_blockManager.GetColor(_NextMino.tetriminoType));
        }
    }

}
