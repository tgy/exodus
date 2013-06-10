using System;
using System.Collections.Generic;
using System.Linq;
using Exodus.GUI;
using Exodus.GUI.Components;
using Exodus.GUI.Components.Buttons;
using Exodus.GUI.Components.Buttons.MenuButtons;
using Exodus.GUI.Items;
using Exodus.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using Exodus.Network;
using Exodus.Network.ClientSide;
using Exodus.Network.ServerSide;

namespace Exodus
{
    public class Application : Microsoft.Xna.Framework.Game
    {
        TextBox login, pass;
        readonly GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        ScrollingSelection _scrollingSelection;
        MenuState _gameLauncherMenu;
        OrangeMenuButton _gameSelectionButton;
        bool _searchingLAN = true;
        Stack<GameState> GameStates;
        public void Push(GameState g)
        {
            g.Initialize();
            g.LoadContent();
            if (GameStates.Count > 0)
            {
                if (GameStates.Peek().GetType() == g.GetType())
                {
                    g.Music = GameStates.Peek().Music;
                }
                else
                {
                    if (GameStates.Peek().Music != null)
                        GameStates.Peek().Music.Pause();
                    if (g.Music != null)
                        g.Music.Play();
                }
            }
            GameStates.Push(g);
        }
        public GameState Peek()
        {
            return GameStates.Peek();
        }
        public GameState Pop()
        {
            GameState g = GameStates.Pop();
            if (GameStates.Count > 0)
            {
                if (GameStates.Peek().GetType() != g.GetType())
                {
                    if (g.Music != null)
                        g.Music.Stop();
                    if (GameStates.Peek().Music != null)
                        GameStates.Peek().Music.Resume();
                }
            }
            g.UnLoad();
            if (GameStates.Count == 0)
                Environment.Exit(0);
            _searchingLAN = true;
            _gameSelectionButton.Text = "SEARCH INTERNET";
            return g;
        }
        public Application()
        {
            _graphics = new GraphicsDeviceManager(this)
                {
                    PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                    PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height
                };

            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            Inputs.Initialise();
            Data.Window.WindowWidth = Window.ClientBounds.Width;
            Data.Window.WindowHeight = Window.ClientBounds.Height;
            Data.Window.ScreenCenter = new Point((int)(Data.Window.WindowWidth / 2), (int)(Data.Window.WindowHeight / 2));
            GameStates = new Stack<GameState>();

            Fonts.Initialize(Content);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.IsMouseVisible = false;
            Inputs.Initialise();
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Data.Load();
            Audio.LoadAudio(Content);
            Textures.LoadParticles(Content);
            Textures.LoadGame(Content);
            Textures.LoadGameItems(Content);
            Textures.LoadGameUI(Content);
            Textures.LoadMenu(Content);
            Textures.LoadMiniGameItems(Content);
            Textures.LoadBigGameItems(Content);
            GameMouse.Initialize(Content);
            StatusBar statusBar = new StatusBar(Data.Window.ScreenCenter.X - Textures.Menu["StatusBar"].Width / 2,
                                                Data.Window.ScreenCenter.Y - 117);
            ParticleEngine.ParticleEngine particleMenu = new ParticleEngine.Engine.StarsEngine(
                Data.Window.WindowWidth / 2, Data.Window.WindowHeight / 2, 0);

            #region Game Launcher
            _gameLauncherMenu = BaseMenu(particleMenu);
            _gameLauncherMenu.Items.Add(new Passive(Textures.Menu["ScrollingSelection"], (Data.Window.WindowWidth - Textures.Menu["ScrollingSelection"].Width) / 2, Data.Window.ScreenCenter.Y - 20, 4 * float.Epsilon));
            MenuHorizontal createGameMenu = new MenuHorizontal(Data.Window.ScreenCenter.X - 330, Data.Window.ScreenCenter.Y - 8, 5);
            OrangeMenuButton createGameButton = new OrangeMenuButton("CREATE A GAME");
            _gameSelectionButton = new OrangeMenuButton("SEARCH INTERNET");
            _gameSelectionButton.DoClick = SwitchSearchingMode;
            createGameButton.DoClick = CreateGame;
            createGameMenu.Create(new List<Component> { createGameButton, _gameSelectionButton });
            MenuVertical joinGameMenu = new MenuVertical(Data.Window.ScreenCenter.X + 175, Data.Window.ScreenCenter.Y + 250, 0);
            OrangeMenuButton joinGameButton = new OrangeMenuButton("JOIN THIS GAME");
            joinGameButton.DoClick = JoinMultiPlay;
            joinGameMenu.Create(new List<Component> { joinGameButton });
            MenuVertical refreshMenu = new MenuVertical(Data.Window.ScreenCenter.X + 175, Data.Window.ScreenCenter.Y - 8, 0);
            OrangeMenuButton refreshButton = new OrangeMenuButton("REFRESH");
            refreshButton.DoClick = RefreshServerLists;
            refreshMenu.Create(new List<Component> { refreshButton });

            _scrollingSelection = new ScrollingSelection((Data.Window.WindowWidth - Textures.Menu["ScrollingSelection"].Width) / 2, Data.Window.ScreenCenter.Y - 20, new Tuple<string, string, string>("PLAYER", "IP                                MAP", "CREATED"), new List<int> { 10, 164, 590 });
            _scrollingSelection.Reset(new List<Tuple<string, string, string>>());
            _gameLauncherMenu.Items.Add(_scrollingSelection);
            _gameLauncherMenu.Items.Add(joinGameMenu);
            _gameLauncherMenu.Items.Add(createGameMenu);
            _gameLauncherMenu.Items.Add(refreshMenu);
            #endregion

            #region Main Menu

            MenuState mainMenu = BaseMenu(particleMenu);
            MenuVertical rightMenu = new MenuVertical(Data.Window.ScreenCenter.X + 112, Data.Window.ScreenCenter.Y + 42,
                                                      7);
            MenuButton playOnline = new BlueMenuButton("PLAY ONLINE");
            playOnline.SubMenu = _gameLauncherMenu;
            playOnline.DoClick = LaunchLobby;
            MenuButton playSolo = new BlueMenuButton("PLAY AGAINST AI");
            playSolo.DoClick = PlaySinglePlayer;
            MenuButton mapEditor = new BlueMenuButton("MAP EDITOR");
            mapEditor.DoClick = Editor;
            MenuButton settings = new BlueMenuButton("SETTINGS");
            settings.DoClick = DoNothing;
            MenuButton credits = new BlueMenuButton("CREDITS");
            credits.DoClick = DoNothing;
            MenuButton exit = new BlueMenuButton("EXIT THE GAME");
            exit.DoClick = Exit;
            rightMenu.Create(new List<Component> { playOnline, playSolo, mapEditor, settings, credits, exit });
            mainMenu.Items.Add(rightMenu);
            mainMenu.Items.Add(statusBar);

            #endregion

            #region Connection Menu

            MenuState connectionMenu = BaseMenu(particleMenu);

            Form connectionForm = new Form(Data.Window.ScreenCenter.X - 140, Data.Window.ScreenCenter.Y - 25,
                                           new Padding(14, 17), 8, 4 * Data.GameDisplaying.Epsilon);
            connectionForm.Components.Add(new JustTexture(Textures.Menu["ConnectionBackground"], connectionForm.Area.X,
                                                          connectionForm.Area.Y, connectionForm.Depth));
            connectionForm.Components.Add(new Label(Fonts.Eurostile12, "LOGIN", 0, 0));
            login = new TextBox(0, 0, "", "ConnectionTextBox", new Padding(14, -6), 30, 2 * Data.GameDisplaying.Epsilon);
            connectionForm.Components.Add(login);
            connectionForm.Components.Add(new Label(Fonts.Eurostile12, "PASSWORD", 0, 0));
            pass = new TextBox(0, 0, "", "ConnectionTextBox", new Padding(14, -6), 30, 2 * Data.GameDisplaying.Epsilon);
            connectionForm.Components.Add(pass);
            ConnectionOrangeButton connectionFormSubmitter = new ConnectionOrangeButton("CONNECT TO EXODUS")
            {
                SubMenu = mainMenu,
                DoClick = ConnectionFormSubmit
            };
            connectionForm.SubmitterId = connectionForm.Components.Count;
            connectionForm.Components.Add(connectionFormSubmitter);
            ConnectionGreenButton connectionFormSignup = new ConnectionGreenButton("GET AN ACCOUNT FOR FREE");
            // connectionFormSignup.DoClick = FIX ME (lance une page html vers la page d'inscription du site)
            connectionForm.Components.Add(connectionFormSignup);
            connectionForm.Initialize();
            connectionMenu.Items.Add(connectionForm);
            connectionMenu.Initialize();

            #endregion

            Push(connectionMenu);

            base.LoadContent();
        }

        
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            Data.Window.GameFocus = IsActive;
            GameMouse.Update(gameTime);
            Inputs.Update(gameTime);
            if (Inputs.KeyPress(Keys.F11))
                _graphics.ToggleFullScreen();
            /*if (Inputs.KeyPress(Keys.Escape))
            {
                if (GameStates.Count > 1)
                    GameStates.Pop().UnLoad();
                else
                    Environment.Exit(0);
            }*/
            GameStates.Peek().Update(gameTime);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            #region On récupère la totalité des maps existantes et on met à jour pour l'affichage
            List<Network.Game> lg;
            if (_searchingLAN)
                lg = Client.ServerList;
            else
                lg = SyncClient.InternetGames;
            List<Tuple<string, string, string>> l = new List<Tuple<string, string, string>>();

