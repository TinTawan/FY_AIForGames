using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSpriteGrid : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private float cellSize = 50;
 
    RectTransform rectTransform, cellRectTransform;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        cellRectTransform = cellPrefab.GetComponent<RectTransform>();

        //int cellWidth = (int)cellRectTransform.rect.width;
        //int cellHeight = (int)cellRectTransform.rect.height;

        for (int i = 1; i < rectTransform.rect.width / cellSize; i++)
        {
            for (int j = 1; j < rectTransform.rect.height / cellSize; j++)
            {
                Vector2 place = new(i * cellSize, j * cellSize);
                GameObject initCell = Instantiate(cellPrefab, place, Quaternion.identity, this.transform);
                initCell.name = $"Cell {i}-{j}";

                initCell.GetComponent<RectTransform>().sizeDelta = new(cellSize, cellSize);
                initCell.GetComponent<BoxCollider2D>().size = new(cellSize, cellSize);

                Debug.Log(i*j);
            }
        }
    }

}
