namespace LogisticsTrack.Service
{
    public class ColumnMappingService
    {
        /// <summary>
        /// custom columns mappings (if DTO has another property name than there is in DB) - (DTOClassName.PropertyName, EntityClassName.PropertyName)
        /// namespaces not supported, only 'ClassName.Propertyname' combinations
        /// </summary>
        public static Dictionary<string, string> ColumnMappings { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// adds or overrides mapping - (DTOClassName.PropertyName, EntityClassName.PropertyName)
        /// namespaces not supported, only 'ClassName.Propertyname' combinations
        /// </summary>
        /// <param name="from">from class.column name ('lower layer class')</param>
        /// <param name="to">to class.column name ('upper layer class')</param>
        public static void SetColumnMapping(string from, string to)
        {
            if (from.Count(c => c == '.') != 1 || to.Count(c => c == '.') != 1)
            {
                throw new ArgumentException("values not valid");
            }

            if (ColumnMappings.ContainsKey(to) == false)
            {
                ColumnMappings.Add(to, from);
            }
            else
            {
                ColumnMappings[to] = from;
            }
        }

        public static void RemoveColumnMapping(string key)
        {
            ColumnMappings.Remove(key);
        }

        /// <summary>
        /// returns name of column taht is custom mapped to another column name in another class - e.g. EntityPropertyName
        /// </summary>
        /// <param name="outputFullName">full name of TO mapped column - e.g. 'DTOClassFoo.PropertyName'</param>
        /// <param name="inputClassName">name of FROM 'input' class - e.g. 'SomeEntityClassName'</param>
        /// <returns></returns>
        public static string MapColumn(string inputClassName, string outputFullName)
        {
            string inputFullName = null;
            var splitted = outputFullName.Split('.');

            if (ColumnMappings == null
                || ColumnMappings.TryGetValue(outputFullName, out inputFullName) == false)
            {
                // no mappings at all or no custom one
                var res = splitted.Last();
                return string.IsNullOrEmpty(res) ? null : res;
            }

            splitted = inputFullName.Split('.');
            if (string.Compare(inputClassName, splitted.First(), true) != 0)
            {
                // not mapping for that class so take output one again
                splitted = outputFullName.Split('.');
            }

            // return last 'splitted name' (column name)
            var result = splitted.Last();
            return string.IsNullOrEmpty(result) ? null : result;
        }
    }
}
