using System.Collections.Generic;

public partial class GameLogic
{
    public const int TurnPeriod = 500;

    public GameLogic(int memberId, IReadOnlyList<int> moveList)
    {
        _field = new Cell[6*N*N];
        _agents = new Agent[6];
        _turn = 0;
        _move = new int[6];
        _score = new int[3];
        _area = new int[3];
        _special = new int[6];
        for (var i = 0; i < _field.Length; i++)
        {
            _field[i] = new Cell { Owner = -1, Val = 0 };
        }
        for (var i = 0; i < 3; i++)
        {
            _area[i] = 2;
        }
        for (var i = 0; i < 6; i++)
        {
            _agents[i] = new Agent { I = i, J = N / 2, K = N / 2, D = 0 };
            var fi = FieldIdx(i, N / 2, N / 2);
            _field[fi].Owner = i < 3 ? i : (5 - i);
            _field[fi].Val = 2;
            _special[i] = 1;
        }

        Progress(memberId, moveList);
    }

    public static int[] ConvertMove(int memberId, int[] a)
    {
        var res = new int[6];
        for (var idx = 0; idx < 6; idx++)
        {
            res[idx] = a[Func1(memberId, idx)];
        }

        return res;
    }

    public static string[] ConvertUserIds(int memberId, string[] a)
    {
        if (a.Length < 3) return a;
        var res = new string[3];
        for (var idx = 0; idx < 3; idx++)
        {
            res[idx] = a[Func1(memberId, idx)];
        }

        return res;
    }

    public int[] GetScoreToSave(int memberId)
    {
        var res = new int[3];
        for (var idx = 0; idx < 3; idx++)
        {
            res[Func1(memberId, idx)] = _score[idx];
        }

        return res;
    }

    public Dictionary<string, object> GetResponseData(long now)
    {
        var field = new int[6][][][];
        var agent = new int[6][];
        for (var i = 0; i < 6; i++)
        {
            agent[i] = new[]{ _agents[i].I, _agents[i].J, _agents[i].K, _agents[i].D };
            field[i] = new int[N][][];
            for (var j = 0; j < N; j++)
            {
                field[i][j] = new int[N][];
                for (var k = 0; k < N; k++)
                {
                    var c = _field[FieldIdx(i,j,k)];
                    field[i][j][k] = new[]{ c.Owner, c.Val };
                }
            }
        }

        return new Dictionary<string, object>
        {
            ["status"] = "ok",
            ["now"] = now,
            ["turn"] = _turn,
            ["move"] = _move,
            ["score"] = _score,
            ["field"] = field,
            ["agent"] = agent,
            ["special"] = _special,
        };
    }
}
