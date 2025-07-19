namespace SunamoDevCode._sunamo;

internal class SF
{
    internal static List<string> RemoveComments(List<string> tf)
    {
        //CA.RemoveStringsEmpty2(tf);
        tf = tf.Where(d => !string.IsNullOrWhiteSpace(d)).ToList();
        // Nevím vůbec co toto má znamenat ael nedává mi to smysl
        // Příště dopsat komentář pokud budu odkomentovávat
        //if (tf.Count > 0)
        //{
        //    if (tf[0].StartsWith("#"))
        //    {
        //        return tf[0];
        //    }
        //}
        //CA.RemoveStartingWith("#", tf);
        tf = tf.Where(d => !d.StartsWith("#")).ToList();
        return tf;
    }
}