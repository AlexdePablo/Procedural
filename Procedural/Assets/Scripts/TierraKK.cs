using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TierraKK : MonoBehaviour
{
    [SerializeField]
    private RuleTile[] tiles;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private float[] levels;
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

    Vector2 Seed;
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            StartCoroutine("GenTerrain");
    }
    public IEnumerator GenTerrain()
    {
        for (int y = 0; y < sizeMap.y; y++)
        {
            yield return null;
            for (int x = 0; x < sizeMap.x; x++)
            {
                float xCoord = Seed.x + (x / scale) * m_Frequency;
                float yCoord = Seed.y + (y / scale) * m_Frequency;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                //Acte seguit calculem les octaves
                for (int octave = 1; octave <= m_Octaves; octave++)
                {
                    //La Lacunarity afecta a la freqencia de cada subseqent octava. El limitem a [2,3] de forma
                    //que cada nou valor sigui x2 o x3 de la freqencia anterior (doble o triple de soroll)
                    float newFreq = m_Frequency * m_Lacunarity * octave;
                    float xOctaveCoord = Seed.x + (x / scale) * newFreq;
                    float yOctaveCoord = Seed.y + (y / scale) * newFreq;

                    //valor base de l'octava
                    float octaveSample = Mathf.PerlinNoise(xOctaveCoord, yOctaveCoord);

                    //La Persistence afecta a l'amplitud de cada subseqent octava. El limitem a [0.1, 0.9] de forma
                    //que cada nou valor afecti menys al resultat final.
                    //addicionalment, farem que el soroll en comptes de ser un valor base [0,1] sigui [-0.5f,0.5f]
                    //i aix pugui sumar o restar al valor inicial
                    octaveSample = (octaveSample - .5f) * (m_Persistence / octave);

                    //acumulaci del soroll amb les octaves i base anteriors
                    sample += octaveSample;
                }
                tilemap.SetTile(new Vector3Int(x, y, 0), SelectTile(sample));
            }
        }
    }
    RuleTile SelectTile(float p)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (p < levels[i]) return tiles[i];
        }
        return null;
    }
}
