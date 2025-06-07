using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegratedGameplaySystem
{
    public interface IPoolable
    {
        void Disable();
        void Enable();
        void Flush();
    }
}
