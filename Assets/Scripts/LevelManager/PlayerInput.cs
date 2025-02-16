using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask colliderMasks;
    [SerializeField] private List<ShopItem> shopItems;
    [SerializeField] private GameObject invisibleTurretPrefab;
    [SerializeField] public Tilemap ground;
    private int _spawnID = -1;
    
    [SerializeField] private Color previewColorCanPlace = new(0, 1, 0, 0.25f); // Verde semitrasparente
    [SerializeField] private Color previewColorCannotPlace = new(1, 0, 0, 0.25f); // Rosso semitrasparente
    [SerializeField] private GameObject turretPreviewPrefab;
    [SerializeField] private GameObject turretRangePreviewPrefab;
    private GameObject _currentTurretPreview;
    private GameObject _currentTurretRangePreview;
    
    [SerializeField] private TextMeshProUGUI alertTextUI;
    [SerializeField] private GameObject alertPanel;

    public static PlayerInput Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DeselectTowers();
            // DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if (_spawnID != -1)
        {
            DetectSpawnPoint();
        }        
    }

    public string GetSelectedItemDecription() {
        return _spawnID != -1 ? shopItems[_spawnID].description : "Select an item";
    }

    private void DetectSpawnPoint()
    {
        if (!Input.GetMouseButtonDown(0)) {
            PreviewTurret();
            return;
        }

        var mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        var cellPosDefault = ground.WorldToCell(mousePos);
        var cellPosCentered = ground.GetCellCenterWorld(cellPosDefault);

        // Do not allow to put turret on spawn, base and decorations
        if (ground.GetColliderType(cellPosDefault)
            != UnityEngine.Tilemaps.Tile.ColliderType.Sprite) return;

        var turretCost = shopItems[_spawnID].cost;
        if (CurrencyManager.Instance.CanSpendCurrency(turretCost) && CanPlaceTurret(cellPosCentered))
        {
            GameObject g = Instantiate(shopItems[_spawnID].prefab, cellPosCentered, Quaternion.identity);
            if (TurretBlockEnemies(cellPosCentered))
            {
                Instantiate(shopItems[_spawnID].prefab, cellPosCentered, Quaternion.identity);
                CurrencyManager.Instance.SpendCurrency(turretCost);
                EnemySpawner.Instance.RecalculatePaths();
                DeselectTowers();
                ground.SetColliderType(cellPosDefault, UnityEngine.Tilemaps.Tile.ColliderType.None);
            }
            else
            {
                DisplayAlert("Can't place turret, enemies might be stuck!");
            }
            Destroy(g);
            DeselectTowers();
        }
        else
        {
            if(!CurrencyManager.Instance.CanSpendCurrency(turretCost)) DisplayAlert("Can't afford this turret!");
            else DisplayAlert("Can't place here!");
        }
    }
    
    public void PreviewTurret()
    {
        if (Input.GetMouseButtonDown(0)) return;
        
        var mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        var cellPosDefault = ground.WorldToCell(mousePos);
        var cellPosCentered = ground.GetCellCenterWorld(cellPosDefault);

        if (!ground.HasTile(ground.WorldToCell(cellPosCentered)))
        {
            HideTurretPreview();
            return;
        }
        
        if (_currentTurretPreview is null || _currentTurretRangePreview is null)
        {
            _currentTurretPreview = Instantiate(turretPreviewPrefab, (Vector3.one * 10000), Quaternion.identity);
            _currentTurretRangePreview = Instantiate(turretRangePreviewPrefab, (Vector3.one * 10000), Quaternion.identity);
            return;
        }

        bool canPlace = CanPlaceTurret(cellPosCentered) && CurrencyManager.Instance.CanSpendCurrency(shopItems[_spawnID].cost);
        _currentTurretPreview.transform.position = cellPosCentered;
        _currentTurretRangePreview.transform.position = _currentTurretPreview.transform.position;
        var turret = shopItems[_spawnID].prefab.GetComponent<Turret.Turret>();
        var range = turret? turret.GetRange() * 2 : 0;
        _currentTurretRangePreview.transform.localScale = new Vector3(range, range, 1f);
        _currentTurretPreview.SetActive(true);
        _currentTurretRangePreview.SetActive(true);
    
        // Cambia il colore del `SpriteRenderer` in base alla possibilità di posizionamento
        var spriteRenderer = _currentTurretPreview.GetComponent<SpriteRenderer>();
        spriteRenderer.color = canPlace ? previewColorCanPlace : previewColorCannotPlace;
        spriteRenderer = _currentTurretRangePreview.GetComponent<SpriteRenderer>();
        spriteRenderer.color = canPlace ? previewColorCanPlace : previewColorCannotPlace;
    }

    public void HideTurretPreview()
    {
        if (_currentTurretPreview is not null)
        {
            _currentTurretPreview.SetActive(false);
            Destroy(_currentTurretPreview);
            _currentTurretPreview = null;
            _currentTurretRangePreview.SetActive(false);
            Destroy(_currentTurretRangePreview);
            _currentTurretRangePreview = null;
        }
    }

    private bool CanPlaceTurret(Vector3 position)
    {
        return Physics2D.OverlapCircle(position, 0.25f, colliderMasks) is null;
    }

    private bool TurretBlockEnemies(Vector3 position)
    {
        // Check if exists at least a path from start to end
        if (!EnemySpawner.Instance.IsPathAvailable()) return false;
        // Check if some enemy would be trapped after turret placement
        List<Vector3Int> enemyCells = FloodFindManager.Instance.FloodFind(ground.WorldToCell(position));    // gives cells that contains at least one enemies
        foreach (var enemyCell in enemyCells)
        {
            if (!EnemySpawner.Instance.IsPathAvailable(ground.CellToWorld(enemyCell))) return false;
        }
        return true;
    }
    
    public void SelectTower(int id)
    {
        if(_spawnID == id)
        {
            DeselectTowers();
            return;
        }
        
        DeselectTowers();
        
        _spawnID = id;
        shopItems[id].image.color = new Color(1f, 1f, 1f);
    }

    private void DeselectTowers()
    {
        foreach (var item in shopItems)
        {
            item.image.color = new Color(0.4f, 0.4f, 0.4f, 0.6f);
        }
        _spawnID = -1;

        HideTurretPreview();
    }

    private void DisplayAlert(string alert)
    {
        alertTextUI.text = alert;
        alertPanel.SetActive(true);
        StartCoroutine(HideAlert());
    }

    private IEnumerator HideAlert()
    {
        yield return new WaitForSeconds(3f);
        alertPanel.SetActive(false);
    }

    public void Restore()
    {
        Instance = null;
    }
}