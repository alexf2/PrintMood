using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace PrintMood.Resources
{
    public class Shared
    {
        readonly IStringLocalizerFactory _fac;
        public Shared(IStringLocalizerFactory fac)
        {
            _fac = fac;
        }

        public IStringLocalizer Localizer => _fac.Create(nameof(Shared), null);
    }
}
