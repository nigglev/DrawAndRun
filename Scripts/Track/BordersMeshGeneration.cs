using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BordersMeshGeneration : MonoBehaviour
{

    TrackData _track_data;
    BorderLeftMeshGenerator _left_border;
    BorderRightMeshGenerator _right_border;

    List<Vector3> _points;
    float _road_width;
    float _border_left_width;
    float _border_left_height;
    float _border_right_width;
    float _border_right_height;

    Vector3[] _vertices_left;
    int[] _triangles_left;

    Vector3[] _vertices_right;
    int[] _triangles_right;

    Vector2[] _uvs;

    private void Awake()
    {
        _track_data = GetComponentInParent<TrackData>();
        _left_border = GetComponentInChildren<BorderLeftMeshGenerator>();
        _right_border = GetComponentInChildren<BorderRightMeshGenerator>();
        
        
        _road_width = _track_data.RoadWidth;
        _border_left_width = _track_data.BorderLeftWidth;
        _border_left_height = _track_data.BorderLeftHeight;
        _border_right_width = _track_data.BorderRightWidth;
        _border_right_height = _track_data.BorderRightHeight;
    }

    public void CreateBorderMesh(List<Vector3> in_points)
    {
        _points = in_points;
        int n = _points.Count;

        Mesh mesh_left = new Mesh();
        Mesh mesh_right = new Mesh();


        
        _vertices_left = new Vector3[n * 4];
        _triangles_left = new int[n * 3 * 8];
        
        _vertices_right = new Vector3[n * 4];
        _triangles_right = new int[n * 3 * 8];

        _uvs = new Vector2[n * 4];


        for (int i = 0; i < n; i++)
        {
            Vector3 forward = Vector3.zero;

            int st = 0;
            if (i > 0)
            {
                Vector3 p1 = in_points[i];
                Vector3 p0 = in_points[i - 1];
                forward += (p1 - p0).normalized;
                st++;
            }

            if (i < n - 1)
            {
                Vector3 p1 = in_points[i + 1];
                Vector3 p0 = in_points[i];
                forward += (p1 - p0).normalized;
                st++;
            }

            forward /= st;


            Vector3 up = Vector3.up;
            Vector3 left = -1 * Vector3.Cross(up, forward);

            int vert_index = i * 4;
            int tri_index = i * 3 * 8;

            LeftBorderMesh(left, vert_index, tri_index, i);
            RightBorderMesh(left, vert_index, tri_index, i);

            float completion_percent = i;
            _uvs[vert_index] = new Vector2(0, completion_percent);
            _uvs[vert_index + 1] = new Vector2(0, completion_percent);
            _uvs[vert_index + 2] = new Vector2(1, completion_percent);
            _uvs[vert_index + 3] = new Vector2(1, completion_percent);
        }

       

        mesh_left.vertices = _vertices_left;
        mesh_left.triangles = _triangles_left;
        mesh_left.uv = _uvs;

        mesh_right.vertices = _vertices_right;
        mesh_right.triangles = _triangles_right;
        mesh_right.uv = _uvs;

        var left_border = GetComponentInChildren<BorderLeftMeshGenerator>();
        var right_border = GetComponentInChildren<BorderRightMeshGenerator>();

        left_border.mesh = mesh_left;
        right_border.mesh = mesh_right;
    }

    private void LeftBorderMesh(Vector3 in_left, int in_vert_index, int in_tri_index, int in_index)
    {
        Vector3 v1 = _points[in_index] + in_left * _road_width * 0.5f;
        Vector3 v2 = v1 - in_left * _border_left_width;
        Vector3 v3 = new Vector3(v1.x, v1.y + _border_left_height, v1.z);
        Vector3 v4 = new Vector3(v2.x, v2.y + _border_left_height, v2.z);

        _vertices_left[in_vert_index] = v1;
        _vertices_left[in_vert_index + 1] = v2;
        _vertices_left[in_vert_index + 2] = v3;
        _vertices_left[in_vert_index + 3] = v4;



        _triangles_left[in_tri_index] = in_vert_index;
        _triangles_left[in_tri_index + 1] = in_vert_index + 2;
        _triangles_left[in_tri_index + 2] = in_vert_index + 1;

        _triangles_left[in_tri_index + 3] = in_vert_index + 2;
        _triangles_left[in_tri_index + 4] = in_vert_index + 3;
        _triangles_left[in_tri_index + 5] = in_vert_index + 1;

        if (in_index > 0)
        {
            _triangles_left[in_tri_index + 6] = in_vert_index - 2;
            _triangles_left[in_tri_index + 7] = in_vert_index + 2;
            _triangles_left[in_tri_index + 8] = in_vert_index - 1;

            _triangles_left[in_tri_index + 9] = in_vert_index + 2;
            _triangles_left[in_tri_index + 10] = in_vert_index + 3;
            _triangles_left[in_tri_index + 11] = in_vert_index - 1;

            _triangles_left[in_tri_index + 12] = in_vert_index - 1;
            _triangles_left[in_tri_index + 13] = in_vert_index + 3;
            _triangles_left[in_tri_index + 14] = in_vert_index - 3;

            _triangles_left[in_tri_index + 15] = in_vert_index + 3;
            _triangles_left[in_tri_index + 16] = in_vert_index + 1;
            _triangles_left[in_tri_index + 17] = in_vert_index - 3;

            _triangles_left[in_tri_index + 18] = in_vert_index - 2;
            _triangles_left[in_tri_index + 19] = in_vert_index - 4;
            _triangles_left[in_tri_index + 20] = in_vert_index;

            _triangles_left[in_tri_index + 21] = in_vert_index;
            _triangles_left[in_tri_index + 22] = in_vert_index + 2;
            _triangles_left[in_tri_index + 23] = in_vert_index - 2;
        }
    }


    private void RightBorderMesh(Vector3 in_left, int in_vert_index, int in_tri_index, int in_index)
    {
        Vector3 v1 = _points[in_index] - in_left * _road_width * 0.5f;
        Vector3 v2 = v1 + in_left * _border_right_width;
        Vector3 v3 = new Vector3(v1.x, v1.y + _border_right_height, v1.z);
        Vector3 v4 = new Vector3(v2.x, v2.y + _border_right_height, v2.z);

        _vertices_right[in_vert_index] = v1;
        _vertices_right[in_vert_index + 1] = v2;
        _vertices_right[in_vert_index + 2] = v3;
        _vertices_right[in_vert_index + 3] = v4;

        _triangles_right[in_tri_index] = in_vert_index + 1;
        _triangles_right[in_tri_index + 1] = in_vert_index + 3;
        _triangles_right[in_tri_index + 2] = in_vert_index;

        _triangles_right[in_tri_index + 3] = in_vert_index + 3;
        _triangles_right[in_tri_index + 4] = in_vert_index + 2;
        _triangles_right[in_tri_index + 5] = in_vert_index;

        if (in_index > 0)
        {
            _triangles_right[in_tri_index + 6] = in_vert_index - 1;
            _triangles_right[in_tri_index + 7] = in_vert_index - 3;
            _triangles_right[in_tri_index + 8] = in_vert_index + 1;

            _triangles_right[in_tri_index + 9] = in_vert_index + 1;
            _triangles_right[in_tri_index + 10] = in_vert_index + 3;
            _triangles_right[in_tri_index + 11] = in_vert_index - 1;

            _triangles_right[in_tri_index + 12] = in_vert_index - 2;
            _triangles_right[in_tri_index + 13] = in_vert_index - 1;
            _triangles_right[in_tri_index + 14] = in_vert_index + 3;

            _triangles_right[in_tri_index + 15] = in_vert_index + 3;
            _triangles_right[in_tri_index + 16] = in_vert_index + 2;
            _triangles_right[in_tri_index + 17] = in_vert_index - 2;

            _triangles_right[in_tri_index + 18] = in_vert_index - 4;
            _triangles_right[in_tri_index + 19] = in_vert_index - 2;
            _triangles_right[in_tri_index + 20] = in_vert_index + 2;

            _triangles_right[in_tri_index + 21] = in_vert_index + 2;
            _triangles_right[in_tri_index + 22] = in_vert_index;
            _triangles_right[in_tri_index + 23] = in_vert_index - 4;


        }
    }


}
