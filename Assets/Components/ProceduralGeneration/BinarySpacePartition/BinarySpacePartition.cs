using System.Collections.Generic;
using System.Threading;
using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace deblob123456789
{
    [CreateAssetMenu(menuName = "Procedural Generation Method/Binary Space Partition")]
    public class BinarySpacePartition : ProceduralGenerationMethod
    {
        protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
        {
            List<BSPCell> BPSCells = new();
            int attempts = 0;

            for (int i = 0; i < _maxSteps; i++)
            {
                // Check for cancellation
                cancellationToken.ThrowIfCancellationRequested();

                if (BPSCells.Count >= _maxRooms)
                    break;

                attempts++;

                RectInt InitialAvailableRoom = new(0, 0, Grid.Width, Grid.Length);

                GenerateCellIteration(ref BPSCells, InitialAvailableRoom);
                //SplitCell(Grid.Length, Grid.Width);

                await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
            }
        }

        void GenerateCellIteration(ref List<BSPCell> BPSCells, RectInt availableRoom, BSPCell parent = null)
        {
            int splitBoundary;
            int minimumSize = 3;
            RectInt LeftOrUnderCellRect = new();
            RectInt RightOrAboveCellRect = new();
            bool splitVertical = RandomService.Chance(0.5f);

            if (splitVertical) //left and right
            {
                splitBoundary = RandomService.Range(minimumSize, availableRoom.y - minimumSize);

                LeftOrUnderCellRect = new(availableRoom.x, availableRoom.y, availableRoom.width, splitBoundary);
                RightOrAboveCellRect = new(splitBoundary, availableRoom.y, availableRoom.width, splitBoundary);

                //RectInt splitBoundsLeft = new RectInt(_bounds.xMin, _bounds.yMin, _bounds.width / 2, _bounds.height);
                //RectInt splitBoundsRight = new RectInt(_bounds.xMin + _bounds.width / 2, _bounds.yMax, _bounds.width / 2, _bounds.height);

            }
            else //down and up
            {
                //splitBoundary = RandomService.Range(minimumSize, maxX - minimumSize);


            }

            BSPCell LeftOrUnderCell = new(LeftOrUnderCellRect, false);
            BSPCell RightOrAboveCell = new(RightOrAboveCellRect, true);

            LeftOrUnderCell.sibling = RightOrAboveCell;
            RightOrAboveCell.sibling = LeftOrUnderCell;

            BPSCells.Add(LeftOrUnderCell);
            BPSCells.Add(RightOrAboveCell);
        }

        /*void SplitCell(int boundaryX, int boundaryY)
        {
            bool horizontalSplit = RandomService.Range(0, 1) == 0;

            if (horizontalSplit)
            {
                int HalfWidth = Grid.Width / 2;

                RectInt LeftBounds = new (_room.xMin, _room.yMin, HalfWidth, _room.height);
            }
        }*/

        class BSPCell
        {
            RectInt RectInt;

            internal BSPCell sibling;
            BSPCell parent, leftChild, rightChild;

            internal BSPCell(RectInt _RectInt, bool IsRightCell, BSPCell _parent = null)
            {
                RectInt = _RectInt;

                if (_parent == null) return;

                parent = _parent;
                if (IsRightCell)
                    parent.rightChild = this;
                else
                    parent.leftChild = this;
            }
        }
    }

}