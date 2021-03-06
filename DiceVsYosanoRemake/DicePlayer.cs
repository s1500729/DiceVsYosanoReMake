﻿using Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace DiceVsYosanoRemake
{
    public class DicePlayer : CharacterBase<Rectangle>
    {
        private List<Texture> diceList;
        private Texture damagedDice;

        private Texture statusTexture;

        private int maxHp { get; } = 100;

        public List<Bullet> Bullets { get; private set; } = new List<Bullet>();

        public Color Color { get; }

        public int ShotSize { get; private set; } = 7;

        public void CollectItem()
        {
            ShotSize++;
        }

        public Vector2D MoveAmount(Direction dir)
        {
            return dir.ToVector() * Speed;
        }

        public void Move(Direction dir)
        {
            Area.TopLeft += MoveAmount(dir);

            // 弾も自機の移動を受けるようにする
            foreach(var bullet in Bullets)
            {
                bullet.Area.TopLeft += dir.ToVector() * Speed;
            }

            if(statusTexture != damagedDice)
            {
                statusTexture = diceList[(int)dir + 1];
            }
        }

        public void Hit(int damage)
        {
            Hp -= damage;

            if (Hp < 0) Hp = 0;

            statusTexture = damagedDice;
        }

        public override void Draw()
        {
            // プレイヤー自身の描画
            statusTexture.Scaled(Area.Size.w, BasedOn.Width);

            statusTexture.Draw(Area.TopLeft);

            statusTexture = diceList[0];

            // HPバーの描画
            (int w, int h) barSize = ((int)(Area.Size.w * ((double)Hp / maxHp)), Area.Size.h / 6);
            new Rectangle(Area.TopLeft - (0, Area.Size.w / 2), barSize).Draw(HpColor());
        }

        public Color HpColor()
        {
            if(Hp > maxHp / 2)
            {
                return Palette.Blue;
            }

            if(Hp > maxHp / 4)
            {
                return Palette.Yellow;
            }

            return Palette.Red;
        }

        public void Shot()
        {
            if(Bullets.Count() != 0)
            {
                return;
            }

            // 四方向に弾を生成する
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                var bulletArea = new Rectangle(Area.Center, (ShotSize, ShotSize), Location.Center);
                Bullets.Add(new Bullet(bulletArea, dir, Color));
            }
        }

        
        public DicePlayer(Rectangle area, IEnumerable<Texture> diceList, Color color)
        {
            this.diceList = diceList.ToList();
            this.damagedDice = diceList.Last();

            this.diceList.Remove(damagedDice);

            Area = area;

            // 状態を初期化
            statusTexture = this.diceList[0];

            Hp = maxHp;

            Color = color;
        }
    }
}
