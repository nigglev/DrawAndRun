using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using System;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RoadMeshGenerator : MonoBehaviour
{
    TrackData _track_data;
    float _road_width;

    // Start is called before the first frame update
    private void Awake()
    {
        _track_data = GetComponentInParent<TrackData>();
        _road_width = _track_data.RoadWidth;
    }

    public void BuildRoad(List<Vector3> in_points)
    {
        GetComponent<MeshFilter>().mesh = CreateRoadMesh(in_points);
    }

    private Mesh CreateRoadMesh(List<Vector3> in_points)
    {   
        int n = in_points.Count;
        Vector3[] vertices = new Vector3[n * 2];
        Vector3[] normals = new Vector3[n * 2];
        Vector2[] uvs = new Vector2[n * 2];
        int[] triangles = new int[(n - 1) * 6];

        int number_of_squares = 0;

        for (int i = 0; i < n; i++)
        {
            int vert_index = i * 2;

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

            vertices[vert_index] = in_points[i] + left * _road_width * 0.5f;
            vertices[vert_index + 1] = in_points[i] - left * _road_width * 0.5f;

            float completion_percent = i;
            uvs[vert_index] = new Vector2(0, completion_percent);
            uvs[vert_index + 1] = new Vector2(1, completion_percent);

            int tri_index = i * 6;
            if (i < n - 1)
            {
                triangles[tri_index] = vert_index;
                triangles[tri_index + 1] = vert_index + 2;
                triangles[tri_index + 2] = vert_index + 1;

                triangles[tri_index + 3] = vert_index + 1;
                triangles[tri_index + 4] = vert_index + 2;
                triangles[tri_index + 5] = vert_index + 3;

                number_of_squares++;
            }
        }


        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uvs;
        return mesh;
    }
}
