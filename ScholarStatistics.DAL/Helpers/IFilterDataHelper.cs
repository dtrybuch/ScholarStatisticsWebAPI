﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ScholarStatistics.DAL.Helpers
{
    public interface IFilterDataHelper
    {
        void SaveArxivData();
        void SavePublicationsFromScopus();
    }
}