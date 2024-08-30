using UnityEngine;

public class DefensiveStructure : MonoBehaviour
{
    [SerializeField] protected GameObject removeUI;

    private void OnMouseDown()
    {
        if (!removeUI.activeSelf)
        {
            removeUI.SetActive(true);
            return;
        }
    
        Vector3Int cellPosition = PlayerInput.Instance.ground.WorldToCell(transform.position);
        PlayerInput.Instance.ground.SetColliderType(cellPosition, UnityEngine.Tilemaps.Tile.ColliderType.Sprite);

        Destroy(gameObject);
        EnemySpawner.Instance.RecalculatePaths();
    }
    
    private void OnMouseExit()
    {
        if(removeUI.activeSelf) removeUI.SetActive(false);
    }
}
