using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(CreatePerlin))]
public class GenerateTerrain : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private RuleTile[] tiles;
    [SerializeField]
    private float[] LevelsTiles;
    [SerializeField]
    private GameObject[] Objects;
    [SerializeField]
    private float[] LevelsObjects;

    private float[,] m_Terrain;
    private void GenerateBiome()
    {
        if (tiles.Length != LevelsTiles.Length)
        {
            Debug.LogError("Las Listas de los Tiles No Coinciden!!");
            return;
        }
        if (tiles.Length != LevelsObjects.Length)
        {
            Debug.LogError("Las Listas de los Objects No Coinciden!!");
            return;
        }
        StartCoroutine(WaitGenerator());
    }
    private void DrawAllTerrain()
    {
        for (int i = 0; i < m_Terrain.GetLength(0); i++)
            for (int j = 0; j < m_Terrain.GetLength(1); j++)
            {
                tilemap.SetTile(new Vector3Int(j, i, 0), SelectTile(m_Terrain[j, i]));
                Instantiate(selctObject(GetComponent<CreatePerlin>().GetValueOfPerlin(j, i)));
            }
    }
    private void DrawTile(int j, int i)
    {
        tilemap.SetTile(new Vector3Int(j, i, 0), SelectTile(GetComponent<CreatePerlin>().GetValueOfPerlin(j,i)));
        GameObject obj = selctObject(GetComponent<CreatePerlin>().GetValueOfPerlin(j, i));
        if (obj)
        {
            Instantiate(obj, transform);
            obj.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(j, i, 0));
        }
    }

    private RuleTile SelectTile(float p)
    {
        for (int i = 0; i < LevelsTiles.Length; i++)
        {
            if (p < LevelsTiles[i]) return tiles[i];
        }
        return null;
    }
    
    private GameObject selctObject(float p)
    {
        for (int i = 0; i < LevelsObjects.Length; i++)
        {
            if (p < LevelsObjects[i]) return Objects[i];
        }
        return null;
    }

    private IEnumerator WaitGenerator()
    {
        while (m_Terrain == null)
        {
            m_Terrain = GetComponent<CreatePerlin>().GetPerlin();
            yield return new WaitForSeconds(1);
        }
        DrawAllTerrain();
    }

    public void GetAllBiomeTerrain()
    {
        GenerateBiome();
    }

    public void GetTileOfTerrain(int j, int i)
    {
        DrawTile(j, i);
    }
}
