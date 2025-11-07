using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VTools.Grid;
using VTools.ScriptableObjectDatabase;

namespace Components.ProceduralGeneration.SimpleRoomPlacement
{
    [CreateAssetMenu(menuName = "Procedural Generation Method/Simple Room Placement")]
    public class SimpleRoomPlacement : ProceduralGenerationMethod
    {
        protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
        {
            Vector2Int MinRoomSize = new(3, 3);
            Vector2Int MaxRoomSize = new(10, 10);

            List<RectInt> Rooms = new();

            for (int i = 0; i < _maxSteps; i++)
            {
                // Check for cancellation
                cancellationToken.ThrowIfCancellationRequested();

                RectInt newRoom = new(
                    RandomService.Range(0, Grid.Width - 1),
                    RandomService.Range(0, Grid.Length - 1),
                    RandomService.Range(MinRoomSize.x, MaxRoomSize.x),
                    RandomService.Range(MinRoomSize.y, MaxRoomSize.y)
                );

                if (!CanPlaceRoom(newRoom, 3)) continue;

                for (int x = newRoom.xMin; x < newRoom.xMax; x++)
                {
                    for (int y = newRoom.yMin; y < newRoom.yMax; y++)
                    {
                        if (Grid.TryGetCellByCoordinates(x, y, out Cell cell))
                            AddTileToCell(cell, ROOM_TILE_NAME, true);
                    }
                }

                Rooms.Add(newRoom);

                // Waiting between steps to see the result.
                await UniTask.Delay(GridGenerator.StepDelay, cancellationToken : cancellationToken);
            }

            //BuildCorridors();

            // Final ground building.
            BuildGround();
        }

        /*bool OverlapsARoom(RectInt newRoom, ref List<RectInt> Rooms)
        {
            foreach (RectInt room in Rooms)
                if (room.Overlaps(newRoom))
                    return true;

            return false;
        }*/

        void BuildCorridors(Vector2Int start, Vector2Int end)
        {
            //build a list of the cells from left to right?
            //randomly pick horizontal or vertical first


        }

        private void BuildGround()
        {
            var groundTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Grass");
            
            // Instantiate ground blocks
            for (int x = 0; x < Grid.Width; x++)
            {
                for (int z = 0; z < Grid.Length; z++)
                {
                    if (!Grid.TryGetCellByCoordinates(x, z, out var chosenCell))
                    {
                        Debug.LogError($"Unable to get cell on coordinates : ({x}, {z})");
                        continue;
                    }
                    
                    GridGenerator.AddGridObjectToCell(chosenCell, groundTemplate, false);
                }
            }
        }
    }
}