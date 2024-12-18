using Model;
using Model.UnityOyun.Assets.Model;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public GameObject tilesetPrefab; // Tileset prefabını buraya atayın
    public int mapWidth = 10;        // Harita genişliği
    public int mapHeight = 10;       // Harita yüksekliği
    public float tileWidth = 1f;     // Karo genişliği
    public float tileHeight = 0.5f;  // Karo yüksekliği

    void Start()
    {
        GenerateMap();
    }


    void GenerateMap()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // Rastgele veya belirli bir karo seçimi
                string tileName = GetRandomTileName(); // Rastgele bir karo adı seç

                // Tileset prefabından belirli bir karoyu bul
                Transform selectedTile = tilesetPrefab.transform.Find(tileName);
                if (selectedTile == null)
                {
                    Debug.LogError("Karo bulunamadı: " + tileName);
                    continue;
                }

                // Isometrik pozisyon hesaplama
                float xPos = (x - y) * tileWidth / 2;
                float yPos = (x + y) * tileHeight / 2;
                Vector3 tilePosition = new Vector3(xPos, yPos, 0);

                // Karo prefabını sahneye instantiate et
                Instantiate(selectedTile.gameObject, tilePosition, Quaternion.identity, transform);
            }
        }
    }

    // Rastgele bir karo adı döndüren örnek fonksiyon
    string GetRandomTileName()
    {
        string[] tileNames = { "Bozkir", "Buzul", "Cayir", "Col", "Deniz", "Orman1", "Orman2" }; // Tileset içindeki karo adları
        int randomIndex = Random.Range(0, tileNames.Length);
        return tileNames[randomIndex];
    }

    string GetTileName(MapCellBase mapCell)
    {
        if (mapCell == null)
            return null;
        if (mapCell.Altitude == Model.UnityOyun.Assets.Model.CellAltitude.Deniz)
            return "Deniz";

        if (mapCell.Vegetation == Model.UnityOyun.Assets.Model.CellVegetation.Orman)
        {
            return $"Orman{mapCell.X % 2}";
        }

        switch (mapCell.Terrain)
        {
            case Model.UnityOyun.Assets.Model.CellTerrain.Buzul:
                return "Buzul";
            case Model.UnityOyun.Assets.Model.CellTerrain.Cöl:
                return "Col";
            case Model.UnityOyun.Assets.Model.CellTerrain.Okyanus:
                return "Deniz";
            case Model.UnityOyun.Assets.Model.CellTerrain.Otlak:
                return "Cayir";
            case Model.UnityOyun.Assets.Model.CellTerrain.Bozkir:
                return "Bozkir";
            default:
                return "Cayir";
        }
    }
}
