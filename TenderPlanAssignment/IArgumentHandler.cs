using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderPlanAssignment
{
    public interface IArgumentHandler<T>
    {
        T formObjectFromArguments(string[] arguments);

        bool ensureArgumentsAreValid(string[] arguments);

    }
}
