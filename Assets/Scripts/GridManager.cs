using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] int row, column;
    public float square_offset;
    public GameObject grid_square;
    public Vector2 start_position = new Vector2(0f,0f);
    public float square_scale = 1f;

    private List<GameObject> grid_squares = new List<GameObject>();


    [SerializeField] Transform whereToSpawn;

    private Dictionary<Vector2, Tile> _tiles;

    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetSquarePosition();
    }
    private void SpawnGridSquares()
    {
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < column; y++)
            {
                grid_squares.Add(Instantiate(grid_square) as GameObject);
                grid_squares[grid_squares.Count - 1].transform.parent = whereToSpawn;
         //       grid_squares[grid_squares.Count - 1].transform.localScale = new Vector3(square_scale, square_scale, square_scale);

            }
        }
    }

    private void SetSquarePosition()
    {
        var square_rect = grid_squares[0].GetComponent<RectTransform>();
        Vector2 offset = new Vector2();
        offset.x = square_rect.rect.width * square_rect.transform.localScale.x * square_offset;
        offset.y = square_rect.rect.height * square_rect.transform.localScale.y * square_offset;


        int column_number = 0;
        int row_number = 0;


        foreach (GameObject square in grid_squares)
        {
            if (column_number+1<column)
            {
                row_number++;
                column_number = 0;
            }

            var pos_x_offset = offset.x * column_number;
            var pos_y_offset = offset.y * row_number;

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(start_position.x + pos_x_offset, start_position.y - pos_y_offset);
            column_number++;
        }       

    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}