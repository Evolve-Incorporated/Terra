using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private int size = 5;


    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid(){
        GameObject referenceTile = (GameObject)Instantiate(Resources.Load("GrassTile"));

        for(int row = 0; row < size; row++){
            for(int col = 0; col < size; col++){
                GameObject tile = (GameObject) Instantiate(referenceTile, transform);

                float posX = col;
                float posY = row;

                tile.transform.position = new Vector2(posX, posY);
            }
        }

        Destroy(referenceTile);

        float gridW = size;
        float gridH = size;
        transform.position = new Vector2(-gridW / 2 , gridH / 2);
    }
}
