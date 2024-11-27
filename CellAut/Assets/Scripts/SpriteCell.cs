using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteCell : MonoBehaviour
{
    [SerializeField] private float checkRadius = 8f;
    [SerializeField] private LayerMask cellLayer;

    Image image;

    [SerializeField] private float cellLevel;
    [SerializeField] private Color minColour = Color.blue, maxColour = Color.white;

    float subtractVal;

    private List<Cell> neighbourCells = new List<Cell>();
    //private Cell[] allCells;

    public void Start()
    {
        image = GetComponentInChildren<Image>();

        cellLevel = Random.Range(0f, 1f);
        image.color = SetCellColour(cellLevel);


    }

    void GetNeighbourCells()
    {
        Collider[] neighbours;
        neighbours = Physics.OverlapSphere(image.transform.position, checkRadius, cellLayer);

        foreach (Collider col in neighbours)
        {
            //skip this object so it doesnt include itself
            if (col == this.GetComponent<Collider>())
            {
                continue;
            }

            if (col.gameObject.TryGetComponent(out Cell cell))
            {
                neighbourCells.Add(cell);
            }


        }

    }

    Color SetCellColour(float newCellLevel)
    {
        return Color.Lerp(minColour, maxColour, newCellLevel);

    }

}
