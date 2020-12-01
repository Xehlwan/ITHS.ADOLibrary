using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DBContactLibrary
{
    internal static class SqlHelper
    {
        /// <summary>
        /// Gets the value of the specified column as an instance of <see cref="object"/>.
        /// </summary>
        /// <param name="record">The record to get the column value from.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>The value of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// The name specified is not a valid column name.
        /// </exception>
        public static object GetValue(this IDataRecord record, string name) => record.GetValue(record.GetOrdinal(name));

        /// <summary>
        /// Add a parameter of any type to this <see cref="SqlCommand"/>.
        /// </summary>
        /// <typeparam name="T">The type of the parameter's value.</typeparam>
        /// <param name="cmd">This <see cref="SqlCommand"/>.</param>
        /// <param name="paramName">
        /// The name of the parameter, if not <see langword="null"/> or empty. If the name is
        /// missing a leading '@' it will be added.
        /// </param>
        /// <param name="value">
        /// The value of this parameter. <see langword="null"/> is converted to a <see cref="DBNull"/>.
        /// </param>
        /// <param name="type">The <see cref="SqlDbType"/> of this paramter.</param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of this parameter.</param>
        /// <param name="size">This parameter is used if the <see cref="SqlDbType"/> takes a size.</param>
        /// <returns>The <see cref="SqlParameter"/> that was added to this <see cref="SqlCommand"/>.</returns>
        public static SqlParameter ParamAny<T>(this SqlCommand cmd, string paramName, T value, SqlDbType type,
                                               int? size = null, ParameterDirection direction = ParameterDirection.Input)
            => AddParam(cmd, paramName, value, type, direction, size);

        /// <summary>
        /// Add a <b>DATETIME</b> parameter to this <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="cmd">This <see cref="SqlCommand"/>.</param>
        /// <param name="paramName">
        /// The name of the parameter, if not <see langword="null"/> or empty. If the name is
        /// missing a leading '@' it will be added.
        /// </param>
        /// <param name="value">
        /// The value of this parameter. <see langword="null"/> is converted to a <see cref="DBNull"/>.
        /// </param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of this parameter.</param>
        /// <returns>The <see cref="SqlParameter"/> that was added to this <see cref="SqlCommand"/>.</returns>
        public static SqlParameter ParamDateTime(this SqlCommand cmd, string paramName, DateTime? value,
                                                 ParameterDirection direction = ParameterDirection.Input)
            => AddParam(cmd, paramName, value, SqlDbType.DateTime, direction);

        /// <inheritdoc cref="ParamDateTime(SqlCommand, string, DateTime?, ParameterDirection)"/>
        public static SqlParameter ParamDateTime(this SqlCommand cmd, ParameterDirection direction)
            => AddParam<object>(cmd, null, null, SqlDbType.DateTime, direction);

        /// <summary>
        /// Add a <b>DATETIME2</b> parameter to this <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="cmd">This <see cref="SqlCommand"/>.</param>
        /// <param name="paramName">
        /// The name of the parameter, if not <see langword="null"/> or empty. If the name is
        /// missing a leading '@' it will be added.
        /// </param>
        /// <param name="value">
        /// The value of this parameter. <see langword="null"/> is converted to a <see cref="DBNull"/>.
        /// </param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of this parameter.</param>
        /// <returns>The <see cref="SqlParameter"/> that was added to this <see cref="SqlCommand"/>.</returns>
        public static SqlParameter ParamDateTime2(this SqlCommand cmd, string paramName, DateTime? value,
                                                  ParameterDirection direction = ParameterDirection.Input)
            => AddParam(cmd, paramName, value, SqlDbType.DateTime2, direction);

        /// <inheritdoc cref="ParamDateTime2(SqlCommand, string, DateTime?, ParameterDirection)"/>
        public static SqlParameter ParamDateTime2(this SqlCommand cmd, ParameterDirection direction)
            => AddParam<object>(cmd, null, null, SqlDbType.DateTime2, direction);

        /// <summary>
        /// Add an <b>INT</b> parameter to this <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="cmd">This <see cref="SqlCommand"/>.</param>
        /// <param name="paramName">
        /// The name of the parameter, if not <see langword="null"/> or empty. If the name is
        /// missing a leading '@' it will be added.
        /// </param>
        /// <param name="value">
        /// The value of this parameter. <see langword="null"/> is converted to a <see cref="DBNull"/>.
        /// </param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of this parameter.</param>
        /// <returns>The <see cref="SqlParameter"/> that was added to this <see cref="SqlCommand"/>.</returns>
        public static SqlParameter ParamInt(this SqlCommand cmd, string paramName, int? value,
                                            ParameterDirection direction = ParameterDirection.Input)
            => AddParam(cmd, paramName, value, SqlDbType.Int, direction);

        /// <inheritdoc cref="ParamInt(SqlCommand, string, int?, ParameterDirection)"/>
        public static SqlParameter ParamInt(this SqlCommand cmd, ParameterDirection direction)
            => AddParam<object>(cmd, null, null, SqlDbType.Int, direction);

        /// <summary>
        /// Add a <b>MONEY</b> parameter to this <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="cmd">This <see cref="SqlCommand"/>.</param>
        /// <param name="paramName">
        /// The name of the parameter, if not <see langword="null"/> or empty. If the name is
        /// missing a leading '@' it will be added.
        /// </param>
        /// <param name="value">
        /// The value of this parameter. <see langword="null"/> is converted to a <see cref="DBNull"/>.
        /// </param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of this parameter.</param>
        /// <returns>The <see cref="SqlParameter"/> that was added to this <see cref="SqlCommand"/>.</returns>
        public static SqlParameter ParamMoney(this SqlCommand cmd, string paramName, decimal? value,
                                              ParameterDirection direction = ParameterDirection.Input)
            => AddParam(cmd, paramName, value, SqlDbType.Int, direction);

        /// <inheritdoc cref="ParamMoney(SqlCommand, string, decimal?, ParameterDirection)"/>
        public static SqlParameter ParamMoney(this SqlCommand cmd, ParameterDirection direction)
            => AddParam<object>(cmd, null, null, SqlDbType.Money, direction);

        /// <summary>
        /// Add an <b>NVARCHAR</b> parameter to this <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="cmd">This <see cref="SqlCommand"/>.</param>
        /// <param name="paramName">
        /// The name of the parameter, if not <see langword="null"/> or empty. If the name is
        /// missing a leading '@' it will be added.
        /// </param>
        /// <param name="value">
        /// The value of this parameter. <see langword="null"/> is converted to a <see cref="DBNull"/>.
        /// </param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of this parameter.</param>
        /// <returns>The <see cref="SqlParameter"/> that was added to this <see cref="SqlCommand"/>.</returns>
        public static SqlParameter ParamNVarChar(this SqlCommand cmd, string paramName, string value, int size,
                                                 ParameterDirection direction = ParameterDirection.Input)
            => AddParam(cmd, paramName, value, SqlDbType.NVarChar, direction, size);

        /// <inheritdoc cref="ParamNVarChar(SqlCommand, string, string, int, ParameterDirection)"/>
        public static SqlParameter ParamNVarChar(this SqlCommand cmd, ParameterDirection direction)
            => AddParam<object>(cmd, null, null, SqlDbType.NVarChar, direction);

        /// <summary>
        /// Add a <b>VARCHAR</b> parameter to this <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="cmd">This <see cref="SqlCommand"/>.</param>
        /// <param name="paramName">
        /// The name of the parameter, if not <see langword="null"/> or empty. If the name is
        /// missing a leading '@' it will be added.
        /// </param>
        /// <param name="value">
        /// The value of this parameter. <see langword="null"/> is converted to a <see cref="DBNull"/>.
        /// </param>
        /// <param name="direction">The <see cref="ParameterDirection"/> of this parameter.</param>
        /// <returns>The <see cref="SqlParameter"/> that was added to this <see cref="SqlCommand"/>.</returns>
        public static SqlParameter ParamVarChar(this SqlCommand cmd, string paramName, string value, int size,
                                                ParameterDirection direction = ParameterDirection.Input)
            => AddParam(cmd, paramName, value, SqlDbType.VarChar, direction, size);

        /// <inheritdoc cref="ParamVarChar(SqlCommand, string, string, int, ParameterDirection)"/>
        public static SqlParameter ParamVarChar(this SqlCommand cmd, ParameterDirection direction)
            => AddParam<object>(cmd, null, null, SqlDbType.VarChar, direction);

        /// <summary>
        /// Provides an interface allowing Linq-queries on an instance of <see
        /// cref="SqlDataReader"/>. Only a weak reference will be held, to avoid keeping the
        /// connection open.
        /// </summary>
        /// <param name="reader">The <see cref="SqlDataReader"/> instance to query.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> allowing Linq-queries. If the <see
        /// cref="SqlDataReader"/> is disposed, the collection will be empty.
        /// </returns>
        public static IEnumerable<IDataRecord> ToLinq(this SqlDataReader reader)
        {
            return new EnumerableReader(reader);
        }

        private static SqlParameter AddParam<T>(SqlCommand cmd, string name, T value, SqlDbType type,
                                                        ParameterDirection direction = ParameterDirection.Input,
                                                int? size = null)
        {
            SqlParameter param = new()
            {
                Value = value ?? (object)DBNull.Value,
                SqlDbType = type,
                Direction = direction
            };

            if (!string.IsNullOrWhiteSpace(name))
            {
                param.ParameterName = name.First() == '@' ? name : '@' + name;
            }

            if (size is not null)
                param.Size = size.Value;

            return cmd.Parameters.Add(param);
        }

        private class EnumerableReader : IEnumerable<IDataRecord>
        {
            private WeakReference<SqlDataReader> reader;

            public EnumerableReader(SqlDataReader dataReader)
            {
                reader = new(dataReader);
            }

            public IEnumerator<IDataRecord> GetEnumerator()
            {
                if (reader.TryGetTarget(out var dataReader))
                {
                    var enumerator = dataReader.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        yield return (IDataRecord)enumerator.Current;
                    }
                }
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}