// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
﻿namespace SunamoDevCode._public;

public class AsyncLoadingBaseDC<T, ProgressBar>
{
    public ProgressBar pb;
    public long processedCount = 0;
    public Action<T> statusAfterLoad;
}