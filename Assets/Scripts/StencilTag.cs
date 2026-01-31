using System;


[Flags]
public enum StencilTag
{
    Plant = 1,
    Animal = 1 << 1,
    Predator = 1 << 2,
    Prey = 1 << 3,
    Scenery = 1 << 4,
    Tree = 1 << 5,
    Flower = 1 << 6,
}
