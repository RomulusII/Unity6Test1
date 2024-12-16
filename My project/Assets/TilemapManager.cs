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
        string[] tileNames = { "Bozkir", "Buzul", "Cayir", "Col", "Deniz", "Orman1", "Orman2"}; // Tileset içindeki karo adları
        int randomIndex = Random.Range(0, tileNames.Length);
        return tileNames[randomIndex];
    }

}
