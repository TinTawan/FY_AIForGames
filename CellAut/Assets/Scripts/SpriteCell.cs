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

    private List<SpriteCell> neighbourCells = new List<SpriteCell>();
    //private Cell[] allCells;

    RectTransform rectTransform;

    public void Start()
    {
        image = GetComponentInChildren<Image>();
        rectTransform = GetComponent<RectTransform>();

        cellLevel = Random.Range(0f, 1f);
        image.color = SetCellColour(cellLevel);


    }

    void GetNeighbourCells()
    {
        Collider2D[] neighbours;
        //neighbours = Physics.OverlapSphere(image.transform.position, checkRadius, cellLayer);
        neighbours = Physics2D.OverlapCircleAll(transform.position, checkRadius, cellLayer);

        foreach (Collider2D col in neighbours)
        {
            //skip this object so it doesnt include itself
            if (col == this.GetComponent<Collider2D>())
            {
                continue;
            }

            if (col.gameObject.TryGetComponent(out SpriteCell cell))
            {
                neighbourCells.Add(cell);
            }


        }

    }

    public void NextGeneration()
    {
        GetNeighbourCells();

        //subtract proportion of cells value (0-1.5x)
        subtractVal = Random.Range(0, cellLevel * 0.5f);
        cellLevel -= subtractVal;

        float ratio = Random.value;
        float val1 = subtractVal * ratio;
        float val2 = subtractVal * (1 - ratio);

        //then get up and right cell
        // -- for up check x pos is the same and y is >, for right check y is same and x >
        //Cell upCell, rightCell;
        foreach (SpriteCell cell in neighbourCells)
        {
            RectTransform currentCellRectTransform = cell.GetComponent<RectTransform>();
            //and add a proportion of the subtract val to each of their cell levels
            //up cell
            if (currentCellRectTransform.rect.x == rectTransform.rect.x && currentCellRectTransform.rect.y > rectTransform.rect.y)
            {
                /*cell.cellLevel += (val1 > val2) ? val2 : val1;

                cell.cellLevel = Mathf.Clamp(cellLevel, 0f, 1f);*/

                Debug.Log($"{gameObject.name} - up cell: {cell.name}");
            }
            //right cell
            if (currentCellRectTransform.rect.x > rectTransform.rect.x && currentCellRectTransform.rect.y == rectTransform.rect.y)
            {
                /*cell.cellLevel += (val1 > val2) ? val1 : val2;

                cell.cellLevel = Mathf.Clamp(cellLevel, 0f, 1f);*/

                Debug.Log($"{gameObject.name} - right cell: {cell.name}");
            }

            
        }

        //then randomly choose a number of cells to set to given value;
        //ChooseRandCellsToSet(0.8f);

        //finally clamp value between 0 and 1 and set colour
        //cellLevel = Mathf.Clamp(cellLevel, 0, 1);
        image.color = SetCellColour(cellLevel);


    }

    Color SetCellColour(float newCellLevel)
    {
        return Color.Lerp(minColour, maxColour, newCellLevel);

    }

    public void SetCellLevel(float inLevel)
    {
        cellLevel = inLevel;
    }
}
