using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exodus.GUI.Components.Buttons.GameButtons;
using Exodus.GUI.Components.Buttons.MenuButtons;
using Exodus.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exodus.PlayGame;
using Exodus.PlayGame.Items.Buildings;
using Exodus.PlayGame.Items.Units;

namespace Exodus.GUI.Items
{
    internal class TasksDisplayer : Item
    {
        private Stack<List<Component>> stackComponents;
        private List<PlayGame.Item> canProduceUnits;
        private List<Type> unitProductibles;
        private List<PlayGame.Item> canProduceBuildings;
        private List<Type> buildingsProductibles;
        private bool[] taskPossibles;
        private int _step = 5;

        private void Pop()
        {
            stackComponents.Pop();
            Components = stackComponents.Peek();
        }
        private void Push(List<Component> l)
        {
            stackComponents.Push(l);
            Components = l;
        }
        public TasksDisplayer(int x, int y)
        {
            Area = new Rectangle(x, y, Textures.GameUI["actions"].Width, Textures.GameUI["actions"].Height);
            stackComponents = new Stack<List<Component>>();
            taskPossibles = new bool[Enum.GetValues(typeof(MenuTask)).Length];
            canProduceBuildings = new List<PlayGame.Item>();
            canProduceUnits = new List<PlayGame.Item>();
            unitProductibles = new List<Type>();
            buildingsProductibles = new List<Type>();
            Reset();
        }
        public override void Update(GameTime gameTime)
        {
            if (Inputs.KeyPress(Microsoft.Xna.Framework.Input.Keys.Escape) && stackComponents.Count > 1)
                Pop();
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.GameUI["actions"], Area, null,
                             Color.White, 0f, Vector2.Zero, SpriteEffects.None, 41 * Data.GameDisplaying.Epsilon);
            base.Draw(spriteBatch);
        }
        private void Hold(Type t)
        {
            if (Data.Network.SinglePlayer)
            {
                for (int i = 0; i < Map.ListSelectedItems.Count; i++)
                    if (Map.ListSelectedItems[i] is Unit)
                        Map.ListSelectedItems[i].AddTask(new PlayGame.Tasks.Hold(Map.ListSelectedItems[i]), true, false);
            }
            else
            {
                for (int i = 0; i < Map.ListSelectedItems.Count; i++)
                    if (Map.ListSelectedItems[i] is Unit)
                        Network.ClientSide.Client.SendObject(
                            new Network.Orders.Tasks.Hold(Map.ListSelectedItems[i].PrimaryId, true)
                        );
            }
        }
        public void Reset()
        {
            stackComponents.Clear();
            canProduceUnits.Clear();
            canProduceBuildings.Clear();
            unitProductibles.Clear();
            buildingsProductibles.Clear();
            FindTasksPossibles();
            int currentX = Area.X + 9;
            List<Component> display = new List<Component>();
            Texture2D t;
            if (taskPossibles[(int)MenuTask.Attack])
            {
                t = Textures.GameUI["Attack"];
                display.Add(
                    new Mini(Attack, default(Type), t, Textures.GameUI["Attackhover"], currentX, Area.Y + 14)
                    );
                currentX += t.Width + _step;
            }
            if (taskPossibles[(int)MenuTask.Hold])
            {
                t = Textures.GameUI["Hold"];
                display.Add(
                    new Mini(Hold, default(Type), t, Textures.GameUI["Holdhover"], currentX, Area.Y + 14)
                    );
                currentX += t.Width + _step;
            }
            if (taskPossibles[(int)MenuTask.Patrol])
            {
                t = Textures.GameUI["Patrol"];
                display.Add(
                    new Mini(Patrol, default(Type), t, Textures.GameUI["Patrolhover"], currentX, Area.Y + 14)
                    );
                currentX += t.Width + _step;
            }
            if (taskPossibles[(int)MenuTask.Build])
            {
                t = Textures.GameUI["Build"];
                display.Add(
                    new Mini(AddBuildings, default(Type), t, Textures.GameUI["Buildhover"], currentX, Area.Y + 14)
                    );
                currentX += t.Width + _step;
            }
            if (taskPossibles[(int)MenuTask.ProductUnits])
            {
                t = Textures.GameUI["Build"];
                display.Add(
                    new Mini(AddUnits, default(Type), t, Textures.GameUI["Buildhover"], currentX, Area.Y + 14)
                    );
                currentX += t.Width + _step;
            }
            if (taskPossibles[(int)MenuTask.Research])
            {
                t = Textures.GameUI["Research"]; display.Add(
                     new Mini(AddResearchs, default(Type), t, Textures.GameUI["Researchhover"], currentX, Area.Y + 14)
                     );
                currentX += t.Width + _step;
            }
            if (taskPossibles[(int)MenuTask.Die])
            {
                t = Textures.GameUI["Die"];
                display.Add(
                    new Mini(Die, default(Type), t, Textures.GameUI["Diehover"], currentX, Area.Y + 14)
                    );
                currentX += t.Width + _step;
            }
            if (display.Count == 1)
            {
                if (taskPossibles[(int)MenuTask.ProductUnits])
                    AddUnits(default(Type));
                else if (taskPossibles[(int)MenuTask.Build])
                    AddBuildings(default(Type));
                else if (taskPossibles[(int)MenuTask.ProductUnits])
                    AddResearchs(default(Type));
            }
            else
                Push(display);
        }
        public void FindTasksPossibles()
        {
            int falseLeft = taskPossibles.Length;
            int i;
            for (i = 0; i < falseLeft; i++)
                taskPossibles[i] = false;
            i = 0;
            while (i < Map.ListSelectedItems.Count && falseLeft > 0)
            {
                for (int j = 0; j < Map.ListSelectedItems[i].TasksOnMenu.Count; j++)
                    if (!taskPossibles[(int)Map.ListSelectedItems[i].TasksOnMenu[j]])
                    {
                        if (Map.ListSelectedItems[i].TasksOnMenu[j] == MenuTask.ProductUnits)
                        {
                            canProduceUnits.Add(Map.ListSelectedItems[i]);
                            foreach (Type t in Map.ListSelectedItems[i].ItemsProductibles)
                                if (!unitProductibles.Contains(t))
                                    unitProductibles.Add(t);
                        }
                        else if (Map.ListSelectedItems[i].TasksOnMenu[j] == MenuTask.Build)
                        {
                            canProduceBuildings.Add(Map.ListSelectedItems[i]);
                            foreach (Type t in Map.ListSelectedItems[i].ItemsProductibles)
                                if (!buildingsProductibles.Contains(t))
                                    buildingsProductibles.Add(t);
                        }
                        taskPossibles[(int)Map.ListSelectedItems[i].TasksOnMenu[j]] = true;
                        falseLeft--;
                    }
                i++;
            }

        }
        private void AddBuildings(Type t)
        {
            List<Component> result = new List<Component>();
            int currentX = Area.X + 9;
            Texture2D te;
            foreach (Type ty in buildingsProductibles)
                if (ty == typeof(Habitation) || ty == typeof(Labo))
                {
                    te = Textures.MiniGameItems[ty];
                    result.Add(new Mini(CreateBuilding, ty, te, te, currentX, Area.Y + 14));
                    currentX += te.Width + _step;
                }
            Push(result);
        }
        private void AddUnits(Type t)
        {
            List<Component> result = new List<Component>();
            int currentX = Area.X + 9;
            Texture2D te;
            foreach (Type ty in unitProductibles)
                if (ty == typeof(Worker) || ty == typeof(Gunner))
                {
                    te = Textures.MiniGameItems[ty];
                    result.Add(new Mini(CreateUnit, ty, te, te, currentX, Area.Y + 14));
                    currentX += te.Width + _step;
                }
            Push(result);
        }
        private void AddResearchs(Type t)
        {
        }
        private void CreateBuilding(Type t)
        {
            Data.GameInfos.item = Map.ListSelectedItems.FirstOrDefault(s => s is Unit);
            if (Data.GameInfos.item != null)
            {
                Data.GameInfos.currentMode = Data.GameInfos.ModeGame.Building;
                Data.GameInfos.type = t;
            }
        }
        private void CreateUnit(Type t)
        {
            PlayGame.Item item = null;
            // = Map.ListSelectedItems.FirstOrDefault(s => s.ItemsProductibles.Exists(ty => ty == t));
            for (int i = 0; i < Map.ListSelectedItems.Count; i++)
            {
                if (Map.ListSelectedItems[i].ItemsProductibles.Exists(ty => ty == t) &&
                    (item == null || item.TasksList.Count > Map.ListSelectedItems[i].TasksList.Count))
                    item = Map.ListSelectedItems[i];
            }
            if (item != null)
            {

                if (Data.Network.SinglePlayer)
                {
                    item.AddTask(
                        new PlayGame.Tasks.ProductItem(item, Data.GameInfos.timeCreatingItem[t],
                                                       PlayGame.Items.Loader.LoadUnit(t, item.IdPlayer), item.pos.Value, true, true, false),
                        false, false);
                }
                else
                {
                    Network.ClientSide.Client.SendObject(
                        new Network.Orders.Tasks.ProductItem(
                            item.PrimaryId,
                            false,
                            t,
                            item.pos.Value.X,
                            item.pos.Value.Y,
                            true,
                            true,
                            false)
                    );
                }
            }
        }
        public static void Die(Type t)
        {
            if (Data.Network.SinglePlayer)
            {
                for (int i = 0; i < Map.ListSelectedItems.Count; i++)
                    if (Map.ListSelectedItems[i] is Unit)
                        Map.ListSelectedItems[i].AddTask(new PlayGame.Tasks.Die(Map.ListSelectedItems[i]), true, false);
            }
            else
            {
                for (int i = 0; i < Map.ListSelectedItems.Count; i++)
                    if (Map.ListSelectedItems[i] is Unit)
                        Network.ClientSide.Client.SendObject(
                            new Network.Orders.Tasks.Die(Map.ListSelectedItems[i].PrimaryId, true)
                        );
            }
        }
        private void DoNothing(Type t)
        {

        }
        private void Patrol(Type t)
        {

        }
        public static void Attack(Type t)
        {
            int enemyIndex = 0;
            while (enemyIndex < Map.ListItems.Count && (!(Map.ListItems[enemyIndex].Intersect(Inputs.MouseState.X, Inputs.MouseState.Y)) || Map.ListItems[enemyIndex].IdPlayer == Data.Network.IdPlayer))
            {
                enemyIndex++;
            }
            if (enemyIndex < Map.ListItems.Count)
            {
                if (Data.Network.SinglePlayer)
                {
                    foreach (PlayGame.Item selected in Map.ListSelectedItems)
                    {
                        selected.AddTask(new PlayGame.Tasks.Attack(selected, Map.ListItems[enemyIndex]), false, false);
                    }
                }
                else
                {
                    foreach (PlayGame.Item item in Map.ListSelectedItems)
                    {
                        Network.ClientSide.Client.SendObject(
                            new Network.Orders.Tasks.Attack(item.PrimaryId, enemyIndex, true)
                        );
                    }
                }
            }
        }
    }
}
