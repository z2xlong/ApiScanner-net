using System.Collections.Generic;

namespace ApiScanner.Core
{
    public interface ICompatible<T>
    {
        /// <summary>
        /// Check the Api whether is compatible with older version.
        /// </summary>
        /// <param name="old">older version syntax</param>
        /// <param name="incompatibility"></param>
        /// <returns>
        ///     -1: Incompatible
        ///      0: no change
        ///      1: Compatible change
        /// </returns>
        Compatibility IsCompatible(T old);
    }
}
