﻿using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace WebApplication1.Repsitory
{
    public interface IOwnerRepository
    {
        public  Task<bool> customerRequestProcess(int rentId,String requestStatus);



    }
}
