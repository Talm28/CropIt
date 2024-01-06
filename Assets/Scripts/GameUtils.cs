using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static int[] storePrices = {0,10,30,50,70,100,150,200};

    public static Dictionary<string,int> cropValues = new Dictionary<string, int>{
        {"Cabbage", 1},
        {"Carrot", 2},
        {"Corn", 4},
        {"Cucumber", 8},
        {"Onion", 16},
        {"Potato", 32},
        {"Radish", 64},
        {"Tomato", 128}
    };

    public static Dictionary<string,int> cropGrowTime = new Dictionary<string, int>{
        {"Cabbage", 5},
        {"Carrot", 10},
        {"Corn", 15},
        {"Cucumber", 20},
        {"Onion", 25},
        {"Potato", 30},
        {"Radish", 30},
        {"Tomato", 30}
    };

    public static List<Dictionary<string,int>>[] orderList = 
    {
        new List<Dictionary<string, int>>() // Level 1
        {
            new Dictionary<string, int>{{"Cabbage", 1}},
            new Dictionary<string, int>{{"Cabbage", 2}},
            new Dictionary<string, int>{{"Cabbage", 3}}
        },
        new List<Dictionary<string, int>>() // Level 2
        {
            new Dictionary<string, int>{{"Cabbage", 1},{"Carrot", 1}},
            new Dictionary<string, int>{{"Cabbage", 1},{"Carrot", 2}},
            new Dictionary<string, int>{{"Cabbage", 2},{"Carrot", 2}},
            new Dictionary<string, int>{{"Carrot", 3}}
        },
        new List<Dictionary<string, int>>() // Level 3
        {
            new Dictionary<string, int>{{"Cabbage", 1},{"Carrot", 1},{"Corn", 1}},
            new Dictionary<string, int>{{"Carrot", 1},{"Corn", 1}},
            new Dictionary<string, int>{{"Corn", 2}},
        },
        new List<Dictionary<string, int>>() // Level 4
        {
            new Dictionary<string, int>{{"Carrot", 1},{"Corn", 1},{"Cucumber", 1}},
            new Dictionary<string, int>{{"Cucumber", 1}},
            new Dictionary<string, int>{{"Cucumber", 2}},
            new Dictionary<string, int>{{"Corn", 1}, {"Cucumber", 1}},
            new Dictionary<string, int>{{"Cabbage", 2}, {"Cucumber", 2}},
        },
        new List<Dictionary<string, int>>() // Level 5
        {
            new Dictionary<string, int>{{"Onion", 1}},
            new Dictionary<string, int>{{"Onion", 2}},
            new Dictionary<string, int>{{"Cucumber", 1}, {"Onion", 1}},
            new Dictionary<string, int>{{"Carrot", 1}, {"Onion", 2}},
        },
        new List<Dictionary<string, int>>() // Level 6
        {
            new Dictionary<string, int>{{"Onion", 1},{"Potato", 1}},
            new Dictionary<string, int>{{"Potato", 1}},
            new Dictionary<string, int>{{"Corn", 1}, {"Onion", 2}},
        },
        new List<Dictionary<string, int>>() // Level 7
        {
            new Dictionary<string, int>{{"Cabbage", 1}, {"Cucumber", 1}, {"Radish", 1}},
            new Dictionary<string, int>{{"Radish", 1}},
            new Dictionary<string, int>{{"Onion", 1}, {"Radish", 1}},
            new Dictionary<string, int>{{"Radish", 2}},
        },
        new List<Dictionary<string, int>>() // Level 8
        {
            new Dictionary<string, int>{{"Tomato", 1}},
            new Dictionary<string, int>{{"Tomato", 2}},
            new Dictionary<string, int>{{"Corn", 1}, {"Tomato", 1}},
            new Dictionary<string, int>{{"Cabbage", 1},{"Carrot", 1}, {"Tomato", 1}},
        }
    };

    public static List<Dictionary<string,int>> specialOrderList = new List<Dictionary<string, int>>() {
        new Dictionary<string, int>{{"Cabbage", 3}},  // Level 1
        new Dictionary<string, int>{{"Carrot", 3},},  // Level 2
        new Dictionary<string, int>{{"Corn", 3}},     // Level 3
        new Dictionary<string, int>{{"Cucumber", 3}}, // Level 4
        new Dictionary<string, int>{{"Onion", 3}},    // Level 5
        new Dictionary<string, int>{{"Potato", 3}},   // Level 6
        new Dictionary<string, int>{{"Radish", 3}},   // Level 7
        new Dictionary<string, int>{{"Tomato", 3}}    // Level 8
    };

    public static List<Dictionary<string,int>> superSpecialOrderList = new List<Dictionary<string, int>>() {
        new Dictionary<string, int>{{"Cabbage", 4}},  // Level 1
        new Dictionary<string, int>{{"Carrot", 4},},  // Level 2
        new Dictionary<string, int>{{"Corn", 4}},     // Level 3
        new Dictionary<string, int>{{"Cucumber", 4}}, // Level 4
        new Dictionary<string, int>{{"Onion", 4}},    // Level 5
        new Dictionary<string, int>{{"Potato", 4}},   // Level 6
        new Dictionary<string, int>{{"Radish", 4}},   // Level 7
        new Dictionary<string, int>{{"Tomato", 4}}    // Level 8
    };

    public static Dictionary<string,int> GetOrder()
    {
        System.Random random = new System.Random();
        int gameStage = GameManager.instance.gameStage;
        int levelIndex = random.Next(Mathf.Max(0,gameStage-2), gameStage);
        int orderIndex = random.Next(0,GameUtils.orderList[levelIndex].Count);
        return GameUtils.orderList[levelIndex][orderIndex];
    }

    public static Dictionary<string,int> GetSpecialOrder()
    {
        System.Random random = new System.Random();
        int index = random.Next(0, GameManager.instance.gameStage);
        return specialOrderList[index];
    }

    public static Dictionary<string,int> GetSuperSpecialOrder()
    {
        System.Random random = new System.Random();
        int index = random.Next(0, GameManager.instance.gameStage);
        return superSpecialOrderList[index];
    }

    public static Vector3 GetyMousePos()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(screenPosition);
        return mousePosition;
    }
}
