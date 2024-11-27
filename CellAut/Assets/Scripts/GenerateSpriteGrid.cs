using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSpriteGrid : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;

    RectTransform rectTransform;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        for (int i = 0; i < rectTransform.rect.width / 50; i++)
        {
            for (int j = 0; j < rectTransform.rect.height / 50; j++)
            {
                Vector2 place = new(i * 50, j * 50);
                GameObject initCell = Instantiate(cellPrefab, place, Quaternion.identity, this.transform);
                initCell.name = $"Cell {i}-{j}";


            }
        }
    }
}
