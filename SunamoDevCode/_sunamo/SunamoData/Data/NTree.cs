namespace SunamoDevCode._sunamo.SunamoData.Data;

/// <summary>
/// Generic N-ary tree data structure
/// Another big popular tree implementation is on https://www.codeproject.com/Articles/12592/Generic-Tree-T-in-C
/// </summary>
/// <typeparam name="T">Type of data stored in tree nodes</typeparam>
internal class NTree<T>
{
    /// <summary>
    /// Data stored in this node
    /// </summary>
    internal T data;

    /// <summary>
    /// Child nodes of this node
    /// </summary>
    internal LinkedList<NTree<T>> children;

    /// <summary>
    /// Initializes a new instance of NTree with specified data
    /// </summary>
    /// <param name="data">Data to store in the node</param>
    internal NTree(T data)
    {
        this.data = data;
        children = new LinkedList<NTree<T>>();
    }

    /// <summary>
    /// Traverses the tree in depth-first order and applies visitor action to each node
    /// </summary>
    /// <param name="node">Starting node for traversal</param>
    /// <param name="visitor">Action to apply to each node's data</param>
    internal void Traverse(NTree<T> node, Action<T> visitor)
    {
        visitor(node.data);
        foreach (NTree<T> childNode in node.children)
            Traverse(childNode, visitor);
    }
}