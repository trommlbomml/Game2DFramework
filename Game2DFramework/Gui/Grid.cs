using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;

namespace Game2DFramework.Gui
{
    internal class GridItemDefinition
    {
        public const int AutoSize = int.MinValue;
        public const int MaxSize = int.MaxValue;

        public int Size { get; set; }

        public GridItemDefinition(int size)
        {
            Size = size;
        }

        public GridItemDefinition(string sizeAttributeName, XmlElement element)
        {
            Size = MaxSize;
            if (element.HasAttribute(sizeAttributeName))
            {
                var widthAsString = element.GetAttribute(sizeAttributeName);
                if(widthAsString.Equals("auto", StringComparison.OrdinalIgnoreCase))
                {
                    Size = AutoSize;
                }
                else if(widthAsString.Equals("*"))
                {
                    Size = MaxSize;
                }
                else
                {
                    Size = int.Parse(widthAsString);
                }
            }
        }
    }

    public class Grid : GuiElement
    {
        private readonly List<GridItemDefinition> _rowDefinitions = new List<GridItemDefinition>();
        private readonly List<GridItemDefinition> _columnDefinitions = new List<GridItemDefinition>();

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
                _columnDefinitions.Add(new GridItemDefinition(GridItemDefinition.MaxSize));
            }
            else
            {
                foreach(XmlElement columnElement in columnDefinitionNodes)
                {
                    _columnDefinitions.Add(new GridItemDefinition("Width", columnElement));
                }
            }

            if (rowDefinitionNodes == null || rowDefinitionNodes.Count == 0)
            {
                _rowDefinitions.Add(new GridItemDefinition(GridItemDefinition.MaxSize));
            }
            else
            {
                foreach (XmlElement columnElement in rowDefinitionNodes)
                {
                    _rowDefinitions.Add(new GridItemDefinition("Height", columnElement));
                }
            }
        }

        public override Rectangle GetMinSize()
        {
            throw new NotImplementedException();
        }

        public override void Arrange(Rectangle target)
        {
            throw new NotImplementedException();
        }

        public override void Draw()
        {
            throw new NotImplementedException();
        }

        public override void Update(float elapsedTime)
        {
            throw new NotImplementedException();
        }
    }
}
