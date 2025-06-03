using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    public interface IBindingsSource
    {
        List<Binding> GetBindings();
    }
}