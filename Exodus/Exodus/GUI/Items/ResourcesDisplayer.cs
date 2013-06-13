using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Exodus.GUI.Items
{
    class ResourcesDisplayer : Item
    {
        Components.Label electrecity,
                         iron,
                         steel,
                         graphene,
                         hydrogen;
        int _step = 5,
            _XBase = Data.Window.WindowWidth - 35;
        SpriteFont _font;
        public string Electricity 
        {
            get
            {
                return electrecity.Txt;
            }
            set
            {
                electrecity.Txt = value;
                electrecity.Pos.X = _XBase - _font.MeasureString(electrecity.Txt).X;
            }
        }
        public string Iron   
        {
            get
            {
                return iron.Txt;
            }
            set
            {
                iron.Txt = value;
                iron.Pos.X = _XBase - _font.MeasureString(iron.Txt).X;
            }
        }
        public string Steel       
        {
            get
            {
                return steel.Txt;
            }
            set
            {
                steel.Txt = value;
                steel.Pos.X = _XBase - _font.MeasureString(steel.Txt).X;
            } 
        }
        public string Graphene   
        {
            get
            {
                return graphene.Txt;
            }
            set
            {
                graphene.Txt = value;
                graphene.Pos.X = _XBase - _font.MeasureString(graphene.Txt).X;
            } 
        }
        public string Hydrogen 
        {
            get
            {
                return hydrogen.Txt;
            }
            set
            {
                hydrogen.Txt = value;
                hydrogen.Pos.X = _XBase - _font.MeasureString(hydrogen.Txt).X;
            }
        }
        public ResourcesDisplayer()
        {
            _font = Fonts.Eurostile12;
            int currentY = 10,
                height = (int)_font.MeasureString("1234567890").Y;
            Texture2D t = Textures.GameUI["BGResources"];
            Components.Add(new Components.JustTexture(t, Data.Window.WindowWidth - t.Width, 0, 43 * Data.GameDisplaying.Epsilon));
            t = Textures.GameUI["Electricity"];
            Components.Add(new Components.JustTexture(t, _XBase + _step, currentY, 42 * Data.GameDisplaying.Epsilon));
            electrecity = new GUI.Components.Label(_font, "", _XBase, currentY + (t.Height - height) / 2);
            electrecity.SetColor(255, 222, 0);
            Components.Add(electrecity);
            currentY += t.Height + _step;
            t = Textures.GameUI["Hydrogen"];
            Components.Add(new Components.JustTexture(t, _XBase + _step, currentY, 42 * Data.GameDisplaying.Epsilon));
            hydrogen = new GUI.Components.Label(_font, "", _XBase, currentY + (t.Height - height) / 2);
            hydrogen.SetColor(109, 226, 255);
            Components.Add(hydrogen);
            currentY += t.Height + _step;
            t = Textures.GameUI["Iron"];
            Components.Add(new Components.JustTexture(t, _XBase + _step, currentY, 42 * Data.GameDisplaying.Epsilon));
            iron = new GUI.Components.Label(_font, "", _XBase, currentY + (t.Height - height) / 2);
            iron.SetColor(183, 199, 206);
            Components.Add(iron);
            currentY += t.Height + _step;
            t = Textures.GameUI["Steel"];
            Components.Add(new Components.JustTexture(t, _XBase + _step, currentY, 42 * Data.GameDisplaying.Epsilon));
            steel = new GUI.Components.Label(_font, "", _XBase, currentY + (t.Height - height) / 2);
            steel.SetColor(255, 255, 255);
            Components.Add(steel);
            currentY += t.Height + _step;
            t = Textures.GameUI["Graphene"];
            Components.Add(new Components.JustTexture(t, _XBase + _step, currentY, 42 * Data.GameDisplaying.Epsilon));
            graphene = new GUI.Components.Label(_font, "", _XBase, currentY + (t.Height - height) / 2);
            graphene.SetColor(0, 255, 150);
            Components.Add(graphene);
            currentY += t.Height + _step;
            Focused = false;
        }
        public void Set(PlayGame.Resource resource)
        {
            Electricity = ((int)resource.Electricity).ToString();
            Iron = ((int)resource.Iron).ToString();
            Steel = ((int)resource.Steel).ToString();
            Graphene = ((int)resource.Graphene).ToString();
            Hydrogen = ((int)resource.Hydrogen).ToString();
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
