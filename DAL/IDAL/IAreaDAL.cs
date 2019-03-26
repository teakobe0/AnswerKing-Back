using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IAreaDAL
    {
        List<Area> GetList();
        List<Area> GetList(string name );
        List<Area> GetCountryList();
    }
}
