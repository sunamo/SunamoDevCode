namespace SunamoDevCode._sunamo.SunamoDelegates;

/// <summary>
/// Delegate for void methods that take a List parameter
/// </summary>
/// <typeparam name="T">Type of list elements</typeparam>
/// <param name="list">List parameter</param>
internal delegate void VoidListT<T>(List<T> list);
