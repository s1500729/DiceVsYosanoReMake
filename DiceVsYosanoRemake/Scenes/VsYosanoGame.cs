﻿using DxLogic;
using Graphics;
using Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceVsYosanoRemake.Scenes
{
    class VsYosanoGame : Game
    {
        private YosanoEnemy yosano;
        private Texture yosanoImage = new Texture("Resource/Yosano.jpg");
        private Texture damageImage = new Texture("Resource/DamageYosano.png");

        public VsYosanoGame(int yosanoHp)
            : base(new Vector2D(600, 140), new Vector2D(100, 100))
        {
            yosano = new YosanoEnemy(yosanoImage, damageImage, yosanoHp, gameField);
        }

        public override void Draw()
        {
            base.Draw();

            yosano.Draw();

            foreach(var ball in yosano.Balls)
            {
                ball.Draw();
            }

            gameText.Draw($"HP: {yosano.Hp}", new Vector2D(280, 30), Palette.Darkgray);
        }

        public override SceneBase<MyData> Update()
        {
            var next = base.Update();

            if(next != this)
            {
                return next;
            }
            
            yosano.Update();

            for (int i = yosano.Balls.Count - 1; i >= 0; i--)
            {
                var ball = yosano.Balls[i];

                ball.Move();

                if (!gameField.Intersects(ball.Area))
                {
                    yosano.Balls.Remove(ball);
                }
            }

            return this;
        }

        protected override void HitDecision()
        {
            foreach (var player in players) 
            {
                foreach (var ball in yosano.Balls)
                {
                    if(player.Area.Intersects(ball.Area))
                    {
                        player.Hit(damage: 1);
                    }
                }

                if(player.Area.Intersects(yosano.Area))
                {
                    player.Hit(1);
                    yosano.Hit(1);
                }

                foreach(var bullet in player.Bullets)
                {
                    if(yosano.Area.Intersects(bullet.Area))
                    {
                        yosano.Hit(1);
                    }
                }
            }
        }

        protected override bool IsGameOver()
        {
            for (int i = 0; i < 2; i++)
            {
                if (players[i].Hp <= 0)
                {
                    Data.Winner = 1;

                    return true;
                }

            }

            if(yosano.Hp <= 0)
            {
                Data.Winner = 0;

                return true;
            }


            return false;
        }
    }
}
