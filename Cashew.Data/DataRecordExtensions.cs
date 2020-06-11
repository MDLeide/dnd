using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashew.Data
{
    public static class DataRecordExtensions
    {
        public static double ToDouble(this IDataRecord record, string columnName)
        {
            return record.GetDouble(record.GetOrdinal(columnName));
        }

        public static float ToFloat(this IDataRecord record, string columnName)
        {
            return record.GetFloat(record.GetOrdinal(columnName));
        }

        public static float ToSingle(this IDataRecord record, string columnName) =>
            record.ToFloat(columnName);

        public static int ToInt(this IDataRecord record, string columnName)
        {
            return record.GetInt32(record.GetOrdinal(columnName));
        }

        public static int? ToNullableInt(this IDataRecord record, string columnName)
        {
            if (record.IsDBNull(record.GetOrdinal(columnName)))
                return null;

            return record.ToInt(columnName);
        }

        public static string ToString(this IDataRecord record, string columnName)
        {
            if (record.IsDBNull(record.GetOrdinal(columnName)))
                return null;

            return record.GetString(record.GetOrdinal(columnName));
        }

        public static bool ToBool(this IDataRecord record, string columnName)
        {
            return record.GetBoolean(record.GetOrdinal(columnName));
        }
    }
}
