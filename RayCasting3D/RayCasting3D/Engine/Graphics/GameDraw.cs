using SFML.Graphics;
using SFML.System;
using RayCasting3D.Engine.Object;
using RayCasting3D.Engine.Logic;

namespace RayCasting3D.Engine.Graphics
{
    internal class GameDraw
    {
        private GameObject _gameObject;
        private GameLogic _gameLogic;
        private RenderWindow _window;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="windowToDraw"> fenêtre sur laquel dessiner </param>
        public GameDraw(RenderWindow windowToDraw, GameObject gameObject, GameLogic gameLogic)
        {
            _window = windowToDraw;
            _gameObject = gameObject;
            _gameLogic = gameLogic;
        }

        /// <summary>
        /// Dessine un carré
        /// </summary>
        /// <param name="position"> Position du carré </param>
        /// <param name="color"> Couleur du carré </param>
        /// <param name="size"> taille du carré </param>
        public void Square(Vector2f position, Color color, float size)
        {
            Rectangle(position, color, new Vector2f(size, size));
        }

        /// <summary>
        /// Dessine un rectangle
        /// </summary>
        /// <param name="position"> Position du rectangle </param>
        /// <param name="color"> Couleur du rectangle </param>
        /// <param name="size"> Taille du rectangle </param>
        public void Rectangle(Vector2f position, Color color, Vector2f size)
        {
            RectangleShape rectangleShape = new RectangleShape();
            rectangleShape.Position = position;
            rectangleShape.FillColor = color;
            rectangleShape.Size = size;
            _window.Draw(rectangleShape);
        }

        /// <summary>
        /// Dessine une ligne
        /// </summary>
        /// <param name="startPosition"> Position de départ </param>
        /// <param name="endPosition"> Position d'arrivé </param>
        /// <param name="color"> Couleur de la ligne </param>
        public void Line(Vector2f startPosition, Vector2f endPosition, Color color)
        {
            Vertex[] line = new Vertex[2];
            line[0] = new Vertex(startPosition, color);
            line[1] = new Vertex(endPosition, color);
            _window.Draw(line, PrimitiveType.Lines);
        }

        public void Ground()
        {
            _window.Draw(_gameObject.Ground);
        }

        public void Map()
        {
            foreach(Cell c in _gameObject.Map.Cells)
            {
                Square(c.Position, c.Color, c.Size);
            }
        }

        public void Player()
        {
            Square(_gameObject.Player.Position, _gameObject.Player.Color, _gameObject.Player.Size);
        }

        public void AllRays()
        {
            foreach(Ray r in _gameObject.Rays)
            {
                Ray(r);
            }
        }

        public void Ray(Ray ray)
        {
            Line(ray.StartPoint, ray.HitPoint, ray.Color);
        }

        public void AllRayLine(bool fisheye, uint windowHeigth, bool wallColorNear)
        {
            int xPos = 0;
            foreach(Ray r in _gameObject.Rays)
            {
                RayLine(r, xPos, fisheye, windowHeigth, wallColorNear);
                xPos++;
            }
        }

        public void RayLine(Ray ray, int xPos, bool fisheye, uint windowHeight, bool wallColorNear)
        {
            double distance;
            double lineLength;
            Vector2f startPos;
            Vector2f endPos;
            Color WallColor;

            //Obtient la longueur de la ligne à afficher selon si on souhaite l'effet fisheye
            if (fisheye)
            {
                distance = ray.Length;
            }
            else
            {
                float angle = (float)_gameLogic.ToRadians((double)Math.Abs(_gameObject.Player.ViewAngle - ray.Angle));
                float baseDistance = (float)ray.Length;
                distance = baseDistance * Math.Cos(angle);
            }
            lineLength = (windowHeight / distance) * _gameObject.Map.Cells[0, 0].Size;
            startPos = new Vector2f(xPos, (float)(windowHeight / 2 - lineLength / 2));
            endPos = new Vector2f(xPos, startPos.Y + (float)lineLength);

            //Change la couleur du mur selon son orientation
            if (wallColorNear)
            {
                if (distance < 255)
                {
                    WallColor = new Color((byte)(255 - distance), 0, 0);
                }
                else
                {
                    WallColor = new Color(0, 0, 0);
                }
            }
            else
            {
                if (ray.Horizontal)
                {
                    WallColor = new Color(255, 0, 0);
                }
                else
                {
                    WallColor = new Color(100, 0, 0);
                }
            }

            //Dessine le mur si le rayon le touche
            if (ray.HasRayHit)
            {
                Line(startPos,endPos, WallColor);
            }
        }
    }
}
