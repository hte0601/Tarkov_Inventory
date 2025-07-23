using System;

public struct ItemCategoryData
{
    private readonly ItemMainCategory1 category1;
    private readonly Enum category2;

    public ItemCategoryData(ItemMainCategory1 category1)
    {
        this.category1 = category1;
        category2 = null;
    }

    public ItemCategoryData(ItemMainCategory1 category1, Enum category2)
    {
        this.category1 = category1;
        this.category2 = category2;
    }


    public readonly bool Matches(ItemMainCategory1 category1)
    {
        return this.category1 == category1;
    }

    public readonly bool Matches<T2>(ItemMainCategory1 category1, T2 category2) where T2 : Enum
    {
        if (this.category1 != category1)
        {
            return false;
        }
        else
        {
            return this.category2 is T2 c2 && c2.Equals(category2);
        }
    }
}
