using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateCellGrid : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] int gridSize = 10;

    Cell[] cells;

    [SerializeField] float genTime = 1f;
    float timer = 0f;

    bool isAuto = false;

    [SerializeField] Button autoButton;

    void Start()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for(int j = 0; j < gridSize; j++)
            {
                Vector3 place = new (i * 10, 0, j * 10);
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
        if(timer > genTime)
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

        if(isAuto )
        {
            autoButton.GetComponent<Image>().color = Color.green;
        }
        else
        {
            autoButton.GetComponent<Image>().color = Color.white;

        }

    }
}
