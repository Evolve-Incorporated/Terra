using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    [SerializeField]
    private int size;

    public static Map instance;
    public Dictionary<string, float> gridPosition;

    void Awake(){
        if(instance != null){
            Debug.LogError("More than one mapr in scene!");
            return;
        }
        instance = this;
        GenerateGrid();
        SetGridPosition();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCamera();
    }

    private void SetGridPosition(){
        this.gridPosition = new Dictionary<string, float>();
        Transform firstChild = transform.GetChild(0);
        Transform lastChild = transform.GetChild(transform.childCount - 1);
        float gridWidth = lastChild.position.x - firstChild.position.x;
        float gridHeight = lastChild.position.y - firstChild.position.y;
        float gridXCenter = (gridWidth / 2) + transform.position.x;
        float gridYCenter = (gridHeight / 2) + transform.position.y;
        this.gridPosition.Add("left", firstChild.position.x);
        this.gridPosition.Add("right", lastChild.position.x);
        this.gridPosition.Add("bottom", firstChild.position.y);
        this.gridPosition.Add("top", lastChild.position.y);
        this.gridPosition.Add("width", gridWidth);
        this.gridPosition.Add("height", gridHeight);
        this.gridPosition.Add("xCenter", gridXCenter);
        this.gridPosition.Add("yCenter", gridYCenter);
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
       Camera.main.transform.position = new Vector3(this.gridPosition["xCenter"], this.gridPosition["yCenter"], -10);
       Camera.main.orthographicSize = gridPosition["height"] * 2 / 3;
    }

    public void setSize(int size){
        this.size = size;
    }
}
