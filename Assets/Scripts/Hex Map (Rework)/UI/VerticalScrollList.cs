using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalScrollList : LayoutGroup {

    public int rows, columns;
    public Vector2 cellSize;
    public Vector2 spacing;

    [Min(1f)]
    public float minimumHeight;

    int lastChildCount = 0;

    public override void CalculateLayoutInputVertical() {

        base.CalculateLayoutInputHorizontal();

        if(transform.childCount == 0 ) {
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width , minimumHeight);
            return;
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = (cellSize.y + ((spacing.y / (float)rows) * (rows - 1)) + (padding.top / (float)rows) + (padding.bottom / (float)rows)) * rows;
        if(parentHeight < minimumHeight) {
            parentHeight = minimumHeight;
        }

        float deltaHeight = parentHeight - rectTransform.rect.height;
        rectTransform.sizeDelta = new Vector2(parentWidth , parentHeight);

        rows = Mathf.CeilToInt(transform.childCount / (float)columns);

        float cellWidth = (parentWidth / (float)columns) - ((spacing.x / (float)columns) * (columns - 1)) - (padding.left / (float)columns) - (padding.right / (float)columns);
        cellSize.x = cellWidth;

        int rowCount = 0;
        int columnCount = 0;

        for(int i = 0; i < rectChildren.Count; i++) {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item , 0 , xPos , cellSize.x);
            SetChildAlongAxis(item , 1 , yPos , cellSize.y);
        }

        lastChildCount = transform.childCount;
    }

    public override void CalculateLayoutInputHorizontal() {
        CalculateLayoutInputVertical();
    }

    public override void SetLayoutHorizontal() {

    }

    public override void SetLayoutVertical() {

    }
}
