using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Storage.Base;

public interface ILocalStorage
{
    public string RelativePath { get; init; }
}
