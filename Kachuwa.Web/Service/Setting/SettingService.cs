﻿using System;
using System.Collections.Generic;
using System.Text;
using Kachuwa.Data;
using Kachuwa.Web.Model;

namespace Kachuwa.Web.Service
{
    public class SettingService: ISettingService
    {
        public CrudService<Setting> CrudService { get; set; }=new CrudService<Setting>();
    }
}
