using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameBehavior : MonoBehaviour {

    int[,] gameTable = new int[10, 10];

    GameObject[,] objectTable = new GameObject[10, 10];

    public GameObject block;

    public int score;

    public Material red;
    public Material blue;
    public Material green;
    public Material purple;
    public Material yellow;

    // Use this for initialization
    void Start() { 
        System.Random rand = new System.Random();
        for (int i = 1; i < gameTable.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < gameTable.GetLength(1) - 1; j++)
            {
                gameTable[i, j] = rand.Next(1, 5);

                GameObject thisPlane = PrefabUtility.InstantiatePrefab(block) as GameObject;

                objectTable[i, j] = thisPlane;

                thisPlane.transform.position = new Vector3(i * 10, j * 10, 0);

                switch (gameTable[i, j])
                {
                    case 1:
                        thisPlane.GetComponent<Renderer>().material = red;
                        break;
                    case 2:
                        thisPlane.GetComponent<Renderer>().material = blue;
                        break;
                    case 3:
                        thisPlane.GetComponent<Renderer>().material = green;
                        break;
                    case 4:
                        thisPlane.GetComponent<Renderer>().material = purple;
                        break;
                    case 5:
                        thisPlane.GetComponent<Renderer>().material = yellow;
                        break;
                }
            }
        }
        printTable();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int[] tI = findHitObject();
            if (tI != null)
            {
                Queue<int[]> consumed = new Queue<int[]>();
                consumed.Enqueue(tI);
                int numOfBlocks = 0;
                while (consumed.Count > 0)
                {
                    numOfBlocks++;
                    tI = consumed.Dequeue();
                    if (gameTable[tI[0], tI[1]] != -1)
                    {


                        if (gameTable[tI[0], tI[1] + 1] == gameTable[tI[0], tI[1]]) { int[] up = { tI[0], tI[1] + 1 }; consumed.Enqueue(up); }
                        if (gameTable[tI[0], tI[1] - 1] == gameTable[tI[0], tI[1]]) { int[] dn = { tI[0], tI[1] - 1 }; consumed.Enqueue(dn); }
                        if (gameTable[tI[0] + 1, tI[1]] == gameTable[tI[0], tI[1]]) { int[] lf = { tI[0] + 1, tI[1] }; consumed.Enqueue(lf); }
                        if (gameTable[tI[0] - 1, tI[1]] == gameTable[tI[0], tI[1]]) { int[] rt = { tI[0] - 1, tI[1] }; consumed.Enqueue(rt); }
                        GameObject.Destroy(objectTable[tI[0], tI[1]]);
                        gameTable[tI[0], tI[1]] = -1;
                        objectTable[tI[0], tI[1]] = null;
                    }
                }

                score += (numOfBlocks - 2) * Mathf.Abs(numOfBlocks - 2);
                print("score: " + score);

                for (int k = 1; k < gameTable.GetLength(0) - 1; k++)
                {
                    for (int i = 1; i < gameTable.GetLength(1) - 1; i++)
                    {
                        for (int j = 1; j < gameTable.GetLength(1) - 1; j++)
                        {
                            if (gameTable[i, j - 1] == -1 && gameTable[i, j] != -1 && gameTable[i, j] != 0)
                            {
                                gameTable[i, j - 1] = gameTable[i, j];
                                gameTable[i, j] = -1;

                                objectTable[i, j].transform.Translate(new Vector3(0, 0, -10));
                                objectTable[i, j - 1] = objectTable[i, j];
                                objectTable[i, j] = null;
                            }
                        }
                    }
                }
                for (int k = 1; k < gameTable.GetLength(0) - 1; k++)
                {
                    for (int i = 1; i < gameTable.GetLength(1) - 1; i++)
                    {
                        bool isEmpty = true;
                        for (int j = 1; j < gameTable.GetLength(0) - 1; j++)
                        {
                            if (gameTable[i, j] != -1)
                            {
                                isEmpty = false;
                            }
                        }
                        if(isEmpty)
                        {
                            moveDownByOneStartingAt(i);
                        }
                    }
                }
            }
        }
    }
    /**
     * Prints gameTable to console
     **/
    void printTable()
    {
        printOTable();
        string output = "";
        for (int i = gameTable.GetLength(0) - 1; i >= 0; i--)
        {
            output += "\n";
            for (int j = 0; j < gameTable.GetLength(1); j++)
            {
                output += gameTable[j, i];
            }
        }
        print(output);
    }

    void printOTable()
    {
        string output = "";
        for (int i = gameTable.GetLength(0) - 1; i >= 0; i--)
        {
            output += "\n";
            for (int j = 0; j < gameTable.GetLength(1); j++)
            {
                if (objectTable[j, i] == null)
                {
                    output += "n";
                }else
                {
                    if (objectTable[j, i].GetComponent<Renderer>().material.color  == red.color) {
                        output += "R";
                    } else
                    {
                        output += "o";
                    }
                }
            }
        }
        print(output);
    }

    /**
     * Checks which object in the objectTable the mouse is hovering over
     * using a Raycast. Returns an array containing the x and y indeces
     * of the hit object, or null if no object is hit.
     **/
    int[] findHitObject()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(mousePos, Vector3.forward, out hit, Mathf.Infinity))
        {
            for (int i = 1; i < gameTable.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < gameTable.GetLength(1) - 1; j++)
                {
                    if (hit.collider.gameObject == objectTable[i, j])
                    {
                        int[] returnVal = new int[2];
                        returnVal[0] = i;
                        returnVal[1] = j;
                        return returnVal;
                    }
                }
            }
        }
        return null;
    }
    private void moveDownByOneStartingAt(int index)
    {
        for (int i = index + 1; i < gameTable.GetLength(1) - 1; i++)
        {
            for (int j = 1; j < gameTable.GetLength(0) - 1; j++)
            {
                gameTable[i - 1, j] = gameTable[i, j];
                gameTable[i, j] = -1;

                if (objectTable[i, j] != null)
                {
                    objectTable[i, j].transform.Translate(new Vector3(-10, 0, 0));
                }
                objectTable[i - 1, j] = objectTable[i, j];
                objectTable[i, j] = null;

            }
        }
    }
}
