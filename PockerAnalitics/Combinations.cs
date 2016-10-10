using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PockerAnalitics
{
    //перечисление возможных комбинаций
    public enum Combinations
    {
        None,
        OverOne,
        Pair,
        TwinPair,
        Set,
        Straight,
        Flush,
        Fullhouse,
        Care,
        StraightFlush,
        RoyalFlush
    }
}
