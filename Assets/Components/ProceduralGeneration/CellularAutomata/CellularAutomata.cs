using System.Threading;
using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VTools.Grid;
using VTools.ScriptableObjectDatabase;

namespace deblob123456789
{
    [CreateAssetMenu(menuName = "Procedural Generation Method/Cellular Automata")]
    public class CellularAutomata : ProceduralGenerationMethod
    {
        //[MinMaxSlider(0,100)] 
        [SerializeField] float NoiseDensity = 0.5f;
        [SerializeField] int Threshold = 4;

        protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
        {
            GridObjectTemplate grass = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Grass");
            GridObjectTemplate water = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Water");

            GenerateNoise(grass, water);

            for (int i = 0; i < _maxSteps; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                for (int x = 0; x < Grid.Width; x++)
                {
                    for (int y = 0; y < Grid.Length; y++)
                    {
                        if (Grid.TryGetCellByCoordinates(x, y, out Cell cell))
                        {
                            if (cell.GridObject.Template == water)
                            {
                                if (DoesItBecomeGrass(cell, grass))
                                    GridGenerator.AddGridObjectToCell(cell, grass, true);
                            }
                        }
                    }
                }

                await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
            }
        }

        void GenerateNoise(GridObjectTemplate grass, GridObjectTemplate water)
        {
            for (int x = 0; x < Grid.Width; x++)
            {
                for (int y = 0; y < Grid.Length; y++)
                {
                    bool IsGrass = RandomService.Chance(NoiseDensity);

                    if (!Grid.TryGetCellByCoordinates(x, y, out Cell cell))
                        continue;
                    if (IsGrass)
                        GridGenerator.AddGridObjectToCell(cell, grass, true);
                    else
                        GridGenerator.AddGridObjectToCell(cell, water, true);
                }
            }
        }

        bool DoesItBecomeGrass(Cell cell, GridObjectTemplate grass)
        {
            int grassNeighbors = 0;
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0) continue; //dont scan self (though it should be water so whatever)

                    if (Grid.TryGetCellByCoordinates(cell.Coordinates.x + x, cell.Coordinates.y + y, out Cell neighbor))
                    {
                        if (neighbor.GridObject.Template == grass)
                            grassNeighbors++;
                    }

                    if (grassNeighbors > Threshold) return true;
                }
            }
            return false;
        }




    }
}