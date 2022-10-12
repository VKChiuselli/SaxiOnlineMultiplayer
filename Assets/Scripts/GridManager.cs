using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour {
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;
 
    void Start() {
    
        GenerateGrid();
    }

    void GenerateGrid() {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y, 1f), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
         //       spawnedTile.transform.localScale = new Vector3(variabile_x * risoluzione_x , variabile_y * risoluzione_y);
                var isOffset = (y == 0) || (y == 1) || (y == 8) || (y == 9);
                spawnedTile.Init(isOffset);
                isOffset = ((y == 0) || (y == 1));
                spawnedTile.tag = isOffset ? "PlayerDeployTile" : "GameTile";

                var tempMaterial = spawnedTile.GetComponent<MeshRenderer>().material;
             


                spawnedTile.GetComponent<MeshRenderer>().material = tempMaterial;
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    public Tile GetTileAtPosition(Vector2 pos) {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}