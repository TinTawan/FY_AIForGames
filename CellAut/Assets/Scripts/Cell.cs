using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private bool isAlive;
    //private int aliveNeighbours;

    [SerializeField] private float checkRadius = 8f;
    [SerializeField] private LayerMask cellLayer;

    /*public enum rule
    {
        u,
        s,
        o,
        r
    };

    [SerializeField] private rule cellRule;*/

    MeshRenderer cellRenderer;


    public void Start()
    {
        cellRenderer = GetComponent<MeshRenderer>();
        StartGeneration();

    }

    private void Update()
    {
        cellRenderer.enabled = isAlive;

        CellClicked();

    }

    int CheckNeighbours()
    {
        //-1 as it counts itself
        Collider[] nbs;
        nbs = Physics.OverlapSphere(transform.position, checkRadius, cellLayer);

        int output = 0;
        foreach (Collider col in nbs)
        {
            if (col.gameObject.GetComponent<MeshRenderer>().enabled)
            {
                output++;
            }

        }

        return output;
    }


    bool RuleCheck(int neighbours)
    {
        bool returnVal = false;
        if (isAlive)
        {
            if (neighbours < 2)
            {
                Debug.Log("dead by underpopulation");
                returnVal = false;
            }
            else if (neighbours == 2 || neighbours == 3)
            {
                Debug.Log("alive by sustainable");
                returnVal = true;
            }
            else if (neighbours > 3)
            {
                Debug.Log("dead by overpopulation");
                returnVal = false;
            }
        }
        else
        {
            if (neighbours == 3)
            {
                Debug.Log("alive by reproduction");
                returnVal = true;
            }
        }

        return returnVal;
    }

    public void NextGeneration()
    {
        isAlive = RuleCheck(CheckNeighbours());
    }

    public void StartGeneration()
    {
        //start with all cells "dead"
        isAlive = false;
    }

    void CellClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Cell clickedCell = hit.transform.GetComponent<Cell>();

                //clickedCell.cellRenderer.enabled = !clickedCell.cellRenderer.enabled;
                clickedCell.isAlive = true;

            }

        }
    }

    void ToggleCell(Cell hitCell)
    {
        hitCell.cellRenderer.enabled = !hitCell.cellRenderer.enabled;
        hitCell.isAlive = !hitCell.isAlive;

        Debug.Log(hitCell.isAlive);
    }

    public void SetAlive(bool inBool)
    {
        isAlive = inBool;
    }
}
