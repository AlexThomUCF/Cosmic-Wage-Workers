using UnityEngine;
using System.Collections.Generic;

public static class ShelfProgressData
{
    private static Dictionary<int, int> zoneNextShelf = new();
    private static Dictionary<int, int> zoneRowInShelf = new();
    private static int zoneIndex = 0;
    private static Dictionary<int, int> rowsThisZone = new();

    public static void SetShelfProgress(int zone, int nextShelf, int rowInShelf)
    {
        zoneNextShelf[zone] = nextShelf;
        zoneRowInShelf[zone] = rowInShelf;
    }

    public static int GetNextShelf(int zone)
    {
        return zoneNextShelf.ContainsKey(zone) ? zoneNextShelf[zone] : 0;
    }

    public static int GetRowInShelf(int zone)
    {
        return zoneRowInShelf.ContainsKey(zone) ? zoneRowInShelf[zone] : 0;
    }

    public static void SetZoneIndex(int index)
    {
        zoneIndex = index;
    }

    public static int GetZoneIndex()
    {
        return zoneIndex;
    }

    public static void SetRowsStockedForZone(int zone, int rows)
    {
        rowsThisZone[zone] = rows;
    }

    public static int GetRowsStockedThisZone(int zone)
    {
        return rowsThisZone.ContainsKey(zone) ? rowsThisZone[zone] : 0;
    }
}
