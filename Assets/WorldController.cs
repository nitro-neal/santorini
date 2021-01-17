using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{

    List<GameObject> gameObjects = new List<GameObject>();

    public GameObject redPlayer;
    public GameObject bluePlayer;

    public GameObject towerBase;

    public GameObject towerLowerLvl;
    public GameObject towerUpperLvl;
    public GameObject towerCap;

    public GameObject selectionRing;

    public GameObject moveHighlight;

    public GameObject buildHighlight;


    SantoriniGame game = new SantoriniGame();
    public class Pos
    {
        public int row;
        public int col;
    }

    string phase = "place unit";
    int playerTurn = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Pressed left click, casting ray.");
            GameObject tile = castRay();
            Pos pos = convertToGrid(tile);
            SantoriniGame.Tile[,] tiles = game.stepForward(pos.row, pos.col);
            updateBoardGame(tiles);
            if (game.winner >= 0)
            {
                Debug.Log("WE HAVE A WINNER");
                Debug.Log(game.winner);
            }
        }
    }

    void updateBoardGame(SantoriniGame.Tile[,] tiles)
    {

        foreach (GameObject go in gameObjects)
        {
            Destroy(go);
        }

        gameObjects = new List<GameObject>();

        for (int col = 0; col < tiles.GetLength(0); col++)
        {
            for (int row = 0; row < tiles.GetLength(1); row++)
            {

                Pos pos = convertToGame(row, col);
                float height = 1.1f;


                if (tiles[row, col].playerSelected == true)
                {
                    GameObject go = Instantiate(selectionRing, new Vector3(pos.row, 1.1f, pos.col), Quaternion.identity);
                    go.transform.rotation = Quaternion.Euler(-90f, 0, 0);
                    gameObjects.Add(go);
                }




                if (tiles[row, col].building >= 0)
                {
                    GameObject go = Instantiate(towerBase, new Vector3(pos.row, 1.0f, pos.col), Quaternion.identity);
                    gameObjects.Add(go);
                    height = 1.5f;
                }

                if (tiles[row, col].building >= 1)
                {
                    GameObject go = Instantiate(towerLowerLvl, new Vector3(pos.row, 1.3f, pos.col), Quaternion.identity);
                    gameObjects.Add(go);
                    height = 1.75f;
                }

                if (tiles[row, col].building >= 2)
                {
                    GameObject go = Instantiate(towerUpperLvl, new Vector3(pos.row, 1.5f, pos.col), Quaternion.identity);
                    gameObjects.Add(go);
                    height = 2.2f;
                }

                if (tiles[row, col].building >= 3)
                {
                    GameObject go = Instantiate(towerCap, new Vector3(pos.row, 1.75f, pos.col), Quaternion.identity);
                    gameObjects.Add(go);
                }

                if (tiles[row, col].possibleToMove == true)
                {
                    GameObject go = Instantiate(moveHighlight, new Vector3(pos.row, height, pos.col), Quaternion.identity);
                    gameObjects.Add(go);
                }


                if (tiles[row, col].possibleToBuild == true)
                {
                    GameObject go = Instantiate(buildHighlight, new Vector3(pos.row, height, pos.col), Quaternion.identity);
                    go.transform.rotation = Quaternion.Euler(90f, 0, 0);
                    gameObjects.Add(go);
                }

                if (tiles[row, col].player != -1)
                {
                    Debug.Log("place player at" + pos.row + " " + pos.col);
                    GameObject go;

                    if (tiles[row, col].player == 0)
                    {
                        go = Instantiate(bluePlayer, new Vector3(pos.row, height, pos.col), Quaternion.identity);

                    }
                    else
                    {
                        go = Instantiate(redPlayer, new Vector3(pos.row, height, pos.col), Quaternion.identity);

                    }

                    // go.transform.localScale = new Vector3(.35f, .35f, .35f);

                    gameObjects.Add(go);
                }
            }
        }
    }

    public void stepForward(Pos pos)
    {
        Debug.Log("step forward");
        Debug.Log(pos.row);
        Debug.Log(pos.col);
    }
    public GameObject castRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.DrawLine(ray.origin, hit.point);
            Debug.Log("Hit object: " + hit.collider.gameObject.transform.position.x);

            return hit.collider.gameObject;
        }

        return null;
    }

    public Pos convertToGrid(GameObject tile)
    {
        Pos pos = new Pos();
        pos.col = (int)(tile.transform.localPosition.z / 2);
        pos.row = (int)(tile.transform.localPosition.x / 2f);
        return pos;
    }

    public Pos convertToGame(int row, int col)
    {
        Pos pos = new Pos();
        pos.col = (int)(col * 2);
        pos.row = (int)(row * 2);
        return pos;
    }

}