            for (int i = 0, c = lg.Count; i < c; i++)
            {
                l.Add(new Tuple<string, string, string>(lg[i].HostName + " (" + lg[i].NbPlayers + ")", lg[i].IP + "  " + lg[i].Map, lg[i].CreationTime.ToShortTimeString()));
            }
            _scrollingSelection.Reset(l);
            #endregion
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            GameStates.Peek().Draw(_spriteBatch);
            GameMouse.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        #region subFuncs
        private void DoNothing(MenuState m, int i)
        {

        }
        private void LaunchLobby(MenuState m, int i)
        {
            RefreshServerLists(null, 0);
            Push(m);
        }
        private void Exit(MenuState m, int i)
        {
            Environment.Exit(0);
        }
        private void PlaySinglePlayer(MenuState m, int i)
        {
            GameState playState = new PlayState(this);
            Push(playState);
            Data.Network.SinglePlayer = true;
            Data.Network.Client = "";
            Data.Network.ServerIP = "";
            Data.Network.Server = "";
            Data.Network.Error = "";
            Data.Network.IdPlayer = 1;
            NetGame.Start("No");
        }
        private void JoinMultiPlay(MenuState m, int i)
        {
            if (_scrollingSelection.SelectedItem >= 0 && _scrollingSelection.SelectedItem < _scrollingSelection.Entries.Count)
            {
                Data.Config.currentMap = _scrollingSelection.Entries[_scrollingSelection.SelectedItem].Item2;
                PlaySinglePlayer(m, i);
                if (_scrollingSelection.SelectedItem != -1)
                {
                    if (_searchingLAN)
                        Data.Network.LastIP = Client.ServerList[_scrollingSelection.SelectedItem].IP;
                    else
                        Data.Network.LastIP = SyncClient.InternetGames[_scrollingSelection.SelectedItem].IP;
                    //Data.Network.LastIP = "90.24.210.69";
                    Data.Network.SinglePlayer = false;
                    NetGame.Start("C");
                }
            }
        }
        private void CreateGame(MenuState m, int i)
        {
            GameState playState = new PlayState(this);
            Push(playState);
            Data.Network.SinglePlayer = false;
            NetGame.Start("SC");
        }
        private void Editor(MenuState m, int i)
        {
            GameState editorState = new MapEditorState(this);
            Push(editorState);
        }
        private void RefreshServerLists(MenuState m, int i)
        {
            if (_searchingLAN)
            {
                Client.RefreshLANServerList();
                //SyncClient.Stop();
            }
            else
            {
                Client.RefreshInternetServerList();
            }
        }
        public GraphicsDevice GetGraphicDevice()
        {
            return GraphicsDevice;
        }
        private void SwitchSearchingMode(MenuState m, int i)
        {
            if (!_searchingLAN)
            {
                _gameSelectionButton.Text = "SEARCH INTERNET";
            }
            else
            {
                _gameSelectionButton.Text = "SEARCH LAN";
            }
            _gameSelectionButton.SetPosition();
            _searchingLAN = !_searchingLAN;
            RefreshServerLists(null, 0);
        }

