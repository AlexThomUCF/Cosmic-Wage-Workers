using UnityEngine;

public static class ShelfProgressData
{
    private const string ZoneKey = "Shelf_Zone_";
    private const string RowKey = "Shelf_Row_";

    public static void SetShelfProgress(int zoneIndex, int nextShelf, int rowInShelf)
    {
        PlayerPrefs.SetInt(ZoneKey + zoneIndex, nextShelf);
        PlayerPrefs.SetInt(RowKey + zoneIndex, rowInShelf);
        PlayerPrefs.Save();
    }

    public static int GetNextShelf(int zoneIndex)
    {
        return PlayerPrefs.GetInt(ZoneKey + zoneIndex, 0);
    }

    public static int GetRowInShelf(int zoneIndex)
    {
        return PlayerPrefs.GetInt(RowKey + zoneIndex, 0);
    }

    public static int GetRowsStockedThisZone(int zoneIndex)
    {
        int nextShelf = GetNextShelf(zoneIndex);
        int rowInShelf = GetRowInShelf(zoneIndex);
        return nextShelf * 2 + rowInShelf; // 2 rows per shelf
    }

    public static void ResetAllZones(int totalZones)
    {
        for (int i = 0; i < totalZones; i++)
        {
            PlayerPrefs.DeleteKey(ZoneKey + i);
            PlayerPrefs.DeleteKey(RowKey + i);
        }
        PlayerPrefs.Save();
    }
}