using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.CompilerServices;

[ExecuteInEditMode]//Run in the editor
public class MinimapAutogen : MonoBehaviour
{
    

    [MenuItem("Minimap/Generate Map")]
    static void MapGen()
    {
        GameObject mapSquare = Resources.Load("Prefabs/MapCreation/MapSquare") as GameObject;
        GameObject[] areas = GameObject.FindGameObjectsWithTag("ZoneSection");
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("ZoneRoom");
        foreach (GameObject room in rooms)
        {
            GameObject MiniMapRoom = Instantiate(room) as GameObject;
            
        }

        foreach (GameObject area in areas)
        {
            Vector3 position = area.transform.position;
            Vector3 scale = area.transform.localScale;
            GameObject MiniMapSquare = Instantiate(mapSquare, position, Quaternion.identity);
            MiniMapSquare.transform.parent = GameObject.FindWithTag("MiniMapSection").transform;
            MiniMapSquare.tag = ("MinimapSquare");
            MiniMapSquare.transform.localScale = scale;
        }

    }
    [MenuItem("Minimap/Clear Map")]
    static void MapDel()
    {
        GameObject[] mapSquares = GameObject.FindGameObjectsWithTag("MinimapSquare");
        foreach (GameObject square in mapSquares)
        {
            DestroyImmediate(square);
        }
    }
}
