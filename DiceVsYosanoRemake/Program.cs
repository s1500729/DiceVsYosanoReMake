﻿using DxLogic;
using DiceVsYosanoRemake.Scenes;

public class Program
{
    public void Run()
    {
        var manager = new SceneManager<MyData>(new Title());

        while (DxSystem.Update())
        {
            manager.UpdateAndDraw();
        }
    }
}