using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CreatePerlin))]
public class BiomeGenerator : MonoBehaviour
{
    [SerializeField]
    private List<GenerateTerrain> Biomes = new List<GenerateTerrain>();
    [SerializeField]
    private List<float> Levels = new List<float>();
    private Dictionary<GenerateTerrain, float> m_BiomeList;
    private float[,] m_MapBiome;

    void Start()
    {
        m_BiomeList = new Dictionary<GenerateTerrain, float>();
        if (Biomes.Count == Levels.Count)
            for (int i = 0; i < Biomes.Count; i++)
                m_BiomeList.Add(Biomes[i], Levels[i]);
        else
            Debug.LogError("Las Listas de los Biomas No Coinciden!!");
        StartCoroutine(WaitGenerator());
    }

    private void GenerateBiomes()
    {
        for(int i = m_MapBiome.GetLength(0)-1; i > 0; i--)
            for(int j = m_MapBiome.GetLength(1)-1; j > 0; j--)
                foreach(GenerateTerrain obj in m_BiomeList.Keys)
                    if (m_MapBiome[i, j] < m_BiomeList.GetValueOrDefault(obj))
                    {
                        obj.GetTileOfTerrain(i, j);
                        break;
                    }
    }

    private IEnumerator WaitGenerator()
    {
        while (m_MapBiome == null)
        {
            m_MapBiome = GetComponent<CreatePerlin>().GetPerlin();
            yield return new WaitForSeconds(1);
        }
        GenerateBiomes();
    }
}
