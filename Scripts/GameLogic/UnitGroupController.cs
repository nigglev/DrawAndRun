using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitGroupController : MonoBehaviour
{   
    [SerializeField]
    private float _default_unit_space;
    [SerializeField]
    [Range(1, 10)]
    private int _default_number_of_units_in_row;
    [SerializeField]
    [Range(1, 30)]
    private int _unit_count;
    [SerializeField]
    private GameObject _unit_to_spawn;

    private List<Unit> _units;
    private ulong _unit_id_counter;

    private SplineFollower _spline_follower;

    private float _area_width;
    private float _area_height;
    public ulong GetNewId() { return ++_unit_id_counter; }

    private void Awake()
    {
        CGameManager.Instance.SetUnitGroupController(this);
        CGameManager.Instance.OnLevelStart += LevelStart;
        _units = new List<Unit>();

    }

    void Start()
    {   
        
        _spline_follower = GetComponent<SplineFollower>();
        if (CDebug.AssertNull(_spline_follower, "error"))
            return;


        _spline_follower.followSpeed = 0f;
        _spline_follower.onEndReached += FinishReached;

        _area_width = CGameManager.Instance.GetRoadWidth();
        _area_height = _area_width;

        for (int i = 0; i < _unit_count; i++)
        {
            AddNewUnit(Vector3.zero);
        }

        DefaultPositions();
    }

    private void LevelStart()
    {
        _spline_follower.followSpeed = 0.1f;
    }

    private void FinishReached()
    {
        DefaultPositions();
        CGameManager.Instance.LevelFinished();
    }

    public void AddNewUnit(Vector3 in_pos)
    {
        GameObject unit = Instantiate(_unit_to_spawn, in_pos, Quaternion.identity, transform);
        Unit u = unit.GetComponent<Unit>();

        u.SetID(GetNewId());
        u.OnDeath += OnUnitDeath;
        _units.Add(u);

        CDebug.Trace(ETraceLevel.Trace, $"Number of units = {_units.Count}");
    }
    public void ProjectDrawPanelPointsOnUnitArea(List<Vector2> in_points)
    {
        int jump = Mathf.FloorToInt(in_points.Count / _units.Count);
        int ind = 0;

        foreach (Unit u in _units)
        {
            float new_pos_x = -_area_width / 2 + in_points[ind].x * _area_width;
            float new_pos_z = -_area_height / 2 + in_points[ind].y * _area_height;
            u.transform.localPosition = new Vector3(new_pos_x, 0, new_pos_z);
            ind += jump;
        }
    }


    private void DefaultPositions()
    {
        int units_in_row = _default_number_of_units_in_row;
        float unit_space = _default_unit_space;

        _unit_count = _units.Count;

        int rows = Mathf.CeilToInt(_unit_count / (float)units_in_row);
        int placed_units = 0;

        for (int j = 0; j < rows; j++)
        {
            if ((_unit_count - placed_units) < units_in_row)
                units_in_row = _unit_count - placed_units;

            float row_width = (units_in_row - 1) * unit_space;
            for (int i = 0; i < units_in_row; i++)
            {
                _units[placed_units].transform.localPosition = new Vector3((-row_width / 2) + i * unit_space, 0.001f, j * unit_space);
                _units[placed_units].transform.localRotation = Quaternion.identity;
                placed_units++;
            }
        }

    }
    private void RemoveLastAddedUnit()
    {
        Destroy(_units[_units.Count - 1].gameObject);
        _units.RemoveAt(_units.Count - 1);
    }
    private void OnUnitDeath(ulong in_unit_id)
    {   
        var ind = _units.FindIndex(x => x.ID == in_unit_id);
        Destroy(_units[ind].gameObject);
        _units.RemoveAt(ind);
        CDebug.Trace(ETraceLevel.Trace, $"{in_unit_id} is dead; Current number of units = {_units.Count}");

        if(_units.Count == 0)
            CGameManager.Instance.AllUnitsDead();
        
    }
}