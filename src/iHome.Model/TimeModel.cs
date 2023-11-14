﻿namespace iHome.Model;

public record struct TimeModel(int Hour, int Minute, bool wasValid)
{
    public override readonly string ToString() =>
        $"{Hour}:{Minute}";
}
