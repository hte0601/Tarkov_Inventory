using System;

public struct ItemCategoryData
{
    public ItemMainCategory1 Category1 { get; private set; }
    public Enum Category2 { get; private set; }

    public ItemCategoryData(ItemMainCategory1 category1)
    {
        Category1 = category1;
        Category2 = null;
    }

    public ItemCategoryData(ItemMainCategory1 category1, Enum category2)
    {
        Category1 = category1;
        Category2 = category2;
    }


    public readonly bool Matches(ItemMainCategory1 category1)
    {
        return Category1 == category1;
    }

    public readonly bool Matches<T2>(ItemMainCategory1 category1, T2 category2) where T2 : Enum
    {
        if (Category1 != category1)
        {
            return false;
        }
        else
        {
            return Category2 is T2 c2 && c2.Equals(category2);
        }
    }
}
