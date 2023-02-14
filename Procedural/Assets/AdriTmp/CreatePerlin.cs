using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class CreatePerlin : MonoBehaviour
{

    [SerializeField]
    private Vector2 sizeMap;
    [SerializeField]
    private float scale;

    //Scale
    [SerializeField]
    private float m_Frequency = 10f;

    //octaves
    private const int MAX_OCTAVES = 8;
    [SerializeField]
    [Range(0, MAX_OCTAVES)]
    private int m_Octaves = 0;
    [Range(2, 3)]
    [SerializeField]
    private int m_Lacunarity = 2;
    [SerializeField]
    [Range(0.1f, 0.9f)]
    private float m_Persistence = 0.5f;
    [SerializeField]
    private int Seed;
    private bool m_Genearting;

    //Perlin
    private float[,] m_PerlinArray;

    private void Start()
    {
        m_Genearting = false;
    }

    private IEnumerator GenPerlin()
    {
        m_Genearting = true;
        m_PerlinArray = new float[(int)sizeMap.y,(int)sizeMap.x];
        for (int y =0; y < sizeMap.y; y++)
        {
            for(int x=0; x< sizeMap.x; x++)
            {
                m_PerlinArray[y,x] = GetValueOfPerlin(x, y); ;
            }
        }
        m_Genearting = false;
        yield return null;
    }

    public float[,] GetPerlin()
    {
        if (m_PerlinArray == null)
            StartCoroutine(GenPerlin());
        if (m_Genearting)
            return null;
        else
            return m_PerlinArray;
    }

    public float GetValueOfPerlin(int x, int y)
    {
        float xCoord = Seed + (x / scale) * m_Frequency;
        float yCoord = Seed + (y / scale) * m_Frequency;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        for (int octave = 1; octave <= m_Octaves; octave++)
        {
            float newFreq = m_Frequency * m_Lacunarity * octave;
            float xOctaveCoord = Seed + (x / scale) * newFreq;
            float yOctaveCoord = Seed + (y / scale) * newFreq;
            float octaveSample = Mathf.PerlinNoise(xOctaveCoord, yOctaveCoord);
            octaveSample = (octaveSample - .5f) * (m_Persistence / octave);
            sample += octaveSample;
        }
        return sample;
    }

    public void GeneratePerlin()
    {
        StartCoroutine(GenPerlin());
    }
}
