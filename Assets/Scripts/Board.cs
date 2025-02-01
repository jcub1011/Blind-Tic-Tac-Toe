using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    [SerializeField]
    int _xCellCount = 3;
    [SerializeField]
    int _yCellCount = 3;
    [SerializeField]
    float _cellWidth = 100;
    [SerializeField]
    float _cellHeight = 100;
    [SerializeField]
    float _xPadding = 10;
    [SerializeField]
    float _yPadding = 10;
    [SerializeField]
    Vector2 _boardOffset = Vector2.zero;

    [SerializeField]
    GameObject _cell;

    private void Awake()
    {
        for (int i = 0; i < _xCellCount *  _yCellCount; i++)
        {
            var cell = Instantiate(_cell, transform);
            cell.transform.localPosition = CellToPosition(new (i % 3, i / 3));
        }
    }

    Vector2 GetOrigin()
    {
        return new Vector2(
            _boardOffset.x - _cellWidth * _xCellCount / 2 + _xPadding * (1 + _xCellCount),
            _boardOffset.y - _cellHeight * _yCellCount / 2 + _xPadding * (1 + _yCellCount)
        );
    }

    Vector2 CellToPosition(Vector2Int cell)
    {
        // Bottom left is origin.
        var origin = GetOrigin();
        //print(origin);
        return new Vector2(origin.x + cell.x * (_cellWidth + _xPadding), origin.y + cell.y * (_cellHeight + _yPadding));
    }

    Vector2Int PositionToCell(Vector2 screenPos)
    {
        var rect = transform as RectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPos, null, out var guiPoint);
        Vector2Int closestCell = Vector2Int.zero;
        float closestDist = Vector2.Distance(CellToPosition(closestCell), guiPoint);
        for (int i = 1; i < _xCellCount * _yCellCount; i++)
        {
            var cell = new Vector2Int(i % 3, i / 3);
            var cellPos = CellToPosition(cell);
            float distance = Vector2.Distance(guiPoint, cellPos);
            if (distance < closestDist)
            {
                closestDist = distance;
                closestCell = cell;
            }
            //print($"Cell {cell} of pos {cellPos} is dist {distance} from gui pos {guiPoint}.");
        }

        return closestCell;
    }

    void OnPoint(InputValue value)
    {
        print($"Screen Pos {value.Get<Vector2>()} translates to board position {PositionToCell(value.Get<Vector2>())}.");
    }
}
