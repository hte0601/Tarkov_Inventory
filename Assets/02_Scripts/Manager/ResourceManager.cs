using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceManager
{
    private static readonly Dictionary<string, Sprite> itemIconDict;

    static ResourceManager()
    {
        itemIconDict = new Dictionary<string, Sprite>();
    }

    public static bool TryGetItemIcon(string iconPath, out Sprite iconSprite)
    {
        if (itemIconDict.TryGetValue(iconPath, out iconSprite))
        {
            return true;
        }
        else
        {
            iconSprite = Resources.Load<Sprite>(iconPath);

            if (iconSprite != null)
            {
                itemIconDict.Add(iconPath, iconSprite);

                return true;
            }
            else
            {
                Debug.LogError("아이템 아이콘 리소스 로드 실패");

                return false;
            }
        }
    }
}
