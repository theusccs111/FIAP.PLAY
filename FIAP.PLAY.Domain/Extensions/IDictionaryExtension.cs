using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIAP.PLAY.Domain.Extensions
{
    public static class IDictionaryExtension
    {
        public static string[] ConvertDictionaryToArray(this IDictionary<string, string[]> keyValuePairs)
        {
            var listOfList = keyValuePairs.Values.Select(v => v);
            List<string> ret = new List<string>();
            foreach (var array in listOfList)
            {
                foreach (var item in array)
                {
                    ret.Add(item);
                }
            }

            return ret.ToArray();
        }
    }
}
