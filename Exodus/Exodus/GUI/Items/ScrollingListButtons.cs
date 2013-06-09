using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Exodus.GUI.Items
{
    class ScrollingListButtons : Item
    {
        Components.ScrollBar _scrollbar;
        Components.ListButtons _listButtons;
        public int displayedLines { get; private set; }
        public bool Selectable = false;
        public ScrollingListButtons(int x, int y, int width, int height)
        {
            Area.X = x;
            Area.Y = y;
            Area.Width = width;
            Area.Height = height;
            _scrollbar = new Components.ScrollBar(Area, 0, 0);
            _listButtons = new Components.ListButtons(Area.X, Area.Y, Area.Width, Area.Height, 3);
            _listButtons.displayedLinesElements = 3;
            Components.Add(_listButtons);
            Components.Add(_scrollbar);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _listButtons.indexBegin = (int)(_listButtons.height * _scrollbar.Value);
            if (Focused)
            {
                if (Inputs.MouseState.ScrollWheelValue - Inputs.PreMouseState.ScrollWheelValue > 0 && _listButtons.indexBegin > 0)
                {
                    _listButtons.indexBegin--;
                    _scrollbar.Value = (double)_listButtons.indexBegin / (double)_listButtons.height;
                }
                else if (Inputs.MouseState.ScrollWheelValue - Inputs.PreMouseState.ScrollWheelValue < 0 &&
                    _listButtons.indexBegin + _listButtons.displayedLinesElements < _listButtons.height)
                {
                    _listButtons.indexBegin++;
                    _scrollbar.Value = (double)_listButtons.indexBegin / (double)_listButtons.height;
                }
            }
        }
        public void Reset(List<Texture2D> l)
        {
            _listButtons.Reset(l);
            _scrollbar.Reset(_listButtons.height, _listButtons.displayedLinesElements);
        }
        public void SetDisplayedLines(int n)
        {
            displayedLines = n;
            _listButtons.displayedLinesElements = n;
        }
        public int GetIndexSelected()
        {
            int x = (Inputs.MouseState.X - Area.X) / (_listButtons.widthTexture + _listButtons.step),
                y = _listButtons.indexBegin + (Inputs.MouseState.Y - Area.Y) / (_listButtons.heightTexture + _listButtons.step),
                result = x + y * _listButtons.width;
            if (Selectable)
                _listButtons.selectedButton = result;
            return result;
        }
        public void SetPosition(int x, int y)
        {
            this.Area.X = x;
            this.Area.Y = y;
            _listButtons.Area.X = x;
            _listButtons.Area.Y = y;
            _scrollbar.SetPosition(Area);
        }
    }
}
