using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GenerarTerreno : MonoBehaviour
{
    [SerializeField]
    private VariablesTerreno variablesTerreno;
    [SerializeField]
    public Tilemap tilemap;

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
            StartCoroutine("GenTerrain");
    }
    public IEnumerator GenTerrain()
    {
        for(int y =0; y < variablesTerreno.sizeMap.y; y++)
        {
            yield return null;
            for(int x=0; x< variablesTerreno.sizeMap.x; x++)
            {
                float xCoord = variablesTerreno.Seed.x + (x / variablesTerreno.scale) * variablesTerreno.m_Frequency;
                float yCoord = variablesTerreno.Seed.y + (y / variablesTerreno.scale) * variablesTerreno.m_Frequency;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                //Acte seguit calculem les octaves
                for (int octave = 1; octave <= variablesTerreno.m_Octaves; octave++)
                {
                    //La Lacunarity afecta a la freqencia de cada subseqent octava. El limitem a [2,3] de forma
                    //que cada nou valor sigui x2 o x3 de la freqencia anterior (doble o triple de soroll)
                    float newFreq = variablesTerreno.m_Frequency * variablesTerreno.m_Lacunarity * octave;
                    float xOctaveCoord = variablesTerreno.Seed.x + (x / variablesTerreno.scale) * newFreq;
                    float yOctaveCoord = variablesTerreno.Seed.y + (y / variablesTerreno.scale) * newFreq;

                    //valor base de l'octava
                    float octaveSample = Mathf.PerlinNoise(xOctaveCoord, yOctaveCoord);

                    //La Persistence afecta a l'amplitud de cada subseqent octava. El limitem a [0.1, 0.9] de forma
                    //que cada nou valor afecti menys al resultat final.
                    //addicionalment, farem que el soroll en comptes de ser un valor base [0,1] sigui [-0.5f,0.5f]
                    //i aix pugui sumar o restar al valor inicial
                    octaveSample = (octaveSample - .5f) * (variablesTerreno.m_Persistence / octave);

                    //acumulaci del soroll amb les octaves i base anteriors
                    sample += octaveSample;
                }
                tilemap.SetTile(new Vector3Int(x, y, 0), SelectTile(sample));
            }
        }
    }
    Tile SelectTile(float p)
    {
        for(int i = 0; i< variablesTerreno.levels.Length; i++)
        {
            if(p < variablesTerreno.levels[i]) return variablesTerreno.tiles[i];
        }
        return null;
    }
}
