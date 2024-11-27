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

    public void Start()
    {
        image = GetComponentInChildren<Image>();

        cellLevel = Random.Range(0f, 1f);
        image.color = SetCellColour(cellLevel);

        GetNeighbourCells();
    }

    void GetNeighbourCells()
    {
        Collider2D[] neighbours;
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
        //subtract proportion of cells value (0-0.5x) to fade from max to min colour
        subtractVal = Random.Range(0, cellLevel * 0.5f);
        cellLevel -= subtractVal;

        //create random value to multiply by the subtracted value
        float ratio = Random.value * 1.5f;
        //split the value unequally
        float val1 = subtractVal * ratio;
        float val2 = subtractVal * (1 - ratio);

        //then get up and right cell
        // -- for up check x pos is the same and y is >, for right check y is same and x >
        //the greater value to add favours the right so the water "flows" more to the right rather than up
        foreach (SpriteCell cell in neighbourCells)
        {
            //up cell
            if(cell.gameObject.transform.position.x == transform.position.x && cell.gameObject.transform.position.y > transform.position.y)
            {
                //and add a proportion of the subtract val to each of their cell levels
                cell.cellLevel += (val1 > val2) ? val2 : val1;

                //clamp between 0 and 1 incase any additions/subtractions cause it to go outside the bounds it should be within
                cell.cellLevel = Mathf.Clamp(cellLevel, 0f, 1f);
              
            }
            //right cell
            if (cell.gameObject.transform.position.x > transform.position.x && cell.gameObject.transform.position.y == transform.position.y)
            {
                cell.cellLevel += (val1 > val2) ? val1 : val2;

                cell.cellLevel = Mathf.Clamp(cellLevel, 0f, 1f);

            }

        }

        //set the cells colour since it has now changed
        image.color = SetCellColour(cellLevel);


    }

    //use lerp to set colour from min (0) to max (1)
    Color SetCellColour(float newCellLevel)
    {
        return Color.Lerp(minColour, maxColour, newCellLevel);

    }

    public void SetCellLevel(float inLevel)
    {
        cellLevel = inLevel;
    }

    public void SetCheckRadius(float inRadius)
    {
        checkRadius = inRadius;
    }
}