        public void ConnectionFormSubmit(MenuState m, int i)
        {
            // FIX ME -> verifications des identifiants via le GameManager
            //
            // FIXME: Regarde SyncClient.UserIsValid(...)
            // 
            SyncClient.IsAuthenticated = false;
            SyncClient.UserIsValid(login.Value, pass.Value); /*ATTENTION: Cette methode impliquant du reseau et du SQL, prevoir au moins 100ms pour qu'elle soit effectuee (peut prendre plus de 3 s)*/
            // if (Player.ConnectionState == 1)
                Push(m);
            // else
            //     Tell the user he is stupid
        }
        #endregion

        private MenuState BaseMenu(ParticleEngine.ParticleEngine particleMenu)
        {
            Texture2D t;
            MenuState baseMenuState = new MenuState(this);
            baseMenuState.Items.Add(new Background(Textures.Menu["MainBackground"]));
            baseMenuState.Items.Add(new GUI.Items.ParticleEngine(particleMenu));
            t = Textures.Menu["logo"];
            baseMenuState.Items.Add(new Passive(Textures.Menu["logo"], (Data.Window.WindowWidth - t.Width) / 2,
                                                Data.Window.ScreenCenter.Y / 2 - 177, 5 * Data.GameDisplaying.Epsilon));
            t = Textures.Menu["interface"];
            baseMenuState.Items.Add(new Passive(t, (Data.Window.WindowWidth - t.Width) / 2,
                                                Data.Window.ScreenCenter.Y - (t.Height / 2) + 128,
                                                5 * Data.GameDisplaying.Epsilon));
            return baseMenuState;
        }
    }
}
