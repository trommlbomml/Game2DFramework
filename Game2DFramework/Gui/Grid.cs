using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;

namespace Game2DFramework.Gui
{
    public class Grid : GuiElement
    {
        private readonly List<GridItemDefinition> _rowDefinitions = new List<GridItemDefinition>();
        private readonly List<GridItemDefinition> _columnDefinitions = new List<GridItemDefinition>();
        private readonly Dictionary<int, GuiElement> _childElementsByLinearTableIndex = new Dictionary<int, GuiElement>();

        public Grid(GuiSystem guiSystem)
            : base(guiSystem)
        {
        }

        public Grid(GuiSystem guiSystem, XmlElement element)
            : base(guiSystem, element)
        {
            var columnDefinitionNodes = element.SelectNodes("Grid.ColumnDefinitions/ColumnDefinition");
            var rowDefinitionNodes = element.SelectNodes("Grid.RowDefinitions/RowDefinition");

            if(columnDefinitionNodes == null || columnDefinitionNodes.Count == 0)
            {
                _columnDefinitions.Add(new GridItemDefinition(GridItemDefinition.MaxSize, 0));
            }
            else
            {
                var index = 0;
                foreach(XmlElement columnElement in columnDefinitionNodes)
                {
                    _columnDefinitions.Add(new GridItemDefinition("Width", index++, columnElement));
                }
            }

            if (rowDefinitionNodes == null || rowDefinitionNodes.Count == 0)
            {
                _rowDefinitions.Add(new GridItemDefinition(GridItemDefinition.MaxSize, 0));
            }
            else
            {
                var index = 0;
                foreach (XmlElement columnElement in rowDefinitionNodes)
                {
                    _rowDefinitions.Add(new GridItemDefinition("Height", index++, columnElement));
                }
            }

            foreach (XmlElement childElement in element.ChildNodes)
            {
                if (childElement.LocalName == "Grid.ColumnDefinitions") continue;
                if (childElement.LocalName == "Grid.RowDefinitions") continue;

                var gridRow = Math.Min(_rowDefinitions.Count-1, int.Parse(childElement.GetAttribute("Grid.Row")));
                var gridColumn = Math.Min(_columnDefinitions.Count-1, int.Parse(childElement.GetAttribute("Grid.Column")));
                var childGuiElement = CreateFromXmlType(GuiSystem, childElement);

                AddChild(gridColumn, gridRow, childGuiElement);
            }
        }

        private void AddChild(int column, int row, GuiElement element)
        {
            var linearIndex = row*_columnDefinitions.Count + column;

            _childElementsByLinearTableIndex.Add(linearIndex, element);
            Children.Add(element);
        }

        public override Rectangle GetMinSize()
        {
            var rowHeights = new Dictionary<int, int>();
            var columnWidths = new Dictionary<int, int>();

            foreach (var elementByIndex in _childElementsByLinearTableIndex)
            {
                var row = elementByIndex.Key /_rowDefinitions.Count;
                var column = elementByIndex.Key % _rowDefinitions.Count;
                var minSizeOfElement = elementByIndex.Value.GetMinSize();

                if (rowHeights.ContainsKey(row))
                {
                    rowHeights[row] += minSizeOfElement.Height;    
                }
                else
                {
                    rowHeights[row] = minSizeOfElement.Height;
                }

                if (columnWidths.ContainsKey(column))
                {
                    columnWidths[column] += minSizeOfElement.Width;    
                }
                else
                {
                    columnWidths[column] = minSizeOfElement.Width;    
                }
            }

            return new Rectangle(0,0,columnWidths.Values.Max(), rowHeights.Values.Max());
        }

        public override void Arrange(Rectangle target)
        {
            var rowHeights = new Dictionary<int, int>();
            var columnWidths = new Dictionary<int, int>();

            var totalHorizontalSpace = target.Width;
            var totalVerticalSpace = target.Height;

            var autoColumns = _columnDefinitions.Where(c => c.Size == GridItemDefinition.AutoSize).ToArray();
            var maxWidthColumns = _columnDefinitions.Where(c => c.Size == GridItemDefinition.MaxSize).ToArray();

            foreach (var columnDefinition in autoColumns)
            {
                var width = GetMaxWidthInColumn(columnDefinition.Index);
                columnWidths.Add(columnDefinition.Index, width);
                totalHorizontalSpace -= width;
            }

            if (maxWidthColumns.Length > 0)
            {
                var sizePerMaxWidthColumn = totalHorizontalSpace / maxWidthColumns.Length;
                foreach (var columnDefinition in maxWidthColumns)
                {
                    columnWidths.Add(columnDefinition.Index, sizePerMaxWidthColumn);
                }   
            }
            
            var autoRows = _rowDefinitions.Where(c => c.Size == GridItemDefinition.AutoSize).ToArray();
            var maxHeightRows = _rowDefinitions.Where(c => c.Size == GridItemDefinition.MaxSize).ToArray();

            foreach (var rowDefinition in autoRows)
            {
                var height = GetMaxHeightInRow(rowDefinition.Index);
                rowHeights.Add(rowDefinition.Index, height);
                totalVerticalSpace -= height;
            }

            if (maxHeightRows.Length > 0)
            {
                var sizePerMaxHeightRow = totalVerticalSpace / maxHeightRows.Length;
                foreach (var rowDefininition in maxHeightRows)
                {
                    rowHeights.Add(rowDefininition.Index, sizePerMaxHeightRow);
                }
            }

            var startY = target.Y;
            for (var y = 0; y < _rowDefinitions.Count; y++)
            {
                var startX = target.X;

                for (var x = 0; x < _columnDefinitions.Count; x++)
                {
                    GuiElement element;
                    if (_childElementsByLinearTableIndex.TryGetValue(y*_columnDefinitions.Count + x, out element))
                    {
                        var currentRectangle = new Rectangle(startX, startY, columnWidths[x], rowHeights[y]);
                        element.Arrange(currentRectangle);
                    }

                    startX += columnWidths[x];
                }

                startY += columnWidths[y];
            }
        }

        private int GetMaxWidthInColumn(int column)
        {
            return _childElementsByLinearTableIndex.Where(kvp => kvp.Key % _rowDefinitions.Count == column)
                                                   .Max(kvp => kvp.Value.GetMinSize().Width);
        }

        private int GetMaxHeightInRow(int row)
        {
            return _childElementsByLinearTableIndex.Where(kvp => kvp.Key / _rowDefinitions.Count == row)
                                                   .Max(kvp => kvp.Value.GetMinSize().Height);
        }

        public override void Draw()
        {
            foreach (var guiElement in Children)
            {
                guiElement.Draw();
            }
        }

        public override void Update(float elapsedTime)
        {
            foreach (var guiElement in Children)
            {
                guiElement.Update(elapsedTime);
            }
        }
    }
}
