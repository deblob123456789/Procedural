using System;
using System.Threading;
using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VTools.Grid;
using VTools.ScriptableObjectDatabase;


namespace deblob123456789
{
    [CreateAssetMenu(fileName = "Cellular", menuName = "Procedural Generation Method/Cellular")]
    public class Cellular : ProceduralGenerationMethod
    { 
        //[Range(0, 100)]
        //[Tooltip("test")]
        //[SerializeField] float NoiseDensity = 0.5f;
        //[SerializeField] int Threshold = 4;

        [SerializeField] FastNoiseLite noise = new();

        [Header("Noise Parameters")]
        [SerializeField, Tooltip("Zoom Level"), Range(0.01f, 1.0f)] float frequency = 0.1f;
        [SerializeField, Tooltip("How big/Small variations can go"), Range(0.5f, 1.5f)] float amp = 1f;

        [Header("Noise Parameters")]
        [SerializeField, Tooltip("How many 'Detail Maps' will be merged into the noise"), Range(1, 5)] int octaves = 3;
        [SerializeField, Tooltip("How many details will be added"), Range(1f, 3f)] float lacunarity = 2.0f;
        [SerializeField, Tooltip("How much details matter"), Range(0.5f, 1.0f)] float persistance = 0.5f;

        [Header("Heights")]
        [SerializeField, Range(-1, 1)] float waterheight = -0.6f;
        [SerializeField, Range(-1, 1)] float sandheight = -0.3f;
        [SerializeField, Range(-1, 1)] float grassheight = 0.8f;
        [SerializeField, Range(-1, 1)] float rockheight = 1f;

        protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
        {
            GridObjectTemplate WaterTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Water");
            GridObjectTemplate SandTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Sand");
            GridObjectTemplate GrassTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Grass");
            GridObjectTemplate RockTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Rock");
            
            BuildGround();

            float[,] noiseCoords = new float[Grid.Width, Grid.Length];

            SetNoise();

            for (int i = 0; i < _maxSteps; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                for (int x = 0; x < Grid.Width; x++)
                {
                    for (int y = 0; y < Grid.Length; y++)
                    {
                        noiseCoords[x, y] = noise.GetNoise(x, y);
                    }
                }
                for (int j = 0; j < Grid.Width; j++)
                {
                    for (int k = 0; k < Grid.Length; k++)
                    {
                        if (!Grid.TryGetCellByCoordinates(j, k, out var chosenCell))
                        {
                            Debug.LogError($"Unable to get cell on coordinates : ({j}, {k})");
                            continue;
                        }
                        if (noiseCoords[j, k] <= waterheight)
                            GridGenerator.AddGridObjectToCell(chosenCell, WaterTemplate, true);
                        else if (noiseCoords[j, k] <= sandheight)
                            GridGenerator.AddGridObjectToCell(chosenCell, SandTemplate, true);
                        else if (noiseCoords[j, k] <= grassheight)
                            GridGenerator.AddGridObjectToCell(chosenCell, GrassTemplate, true);
                        else if (noiseCoords[j, k] <= rockheight)
                            GridGenerator.AddGridObjectToCell(chosenCell, RockTemplate, true);
                    }
                }


                await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
            }
        }

        void SetNoise()
        {
            noise.SetSeed(GridGenerator._seed);
            noise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
            //noise.SetRotationType3D(); //maybe?

            //common?
            noise.SetFrequency(frequency);
            noise.SetDomainWarpAmp(amp);
            noise.SetFractalOctaves(octaves);
            noise.SetFractalLacunarity(lacunarity);
            noise.SetFractalGain(persistance);

            //cellular
            //noise.SetCellularDistanceFunction()
            //noise.SetCellularReturnType()
            //noise.SetCellularJitter();
        }
    }
}