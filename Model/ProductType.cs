using System;

namespace Model
{
    [Flags]
    public enum ProductType
    {
        Bugday = 1,
        Meyve = 2,
        Balik = 3,
        Et = 4,
        Yemek = Bugday | Meyve | Et | Balik,
        Odun = 8,
        Tas = 16,
        Alet = 32,
        Altin = 64
    }
}