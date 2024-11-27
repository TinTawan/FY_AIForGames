using UnityEngine;

public class GenerateCellGrid : MonoBehaviour
{
    [Header("Grid Information")]
    
    [SerializeField] private GameObject cellPrefab;
    [Tooltip("Number of cells across")]
    [SerializeField] int gridX = 30;
    [Tooltip("Number of cells up")]
    [SerializeField] int gridY = 15;
    Cell[] cells;

    [Tooltip("How often, in seconds, each generation happens when auto generating")]
    [SerializeField] float genTime = 1f;
    float timer = 0f;

    bool isAuto = false;

    [Header("Cell Generation")]
    [Tooltip("Each generation, a random number of cells will be set to this value\n0 = Deep Blue, 1 = White")]
    [SerializeField] private float randomCellLevel = 0.8f;
    [Tooltip("Lower number = higher chance")]
    [SerializeField][Range(0, 10)] private int chanceOfRandom = 5;
    [Tooltip("When a random number of cells are changed each generation, AllCells.Length is divided by this number to choose how many cells will be changed\nLower number = More cells changed")]
    [SerializeField][Range(5, 20)] private int cellCountDivider = 10;


    void Start()
    {
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                Vector3 place = new(i * 10, 0, j * 10);
                GameObject initCell = Instantiate(cellPrefab, place, Quaternion.identity, this.transform);
                initCell.name = $"Cell {i}-{j}";


            }
        }

        cells = FindObjectsOfType<Cell>();
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
        
        foreach (Cell cell in cells)
        {
            cell.NextGeneration();

            /*int cellsToSet = Random.Range(0, 5);
            if (cellsToSet == 0)
            {
                cells[Random.Range(0, cells.Length)].SetCellLevel(0.8f);
            }*/
        }

        RandCellLevel(randomCellLevel);
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

    void RandCellLevel(float newCellLevel)
    {
        int cellsToSet = Random.Range(0, chanceOfRandom);
        if (cellsToSet == 0)
        {
            int randNumOfCells = Random.Range(0, cells.Length / cellCountDivider);
            for (int i = 0; i < randNumOfCells; i++)
            {
                cells[Random.Range(0, cells.Length)].SetCellLevel(newCellLevel);
                //Debug.Log("pluh");
            }

        }
    }
}
