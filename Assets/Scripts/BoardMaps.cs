using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardMaps : MonoBehaviour
{
    private string[] maps =
    {
            "WWW\n" +
            "WDW\n" +
            "WGW\n" +
            "WGW\n" +
            "WGW\n" +
            "W0W\n" +
            "WWW",

            "EEEEEEE\n" +
            "EDWGGGE\n" +
            "EGWGWGE\n" +
            "EGWGWGE\n" +
            "EGWGWGE\n" +
            "EGGGW0E\n" +
            "EEEEEEE",

            "EEEEEEE\n" +
            "EGGDGGE\n" +
            "EGGGGGE\n" +
            "EGGGGGE\n" +
            "EGG6GGE\n" +
            "EGGGGGE\n" +
            "EEEEEEE",

            "EEEEEEEE\n" +
            "EGGGGGGE\n" +
            "EGDGGDGE\n" +
            "EGGGGGGE\n" +
            "EGGGWGWE\n" +
            "EGWGGGGE\n" +
            "EGGGGGGE\n" +
            "EG0GG0GE\n" +
            "EGGGGGGE\n" +
            "EEEEEEEE",
    };

    public string[] GetMaps ()
    {
        return maps;
    }
}
