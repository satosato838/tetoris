using UnityEngine;

public class OseroView : MonoBehaviour
{
    [SerializeField] private OseroCellView OseroCellPrefab;
    [SerializeField] private Transform OseroCellParent;
    void Start()
    {
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                var cell = Instantiate(OseroCellPrefab, OseroCellParent);
                cell.name = $"Cell({x},{y})";
                cell.Init((x, y), (xy) => { Debug.Log($"Click {xy}"); });
            }
        }

    }


}
