using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class NotifyPropertyChangedInvocatorAttribute : Attribute
    {
        public string ParameterName { get; private set; }

        public NotifyPropertyChangedInvocatorAttribute()
        { 

        }

        public NotifyPropertyChangedInvocatorAttribute([NotNull] string parameterName)
        {
            ParameterName = parameterName;
        }
    }
}
