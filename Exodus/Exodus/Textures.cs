using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Exodus.PlayGame;
using Exodus.PlayGame.Items.Units;
using Exodus.PlayGame.Items.Buildings;
using Exodus.PlayGame.Items.Obstacles;

namespace Exodus
{
    public static class Textures
    {
        public static Dictionary<string, Texture2D> Menu = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D> Game = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D> GameItems = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D> GameUI = new Dictionary<string, Texture2D>();
        public static Dictionary<Type, Texture2D> MiniGameItems = new Dictionary<Type, Texture2D>();
        public static Dictionary<Type, Texture2D> BigGameItems = new Dictionary<Type, Texture2D>();
        public static Dictionary<string, Texture2D> Particles = new Dictionary<string, Texture2D>();

        public static void LoadMenu(ContentManager content)
        {
            Menu["ConnectionBackground"] = content.Load<Texture2D>("Menu/Backgrounds/Connection");
            Menu["ConnectionOrangeButton"] = content.Load<Texture2D>("Menu/Buttons/ConnectionOrangeButton");
            Menu["ConnectionOrangeButtonHover"] = content.Load<Texture2D>("Menu/Buttons/ConnectionOrangeButtonHover");
            Menu["ConnectionGreenButton"] = content.Load<Texture2D>("Menu/Buttons/ConnectionGreenButton");
            Menu["ConnectionGreenButtonHover"] = content.Load<Texture2D>("Menu/Buttons/ConnectionGreenButtonHover");
            Menu["ConnectionTextBox"] = content.Load<Texture2D>("Menu/TextBox/ConnectionTextBox");
            Menu["ConnectionTextBoxHover"] = content.Load<Texture2D>("Menu/TextBox/ConnectionTextBoxHover");
            Menu["ConnectionTextBoxCursor"] = content.Load<Texture2D>("Menu/TextBox/ConnectionTextBoxCursor");
            Menu["Settings"] = content.Load<Texture2D>("Menu/Backgrounds/Settings");

            Menu["MainBackground"] = content.Load<Texture2D>("Menu/Backgrounds/Main");
            Menu["BlueMenuButton"] = content.Load<Texture2D>("Menu/Buttons/BlueMenuButton");
            Menu["BlueMenuButtonClick"] = content.Load<Texture2D>("Menu/Buttons/BlueMenuButtonClick");
            Menu["BlueMenuButtonHover"] = content.Load<Texture2D>("Menu/Buttons/BlueMenuButtonHover");
            Menu["OrangeMenuButton"] = content.Load<Texture2D>("Menu/Buttons/OrangeMenuButton");
            Menu["OrangeMenuButtonClick"] = content.Load<Texture2D>("Menu/Buttons/OrangeMenuButtonClick");
            Menu["OrangeMenuButtonHover"] = content.Load<Texture2D>("Menu/Buttons/OrangeMenuButtonHover");
            Menu["StatusBar"] = content.Load<Texture2D>("Menu/StatusBar");
            Menu["ScrollingSelection"] = content.Load<Texture2D>("Menu/ScrollingSelection");
            Menu["SmallThing"] = content.Load<Texture2D>("Menu/SmallThing");
            Menu["SelectedScrolling"] = content.Load<Texture2D>("Menu/SelectedScrolling");
            Menu["ChatTextBoxCursor"] = content.Load<Texture2D>("Menu/TextBox/ChatTextBoxCursor");
            Menu["ChatTextBox"] = content.Load<Texture2D>("Menu/TextBox/ChatTextbox");
            Menu["ChatTextBoxHover"] = Menu["ChatTextBox"];
            Menu["BasicTextBoxCursor"] = content.Load<Texture2D>("Menu/TextBox/BasicTextBoxCursor");
            Menu["BasicTextBox"] = content.Load<Texture2D>("Menu/TextBox/BasicTextBox");
            Menu["textBoxTexture2"] = content.Load<Texture2D>("Menu/TextBox/textBoxTexture2");
            Menu["textBoxTexture2Hover"] = Menu["textBoxTexture2"];
            Menu["textBoxTexture2Cursor"] = Menu["BasicTextBoxCursor"];
            Menu["BasicTextBoxHover"] = content.Load<Texture2D>("Menu/Textbox/BasicTextBoxHover");
            Menu["BasicLargeTextBox"] = content.Load<Texture2D>("Menu/TextBox/BasicLargeTextBox");
            Menu["BasicLargeTextBoxCursor"] = Menu["BasicTextBoxCursor"];
            Menu["fenster"] = content.Load<Texture2D>("Menu/fenster");
            Menu["multiButton"] = content.Load<Texture2D>("Menu/Buttons/multiButton");
            Menu["fenster2"] = content.Load<Texture2D>("Menu/fenster2");
            Menu["fenster3"] = content.Load<Texture2D>("Menu/fenster3");
            Menu["loadingBackground"] = content.Load<Texture2D>("Menu/loadingBackground");
            Menu["loadingBar"] = content.Load<Texture2D>("Menu/loadingBar");
            Menu["multiButton2"] = content.Load<Texture2D>("Menu/Buttons/multiButton2");
            Menu["interface"] = content.Load<Texture2D>("Menu/Backgrounds/interface");
            Menu["logo"] = content.Load<Texture2D>("Menu/Backgrounds/logo");
            Menu["selectionSquare1"] = content.Load<Texture2D>("Game/Reperes/selection1");
            Menu["selectionSquare2"] = content.Load<Texture2D>("Game/Reperes/selection2");
            Menu["avatarFrame"] = content.Load<Texture2D>("Menu/avatarFrame");
            Menu["avatarFrame2"] = content.Load<Texture2D>("Menu/avatarFrame2");
            Menu["bar"] = content.Load<Texture2D>("Menu/bar");
            Menu["BGLaunchGame"] = content.Load<Texture2D>("Menu/Backgrounds/BGLaunchGame");
            Menu["UI"] = content.Load<Texture2D>("Menu/Backgrounds/UI");
            Menu["Observers"] = content.Load<Texture2D>("Menu/Observers");
            Menu["StartButton"] = content.Load<Texture2D>("Menu/Buttons/StartButton");
            Menu["StartButtonHover"] = content.Load<Texture2D>("Menu/Buttons/StartButtonHover");
            Menu["StartButtonClick"] = Menu["StartButtonHover"];
        }
        public static void LoadGameItems(ContentManager content)
        {
            GameItems[typeof(Worker) + "1"] = content.Load<Texture2D>("Game/Units/Worker1");
            GameItems[typeof(Gunner) + "1"] = content.Load<Texture2D>("Game/Units/gunner1");
            GameItems[typeof(Habitation) + "1"] = content.Load<Texture2D>("Game/Buildings/habitation");
            GameItems[typeof(Worker) + "2"] = content.Load<Texture2D>("Game/Units/Worker2");
            GameItems[typeof(Gunner) + "2"] = content.Load<Texture2D>("Game/Units/gunner2");
            GameItems[typeof(Spider) + "2"] = content.Load<Texture2D>("Game/Units/Spider");
            GameItems[typeof(Spider) + "1"] = content.Load<Texture2D>("Game/Units/Spider");
            GameItems[typeof(Habitation) + "2"] = content.Load<Texture2D>("Game/Buildings/habitationBlue");
            GameItems[typeof(University) + "1"] = content.Load<Texture2D>("Game/Buildings/labo-yellow");
            GameItems[typeof(University) + "2"] = content.Load<Texture2D>("Game/Buildings/labo-yellow");
            GameItems[typeof(Laboratory) + "1"] = content.Load<Texture2D>("Game/Buildings/labo-yellow");
            GameItems[typeof(Laboratory) + "2"] = content.Load<Texture2D>("Game/Buildings/labo-blue");
            GameItems[typeof(Creeper) + "0"] = content.Load<Texture2D>("Game/Obstacles/Creeper");
            GameItems[typeof(Gas) + "0"] = content.Load<Texture2D>("Game/Obstacles/Gas");
            GameItems[typeof(Iron) + "0"] = content.Load<Texture2D>("Game/Obstacles/iron");
            GameItems[typeof(Nothing1x1) + "0"] = content.Load<Texture2D>("Game/UI/minitile");
            GameItems[typeof(Nothing2x2) + "0"] = content.Load<Texture2D>("Game/UI/minitile");
            GameItems[typeof(Laserman) + "1"] = content.Load<Texture2D>("Game/Units/Laserman1");
            GameItems[typeof(Laserman) + "2"] = content.Load<Texture2D>("Game/Units/Laserman2");
        }
        public static void LoadMiniGameItems(ContentManager content)
        {
            MiniGameItems[typeof(Worker)] = content.Load<Texture2D>("Game/Minis/Worker");
            MiniGameItems[typeof(Habitation)] = content.Load<Texture2D>("Game/Minis/Habitation");
            MiniGameItems[typeof(Gunner)] = content.Load<Texture2D>("Game/Minis/Gunner");
            MiniGameItems[typeof(University)] = content.Load<Texture2D>("Game/Minis/Labo");
            MiniGameItems[typeof(Laboratory)] = content.Load<Texture2D>("Game/Minis/Labo");
            MiniGameItems[typeof(Creeper)] = content.Load<Texture2D>("Game/Minis/Creeper");
            MiniGameItems[typeof(Nothing1x1)] = content.Load<Texture2D>("Game/Minis/Creeper");
            MiniGameItems[typeof(Nothing2x2)] = content.Load<Texture2D>("Game/Minis/Creeper");
            MiniGameItems[typeof(Spider)] = content.Load<Texture2D>("Game/Minis/Spider");
            MiniGameItems[typeof(Laserman)] = content.Load<Texture2D>("Game/Minis/LaserMini");
            MiniGameItems[typeof(PlayGame.Tasks.ChangeResources.HToE)] = MiniGameItems[typeof(Worker)];
            MiniGameItems[typeof(PlayGame.Tasks.ChangeResources.HEIToS)] = MiniGameItems[typeof(Gunner)];
            MiniGameItems[typeof(PlayGame.Tasks.ChangeResources.HESToG)] = MiniGameItems[typeof(Spider)];
        }
        public static void LoadBigGameItems(ContentManager content)
        {
            BigGameItems[typeof(Worker)] = content.Load<Texture2D>("Game/Bigs/Worker");
            BigGameItems[typeof(Habitation)] = content.Load<Texture2D>("Game/Bigs/Habitation");
            BigGameItems[typeof(University)] = content.Load<Texture2D>("Game/Bigs/Labo");
            BigGameItems[typeof(Laboratory)] = content.Load<Texture2D>("Game/Bigs/Labo");
            BigGameItems[typeof(Gunner)] = content.Load<Texture2D>("Game/Bigs/Gunner");
            BigGameItems[typeof(Spider)] = content.Load<Texture2D>("Game/Bigs/Spider");
            BigGameItems[typeof(Laserman)] = content.Load<Texture2D>("Game/Bigs/LaserBig");
        }
        public static void LoadGame(ContentManager content)
        {
            Game["pathLine"] = content.Load<Texture2D>("Game/Reperes/pathLine");
            Game["mouseMap"] = content.Load<Texture2D>("Game/Reperes/mouseMap");
            Game["selectCase"] = content.Load<Texture2D>("Game/Reperes/selectCase");
            Game["selectUnit"] = content.Load<Texture2D>("Game/Reperes/circleSelectUnit");
            Game["selectUnitHover"] = content.Load<Texture2D>("Game/Reperes/circleSelectUnitHover");
            Game["tileSet-0-0"] = content.Load<Texture2D>("Game/tileSet-0-0");
            Game["tileSet-0-1"] = content.Load<Texture2D>("Game/tileSet-0-1");
            Game["tileSet-0-2"] = content.Load<Texture2D>("Game/tileSet-0-2");
            Game["tileSet-0-3"] = content.Load<Texture2D>("Game/tileSet-0-3");
            Game["tileSet-1-0"] = content.Load<Texture2D>("Game/tileSet-1-0");
            Game["tileSet-1-1"] = content.Load<Texture2D>("Game/tileSet-1-1");
            Game["tileSet-1-2"] = content.Load<Texture2D>("Game/tileSet-1-2");
            Game["tileSet-1-3"] = content.Load<Texture2D>("Game/tileSet-1-3");
            Game["selectBuilding1"] = content.Load<Texture2D>("Game/Reperes/selectionBuilding/1");
            Game["selectBuilding2"] = content.Load<Texture2D>("Game/Reperes/selectionBuilding/2");
            Game["selectBuilding3"] = content.Load<Texture2D>("Game/Reperes/selectionBuilding/3");
            Game["selectBuilding4"] = content.Load<Texture2D>("Game/Reperes/selectionBuilding/4");
            Game["selectBuilding5"] = content.Load<Texture2D>("Game/Reperes/selectionBuilding/5");
            Game["selectBuilding6"] = content.Load<Texture2D>("Game/Reperes/selectionBuilding/6");
            Game["selectBuilding7"] = content.Load<Texture2D>("Game/Reperes/selectionBuilding/7");
            Game["selectBuilding8"] = content.Load<Texture2D>("Game/Reperes/selectionBuilding/8");
            Game["minimap-background"] = content.Load<Texture2D>("Game/MinimapBackground");
        }
        public static void LoadGameUI(ContentManager content)
        {
            GameUI["actions"] = content.Load<Texture2D>("Game/UI/Actions");
            GameUI["bigLifeBar"] = content.Load<Texture2D>("Game/UI/BigLifeBar");
            GameUI["creationProgressBar"] = content.Load<Texture2D>("Game/UI/CreationProgressBar");
            GameUI["creationProgressBarGradient"] = content.Load<Texture2D>("Game/UI/CreationProgressBarGradient");
            GameUI["gradient"] = content.Load<Texture2D>("Game/UI/Gradient");
            GameUI["launchMenuButton"] = content.Load<Texture2D>("Game/UI/LaunchMenuButton");
            GameUI["right"] = content.Load<Texture2D>("Game/UI/Right");
            GameUI["selection"] = content.Load<Texture2D>("Game/UI/selection");
            GameUI["smallLifeBar"] = content.Load<Texture2D>("Game/UI/SmallLifeBar");
            GameUI["topPattern"] = content.Load<Texture2D>("Game/UI/TopPattern");
            GameUI["unitProduction"] = content.Load<Texture2D>("Game/UI/UnitProduction");
            GameUI["smallItem"] = content.Load<Texture2D>("Game/UI/SmallItem");
            GameUI["smallItemHover"] = content.Load<Texture2D>("Game/UI/SmallItemHover");
            GameUI["bigItem"] = content.Load<Texture2D>("Game/UI/BigItem");
            GameUI["Build"] = content.Load<Texture2D>("Game/UI/Actions/build");
            GameUI["Buildhover"] = content.Load<Texture2D>("Game/UI/Actions/buildhover");
            GameUI["Hold"] = content.Load<Texture2D>("Game/UI/Actions/hold");
            GameUI["Holdhover"] = content.Load<Texture2D>("Game/UI/Actions/holdhover");
            GameUI["Attack"] = content.Load<Texture2D>("Game/UI/Actions/Attack");
            GameUI["Attackhover"] = content.Load<Texture2D>("Game/UI/Actions/AttackHover");
            GameUI["Die"] = content.Load<Texture2D>("Game/UI/Actions/Die");
            GameUI["Diehover"] = content.Load<Texture2D>("Game/UI/Actions/DieHover");
            GameUI["Patrol"] = content.Load<Texture2D>("Game/UI/Actions/Patrol");
            GameUI["Patrolhover"] = content.Load<Texture2D>("Game/UI/Actions/PatrolHover");
            GameUI["Research"] = content.Load<Texture2D>("Game/UI/Actions/Research");
            GameUI["Researchhover"] = content.Load<Texture2D>("Game/UI/Actions/ResearchHover");
            GameUI["MiniTile"] = content.Load<Texture2D>("Game/UI/minitile");
            GameUI["Minimap"] = content.Load<Texture2D>("Game/UI/Minimap");
            GameUI["Electricity"] = content.Load<Texture2D>("Game/UI/Resources/electricity");
            GameUI["Steel"] = content.Load<Texture2D>("Game/UI/Resources/steel");
            GameUI["Iron"] = content.Load<Texture2D>("Game/UI/Resources/iron");
            GameUI["Graphene"] = content.Load<Texture2D>("Game/UI/Resources/graphene");
            GameUI["Hydrogen"] = content.Load<Texture2D>("Game/UI/Resources/hydrogen");
            GameUI["BGResources"] = content.Load<Texture2D>("Game/UI/Resources/background");
            GameUI["LabElectrecity"] = content.Load<Texture2D>("Game/UI/Actions/Electricity");
            GameUI["LabElectrecityHover"] = content.Load<Texture2D>("Game/UI/Actions/ElectricityHover");
            GameUI["LabSteel"] = content.Load<Texture2D>("Game/UI/Actions/Steel");
            GameUI["LabSteelHover"] = content.Load<Texture2D>("Game/UI/Actions/SteelHover");
            GameUI["LabGraphene"] = content.Load<Texture2D>("Game/UI/Actions/Graphene");
            GameUI["LabGrapheneHover"] = content.Load<Texture2D>("Game/UI/Actions/GrapheneHover");
        }
        public static void LoadParticles(ContentManager content)
        {
            Particles["star1"] = content.Load<Texture2D>("Particles/particle1");
            Particles["star2"] = content.Load<Texture2D>("Particles/particle2");
            Particles["star3"] = content.Load<Texture2D>("Particles/particle3");
            Particles["star4"] = content.Load<Texture2D>("Particles/particle4");
            Particles["star5"] = content.Load<Texture2D>("Particles/particle5");
        }
    }
}
