namespace SunamoDevCode._sunamo.SunamoData.Data;

/// <summary>
/// Another big popular tree is on https://www.codeproject.com/Articles/12592/Generic-Tree-T-in-C
/// </summary>
/// <typeparam name="T"></typeparam>
internal class NTree<T>
{
    internal T data;
    internal LinkedList<NTree<T>> children;
    internal NTree(T data)
    {
        this.data = data;
        children = new LinkedList<NTree<T>>();
    }
    internal void Traverse(NTree<T> node, Action<T> visitor)
    {
        visitor(node.data);
        foreach (NTree<T> kid in node.children)
            Traverse(kid, visitor);
    }
}