using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Line
{
    List<Vector2Int> _line_points;
    public Line(List<Vector2Int> in_line_points)
    {
        _line_points = in_line_points;
    }

    public List<Vector2Int> GetLinePoints()
    {
        return _line_points;
    }
}

//FIXME static
public class LineBuilder
{
    public static Line GetLine(Vector2Int in_begin_pos, Vector2Int in_end_pos)
    {
        List<Vector2Int> line_points = new List<Vector2Int>();

        float distance = Vector2Int.Distance(in_begin_pos, in_end_pos);
        int dir_x = Math.Sign(in_end_pos.x - in_begin_pos.x);
        int dir_y = Math.Sign(in_end_pos.y - in_begin_pos.y);
        Vector2Int next_pixel = in_begin_pos;
        line_points.Add(next_pixel);

        float t = 0f;
        float t_x = 0f;
        float t_y = 0f;

        float w_x = 0.5f;
        float w_y = 0.5f;

        float delta_x = Mathf.Abs(in_begin_pos.x - in_end_pos.x);
        float delta_y = Mathf.Abs(in_begin_pos.y - in_end_pos.y);

        float rd = Mathf.Sqrt(0.5f * 0.5f * 2);

        float lx = distance / delta_x;
        float ly = distance / delta_y;

        float inv_lx = 1 / lx;
        float inv_ly = 1 / ly;

        while (t < distance - rd)
        {
            t_x = lx * w_x;
            t_y = ly * w_y;

            if (t_x == t_y)
            {
                next_pixel.x += dir_x;
                next_pixel.y += dir_y;

                t += t_x;
                w_x = 1;
                w_y = 1;
            }

            if (t_x < t_y)
            {
                next_pixel.x += dir_x;
                t += t_x;
                w_x = 1;
                w_y -= t_x * inv_ly;
            }
            else
            {
                next_pixel.y += dir_y;
                t += t_y;
                w_y = 1;
                w_x -= t_y * inv_lx;
            }

            line_points.Add(next_pixel);
        }

        return new Line(line_points);
    }
}
