namespace SunamoDevCode._sunamo.SunamoExceptions;


internal class MayExcHelper
{
    /// <summary>
    /// True when is there error
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    internal static bool MayExc<T>(ResultWithExceptionDC<T> result)
    {
        if (result.exc != null)
        {
            Console.WriteLine(result.exc);
            //ThisApp.Error( result.exc);
            return true;
        }
        return false;
    }
    // /// <summary>
    // /// True when is there error
    // /// </summary>
    // /// <param name="r"></param>
    // /// <returns></returns>
    // internal static bool XmlDocument(ResultWithException<XmlDocument> r)
    // {
    //     return MayExc<XmlDocument>(r);
    // }
}