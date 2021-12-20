using System;
using SFML.System;
using SFML.Graphics;
using SFML.Window;
using RayCasting3D.Engine.Object;

namespace RayCasting3D.Engine.Logic
{
    internal class GameLogic
    {
        private GameObject _gameObject;
        private bool _keyState = false;
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="gameObject"> Objet du jeu </param>
        public GameLogic(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public void PlayerMove(double totalElapsedSeconds)
        {
            //Obtient les angles de mouvement
            double forwardAngle = _gameObject.Player.ViewAngle;
            double backwardAngle = forwardAngle + 180;
            double leftAngle = forwardAngle + 90;
            double rightAngle = forwardAngle + 270;

            //Ajuste les angle pour ne pas dépasser 360 degrés
            if (backwardAngle > 360) { backwardAngle -= 360; }
            if (leftAngle > 360) { leftAngle -= 360; }
            if (rightAngle > 360) { rightAngle -= 360; }

            //Obtient les vecteurs de mouvements
            Vector2f[] moveVectors = new Vector2f[4];

            moveVectors[0] = CastRay(forwardAngle, Color.Black, 1).Vector * -1;
            moveVectors[1] = CastRay(leftAngle, Color.Black, 1).Vector * -1;
            moveVectors[2] = CastRay(backwardAngle, Color.Black, 1).Vector * -1;
            moveVectors[3] = CastRay(rightAngle, Color.Black, 1).Vector * -1;

            for (int i = 0; i < moveVectors.Length; i++)
            {
                float hypotenus = GetHypotenus(moveVectors[i].X, moveVectors[i].Y);
                moveVectors[i].Y = MoveSpeed(moveVectors[i].Y, (float)_gameObject.Player.Speed, hypotenus);
                moveVectors[i].X = MoveSpeed(moveVectors[i].X, (float)_gameObject.Player.Speed, hypotenus);
            }

            Console.WriteLine($"MoveVector Forward   X : {moveVectors[0].X:0.00} Y : {moveVectors[0].Y:0.00}                          ");
            Console.WriteLine($"MoveVector left      X : {moveVectors[1].X:0.00} Y : {moveVectors[1].Y:0.00}                          ");
            Console.WriteLine($"MoveVector backward  X : {moveVectors[2].X:0.00} Y : {moveVectors[2].Y:0.00}                          ");
            Console.WriteLine($"MoveVector right     X : {moveVectors[3].X:0.00} Y : {moveVectors[3].Y:0.00}                          ");

            for (int i = 0; i < moveVectors.Length; i++)
            {
                moveVectors[i].X *= (float)totalElapsedSeconds;
                moveVectors[i].Y *= (float)totalElapsedSeconds;
            }


            //Adapte les vecteures de mouvement aux temps écoulé
            Vector2f nextPos;
            Vector2f nextPos2;
            Vector2i nextCell;
            Vector2i nextCell2;
            float playerSpeed = (float)(_gameObject.Player.Speed * totalElapsedSeconds);
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                nextPos = _gameObject.Player.Position + moveVectors[0];
                nextPos2 = _gameObject.Player.Position + moveVectors[0] + new Vector2f(_gameObject.Player.Size, _gameObject.Player.Size);
                nextCell = new Vector2i((int)Math.Floor(nextPos.X/20), (int)Math.Floor(nextPos.Y/20));
                nextCell2 = new Vector2i((int)Math.Floor(nextPos2.X/20), (int)Math.Floor(nextPos2.Y/20));
                if (_gameObject.Map.Values[nextCell.Y,nextCell.X] == 0 && _gameObject.Map.Values[nextCell2.Y, nextCell2.X] == 0)
                {
                    _gameObject.Player.Position = nextPos;
                }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                nextPos = _gameObject.Player.Position + moveVectors[1];
                nextPos2 = _gameObject.Player.Position + moveVectors[1] + new Vector2f(_gameObject.Player.Size, _gameObject.Player.Size);
                nextCell = new Vector2i((int)Math.Floor(nextPos.X / 20), (int)Math.Floor(nextPos.Y / 20));
                nextCell2 = new Vector2i((int)Math.Floor(nextPos2.X / 20), (int)Math.Floor(nextPos2.Y / 20));
                if (_gameObject.Map.Values[nextCell.Y, nextCell.X] == 0 && _gameObject.Map.Values[nextCell2.Y, nextCell2.X] == 0)
                {
                    _gameObject.Player.Position = nextPos;
                }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                nextPos = _gameObject.Player.Position + moveVectors[2];
                nextPos2 = _gameObject.Player.Position + moveVectors[2] + new Vector2f(_gameObject.Player.Size, _gameObject.Player.Size);
                nextCell = new Vector2i((int)Math.Floor(nextPos.X / 20), (int)Math.Floor(nextPos.Y / 20));
                nextCell2 = new Vector2i((int)Math.Floor(nextPos2.X / 20), (int)Math.Floor(nextPos2.Y / 20));
                if (_gameObject.Map.Values[nextCell.Y, nextCell.X] == 0 && _gameObject.Map.Values[nextCell2.Y, nextCell2.X] == 0)
                {
                    _gameObject.Player.Position = nextPos;
                }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                nextPos = _gameObject.Player.Position + moveVectors[3];
                nextPos2 = _gameObject.Player.Position + moveVectors[3] + new Vector2f(_gameObject.Player.Size, _gameObject.Player.Size);
                nextCell = new Vector2i((int)Math.Floor(nextPos.X / 20), (int)Math.Floor(nextPos.Y / 20));
                nextCell2 = new Vector2i((int)Math.Floor(nextPos2.X / 20), (int)Math.Floor(nextPos2.Y / 20));
                if (_gameObject.Map.Values[nextCell.Y, nextCell.X] == 0 && _gameObject.Map.Values[nextCell2.Y, nextCell2.X] == 0)
                {
                    _gameObject.Player.Position = nextPos;
                }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.E) || Keyboard.IsKeyPressed(Keyboard.Key.Right))
            {
                _gameObject.Player.ViewAngle -= 90 * (float)totalElapsedSeconds;
                if (_gameObject.Player.ViewAngle <= 0) { _gameObject.Player.ViewAngle = 360; }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Q) || Keyboard.IsKeyPressed(Keyboard.Key.Left))
            {
                _gameObject.Player.ViewAngle += 90 * (float)totalElapsedSeconds;
                if (_gameObject.Player.ViewAngle >= 360) { _gameObject.Player.ViewAngle = 0; }
            }
            if (!_keyState && Keyboard.IsKeyPressed(Keyboard.Key.M))
            {
                _keyState = true;
                _gameObject.Map.Displayed = !_gameObject.Map.Displayed;
            }
            if (!Keyboard.IsKeyPressed(Keyboard.Key.M))
            {
                _keyState = false;
            }
        }

        public float GetHypotenus(float x, float y)
        {
            return (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }
        public float MoveSpeed(float actX, float speed, float acth)
        {
            return (float)(actX * Math.Sqrt(Math.Pow(speed, 2) / Math.Pow(acth, 2)));
        }

        public void Raycasting(double fov, uint windowWidth, Color color, int raySize)
        {
            double unit = fov / windowWidth;
            double toAdd = unit;
            double[] angle = new double[windowWidth];
            if (_gameObject.Player.ViewAngle - (fov / 2) < 0)
            {
                angle[0] = 360 + (_gameObject.Player.ViewAngle - (fov / 2));
            }
            else
            {
                angle[0] = _gameObject.Player.ViewAngle - (fov / 2);
            }
            for (int i = 1; i < windowWidth; i++)
            {
                if (angle[0] + toAdd > 360)
                {
                    angle[i] = angle[0] + toAdd - 360;
                }
                else
                {
                    angle[i] = angle[0] + toAdd;
                }
                toAdd += unit;
            }
            for (int i = 0; i < windowWidth; i++)
            {
                _gameObject.Rays[i] = CastRay(angle[(windowWidth - 1) - i], color, raySize);
            }
        }

        public Ray CastRay(double angle, Color color, int raySize)
        {
            Ray horizontal;
            Ray vertical;
            Vector2f delta = new Vector2f();
            Vector2f delta2 = new Vector2f();
            Vector2f steps = new Vector2f();
            Vector2f hitpoint = new Vector2f();
            Vector2i hittedCellPos = new Vector2i();
            bool hasRayhit = false;
            double calcAngle;
            int i = 0;

            //Obtient la position du joueur
            Vector2f PlayerPos = new Vector2f(_gameObject.Player.Position.X + _gameObject.Player.Size / 2, _gameObject.Player.Position.Y + _gameObject.Player.Size / 2);

            //Obtient la cellul actuel
            Cell actuCell = _gameObject.Map.Cells[(int)(Math.Floor(PlayerPos.Y / _gameObject.Map.Cells[0, 0].Size)), (int)(Math.Floor(PlayerPos.X / _gameObject.Map.Cells[0, 0].Size))];

            //Trouve la distance entre le joueur et les bords de la case actuel
            delta.X = PlayerPos.X - (actuCell.Position.X);
            delta2.X = actuCell.Position.X + _gameObject.Map.Cells[0, 0].Size - PlayerPos.X;
            delta.Y = PlayerPos.Y - (actuCell.Position.Y);
            delta2.Y = actuCell.Position.Y + _gameObject.Map.Cells[0, 0].Size - PlayerPos.Y;

            //Calcule du rayon horizontale
            if (angle < 180)
            {
                calcAngle = angle < 90 ? 90 - angle : angle - 90;
                steps.Y = delta.Y;
                steps.X = angle < 90 ? GetOpposed(steps.Y, calcAngle) : -GetOpposed(steps.Y, calcAngle);
                hitpoint = new Vector2f(PlayerPos.X + steps.X, PlayerPos.Y - steps.Y);
                steps.Y = _gameObject.Map.Cells[0, 0].Size;
                steps.X = angle < 90 ? GetOpposed(steps.Y, calcAngle) : -GetOpposed(steps.Y, calcAngle);
                steps.Y = -_gameObject.Map.Cells[0, 0].Size;
            }
            else
            {
                calcAngle = angle < 270 ? 270 - angle : angle - 270;
                steps.Y = delta2.Y;
                steps.X = angle < 270 ? -GetOpposed(steps.Y, calcAngle) : GetOpposed(steps.Y, calcAngle);
                hitpoint = new Vector2f(PlayerPos.X + steps.X, PlayerPos.Y + steps.Y);
                steps.Y = _gameObject.Map.Cells[0, 0].Size;
                steps.X = angle < 270 ? -GetOpposed(steps.Y, calcAngle) : GetOpposed(steps.Y, calcAngle);
            }

            hittedCellPos = FoundHittedSquare(hitpoint, angle, true);
            while (_gameObject.Map.Values[hittedCellPos.Y, hittedCellPos.X] != 1 && i < raySize)
            {
                hitpoint.X += steps.X;
                hitpoint.Y += steps.Y;
                hittedCellPos = FoundHittedSquare(hitpoint, angle, true);
                i++;
            }

            if (i >= raySize)
            {
                hasRayhit = false;
            }
            else
            {
                hasRayhit = true;
            }

            horizontal = new Ray(hitpoint, PlayerPos, (float)angle, color, true, hasRayhit);

            //Calcule du rayon verticale
            if (angle < 90 || angle > 270)
            {
                calcAngle = angle < 90 ? angle : 360 - angle;
                steps.X = delta2.X;
                steps.Y = angle < 90 ? -GetOpposed(steps.X, calcAngle) : GetOpposed(steps.X, calcAngle);
                hitpoint = new Vector2f(PlayerPos.X + steps.X, PlayerPos.Y + steps.Y);
                steps.X = _gameObject.Map.Cells[0,0].Size;
                steps.Y = angle < 90 ? -GetOpposed(steps.X, calcAngle) : GetOpposed(steps.X, calcAngle);
            }
            else
            {
                calcAngle = angle < 180 ? 180 - angle : angle - 180;
                steps.X = delta.X;
                steps.Y = angle < 180 ? -GetOpposed(steps.X, calcAngle) : GetOpposed(steps.X, calcAngle);
                hitpoint = new Vector2f(PlayerPos.X - steps.X, PlayerPos.Y + steps.Y);
                steps.X = _gameObject.Map.Cells[0, 0].Size;
                steps.Y = angle < 180 ? -GetOpposed(steps.X, calcAngle) : GetOpposed(steps.X, calcAngle);
                steps.X = -_gameObject.Map.Cells[0, 0].Size;
            }

            i = 0;
            hittedCellPos = FoundHittedSquare(hitpoint, angle, false);
            while (_gameObject.Map.Values[hittedCellPos.Y, hittedCellPos.X] != 1 && i < raySize)
            {
                hitpoint.X += steps.X;
                hitpoint.Y += steps.Y;
                hittedCellPos = FoundHittedSquare(hitpoint, angle, false);
                i++;
            }

            if (i >= raySize)
            {
                hasRayhit = false;
            }
            else
            {
                hasRayhit = true;
            }

            vertical = new Ray(hitpoint, PlayerPos, (float)angle, color, false, hasRayhit);

            if (vertical.Length < horizontal.Length)
            {
                return vertical;
            }
            else
            {
                return horizontal;
            }
        }

        public float GetOpposed(double adjacent, double angle)
        {
            return (float)(Math.Tan(ToRadians(angle)) * adjacent);
        }

        public double ToRadians(double angle)
        {
            return angle * Math.PI / 180;
        }

        private Vector2i FoundHittedSquare(Vector2f hitpoint, double angle, bool horizontal)
        {
            int CellPosX;
            int CellPosY;
            if (horizontal)
            {
                if (angle < 180)
                {
                    CellPosX = (int)(Math.Floor(hitpoint.X / _gameObject.Map.Cells[0,0].Size));
                    CellPosY = (int)(Math.Floor(hitpoint.Y / _gameObject.Map.Cells[0, 0].Size)) - 1;
                }
                else
                {
                    CellPosX = (int)(Math.Floor(hitpoint.X / _gameObject.Map.Cells[0, 0].Size));
                    CellPosY = (int)(Math.Floor(hitpoint.Y / _gameObject.Map.Cells[0, 0].Size));
                }
            }
            else
            {
                if (angle > 90 && angle < 270)
                {
                    CellPosX = (int)(Math.Floor(hitpoint.X / _gameObject.Map.Cells[0, 0].Size)) - 1;
                    CellPosY = (int)(Math.Floor(hitpoint.Y / _gameObject.Map.Cells[0, 0].Size));
                }
                else
                {
                    CellPosX = (int)(Math.Floor(hitpoint.X / _gameObject.Map.Cells[0, 0].Size));
                    CellPosY = (int)(Math.Floor(hitpoint.Y / _gameObject.Map.Cells[0, 0].Size));
                }
            }
            if (CellPosX < 0) { CellPosX = 0; }
            if (CellPosX >= _gameObject.Map.Width) { CellPosX = _gameObject.Map.Width - 1; }
            if (CellPosY < 0) { CellPosY = 0; }
            if (CellPosY >= _gameObject.Map.Height) { CellPosY = _gameObject.Map.Height - 1; }

            return new Vector2i(CellPosX, CellPosY);
        }

    }
}
