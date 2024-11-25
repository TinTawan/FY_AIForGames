using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private float checkRadius = 8f;
    [SerializeField] private LayerMask cellLayer;

    MeshRenderer cellRenderer;

    [SerializeField] private float cellLevel;
    [SerializeField] private Color minColour = Color.white, maxColour = Color.blue;

    float subtractVal;

    private List<Cell> neighbourCells = new List<Cell>();
    private Cell[] allCells;

    //[SerializeField] private int randomiseCellVariance = 20;

    public void Start()
    {
        cellRenderer = GetComponent<MeshRenderer>();

        cellLevel = RandCellLevel();

        GetAllCells();

        cellRenderer.material.color = SetCellColour(cellLevel);

    }

    private void Update()
    {
        //cellRenderer.material.color = CellColourFromLevel(cellLevel);

        CellClicked();
    }

    void GetNeighbourCells()
    {
        Collider[] neighbours;
        neighbours = Physics.OverlapSphere(transform.position, checkRadius, cellLayer);

        foreach(Collider col in neighbours)
        {
            //skip this object so it doesnt include itself
            if(col == this.GetComponent<Collider>())
            {
                continue;
            }

            if (col.gameObject.TryGetComponent(out Cell cell))
            {
                neighbourCells.Add(cell);
            }


        }

    }



    public void NextGeneration()
    {
        GetNeighbourCells();

        //subtract proportion of cells value (0-1.5x)
        subtractVal = Random.Range(0, cellLevel * 1.5f);
        cellLevel -= subtractVal;

        float ratio = Random.value;
        float val1 = subtractVal * ratio;
        float val2 = subtractVal * (1 - ratio);

        //then get up and right cell
        // -- for up check x pos is the same and z is >, for right check z is same and x >
        //Cell upCell, rightCell;
        foreach (Cell cell in neighbourCells)
        {
            //and add a proportion of the subtract val to each of their cell levels
            

            //up cell
            if (cell.gameObject.transform.position.x == transform.position.x && cell.gameObject.transform.position.z > transform.position.z)
            {
                //upCell = cell;
                //Debug.Log($"{gameObject.name} - up cell: {upCell.name}");

                /*if (val1 > val2)
                {
                    upCell.cellLevel += subtractVal * val2;
                }
                else
                {
                    upCell.cellLevel += subtractVal * val1;
                }*/

                cell.cellLevel += (val1 > val2) ? val2 : val1;

                cell.cellLevel = Mathf.Clamp(cellLevel, 0f, 1f);
            }
            //right cell 
            if(cell.gameObject.transform.position.x > transform.position.x && cell.gameObject.transform.position.z == transform.position.z)
            {
                //rightCell = cell;
                //Debug.Log($"{gameObject.name} - right cell: {rightCell.name}");

                /*if (val1 > val2)
                {
                    rightCell.cellLevel += subtractVal * val1;
                }
                else
                {
                    rightCell.cellLevel += subtractVal * val2;
                }*/

                cell.cellLevel += (val1 > val2) ? val1 : val2;

                cell.cellLevel = Mathf.Clamp(cellLevel, 0f, 1f);
            }
        }

        //then randomly choose a number of cells to set to given value;
        //ChooseRandCellsToSet(0.8f);

        //finally clamp value between 0 and 1 and set colour
        //cellLevel = Mathf.Clamp(cellLevel, 0, 1);
        cellRenderer.material.color = SetCellColour(cellLevel);


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

                clickedCell.cellLevel = 1;
                clickedCell.cellRenderer.material.color = SetCellColour(1);
                

            }

        }
    }

    float RandCellLevel()
    {
        return Random.Range(0f, 1f);
    }


    Color SetCellColour(float newCellLevel)
    {
        return Color.Lerp(minColour, maxColour, newCellLevel);
    }


    public float GetCellLevel()
    {
        return cellLevel;
    }

    public void SetCellLevel(float inLevel)
    {
        cellLevel = inLevel;
    }

    void GetAllCells()
    {
        allCells = GameObject.FindObjectsOfType<Cell>();

    }

    void ChooseRandCellsToSet(float newCellLevel)
    {
        /*int cellsToSet = Random.Range(0, variance);

        for(int i = 0; i < cellsToSet; i++)
        {
            //allCells[Mathf.RoundToInt(Random.value * numOfCells)].cellLevel = newCellLevel;

            allCells[Random.Range(0, allCells.Length)].cellLevel = newCellLevel;

        }*/

        /*int cellsToSet = Random.Range(0, 2);
        for (int i = 0; i < cellsToSet; i++)
        {
            allCells[Random.Range(0, allCells.Length)].SetCellLevel(newCellLevel);
        }*/

        int cellsToSet = Random.Range(0, 5);
        if(cellsToSet == 0)
        {
            allCells[Random.Range(0, allCells.Length)].SetCellLevel(newCellLevel);
        }

    }

}
