using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameManager
{
    static CGameManager game_manager;

    private UIManager _ui;
    private UnitGroupController _ugc;
    private TrackData _track_data;

    //FIXME scriptable object description
    private int _romb_reward_points;

    private int _points_sum;
    private bool _is_level_started;

    public delegate void LevelStartHandler();
    public event LevelStartHandler OnLevelStart;

    public delegate void LevelFinishHandler();
    public event LevelStartHandler OnLevelFinish;

    public delegate void AllUnitsDeadHandler();
    public event AllUnitsDeadHandler OnUnitsDead;

    CGameManager()
    {
        _is_level_started = false;
        _points_sum = 0;
    }

    public static CGameManager Instance
    {
        get
        {
            if (game_manager == null)
                game_manager = new CGameManager();
            return game_manager;
        }
    }


    public  void SetUIManager(UIManager in_UIManager)
    {
        _ui = in_UIManager;
    }


    public void SetUnitGroupController(UnitGroupController in_ugc)
    {
        _ugc = in_ugc;
    }

    public void SetRoadData(TrackData in_td)
    {
        _track_data = in_td;
    }

    public void StartLevel()
    {
        OnLevelStart?.Invoke();
        _is_level_started = true;
        CDebug.Trace(ETraceLevel.Trace, "Level Started");
    }

    public void LevelFinished()
    {
        OnLevelFinish?.Invoke();
        CDebug.Trace(ETraceLevel.Trace, "LevelFinished");
    }

    public void AllUnitsDead()
    {
        OnUnitsDead?.Invoke();
        CDebug.Trace(ETraceLevel.Trace, "All Units Dead");
    }

    public void SetPointsFromDrawPanel(List<Vector2> in_points)
    {
        if (!_is_level_started)
            StartLevel();
        _ugc.ProjectDrawPanelPointsOnUnitArea(in_points);
    }

    public float GetRoadWidth()
    {
        return _track_data.RoadWidth - _track_data.BorderLeftWidth - _track_data.BorderRightWidth;
    }

    public void AddPoints(int in_points)
    {
        _points_sum += in_points;
        _ui.AddPoints(_points_sum);
    }
}
