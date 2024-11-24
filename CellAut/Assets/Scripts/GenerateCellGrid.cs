using UnityEngine;
using UnityEngine.UI;

public class GenerateCellGrid : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;

    [SerializeField] int gridX = 30, gridY = 15;

    Cell[] cells;

    [SerializeField] float genTime = 1f;
    float timer = 0f;

    bool isAuto = false;

    [SerializeField] Button autoButton;

    void Start()
    {
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                Vector3 place = new(i * 10, 0, j * 10);
                Instantiate(cellPrefab, place, Quaternion.identity, this.transform);

            }
        }


    }

    private void Update()
    {
        if (isAuto)
        {
            AutoGen();
        }
    }

    public void NextGen()
    {
        Debug.ClearDeveloperConsole();

        cells = FindObjectsOfType<Cell>();
        foreach (Cell cell in cells)
        {
            cell.NextGeneration_Conway();
        }


    }

    public void TestGetCells()
    {
        cells = FindObjectsOfType<Cell>();
        foreach (Cell cell in cells)
        {
            cell.NextGeneration();
        }
    }

    public void Generate()
    {
        cells = FindObjectsOfType<Cell>();
        foreach (Cell cell in cells)
        {
            cell.StartGeneration();
        }
    }


    void AutoGen()
    {
        if (timer > genTime)
        {
            timer = 0;
            NextGen();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public void ToggleAuto()
    {
        isAuto = !isAuto;

    }

    /*public void RandomiseGrid()
    {
        cells = FindObjectsOfType<Cell>();
        foreach (Cell cell in cells)
        {

            if (Random.Range(0, 3) == 0)
            {
                cell.SetAlive(true);
            }
            else
            {
                cell.SetAlive(false);
            }
        }

        
    }*/
}
