using SFML.System;
using SFML.Graphics;

namespace RayCasting3D.Engine.Object
{
    internal class GameObject
    {
        public Map Map { get; set; } //Contient la carte
        public Ray[] Rays { get; set; } //Contient les rayons
        public Player Player { get; set; } //Contient le joueur
        public RectangleShape Ground { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="windowWidth"> Taille de la fenêtre </param>
        public GameObject(uint windowWidth, uint windowHeight)
        {
            Rays = new Ray[windowWidth];
            Ground = new RectangleShape();
            Ground.Position = new Vector2f(0,windowHeight / 2);
            Ground.Size = new Vector2f(windowWidth, windowHeight / 2);
            Ground.FillColor = Color.White;
        }

        /// <summary>
        /// Crée un joueur
        /// </summary>
        /// <param name="position"> Position du joueur </param>
        /// <param name="color"> Couleur du joueur </param>
        /// <param name="size"> Taille du joueur </param>
        public void CreatePlayer(Vector2f position, Color color, float size, double fov)
        {
            this.Player = new Player(position, size, color, 50, fov);
        }

        /// <summary>
        /// Crée une carte
        /// </summary>
        /// <param name="mapValues"> Valeur de la carte </param>
        /// <param name="cellSize"> Taille d'une cellule de la carte </param>
        /// <param name="wallColor"> Couleur des murs </param>
        public void CreateMap(int[,] mapValues, int cellSize, Color wallColor)
        {
            this.Map = new Map(mapValues, cellSize, wallColor);
        }
    }

    class Map
    {
        public int[,] Values { get; set; }
        public Cell[,] Cells { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public bool Displayed { get; set; }

        public Map(int[,] mapValues, int cellSize, Color wallColor)
        {
            Values = new int[mapValues.GetLength(0), mapValues.GetLength(1)];
            Cells = new Cell[mapValues.GetLength(0), mapValues.GetLength(1)];
            Values = mapValues;
            Height = Values.GetLength(0);
            Width = Values.GetLength(1);
            Displayed = true;
            InitCells(cellSize, wallColor);
        }

        public void InitCells(int size, Color color)
        {
            Vector2f cellPos = new Vector2f(0, 0);
            for (int y = 0; y < Cells.GetLength(0); y++)
            {
                for (int x = 0; x < Cells.GetLength(1); x++)
                {
                    if (Values[y, x] == 1)
                    {
                        Cells[y, x] = new Cell(cellPos, size, color);
                    }
                    else
                    {
                        Cells[y, x] = new Cell(cellPos, size, new Color(20,20,20));
                    }
                    cellPos.X += size;
                }
                cellPos.Y += size;
                cellPos.X = 0;
            }
        }
    }

    class Cell
    {
        public Vector2f Position { get; set; }
        public float Size { get; set; }
        public Color Color { get; set; }

        public Cell(Vector2f position, float size, Color color)
        {
            Position = position;
            Size = size;
            Color = color;
        }
    }
    
    class Ray
    {
        public Vector2f StartPoint { get; set; }
        public Vector2f HitPoint { get; set; }
        public double Angle { get; set; }
        public Color Color { get; set; }
        public Vector2f Vector { get; set; }
        public double Length { get; set; }
        public bool Horizontal { get; set; }
        public bool HasRayHit { get; set; }

        public Ray(Vector2f startPoint, Vector2f hitPoint, double angle, Color color, bool horizontal, bool hasRayHit)
        {
            StartPoint = startPoint;
            HitPoint = hitPoint;
            Angle = angle;
            this.Color = color;
            Vector = new Vector2f(HitPoint.X - StartPoint.X, HitPoint.Y - StartPoint.Y);
            Length = Math.Sqrt(Math.Pow(Math.Abs(Vector.X), 2) + Math.Pow(Math.Abs(Vector.Y), 2));
            Horizontal = horizontal;
            HasRayHit = hasRayHit;
        }
    }

    class Player
    {
        public float Size { get; set; }
        public Vector2f Position { get; set; }
        public Color Color { get; set; }
        public double ViewAngle { get; set; }
        public double Speed { get; set; }
        public double FOV { get; set; }

        public Player(Vector2f position, float size, Color color, double speed, double fov)
        {
            Position = position;
            Size = size;
            this.Color = color;
            ViewAngle = 0;
            Speed = speed;
            FOV = fov;
        }
    }
}
