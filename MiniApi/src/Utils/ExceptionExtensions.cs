using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    public static class ExceptionExtensions
    {
        public static T AddData<T>(this T ex, object key, object value) where T : Exception
        {
            ex.Data.Add(key, value);
            return ex;
        }
    }
}