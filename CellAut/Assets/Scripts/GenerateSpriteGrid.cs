using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSpriteGrid : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private float cellSize = 50;
 
    RectTransform rectTransform, cellRectTransform;

    SpriteCell[] cells;

    [Tooltip("How often, in seconds, each generation happens when auto generating")]
    [SerializeField] float genTime = 1f;
    float timer = 0f;
    bool isAuto = false;

    [Header("Cell Generation")]
    [Tooltip("Each generation, a random number of cells will be set to this value\n0 = Deep Blue, 1 = White")]
    [SerializeField] private float randomCellLevel = 0.8f;
    [Tooltip("Lower number = higher chance")]
    [SerializeField] [Range(0, 10)] private int chanceOfRandom = 5;
    [Tooltip("When a random number of cells are changed each generation, AllCells.Length is divided by this number to choose how many cells will be changed\nLower number = More cells changed")]
    [SerializeField] [Range(5, 20)] private int cellCountDivider = 10;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        //cellRectTransform = cellPrefab.GetComponent<RectTransform>();

        for (int i = 1; i < rectTransform.rect.width / cellSize; i++)
        {
            for (int j = 1; j < rectTransform.rect.height / cellSize; j++)
            {
                Vector2 place = new(i * cellSize, j * cellSize);
                GameObject initCell = Instantiate(cellPrefab, place, Quaternion.identity, this.transform);
                initCell.name = $"Cell {i}-{j}";

                initCell.GetComponent<RectTransform>().sizeDelta = new(cellSize, cellSize);
                initCell.GetComponent<BoxCollider2D>().size = new(cellSize, cellSize);

                initCell.GetComponent<SpriteCell>().SetCheckRadius(cellSize / 2);

                //Debug.Log(i*j);
            }
        }

        cells = FindObjectsOfType<SpriteCell>();
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

        foreach (SpriteCell cell in cells)
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
