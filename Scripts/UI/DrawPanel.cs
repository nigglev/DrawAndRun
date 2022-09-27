using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DrawPanel : MonoBehaviour
{
    private Texture2D _texture2D;
    private int _texture_width;
    private int _texture_height;
    private Color[] _old_colors;
    private Color[] _new_colors;
    [SerializeField]
    private int _pixel_width = 5;

    private RectTransform _rect;
    private float _rect_width;
    private float _rect_height;

    private DrawInput _input;
    private Mouse _mouse;

    private bool _isPressed = false;

    private List<Vector2> _normalized_pixel_points;

    private Vector2Int _current_pixel;
    private Vector2Int _prev_pixel;
    private int _threshold = 1;

    private void Awake()
    {   

        _texture2D = GetComponent<Image>().sprite.texture;
        _texture_width = _texture2D.width;
        _texture_height = _texture2D.height;
        _old_colors = _texture2D.GetPixels();
        _new_colors = SetUpPixelColorArray();

        _rect = GetComponent<RectTransform>();
        _rect_width = _rect.sizeDelta.x * _rect.localScale.x;
        _rect_height = _rect.sizeDelta.y * _rect.localScale.y;

        _normalized_pixel_points = new List<Vector2>();

        _input = new DrawInput();
        _mouse = Mouse.current;
        _input.DrawPanel.Draw.performed += context => Pressed();
        _input.DrawPanel.Draw.canceled += context => Released();


    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
        ReturnDefaultImage();
    }


    private void Update()
    {
        if (_isPressed)
        {
            _prev_pixel = _current_pixel;
            _current_pixel = GetImagePixelCoord();

            if (_current_pixel.x < 0 || _current_pixel.y < 0)
                return;

            if (_normalized_pixel_points.Count == 0)
            {
                _normalized_pixel_points.Add(new Vector2(_current_pixel.x / (float)_texture_width, _current_pixel.y / (float)_texture_height));
                return;
            }

            if (Vector2Int.Distance(_current_pixel, _prev_pixel) >= _threshold)
                _normalized_pixel_points.Add(new Vector2(_current_pixel.x / (float)_texture_width, _current_pixel.y / (float)_texture_height));

            if (_normalized_pixel_points.Count < 2)
                return;

            Line line = LineBuilder.GetLine(_current_pixel, _prev_pixel);
            DrawLine(line);
        }

    }


    private void DrawLine(Line in_line)
    {
        List<Vector2Int> line_points = in_line.GetLinePoints();
        for (int i = 0; i < line_points.Count; i++)
            _texture2D.SetPixels(line_points[i].x - _pixel_width / 2, line_points[i].y - _pixel_width / 2, _pixel_width, _pixel_width, _new_colors);
        _texture2D.Apply();

    }

    private Vector2Int GetImagePixelCoord()
    {   
        Vector2 out_pos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, _mouse.position.ReadValue(), null, out out_pos);
        out_pos.x = out_pos.x + _rect_width / 2;
        out_pos.y = out_pos.y + _rect_height / 2;
        if (IsPointOutOfBounds(out_pos))
            return Vector2Int.down;

        int pixel_x = Mathf.FloorToInt(out_pos.x / _rect_width * _texture_width);
        int pixel_y = Mathf.FloorToInt(out_pos.y / _rect_height * _texture_height);

        return new Vector2Int(pixel_x, pixel_y);
    }

    private Color[] SetUpPixelColorArray()
    {
        Color[] new_colors = new Color[_pixel_width * _pixel_width];

        for (int i = 0; i < new_colors.Length; i++)
        {
            new_colors[i] = Color.blue;
        }

        return new_colors;
    }
    private void ReturnDefaultImage()
    {
        _texture2D.SetPixels(_old_colors);
        _texture2D.Apply();
    }

    private void Pressed()
    {
        _isPressed = true;
        _normalized_pixel_points.Clear();


    }

    private void Released()
    {
        _isPressed = false;
        if (_normalized_pixel_points.Count == 0)
            return;
        CGameManager.Instance.SetPointsFromDrawPanel(_normalized_pixel_points);
        _normalized_pixel_points.Clear();
        ReturnDefaultImage();
    }


    private bool IsPointOutOfBounds(Vector2 in_coord)
    {
        if (in_coord.x <= _rect_width * 0.01f || in_coord.x >= _rect_width - _texture_width * 0.01f || in_coord.y <= _rect_height * 0.01f || in_coord.y >= _rect_height - _rect_height * 0.01f)
            return true;
        return false;

    }


}
