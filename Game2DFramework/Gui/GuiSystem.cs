﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Game2DFramework.Gui.ItemDescriptors;

namespace Game2DFramework.Gui
{
    public class GuiSystem : GameObject
    {
        private GuiSystemSkinParameters _parameters;
        private readonly List<SkinItemDescriptor> _itemDescriptors;

        public GuiSystem(Game2D game) : base(game)
        {
            _itemDescriptors = new List<SkinItemDescriptor>
            {
                new FrameSkinItemDescriptor(),
                new TextBlockSkinItemDescriptor(),
                new ButtonSkinItemDescriptor(),
                new TextBoxSkinItemDescriptor(),
            };
        }

        public void SetSkin(GuiSystemSkinParameters parameters)
        {
            if(parameters == null) throw new ArgumentNullException("parameters");
            _parameters = parameters;

            var document = new XmlDocument();
            document.Load(_parameters.XmlSkinDescriptorFile);
            foreach(XmlElement element in document.DocumentElement.ChildNodes)
            {
                var descriptor = _itemDescriptors.FirstOrDefault(i => i.NodeName == element.LocalName);
                if(descriptor == null)
                {
                    throw new InvalidOperationException(string.Format("Descriptor for Element Type {0} not found", element.LocalName));
                }

                descriptor.Deserialize(element);
            }

            UpdateSkinDefinitions();
        }

        private void UpdateSkinDefinitions()
        {
            _itemDescriptors.ForEach(i =>
            {
                i.SkinTexture = _parameters.SkinTexture;
                i.NormalFont = _parameters.NormalFont;
                i.BigFont = _parameters.BigFont;
            });
        }

        public TGuiElement CreateGuiHierarchyFromXml<TGuiElement>(string xmlFile) where TGuiElement : GuiElement
        {
            var document = new XmlDocument();
            document.Load(xmlFile);

            return (TGuiElement)GuiElement.CreateFromXmlType(this, document.DocumentElement);
        }

        internal TSkinItemDescriptor GetSkinItemDescriptor<TSkinItemDescriptor>()
            where TSkinItemDescriptor : SkinItemDescriptor
        {
            return _itemDescriptors.OfType<TSkinItemDescriptor>().First();
        }
    }
}
