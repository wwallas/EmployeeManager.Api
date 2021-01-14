using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Api.Models
{
    public interface ICountryRepository
    {
        List<Country> SelectAll();
    }
}
