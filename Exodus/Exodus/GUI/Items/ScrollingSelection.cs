using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GUI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Exodus.GUI.Items
{
    public class ScrollingSelection : Item
    {
        public List<Tuple<string, string, string>> Entries;
        public Tuple<string, string, string> Header;

        private Texture2D _selectedTexture = Textures.Menu["SelectedScrolling"];

        public int SelectedItem, IndexBegin, DisplayedElementsNumber;

        private int _stepEntries;

        public List<int> RowsTextPositions;

        public Color TextColor;

        public SpriteFont Font = Fonts.Eurostile12;
        public SpriteFont FontTime = Fonts.Eurostile10;

        private int timer = 0,
                    startdelay = 100;

        public bool selectionable = true;

        public ScrollingSelection(int x, int y, Tuple<string, string, string> header, List<int> rowsTextPositions)
        {
            Area = new Rectangle(x + 15, y + 85, 654, 173);

            SelectedItem = 0;
            IndexBegin = 0;
            DisplayedElementsNumber = 6;

            _stepEntries = 28;

            TextColor = new Color(156, 221, 255);

            RowsTextPositions = rowsTextPositions;

            Header = header;

        }

        public void Reset(List<Tuple<string, string, string>> l)
        {
            Entries = l;
            if (l.Count > 0)
            {
                SelectedItem = Math.Min(l.Count - 1, SelectedItem);
                IndexBegin = Math.Min(l.Count - 1, IndexBegin);
            }
            else
            {
                SelectedItem = 0;
                IndexBegin = 0;
            }
            Components.Clear();
            Components.Add(new ScrollBar(Area, Entries.Count, DisplayedElementsNumber));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            IndexBegin = (int) (Entries.Count*((ScrollBar) Components[0]).Value);

            if (Focused)
            {
                if (Inputs.PreKeyboardState.IsKeyDown(Keys.Down) && SelectedItem + 1 < Entries.Count &&
                    (Inputs.KeyboardState.IsKeyUp(Keys.Down) || timer <= 0))
                {
                    SelectedItem++;
                    if (SelectedItem + 1 > IndexBegin + DisplayedElementsNumber)
                    {
                        IndexBegin++;
                        ((ScrollBar) Components[0]).Value = IndexBegin/Entries.Count;
                    }
                    timer = startdelay;
                }
                else if (Inputs.PreKeyboardState.IsKeyDown(Keys.Up) && SelectedItem > 0 &&
                         (Inputs.KeyboardState.IsKeyUp(Keys.Up) || timer <= 0))
                {
                    SelectedItem--;
                    if (SelectedItem + 1 <= IndexBegin)
                    {
                        IndexBegin--;
                        ((ScrollBar) Components[0]).Value = IndexBegin/Entries.Count;
                    }
                    timer = startdelay;
                }
                if (timer > 0)
                    timer -= (int) gameTime.ElapsedGameTime.TotalMilliseconds;
                if (Inputs.MouseState.ScrollWheelValue - Inputs.PreMouseState.ScrollWheelValue > 0 && IndexBegin > 0)
                {
                    IndexBegin--;
                    ((ScrollBar) Components[0]).Value = IndexBegin/Entries.Count;
                }

                if (Inputs.MouseState.ScrollWheelValue - Inputs.PreMouseState.ScrollWheelValue < 0 &&
                    IndexBegin + DisplayedElementsNumber < Entries.Count)
                {
                    IndexBegin++;
                    ((ScrollBar) Components[0]).Value = IndexBegin/Entries.Count;
                }

                if (Inputs.PreMouseState.LeftButton == ButtonState.Pressed &&
                    Inputs.MouseState.LeftButton == ButtonState.Released)
                {
                    SelectedItem = IndexBegin + DisplayedElementsNumber*(Inputs.MouseState.Y - Area.Y)/Area.Height;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(Font, Header.Item1, new Vector2(Area.X + RowsTextPositions[0], Area.Y - 22),
                                   TextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, float.Epsilon);
            spriteBatch.DrawString(Font, Header.Item2, new Vector2(Area.X + RowsTextPositions[1], Area.Y - 22),
                                   TextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, float.Epsilon);
            spriteBatch.DrawString(Font, Header.Item3, new Vector2(Area.X + RowsTextPositions[2], Area.Y - 22),
                                   TextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, float.Epsilon);

            for (int i = IndexBegin, c = IndexBegin + DisplayedElementsNumber; i < Entries.Count && i < c; i++)
            {
                int stepped = _stepEntries*(i - IndexBegin + 1);
                if (i == SelectedItem && selectionable)
                    spriteBatch.Draw(_selectedTexture,
                                     new Rectangle(Area.X, Area.Y + stepped - _stepEntries, Area.Width, _stepEntries),
                                     null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, float.Epsilon);
                spriteBatch.DrawString(Font, Entries[i].Item1,
                                       new Vector2(Area.X + RowsTextPositions[0], Area.Y - 22 + stepped), TextColor, 0f,
                                       Vector2.Zero, 1f, SpriteEffects.None, float.Epsilon);
                spriteBatch.DrawString(Font, Entries[i].Item2,
                                       new Vector2(Area.X + RowsTextPositions[1], Area.Y - 22 + stepped), TextColor, 0f,
                                       Vector2.Zero, 1f, SpriteEffects.None, float.Epsilon);
                spriteBatch.DrawString(Font, Entries[i].Item2,
                                       new Vector2(Area.X + RowsTextPositions[1], Area.Y - 22 + stepped), TextColor, 0f,
                                       Vector2.Zero, 1f, SpriteEffects.None, float.Epsilon);
                spriteBatch.DrawString(FontTime, Entries[i].Item3,
                                       new Vector2(Area.X + RowsTextPositions[2], Area.Y - 22 + stepped), TextColor, 0f,
                                       Vector2.Zero, 1f, SpriteEffects.None, float.Epsilon);
            }
        }
    }
}
