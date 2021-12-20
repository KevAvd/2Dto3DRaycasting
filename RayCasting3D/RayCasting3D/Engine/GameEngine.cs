using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using RayCasting3D.Engine.Graphics;
using RayCasting3D.Engine.Object;
using RayCasting3D.Engine.Logic;


namespace RayCasting3D.Engine
{
    internal class GameEngine
    {
        private RenderWindow _window;
        private double _totalElapsedSeconds;
        private Color _clearColor;
        private uint _windowWidth;
        private uint _windowHeight;
        private string _windowTitle;
        private GameObject _gameObject;
        private GameLogic _gameLogic;
        private GameDraw _gameDraw;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="windowWidth"> Longueur de la fenêtre </param>
        /// <param name="windowHeight"> Hauteur de la fenêtre </param>
        /// <param name="windowTitle"> Titre de la fenêtre </param>
        public GameEngine(uint windowWidth, uint windowHeight, string windowTitle)
        {
            _window = new RenderWindow(new VideoMode(windowWidth, windowHeight), windowTitle);
            _clearColor = Color.Black;
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
            _windowTitle = windowTitle;
            _gameObject = new GameObject(windowWidth, windowHeight);
            _gameLogic = new GameLogic(_gameObject);
            _gameDraw = new GameDraw(_window, _gameObject, _gameLogic);
        }

        /// <summary>
        /// Lance le moteur de jeu
        /// </summary>
        public void Run()
        {
            //Initialisation
            Console.CursorVisible = false;

            //Charge les éléments
            Load();

            //Lance la boucle de jeu
            DateTime initialTime = DateTime.Now;
            while (_window.IsOpen)
            {
                //Obtient le temps de réalisation d'une image
                _totalElapsedSeconds = (DateTime.Now - initialTime).TotalSeconds;

                //Affiche des info du jeu sur la console
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"FPS : {1 / _totalElapsedSeconds:0.00}      ");
                Console.WriteLine($"Player X : {_gameObject.Player.Position.X:0.00} Y : {_gameObject.Player.Position.Y:0.00}               ");
                Console.WriteLine($"Player view angle : {_gameObject.Player.ViewAngle:0.00}               ");
                Console.WriteLine($"Player Speed : {_gameObject.Player.Speed} ");
                //Console.WriteLine($"Player view angle : {_gameObject.Player.ViewAngle}               ");

                initialTime = DateTime.Now;

                //Met à jour
                Update();

                //Efface la dernière image
                _window.Clear(_clearColor);

                //Crée une nouvelle image
                Render();

                //Affiche l'image
                _window.Display();
            }
        }

        /// <summary>
        /// Charge les éléments nécessaire
        /// </summary>
        private void Load()
        {
            int[,] mapValues =
            {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,1,0,0,1,1,1,0,0,1},
                {1,0,0,1,0,1,0,1,0,0,0,0,0,0,0,1,0,0,0,1},
                {1,0,1,0,1,0,1,0,1,0,0,0,0,0,0,1,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,1,0,0,1,1,1,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1},
                {1,0,1,0,1,0,1,0,1,0,0,1,0,0,0,1,0,0,0,1},
                {1,0,0,1,0,1,0,1,0,0,0,1,0,0,1,1,1,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,1},
                {1,0,1,1,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,1},
                {1,0,1,0,0,1,1,1,0,0,0,1,0,0,0,1,0,0,0,1},
                {1,0,0,0,0,1,0,0,0,0,0,1,0,1,0,0,0,1,0,1},
                {1,0,0,0,0,1,0,1,0,1,0,1,0,0,1,0,1,0,0,1},
                {1,0,1,0,0,0,0,0,0,1,0,1,0,0,1,1,1,0,0,1},
                {1,0,1,1,0,0,0,1,1,1,0,1,0,0,0,1,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
            };
            _gameObject.CreateMap(mapValues, 20, Color.Green);
            _gameObject.CreatePlayer(new Vector2f(30, 30), new Color(255, 0, 0), 6, 70);
            _gameObject.Player.Speed = 50;
            _gameObject.Ground.FillColor = Color.Black;
            _clearColor = Color.Black;
        }

        /// <summary>
        /// Met à jour le jeu
        /// </summary>
        private void Update()
        {
            _gameLogic.PlayerMove(_totalElapsedSeconds);
            _gameLogic.Raycasting(_gameObject.Player.FOV, _windowWidth, Color.Yellow, 18);
        }

        /// <summary>
        /// Fait le rendu de l'image
        /// </summary>
        private void Render()
        {
            _gameDraw.Ground();
            _gameDraw.AllRayLine(false, _windowHeight,true);
            if(_gameObject.Map.Displayed)
            {
                _gameDraw.Map();
                _gameDraw.AllRays();
                _gameDraw.Player();
            }
        }

    }
}
