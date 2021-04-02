using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    private int size = 5;
    [SerializeField]
    private int creaturesCount = 5;
    private Vector2 gridCenter;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
        SetCamera();
        SpawnCreatures();
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

    private void SetCamera(){
       Transform firstChild = transform.GetChild(0);
       Transform lastChild = transform.GetChild(transform.childCount - 1);
       float gridWidth = lastChild.position.x - firstChild.position.x;
       float gridHeight = lastChild.position.y - firstChild.position.y;
       float gridXCenter = gridWidth / 2;
       float gridYCenter = gridHeight / 2;
       this.gridCenter = new Vector2(transform.position.x + gridXCenter, transform.position.y + gridYCenter);
       
       Camera.main.transform.position = new Vector3(transform.position.x + gridXCenter, transform.position.y + gridYCenter, -10);
       Camera.main.orthographicSize = gridHeight * 2 / 3;
    }

    private void SpawnCreatures(){
        GameObject referenceCreature = (GameObject)Instantiate(Resources.Load("Creature"));
        for(int i = 0; i < creaturesCount; i++){
            GameObject creature = (GameObject) Instantiate(referenceCreature, transform);
            creature.transform.position = this.gridCenter;
        }
        Destroy(referenceCreature);
    }
}
