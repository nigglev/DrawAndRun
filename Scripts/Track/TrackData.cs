using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

//[ExecuteInEditMode]
public class TrackData : MonoBehaviour
{

    SplineComputer _spline_computer;
    private List<Vector3> _spline_points;

    [SerializeField]
    [Range(.05f, 1.5f)]
    private float _road_width = 0.11f;
    [SerializeField]
    [Range(.005f, 0.5f)]
    private float _border_left_width;
    [SerializeField]
    [Range(.005f, 0.5f)]
    private float _border_left_height;
    [SerializeField]
    [Range(.005f, 0.5f)]
    private float _border_right_width;
    [SerializeField]
    [Range(.005f, 0.5f)]
    private float _border_right_height;

    // Start is called before the first frame update
    private void Awake()
    {
        CGameManager.Instance.SetRoadData(this);
        _spline_points = GetPointsAcrossSpline();
    }

    public List<Vector3> SplinePoints
    {
        get { return _spline_points; }
    }
    public float RoadWidth
    {
        get { return _road_width; }
    }

    public float BorderLeftWidth
    {
        get { return _border_left_width; }
    }

    public float BorderLeftHeight
    {
        get { return _border_left_height; }
    }

    public float BorderRightWidth
    {
        get { return _border_right_width; }
    }

    public float BorderRightHeight
    {
        get { return _border_right_height; }
    }

    private List<Vector3> GetPointsAcrossSpline()
    {
        _spline_computer = GetComponent<SplineComputer>();
        if (CDebug.AssertNull(_spline_computer, "Spline Computer in NULL"))
            return null;
        Vector3[] p = new Vector3[0];
        _spline_computer.EvaluatePositions(ref p, 0, 1);
        Vector3 dir = p[p.Length - 1] - p[p.Length - 2];
        List<Vector3> points = new List<Vector3>();
        points.AddRange(p);
        points.Add(p[p.Length - 1] + dir);
        return points;
    }
}
