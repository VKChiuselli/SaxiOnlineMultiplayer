using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateSystem : MonoBehaviour
{

    public int x;
    public int y;
    public int typeOfTile;
    //type of tile: 1 == empty tile, where deploy or move without enemy
    //type of tile: 2 == tile filled by at least one friendly card
    //type of tile: 3 == tile filled by at least one enemy card

    void Start()
    {
        SetName();
        SetTagDeploy();
    }

    private void SetTagDeploy()
    {
        switch (x)
        {
            case 1:
                {
                    transform.tag = "DeployTileLeft";
                }
                break;
            case 2:
                {
                    transform.tag = "DeployTileLeft";
                }
                break;
            case 9:
                {
                    transform.tag = "DeployTileRight";
                }
                break;
            case 10:
                {
                    transform.tag = "DeployTileRight";
                }
                break;
        }
    }

    private void SetName()
    {
        string xName = "";
        string yName = "z";

        switch (transform.localPosition.x)
        {
            case 3990f:
                {
                    xName = "1";
                    x = 1;
                }
                break;
            case 4940f:
                {
                    xName = "2";
                    x = 2;
                }
                break;
            case 5890f:
                {
                    xName = "3";
                    x = 3;
                }
                break;
            case 6840f:
                {
                    xName = "4";
                    x = 4;
                }
                break;
            case 7790f:
                {
                    xName = "5";
                    x = 5;
                }
                break;
            case 8740f:
                {
                    xName = "6";
                    x = 6;

                }
                break;
            case 9690:
                {
                    xName = "7";
                    x = 7;

                }
                break;
            case 10640f:
                {
                    xName = "8";
                    x = 8;
                }
                break;
            case 11590f:
                {
                    xName = "9";
                    x = 9;
                }
                break;
            case 12540f:
                {
                    xName = "10";
                    x = 10;
                }
                break;


        }
        switch (transform.localPosition.y)
        {
            case 7060f:
                {
                    yName = "1";
                    y = 1;
                }
                break;
            case 6050f:
                {
                    yName = "2";
                    y = 2;
                }
                break;
            case 5040f:
                {
                    yName = "3";
                    y = 3;
                }
                break;
            case 4030f:
                {
                    yName = "4";
                    y = 4;
                }
                break;
            case 3020f:
                {
                    yName = "5";
                    y = 5;
                }
                break;
            case 2010f:
                {
                    yName = "6";
                    y = 6;
                }
                break;

        }

        string finalString = "(" + xName + "," + yName + ")";
        this.name = finalString;
    }

}
