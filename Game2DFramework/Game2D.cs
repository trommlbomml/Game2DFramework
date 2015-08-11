using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using Game2DFramework.Cameras;
using Game2DFramework.Drawing;
using Game2DFramework.Gui;
using Game2DFramework.Input;
using Game2DFramework.Interaction;
using Game2DFramework.States;
using Game2DFramework.States.Transitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework
{
    public delegate void GlobalObjectChangedEventHandler(string name, GameObject oldValue, GameObject newValue);

    public abstract class Game2D : Game
    {
        private const string DefaultConfigFileName = "game.config.xml";

        private readonly string _configFileName;
        private readonly Dictionary<string, GameProperty> _properties = new Dictionary<string, GameProperty>();
        private StateManager _stateManager;
        private readonly ClearOptions _clearOptions;
        private GamePadEx _gamePad;
        private Type _startupState;
        private readonly Dictionary<string, GameObject> _registeredGlobals;
        private readonly List<ActionTimer> _activeTimers = new List<ActionTimer>();

        public int ScreenWidth { get { return GraphicsDevice.Viewport.Width; } }
        public int ScreenHeight { get { return GraphicsDevice.Viewport.Height; } }
        public Vector2 ScreenSize { get { return new Vector2(ScreenWidth, ScreenHeight);} }
        public DepthRenderer DepthRenderer { get; private set; }
        public ShapeRenderer ShapeRenderer { get; private set; }
        public KeyboardEx Keyboard { get; private set; }
        public MouseEx Mouse { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public Camera Camera { get; private set; }
        public Cursor Cursor { get; private set; }
        public GuiSystem GuiSystem { get; private set; }

        public GamePadEx GamePad
        {
            get
            {
                if (_gamePad == null) throw new InvalidOperationException("Gamepad must be enabled for game to use");
                return _gamePad;
            }
            private set { _gamePad = value; }
        }

        public event GlobalObjectChangedEventHandler GlobalObjectChanged;

        private int GetScreenSizeComponent(int defaultSize, string propertyName, int fallbackValue)
        {
            var size = GetPropertyIntOrDefault(propertyName, defaultSize);
            if (size == 0) size = fallbackValue;
            return size;
        }
        
        protected Game2D(int defaultScreenWidth = 800, int defaultScreenHeight = 600, bool fullscreen = false, bool useGamePad = false, DepthFormat depthFormat = DepthFormat.None, string configFileName = null)
        {
            _configFileName = string.IsNullOrEmpty(configFileName) ? DefaultConfigFileName : configFileName;
            LoadGameProperties();

            var width = GetScreenSizeComponent(defaultScreenWidth, GameProperty.GameResolutionXProperty, 800);
            var height = GetScreenSizeComponent(defaultScreenHeight, GameProperty.GameResolutionYProperty, 600);

            GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = width,
                PreferredBackBufferHeight = height,
                PreferredDepthStencilFormat = depthFormat,
                IsFullScreen = fullscreen
            };
            Content.RootDirectory = "Content";

            Keyboard = new KeyboardEx();
            Mouse = new MouseEx();
            GuiSystem = new GuiSystem(this);
            DepthRenderer = new DepthRenderer();
            Cursor =  new Cursor(this);
            if (useGamePad) GamePad = new GamePadEx();

            _clearOptions = ClearOptions.Target;
            if (depthFormat != DepthFormat.None)
            {
                _clearOptions |= ClearOptions.DepthBuffer;
            }

            _registeredGlobals = new Dictionary<string, GameObject>();
        }

        private GameProperty GetOrCreateProperty(string name)
        {
            GameProperty property;
            if (!_properties.TryGetValue(name, out property))
            {
                property = new GameProperty(name);
                _properties.Add(property.Name, property);
            }
            return property;
        }

        public void SetProperty(string name, string value)
        {
            GetOrCreateProperty(name).Value = value;
        }

        public void SetProperty(string name, int value)
        {
            GetOrCreateProperty(name).Value = value.ToString(CultureInfo.InvariantCulture);
        }

        public void SetProperty(string name, bool value)
        {
            GetOrCreateProperty(name).Value = value.ToString(CultureInfo.InvariantCulture);
        }

        public bool TryGetPropertyString(string name, out string value)
        {
            return TryGetProperty(name, s => s, out value);
        }

        public bool TryGetPropertyInt(string name, out int value)
        {
            return TryGetProperty(name, int.Parse, out value);
        }

        public string GetPropertyStringOrDefault(string name, string defaultValue = "")
        {
            return GetPropertyValueOrDefault(name, s => s, defaultValue);
        }

        public int GetPropertyIntOrDefault(string name, int defaultValue = 0)
        {
            return GetPropertyValueOrDefault(name, int.Parse, defaultValue);
        }

        public bool GetPropertyBoolOrDefault(string name, bool defaultValue = false)
        {
            return GetPropertyValueOrDefault(name, bool.Parse, defaultValue);
        }

        public TPropertyType GetPropertyValueOrDefault<TPropertyType>(string name, Func<string, TPropertyType> converter,
            TPropertyType defaultValue = default(TPropertyType))
        {
            TPropertyType value;
            return TryGetProperty(name, converter, out value) ? value : defaultValue;
        }

        public bool TryGetProperty<TPropertyType>(string name, Func<string, TPropertyType> converter, out TPropertyType value)
        {
            GameProperty property;
            value = _properties.TryGetValue(name, out property) ? converter(property.Value) : default(TPropertyType);
            return property != null;
        }

        public void LoadGameProperties()
        {
            if (!File.Exists(_configFileName)) return;

            var document = new XmlDocument();
            document.Load(_configFileName);

            if (document.DocumentElement != null)
            {
                foreach (XmlElement element in document.DocumentElement.ChildNodes)
                {
                    var name = element.GetAttribute("Name");
                    var value = element.GetAttribute("Value");
                    _properties.Add(name, new GameProperty(name) {Value = value});
                }   
            }
        }

        public void SavePropertyChanges()
        {
            var document = new XmlDocument();
            document.AppendChild(document.CreateElement("GameConfiguration"));

            foreach (var gameProperty in _properties.Values)
            {
// ReSharper disable PossibleNullReferenceException
                var element = (XmlElement)document.DocumentElement.AppendChild(document.CreateElement("Property"));
// ReSharper restore PossibleNullReferenceException
                element.SetAttribute("Name", gameProperty.Name);
                element.SetAttribute("Value", gameProperty.Value);
            }

            document.Save(_configFileName);
        }

        private void InvokeGlobalObjectChanged(string name, GameObject oldValue, GameObject newValue)
        {
            if (GlobalObjectChanged != null) GlobalObjectChanged(name, oldValue, newValue);
        }

        public T RegisterGlobalObject<T>(string name, T gameObject) where T: GameObject
        {
            if (_registeredGlobals.ContainsKey(name)) throw new InvalidOperationException("There is aleady an object registered with name " + name + ".");
            if (gameObject == null) throw new ArgumentNullException("gameObject");
            
            _registeredGlobals.Add(name, gameObject);
            InvokeGlobalObjectChanged(name, null, gameObject);

            return gameObject;
        }

        public void ReplaceGlobalObject(string name, GameObject gameObject)
        {
            if (!_registeredGlobals.ContainsKey(name)) throw new InvalidOperationException("There is no object registered with name " + name + ".");
            if (gameObject == null) throw new ArgumentNullException("gameObject");

            var oldValue = _registeredGlobals[name];
            _registeredGlobals[name] = gameObject;
            InvokeGlobalObjectChanged(name, oldValue, gameObject);
        }

        public T GetGlobalObject<T>(string name) where T: GameObject
        {
            if (!_registeredGlobals.ContainsKey(name)) throw new InvalidOperationException(string.Format("Object {0} is not registered", name));
            return (T) _registeredGlobals[name];
        }

        protected abstract Type RegisterStates();

        protected override void Initialize()
        {
            _stateManager = new StateManager(this, _clearOptions);
            
            RegisterTransition(new BlendTransition());
            RegisterTransition(new FlipTransition(GraphicsDevice));
            RegisterTransition(new GrowTransition(GraphicsDevice));
            RegisterTransition(new SlideTransition(GraphicsDevice));
            RegisterTransition(new CardTransition(GraphicsDevice));
            RegisterTransition(new ThrowAwayTransition(GraphicsDevice));
            RegisterTransition(new ZappoutTransition(GraphicsDevice));

            _startupState = RegisterStates();
            Camera = new Camera(this);
            base.Initialize();
        }

        protected void RegisterTransition(ITransition transition)
        {
            _stateManager.RegisterTransition(transition);
        }

        protected void RegisterState(IState state)
        {
            _stateManager.RegisterState(state);
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            ShapeRenderer = new ShapeRenderer(SpriteBatch, GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            var elapsedTime = gameTime.ElapsedGameTime.Milliseconds*0.001f;

            Cursor.Update();

            if (_startupState != null)
            {
                _stateManager.SetCurrentState(_startupState, null);
                _startupState = null;
            }

            if (!_stateManager.TransitionInProgress)
            {
                Camera.Update(elapsedTime);
                Keyboard.Update();
                Mouse.Update(elapsedTime);
                _activeTimers.ForEach(a => a.Update(elapsedTime));
                _activeTimers.RemoveAll(t => !t.IsRunning);
                if (_gamePad != null) GamePad.Update();   
            }

            if (!_stateManager.Update(elapsedTime)) Exit();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var elapsedTime = gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            _stateManager.Draw(elapsedTime);
            base.Draw(gameTime);
        }

        public void ActivateDefaultView()
        {
            SpriteBatch.End();
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.Identity);
        }

        public void AddDelayedAction(Action action, float deltaTimeSeconds)
        {
            var actionTimer = new ActionTimer(action, deltaTimeSeconds);
            actionTimer.Start();
            _activeTimers.Add(actionTimer);
        }

        public Rectangle GetScreenRectangle()
        {
            return new Rectangle(0,0,ScreenWidth, ScreenHeight);
        }
    }
}
