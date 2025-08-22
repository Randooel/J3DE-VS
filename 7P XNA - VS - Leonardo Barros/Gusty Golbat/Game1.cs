using Gusty_Golbat.Entidades;
using Gusty_Golbat.Geometria;
using Gusty_Golbat.Setup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Gusty_Golbat
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // INTERFACE
        private SpriteFont _font;

        // SETUP
        private Camera _camera;
        public BasicEffect effect;

        // PERSONAGENS
        private Collider[] _collider;
        private Golbat[] _golbats;
        private Magnemite[] _magnemites;

        // CENÁRIO
        private PlaneDrawer _plane;
        private Texture2D _backgroundTexture, _golbatTexture, _magnemiteTexture;

        //ITENS
        private List<Heart> _hearts;
        private int _heartCount = 0;
        private Texture2D _heartTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Screen.GetInstance().SetWidth(_graphics.PreferredBackBufferWidth = 1280);
            Screen.GetInstance().SetHeight(_graphics.PreferredBackBufferHeight = 720);
        }

        protected override void Initialize()
        {
            // SETUP
            _camera = new Camera();
            _camera.SetupView(new Vector3(-5.5f, 0f, 10f), Vector3.Zero, Vector3.Up);

            // PERSONAGENS
            _golbats = new Golbat[]
            {
                new Golbat(this, new Vector3(-15f,0f,-8f), Vector3.Zero, new Vector3(1f,1.5f,0.5f), 10, _golbatTexture, Vector3.One, Color.Green),
                //new Golbat(this, new Vector3(8f,0f,-8f), Vector3.Zero, new Vector3(1f,1.5f,0.5f), 0, _golbatTexture, Vector3.One, Color.Green)
            };

            _magnemites = new Magnemite[]
            {
                new Magnemite(this, new Vector3(30f,0f,-8f), Vector3.One, Color.Red),
                new Magnemite(this, new Vector3(30f,2f,-8f), Vector3.One, Color.Red),
                new Magnemite(this, new Vector3(30f,3f,-8f), Vector3.One, Color.Red),
                new Magnemite(this, new Vector3(30f,-3f,-8f), Vector3.One, Color.Red),
                new Magnemite(this, new Vector3(30f,-2f,-8f), Vector3.One, Color.Red),
                new Magnemite(this, new Vector3(30f,-1f,-8f), Vector3.One, Color.Red),
            };

            _collider = new Collider[]
            {
                new Collider(this, new Vector3(0,2,-6), new Vector3(6,4,0.5f), Color.Green),
                new Collider(this, new Vector3(0,2,6), new Vector3(6,4,0.5f), Color.Green)
            };

            // ITENS
            _hearts = new List<Heart>
            {
                new Heart(this, new Vector3(10f,0f,-8f), Vector3.One, Color.Red),
                new Heart(this, new Vector3(30f,2f,-8f), Vector3.One, Color.Red),
                new Heart(this, new Vector3(30f,3f,-8f), Vector3.One, Color.Red),
                new Heart(this, new Vector3(30f,-3f,-8f), Vector3.One, Color.Red),
                new Heart(this, new Vector3(30f,-2f,-8f), Vector3.One, Color.Red),
                new Heart(this, new Vector3(30f,-1f,-8f), Vector3.One, Color.Red),
                new Heart(this, new Vector3(30f,1f,-8f), Vector3.One, Color.Red),
                new Heart(this, new Vector3(30f,0f,-8f), Vector3.One, Color.Red),
                new Heart(this, new Vector3(30f,2f,-8f), Vector3.One, Color.Red),
                new Heart(this, new Vector3(30f,3f,-8f), Vector3.One, Color.Red),
                new Heart(this, new Vector3(30f,-3f,-8f), Vector3.One, Color.Red),
                new Heart(this, new Vector3(30f,-2f,-8f), Vector3.One, Color.Red),
                new Heart(this, new Vector3(30f,-1f,-8f), Vector3.One, Color.Red),
                new Heart(this, new Vector3(30f,1f,-8f), Vector3.One, Color.Red),
            };

            // CENÁRIO
            _plane = new PlaneDrawer(GraphicsDevice);
            _plane.SetPlaneInitialTransform(new Vector3(0f, 0f, -10f), new Vector3(90f, 0f, 0f), new Vector3(2f, 0f, 0.7f));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // SETUP
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            effect = new BasicEffect(GraphicsDevice) { TextureEnabled = true };

            // INTERFACE
            _font = Content.Load<SpriteFont>("galleryFont");

            // TEXTURES
            _backgroundTexture = Content.Load<Texture2D>("Background");
            _golbatTexture = Content.Load<Texture2D>("Golbat");
            _heartTexture = Content.Load<Texture2D>("Heart");
            _magnemiteTexture = Content.Load<Texture2D>("Magnemite");

            foreach (var golbat in _golbats)
            {
                golbat.texture = _golbatTexture;
            }

            foreach (var heart in _hearts)
            {
                heart.texture = _heartTexture;
            }

            foreach (var magnemite in _magnemites)
            {
                magnemite.texture = _magnemiteTexture;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _camera.Update(gameTime);

            foreach (var golbat in _golbats)
            {
                golbat.Update(gameTime);
            }

            for (int i = _magnemites.Length - 1; i >= 0; i--)
            {
                var magnemite = _magnemites[i];
                magnemite.Update(gameTime);

                if (_golbats[0].currentState != Golbat.State.Damaged)
                {
                    if (magnemite.IsColliding(_golbats[0].GetBoundingBox()))
                    {
                        if(_heartCount > 0)
                        {
                            _heartCount--;
                        }

                        _golbats[0].SwitchState(Golbat.State.Damaged);
                        _golbats[0].RestorePosition();

                        // Debug
                        Window.Title = _golbats[0].currentState.ToString();
                    }
                }
            }


            for (int i = _hearts.Count - 1; i >= 0; i--)
            {
                var heart = _hearts[i];
                heart.Update(gameTime);

                if (heart.IsColliding(_golbats[0].GetBoundingBox()))
                {
                    _heartCount++;
                    //Window.Title = _heartCount.ToString();
                    _hearts.RemoveAt(i);
                }
            }

            foreach (var c in _collider)
            {
                if (c.IsColliding(_golbats[0].GetBoundingBox()))
                {
                    Window.Title = "Colidiu";
                    c.GetLineBox().SetColor(Color.Red);
                    _golbats[0].RestorePosition();
                }
                else
                {
                    c.GetLineBox().SetColor(Color.Green);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // SETUP
            GraphicsDevice.Clear(Color.Black);

            effect.View = _camera.GetView();
            effect.Projection = _camera.GetProjection();

            // ENTIDADES
            foreach (var golbat in _golbats)
            {
                golbat.Draw(_camera);
            }

            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            foreach (var magnemite in _magnemites)
            {
                magnemite.Draw(_camera, effect);
            }

            foreach (var heart in _hearts)
            {
                heart.Draw(_camera, effect);
            }

            // CENÁRIO
            _plane.Draw(effect, _backgroundTexture);

            // INTERFACE
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, "1P: " + _heartCount.ToString(), new Vector2(100, 10), Color.Blue);
            _spriteBatch.End();


            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            base.Draw(gameTime);
        }
    }
}