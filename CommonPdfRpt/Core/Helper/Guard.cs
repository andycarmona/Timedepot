using System;
using System.IO;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// Guard class
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Does the specified directory exist?
        /// </summary>
        /// <param name="fileName">file path</param>
        public static void CheckDirectoryExists(this string fileName)
        {
            var dir = Path.GetDirectoryName(fileName);
            if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir))
                throw new DirectoryNotFoundException(string.Format("{0} directory does not exist.", dir));
        }

        /// <summary>
        /// A safe ToString method.
        /// </summary>        
        /// <returns></returns>
        public static string ToSafeString(this object data, string nullValue = "")
        {
            return data == null ? nullValue : data.ToString().Trim();
        }

        /// <summary>
        /// A safe RunDirection value
        /// </summary>
        /// <param name="runDirection">Possible run direction values, left-to-right or right-to-left</param>
        /// <returns></returns>
        public static PdfRunDirection GetSafeRunDirection(this PdfRunDirection? runDirection)
        {
            return runDirection != null ? runDirection.Value : PdfRunDirection.None;
        }

        /// <summary>
        /// This method allows you to overcome the limitations of the using block
        /// (see http://msdn.microsoft.com/en-us/library/aa355056.aspx for more information) by being
        /// able to catch exceptions in the block and exceptions from the dispose method and if
        /// both occur throw an <see cref="AggregateException"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Note that the semantic of this block is slightly different to a normal using block, since
        /// keywords like return will not "return" the current method; rather they will return the
        /// action delegate you pass to the method.
        /// </para>
        /// <example>
        /// Here is an example of how you could use this:
        /// <code>
        /// new MyWcfClient().SafeUsingBlock(myWcfClient =>
        /// {
        ///	    myWcfClient.MyServiceMethod();
        /// });
        /// </code>
        /// </example>
        /// </remarks>
        /// <typeparam name="TDisposable">The <see cref="IDisposable"/> type</typeparam>
        /// <param name="disposable">The disposable object</param>
        /// <param name="action">A function that will run as "using block"</param>
        public static void SafeUsingBlock<TDisposable>(this TDisposable disposable, Action<TDisposable> action)
            where TDisposable : IDisposable
        {
            disposable.SafeUsingBlock(action, d => d);
        }


        /// <summary>
        /// Internal implementation of SafeUsingBlock that enables custom unwrapping of the object
        /// to pass to the safe using block action.
        /// </summary>
        /// <typeparam name="TDisposable">The disposable type</typeparam>
        /// <typeparam name="T">The type passed to the action</typeparam>
        /// <param name="disposable">The disposable object</param>
        /// <param name="action">The type passed to the action</param>
        /// <param name="unwrapper">
        /// The unwrapper function that takes the disposable and returns the object to pass to the action
        /// </param>
        internal static void SafeUsingBlock<TDisposable, T>(this TDisposable disposable, Action<T> action, Func<TDisposable, T> unwrapper)
            where TDisposable : IDisposable
        {
            try
            {
                action(unwrapper(disposable));
            }
            catch (Exception actionException)
            {
                try
                {
                    disposable.Dispose();
                }
                catch (Exception disposeException)
                {
                    throw new AggregateException(actionException, disposeException);
                }

                throw;
            }

            disposable.Dispose();
        }
    }
}
