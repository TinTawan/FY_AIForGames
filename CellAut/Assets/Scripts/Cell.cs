using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    //[SerializeField] private bool isAlive;
    //private int aliveNeighbours;

    [SerializeField] private float checkRadius = 8f;
    [SerializeField] private LayerMask cellLayer;

    MeshRenderer cellRenderer;


    [SerializeField] private float cellLevel;

    [SerializeField] private Color minColour = Color.white, maxColour = Color.blue;

    float subtractVal;

    private List<Cell> neighbourCells = new List<Cell>();

    public void Start()
    {
        cellRenderer = GetComponent<MeshRenderer>();
        //StartGeneration();

        cellLevel = RandCellLevel();

        //GetNeighbourCells();
    }

    private void Update()
    {
        //cellRenderer.enabled = isAlive;
        /*if(cellLevel == 1)
        {
            //cellRenderer.enabled = true;
            cellRenderer.material.color = maxColour;
        }
        else
        {
            //cellRenderer.enabled = false;
            cellRenderer.material.color = minColour;

        }*/

        cellRenderer.material.color = CellColourFromLevel(cellLevel);

        CellClicked();

    }

    //Conway's
    int CheckNeighbours()
    {
        //-1 as it counts itself
        Collider[] neighbours;
        neighbours = Physics.OverlapSphere(transform.position, checkRadius, cellLayer);

        int output = 0;
        foreach (Collider col in neighbours)
        {
            /*if (col.gameObject.GetComponent<MeshRenderer>().enabled)
            {
                output++;
            }*/

            if (col.gameObject.GetComponent<Cell>().GetCellLevel() == 1)
            {
                output++;
            }

        }

        return output;
    }

    void GetNeighbourCells()
    {
        Collider[] neighbours;
        neighbours = Physics.OverlapSphere(transform.position, checkRadius, cellLayer);

        /*for (int i = 0; i < neighbours.Length; i++)
        {
            neighbourCells.Add(neighbours[i].gameObject.GetComponent<Cell>());
        }

        return neighbourCells;*/

        
        foreach(Collider col in neighbours)
        {
            //neighbourCells.Add(col.gameObject.GetComponent<Cell>());

            //Debug.Log(col.transform.position);

            if(col == this.GetComponent<Collider>())
            {
                continue;
            }

            if (col.gameObject.TryGetComponent(out Cell cell))
            {
                neighbourCells.Add(cell);
            }


        }

        //return neighbourCells;
    }


    //Conway's
    bool RuleCheck(int neighbours)
    {
        bool returnVal = false;
        if (cellLevel == 1)
        {
            if (neighbours < 2)
            {
                Debug.Log("dead by underpopulation");
                returnVal = false;
            }
            else if (neighbours == 2 || neighbours == 3)
            {
                Debug.Log("alive by sustainable");
                returnVal = true;
            }
            else if (neighbours > 3)
            {
                Debug.Log("dead by overpopulation");
                returnVal = false;
            }
        }
        else
        {
            if (neighbours == 3)
            {
                Debug.Log("alive by reproduction");
                returnVal = true;
            }
        }

        return returnVal;
    }

    //Conway's
    public void NextGeneration_Conway()
    {
        //isAlive = RuleCheck(CheckNeighbours());

        if (RuleCheck(CheckNeighbours()))
        {
            cellLevel = 1;
        }
        else
        {
            cellLevel = 0;
        }
    }

    public void NextGeneration()
    {
        GetNeighbourCells();

        //subtract proportion of cells value (0-1.5x)
        subtractVal = Random.Range(0, cellLevel * 1.5f);
        cellLevel -= subtractVal;
    }

    //Conway's
    public void StartGeneration()
    {
        //start with all cells "dead"
        //isAlive = false;

        cellLevel = 0;
    }

    //Conway's
    void CellClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Cell clickedCell = hit.transform.GetComponent<Cell>();

                //clickedCell.cellRenderer.enabled = !clickedCell.cellRenderer.enabled;
                //clickedCell.isAlive = true;
                clickedCell.cellLevel = 1;

            }

        }
    }

    /*void ToggleCell(Cell hitCell)
    {
        hitCell.cellRenderer.enabled = !hitCell.cellRenderer.enabled;
        hitCell.isAlive = !hitCell.isAlive;

        Debug.Log(hitCell.isAlive);
    }*/

    /*public void SetAlive(bool inBool)
    {
        isAlive = inBool;
    }*/

    float RandCellLevel()
    {
        return Random.Range(0f, 1f);
    }


    Color CellColourFromLevel(float level)
    {
        return Color.Lerp(minColour, maxColour, level);
    }


    public float GetCellLevel()
    {
        return cellLevel;
    }

}
