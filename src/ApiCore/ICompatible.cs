using System.Collections.Generic;

namespace ApiCore
{
    public interface ICompatible<T>
    {
        bool IsCompatible(T old, IList<string> incompatibility);
    }
}
