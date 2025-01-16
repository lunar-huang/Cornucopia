using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantManager : MonoBehaviour
{
    public static PlantManager Instance { get; private set; }

    public Tilemap interactableMap;

    public Tile interactableTile;
    public Tile groundHoedTile;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitInteractableMap();
    }

    void InitInteractableMap()
    {
        
        foreach(Vector3Int position in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactableMap.GetTile(position);
            if (tile != null)
            {
                interactableMap.SetTile(position, interactableTile);
            }
        }
    }

    public void HoeGround(Vector3 position)
    {
        Vector3Int tilePosition = interactableMap.WorldToCell(position);
        TileBase tile = interactableMap.GetTile(tilePosition);

        if(tile!=null && tile.name== interactableTile.name)
        {
            interactableMap.SetTile(tilePosition, groundHoedTile);
        }
    }

}
