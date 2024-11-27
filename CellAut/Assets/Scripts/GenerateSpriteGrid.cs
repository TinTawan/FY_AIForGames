using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GenerateSpriteGrid : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private float cellSize = 50;

    [SerializeField] private TextMeshProUGUI genTimeText;
    [SerializeField] private Slider genTimeSlider;

    RectTransform rectTransform;
    SpriteCell[] cells;

    [Header("Cell Generation")]
    [Tooltip("How often, in seconds, each generation happens when auto generating")]
    [SerializeField] float genTime = 1f;
    float timer = 0f;
    bool isAuto = false;

    [Tooltip("Each generation, a random number of cells will be set to this value\n0 = Deep Blue, 1 = White")]
    [SerializeField] private float randomCellLevel = 0.8f;
    [Tooltip("Lower number = higher chance")]
    [SerializeField] [Range(0, 10)] private int chanceOfRandom = 5;
    [Tooltip("When a random number of cells are changed each generation, AllCells.Length is divided by this number to choose how many cells will be changed\nLower number = More cells changed")]
    [SerializeField] [Range(5, 20)] private int cellCountDivider = 10;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        for (int i = 1; i < rectTransform.rect.width / cellSize; i++)
        {
            for (int j = 1; j < rectTransform.rect.height / cellSize; j++)
            {
                //instantiate a cell at the position in the grid, with the parent being this object
                Vector2 place = new(i * cellSize, j * cellSize);
                GameObject initCell = Instantiate(cellPrefab, place, Quaternion.identity, this.transform);

                //name it as its grid position for ease of testing 
                initCell.name = $"Cell {i}-{j}";

                //set the size and collider size
                initCell.GetComponent<RectTransform>().sizeDelta = new(cellSize, cellSize);
                initCell.GetComponent<BoxCollider2D>().size = new(cellSize, cellSize);

                //scale cell's check radius with the size
                initCell.GetComponent<SpriteCell>().SetCheckRadius(cellSize / 2);
            }
        }

        //collect all cells into an array (better to use this over GetComponentsInChildren)
        cells = FindObjectsOfType<SpriteCell>();
    }

    private void Update()
    {
        if (isAuto)
        {
            AutoGen();
            genTimeSlider.gameObject.SetActive(true);
        }
        else
        {
            genTimeSlider.gameObject.SetActive(false);
        }
    }


    public void NextGen()
    {
        foreach (SpriteCell cell in cells)
        {
            cell.NextGeneration();
        }

        //each generation theres a chance for cells to change
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


    void RandCellLevel(float newCellLevel)
    {
        int randChance = Random.Range(0, chanceOfRandom);
        if (randChance == 0)
        {
            //set a fraction of the total cells to a higher level
            int randNumOfCells = Random.Range(0, cells.Length / cellCountDivider);
            for (int i = 0; i < randNumOfCells; i++)
            {
                cells[Random.Range(0, cells.Length)].SetCellLevel(newCellLevel);
            }

        }
    }

    public void ToggleAuto()
    {
        isAuto = !isAuto;

    }

    public bool GetIsAuto()
    {
        return isAuto;
    }
    public float GetAutoGenTime()
    {
        return genTime;
    }

    public void SetAutoGenTime(float inTime)
    {
        genTime = inTime;
    }


    public void Slider()
    {
        genTimeText.text = System.Math.Round(genTime, 2).ToString();
        genTime = genTimeSlider.value;
    }

}
