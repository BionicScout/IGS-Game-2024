using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup {

    public enum FitType { 
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns
    }

    public int rows, columns;
    public Vector2 cellSize;
    public Vector2 spacing;
    public FitType fitType;

    public bool fitX, fitY;

    public override void CalculateLayoutInputVertical() {
        base.CalculateLayoutInputHorizontal();

        if(fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform) { 
            fitX = true;
            fitY = true;
            
            float squareRoot = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(squareRoot);
            columns = Mathf.CeilToInt(squareRoot);
        }

        if(fitType == FitType.Width || fitType == FitType.FixedColumns) {
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);
        }
        if(fitType == FitType.Height || fitType == FitType.FixedRows) {
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / (float)columns) - ((spacing.x / (float)columns) * (columns-1)) - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cellHeight = (parentHeight / (float)rows) - ((spacing.y / (float)rows) * (rows-1)) - (padding.top / (float)rows) - (padding.bottom / (float)rows);

        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        int rowCount = 0;
        int columnCount = 0;

        for(int i = 0; i < rectChildren.Count; i++) {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
    }

    public override void CalculateLayoutInputHorizontal() {
        CalculateLayoutInputVertical();
    }

    public override void SetLayoutHorizontal() {
        
    }

    public override void SetLayoutVertical() {
        
    }
}