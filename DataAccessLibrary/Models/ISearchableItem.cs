using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassFindrDataAccessLibrary.Models
{
    public interface ISearchableItem
    {
        public string? GetSearchableAspect();
    }
}
