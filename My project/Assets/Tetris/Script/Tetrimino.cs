using System.Collections.Generic;
using UnityEngine;

public class Tetrimino
{
    private int partternYLength = 4;
    private int partternXLength = 4;
    private int _rollParttern = 0;
    private Vector2Int basePos;
    public TetorisLogic.TetriminoType tetriminoType;
    private int RollPartternNum
    {
        get { return tetriminoType == TetorisLogic.TetriminoType.Omino ? 1 : 4; }
    }
    private int NextRollParttern
    {
        get { return _rollParttern + 1 < RollPartternNum ? _rollParttern + 1 : 0; }
    }
    public void Init(TetorisLogic.TetriminoType type)
    {
        tetriminoType = type;
        basePos = Vector2Int.zero;
        _rollParttern = 0;
    }
    public Vector2Int[] GetPos()
    {
        return GetPos(_rollParttern);
    }

    public Vector2Int[] GetPos(int rollParttern)
    {
        var position = new Vector2Int[4];
        var parttern = TetriminoPatterns[tetriminoType];
        var positionIndex = 0;
        for (var y = 0; y < partternYLength; y++)
        {
            for (var x = 0; x < partternXLength; x++)
            {
                if (parttern[rollParttern, y, x] == 1)
                {
                    position[positionIndex] = new Vector2Int(basePos.x + x, basePos.y + y);
                    positionIndex++;
                }
            }
        }
        return position;
    }

    public void Move(Vector2Int move)
    {
        basePos.Set(basePos.x + move.x, basePos.y + move.y);
    }

    public Vector2Int[] GetRolledBlocksPositions()
    {
        return GetPos(NextRollParttern);
    }

    public void Roll()
    {
        _rollParttern = NextRollParttern;
    }

    public static readonly Dictionary<TetorisLogic.TetriminoType, int[,,]> TetriminoPatterns = new Dictionary<TetorisLogic.TetriminoType, int[,,]>
    {
        { TetorisLogic.TetriminoType.Imino, new int[,,]{
            {
                {0,0,0,0},
                {1,1,1,1},
                {0,0,0,0},
                {0,0,0,0}
            },
            {
                {0,0,1,0},
                {0,0,1,0},
                {0,0,1,0},
                {0,0,1,0}
            },
            {
                {0,0,0,0},
                {0,0,0,0},
                {1,1,1,1},
                {0,0,0,0}
            },{
                {0,1,0,0},
                {0,1,0,0},
                {0,1,0,0},
                {0,1,0,0}
            }
         } },
        { TetorisLogic.TetriminoType.Omino, new int[,,]{
            {
                {0,1,1,0},
                {0,1,1,0},
                {0,0,0,0},
                {0,0,0,0}
            }
         } },
         { TetorisLogic.TetriminoType.Smino, new int[,,]{
            {
                {0,1,1,0},
                {1,1,0,0},
                {0,0,0,0},
                {0,0,0,0}
            },
            {
                {0,1,0,0},
                {0,1,1,0},
                {0,0,1,0},
                {0,0,0,0}
            },
            {
                {0,0,0,0},
                {0,1,1,0},
                {1,1,0,0},
                {0,0,0,0}
            },{
                {1,0,0,0},
                {1,1,0,0},
                {0,1,0,0},
                {0,0,0,0}
            }
         }  },
         { TetorisLogic.TetriminoType.Zmino, new int[,,]{
            {
                {1,1,0,0},
                {0,1,1,0},
                {0,0,0,0},
                {0,0,0,0}
            },
            {
                {0,0,1,0},
                {0,1,1,0},
                {0,1,0,0},
                {0,0,0,0}
            },
            {
                {0,0,0,0},
                {1,1,0,0},
                {0,1,1,0},
                {0,0,0,0}
            },
            {
                {0,1,0,0},
                {1,1,0,0},
                {1,0,0,0},
                {0,0,0,0}
            }
         }  },
         { TetorisLogic.TetriminoType.Jmino, new int[,,]{
            {
                {1,0,0,0},
                {1,1,1,0},
                {0,0,0,0},
                {0,0,0,0}
            },
            {
                {0,1,1,0},
                {0,1,0,0},
                {0,1,0,0},
                {0,0,0,0}
            },
            {
                {0,0,0,0},
                {1,1,1,0},
                {0,0,1,0},
                {0,0,0,0}
            },{
                {0,1,0,0},
                {0,1,0,0},
                {1,1,0,0},
                {0,0,0,0}
            }
         }  },
         { TetorisLogic.TetriminoType.Lmino, new int[,,]{
            {
                {0,0,1,0},
                {1,1,1,0},
                {0,0,0,0},
                {0,0,0,0}
            },
            {
                {0,1,0,0},
                {0,1,0,0},
                {0,1,1,0},
                {0,0,0,0}
            },
            {
                {0,0,0,0},
                {1,1,1,0},
                {1,0,0,0},
                {0,0,0,0}
            },{
                {1,1,0,0},
                {0,1,0,0},
                {0,1,0,0},
                {0,0,0,0}
            }
         }  },
         { TetorisLogic.TetriminoType.Tmino, new int[,,]{
            {
                {0,1,0,0},
                {1,1,1,0},
                {0,0,0,0},
                {0,0,0,0}
            },
            {
                {0,1,0,0},
                {0,1,1,0},
                {0,1,0,0},
                {0,0,0,0}
            },
            {
                {0,0,0,0},
                {1,1,1,0},
                {0,1,0,0},
                {0,0,0,0}
            },{
                {0,1,0,0},
                {1,1,0,0},
                {0,1,0,0},
                {0,0,0,0}
            }
         }  },

    };


}
