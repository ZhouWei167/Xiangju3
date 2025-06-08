using Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Linq;

namespace MirDB
{
    /// <summary>
    /// 数据库值
    /// </summary>
    public sealed class DBValue
    {
        public bool IsNew = false;
        /// <summary>
        /// 类型列表
        /// </summary>
        internal static readonly Dictionary<string, Type> TypeList;
        /// <summary>
        /// 类型读取
        /// </summary>
        private static readonly Dictionary<Type, Func<BinaryReader, object>> TypeRead;
        /// <summary>
        /// 类型写入
        /// </summary>
        private static readonly Dictionary<Type, Action<object, BinaryWriter>> TypeWrite;

        #region MYSQL类型转换
        /// <summary>
        /// C#类型转SQL表字段定义
        /// </summary>
        internal static readonly Dictionary<string, string> TypeToColumnSQL;
        /// <summary>
        /// 读取SQL字段值转C#对应的类型
        /// </summary>
        private static readonly Dictionary<Type, Func<object, object>> TypeReadFromeSQL;
        /// <summary>
        /// C#类型值转SQL对应的字段类型
        /// </summary>
        private static readonly Dictionary<Type, Func<object, object>> TypeWriteToSQL;
        #endregion

        static DBValue()
        {
            #region 类型
            TypeList = new Dictionary<string, Type>
            {
                [typeof(Boolean).FullName] = typeof(Boolean),
                [typeof(Byte).FullName] = typeof(Byte),
                [typeof(Byte[]).FullName] = typeof(Byte[]),
                [typeof(Char).FullName] = typeof(Char),
                [typeof(System.Drawing.Color).FullName] = typeof(Color),
                [typeof(DateTime).FullName] = typeof(DateTime),
                [typeof(Decimal).FullName] = typeof(Decimal),
                [typeof(Double).FullName] = typeof(Double),
                [typeof(Int16).FullName] = typeof(Int16),
                [typeof(Int32).FullName] = typeof(Int32),
                [typeof(Int32[]).FullName] = typeof(Int32[]),
                [typeof(Int64).FullName] = typeof(Int64),
                [typeof(System.Drawing.Point).FullName] = typeof(Point),
                [typeof(SByte).FullName] = typeof(SByte),
                [typeof(Single).FullName] = typeof(Single),
                [typeof(System.Drawing.Size).FullName] = typeof(Size),
                [typeof(String).FullName] = typeof(String),
                [typeof(TimeSpan).FullName] = typeof(TimeSpan),
                [typeof(UInt16).FullName] = typeof(UInt16),
                [typeof(UInt32).FullName] = typeof(UInt32),
                [typeof(UInt64).FullName] = typeof(UInt64),
                [typeof(System.Drawing.Point[]).FullName] = typeof(Point[]),
                [typeof(Stats).FullName] = typeof(Stats),
                [typeof(BitArray).FullName] = typeof(BitArray)
            };
            #endregion

            #region 读取

            TypeRead = new Dictionary<Type, Func<BinaryReader, object>>
            {
                [typeof(Boolean)] = r => r.ReadBoolean(),
                [typeof(Byte)] = r => r.ReadByte(),
                [typeof(Byte[])] = r => r.ReadBytes(r.ReadInt32()),
                [typeof(Char)] = r => r.ReadChar(),
                [typeof(Color)] = r => Color.FromArgb(r.ReadInt32()),
                [typeof(DateTime)] = r => DateTime.FromBinary(r.ReadInt64()),
                [typeof(Decimal)] = r => r.ReadDecimal(),
                [typeof(Double)] = r => r.ReadDouble(),
                [typeof(Enum)] = r => r.ReadInt32(),
                [typeof(Int16)] = r => r.ReadInt16(),
                [typeof(Int32)] = r => r.ReadInt32(),
                [typeof(Int32[])] = r =>
                {
                    if (!r.ReadBoolean()) return null;

                    int length = r.ReadInt32();

                    Int32[] values = new Int32[length];
                    for (int i = 0; i < length; i++)
                        values[i] = r.ReadInt32();

                    return values;
                },
                [typeof(Int64)] = r => r.ReadInt64(),
                [typeof(Point)] = r => new Point(r.ReadInt32(), r.ReadInt32()),
                [typeof(SByte)] = r => r.ReadSByte(),
                [typeof(Single)] = r => r.ReadSingle(),
                [typeof(Size)] = r => new Size(r.ReadInt32(), r.ReadInt32()),
                [typeof(String)] = r => r.ReadString(),
                [typeof(TimeSpan)] = r => TimeSpan.FromTicks(r.ReadInt64()),
                [typeof(UInt16)] = r => r.ReadUInt16(),
                [typeof(UInt32)] = r => r.ReadUInt32(),
                [typeof(UInt64)] = r => r.ReadUInt64(),
                [typeof(Point[])] = r =>
                {
                    if (!r.ReadBoolean()) return null;

                    int length = r.ReadInt32();

                    Point[] points = new Point[length];
                    for (int i = 0; i < length; i++)
                        points[i] = new Point(r.ReadInt32(), r.ReadInt32());

                    return points;
                },
                [typeof(Stats)] = r => r.ReadBoolean() ? new Stats(r) : null,
                [typeof(BitArray)] = r =>
                {
                    if (!r.ReadBoolean()) return null;

                    return new BitArray(r.ReadBytes(r.ReadInt32()));
                },
            };

            #endregion

            #region 写入

            TypeWrite = new Dictionary<Type, Action<object, BinaryWriter>>
            {
                [typeof(Boolean)] = (v, w) => w.Write((bool)v),
                [typeof(Byte)] = (v, w) => w.Write((Byte)v),
                [typeof(Byte[])] = (v, w) =>
                {
                    w.Write(((Byte[])v).Length);
                    w.Write((Byte[])v);
                },
                [typeof(Char)] = (v, w) => w.Write((Char)v),
                [typeof(Color)] = (v, w) => w.Write(((Color)v).ToArgb()),
                [typeof(DateTime)] = (v, w) => w.Write(((DateTime)v).ToBinary()),
                [typeof(Decimal)] = (v, w) => w.Write((Decimal)v),
                [typeof(Double)] = (v, w) => w.Write((Double)v),
                [typeof(Int16)] = (v, w) => w.Write((Int16)v),
                [typeof(Int32)] = (v, w) => w.Write((Int32)v),
                [typeof(Int32[])] = (v, w) =>
                {
                    w.Write(v != null);
                    if (v == null) return;
                    Int32[] values = (Int32[])v;

                    w.Write(values.Length);

                    foreach (Int32 value in values)
                        w.Write(value);
                },
                [typeof(Int64)] = (v, w) => w.Write((Int64)v),
                [typeof(Point)] = (v, w) =>
                {
                    w.Write(((Point)v).X);
                    w.Write(((Point)v).Y);
                },
                [typeof(SByte)] = (v, w) => w.Write((SByte)v),
                [typeof(Single)] = (v, w) => w.Write((Single)v),
                [typeof(Size)] = (v, w) =>
                {
                    w.Write(((Size)v).Width);
                    w.Write(((Size)v).Height);
                },
                [typeof(String)] = (v, w) => w.Write((String)v ?? string.Empty),
                [typeof(TimeSpan)] = (v, w) => w.Write(((TimeSpan)v).Ticks),
                [typeof(UInt16)] = (v, w) => w.Write((UInt16)v),
                [typeof(UInt32)] = (v, w) => w.Write((UInt32)v),
                [typeof(UInt64)] = (v, w) => w.Write((UInt64)v),
                [typeof(Point[])] = (v, w) =>
                {
                    w.Write(v != null);
                    if (v == null) return;
                    Point[] points = (Point[])v;

                    w.Write(points.Length);

                    foreach (Point point in points)
                    {
                        w.Write(point.X);
                        w.Write(point.Y);
                    }
                },
                [typeof(Stats)] = (v, w) =>
                {
                    w.Write(v != null);
                    if (v == null) return;

                    ((Stats)v).Write(w);
                },
                [typeof(BitArray)] = (v, w) =>
                {
                    w.Write(v != null);
                    if (v == null) return;

                    BitArray array = (BitArray)v;


                    byte[] bytes = new byte[(int)Math.Ceiling(array.Length / 8d)];
                    array.CopyTo(bytes, 0);

                    w.Write(bytes.Length);
                    w.Write(bytes);
                },
            };

            #endregion

            #region MYSQL类型转换
            // C#类型转SQL表字段定义
            TypeToColumnSQL = new Dictionary<string, string>
            {
                //MySQL有四种BLOB类型: ·tinyblob:仅255个字节 ·blob:最大限制到65K字节 ·mediumblob:限制到16M字节 ·longblob:可达4GB
                [typeof(Boolean).FullName] = " TINYINT(1) ",
                [typeof(Byte).FullName] = " TINYINT(3) UNSIGNED ",
                [typeof(Byte[]).FullName] = " BLOB ",
                [typeof(Char).FullName] = " VARCHAR(1) ",
                [typeof(Color).FullName] = " INT(11) ",
                [typeof(DateTime).FullName] = " DATETIME(6) DEFAULT NULL ",
                [typeof(Decimal).FullName] = " DECIMAL(35,5) ",
                [typeof(Double).FullName] = " DOUBLE ",
                [typeof(Enum).FullName] = " INT(11) ",
                [typeof(Int16).FullName] = " SMALLINT(6) ",
                [typeof(Int32).FullName] = " INT(11) ",
                [typeof(Int32[]).FullName] = " BLOB ",
                [typeof(Int64).FullName] = " BIGINT(20) ",
                [typeof(Point).FullName] = " VARCHAR(23) ",
                [typeof(SByte).FullName] = " TINYINT(4) ",
                [typeof(Single).FullName] = " FLOAT ",
                [typeof(Size).FullName] = " VARCHAR(21) ",
                [typeof(String).FullName] = " VARCHAR(1000) ",
                [typeof(TimeSpan).FullName] = " BIGINT(20) ",
                [typeof(UInt16).FullName] = " SMALLINT(5) UNSIGNED ",
                [typeof(UInt32).FullName] = " INT(10) UNSIGNED ",
                [typeof(UInt64).FullName] = " BIGINT(20) UNSIGNED ",
                [typeof(Point[]).FullName] = " MEDIUMBLOB ",
                [typeof(Stats).FullName] = " BLOB ",
                [typeof(BitArray).FullName] = " MEDIUMBLOB ",
            };
            // 读取SQL字段值转C#对应的类型值
            TypeReadFromeSQL = new Dictionary<Type, Func<object, object>>
            {
                [typeof(Boolean)] = v => v == DBNull.Value ? false : Convert.ToBoolean(v),
                [typeof(Byte)] = v => v == DBNull.Value ? (Byte)0 : Convert.ToByte(v),
                [typeof(Byte[])] = v => v == DBNull.Value ? null : (Byte[])v,
                [typeof(Char)] = v => v == DBNull.Value ? '\0' : Convert.ToChar(v),
                [typeof(DateTime)] = v => v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v),
                [typeof(Decimal)] = v => v == DBNull.Value ? 0m : Convert.ToDecimal(v),
                [typeof(Double)] = v => v == DBNull.Value ? 0.0 : Convert.ToDouble(v),
                [typeof(Int16)] = v => v == DBNull.Value ? (Int16)0 : Convert.ToInt16(v),
                [typeof(Int32)] = v => v == DBNull.Value ? 0 : Convert.ToInt32(v),
                [typeof(Int32[])] = r =>
                {
                    if (r == DBNull.Value || r == null) return null;
                    byte[] byteArray = r as byte[];
                    if (byteArray == null) throw new InvalidCastException($"Cannot convert type {r.GetType()} to byte[] for Int32[] deserialization from SQL.");
                    using (MemoryStream mem = new MemoryStream(byteArray))
                    using (BinaryReader br = new BinaryReader(mem))
                    {
                        int length = br.ReadInt32();
                        Int32[] values = new Int32[length];
                        for (int i = 0; i < length; i++)
                            values[i] = br.ReadInt32();
                        return values;
                    }
                },
                [typeof(Int64)] = v => v == DBNull.Value ? 0L : Convert.ToInt64(v),
                [typeof(SByte)] = v => v == DBNull.Value ? (SByte)0 : Convert.ToSByte(v),
                [typeof(Single)] = v => v == DBNull.Value ? 0.0f : Convert.ToSingle(v),
                [typeof(String)] = v => v == DBNull.Value ? null : Convert.ToString(v),
                [typeof(UInt16)] = v => v == DBNull.Value ? (UInt16)0 : Convert.ToUInt16(v),
                [typeof(UInt32)] = v => v == DBNull.Value ? 0u : Convert.ToUInt32(v),
                [typeof(UInt64)] = v => v == DBNull.Value ? 0UL : Convert.ToUInt64(v),
                [typeof(Point[])] = r =>
                {
                    if (r == DBNull.Value || r == null) return null;
                    byte[] byteArray = r as byte[];
                    if (byteArray == null) throw new InvalidCastException($"Cannot convert type {r.GetType()} to byte[] for Point[] deserialization from SQL.");
                    using (MemoryStream mem = new MemoryStream(byteArray))
                    using (BinaryReader br = new BinaryReader(mem))
                    {
                        int length = br.ReadInt32();
                        Point[] points = new Point[length];
                        for (int i = 0; i < length; i++)
                            points[i] = new Point(br.ReadInt32(), br.ReadInt32());
                        return points;
                    }
                },
                [typeof(TimeSpan)] = v => 
                {
                    if (v == DBNull.Value) return TimeSpan.Zero;
                    
                    try 
                    {
                        // 先尝试将值转换为 string，然后再尝试解析为 double
                        // 这样可以避免一些潜在的类型转换问题
                        string strValue = v.ToString();
                        
                        // 检查字符串是否过长，可能表示值太大
                        if (strValue.Length > 15) // 一个合理的长度阈值
                        {
                            System.Diagnostics.Debug.WriteLine($"警告：TimeSpan 值 {strValue} 可能过大，返回最大值。");
                            return TimeSpan.MaxValue;
                        }
                        
                        // 尝试转换为 double
                        if (double.TryParse(strValue, out double milliseconds))
                        {
                            // 检查是否超出 TimeSpan 的有效范围
                            if (milliseconds > TimeSpan.MaxValue.TotalMilliseconds)
                            {
                                System.Diagnostics.Debug.WriteLine($"警告：TimeSpan 值 {milliseconds}ms 超出了最大范围，已截断为最大值。");
                                return TimeSpan.MaxValue;
                            }
                            else if (milliseconds < TimeSpan.MinValue.TotalMilliseconds)
                            {
                                System.Diagnostics.Debug.WriteLine($"警告：TimeSpan 值 {milliseconds}ms 小于最小范围，已截断为最小值。");
                                return TimeSpan.MinValue;
                            }
                            
                            return TimeSpan.FromMilliseconds(milliseconds);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"警告：无法将值 {strValue} 转换为有效的 TimeSpan，返回 TimeSpan.Zero。");
                            return TimeSpan.Zero;
                        }
                    }
                    catch (Exception ex)
                    {
                        // 捕获所有可能的异常，确保不会崩溃
                        System.Diagnostics.Debug.WriteLine($"警告：转换 TimeSpan 时发生异常: {ex.Message}，返回 TimeSpan.Zero。");
                        return TimeSpan.Zero;
                    }
                },
                [typeof(Color)] = v => v == DBNull.Value ? Color.Empty : Color.FromArgb(Convert.ToInt32(v)),
                [typeof(Stats)] = r =>
                {
                    if (r == DBNull.Value || r == null) return null;
                    byte[] byteArray = r as byte[];
                    if (byteArray == null)
                        throw new InvalidCastException($"Cannot convert type {r.GetType()} to byte[] for Stats deserialization from SQL. Ensure the DB column is BLOB and returns byte[].");
                    using (MemoryStream mem = new MemoryStream(byteArray))
                    using (BinaryReader br = new BinaryReader(mem))
                    {
                        return new Stats(br); // Assumes Stats(BinaryReader) constructor exists
                    }
                },
                [typeof(BitArray)] = r =>
                {
                    if (r == DBNull.Value || r == null) return null;
                    byte[] byteArray = r as byte[];
                    if (byteArray == null)
                        throw new InvalidCastException($"Cannot convert type {r.GetType()} to byte[] for BitArray deserialization from SQL. Ensure the DB column is BLOB/MEDIUMBLOB and returns byte[].");
                    return new BitArray(byteArray);
                },
                [typeof(Point)] = v =>
                {
                    if (v is string sPoint && !string.IsNullOrEmpty(sPoint) && sPoint.StartsWith("<") && sPoint.EndsWith(">"))
                    {
                        string[] parts = sPoint.Trim('<', '>').Split(',');
                        if (parts.Length == 2 && int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y))
                        {
                            return new Point(x, y);
                        }
                    }
                    return Point.Empty; // Or throw an exception for invalid format
                },
                [typeof(Size)] = v =>
                {
                    if (v is string sSize && !string.IsNullOrEmpty(sSize) && sSize.StartsWith("<") && sSize.EndsWith(">"))
                    {
                        string[] parts = sSize.Trim('<', '>').Split(',');
                        if (parts.Length == 2 && int.TryParse(parts[0], out int w) && int.TryParse(parts[1], out int h))
                        {
                            return new Size(w, h);
                        }
                    }
                    return Size.Empty; // Or throw an exception for invalid format
                }
            };
            // C#类型值转SQL对应的字段类型值
            TypeWriteToSQL = new Dictionary<Type, Func<object, object>>
            {
                [typeof(Boolean)] = v => v == null ? DBNull.Value : (object)(((bool)v) ? 1 : 0),
                [typeof(Byte)] = v => v ?? DBNull.Value,
                [typeof(Byte[])] = v => v ?? DBNull.Value,
                [typeof(Char)] = v => v ?? DBNull.Value,
                [typeof(DateTime)] = v => v ?? DBNull.Value,
                [typeof(Decimal)] = v => v ?? DBNull.Value,
                [typeof(Double)] = v => v ?? DBNull.Value,
                [typeof(Int16)] = v => v ?? DBNull.Value,
                [typeof(Int32)] = v => v ?? DBNull.Value,
                [typeof(Int32[])] = r =>
                {
                    if (r == null) return DBNull.Value;
                    Int32[] values = (Int32[])r;
                    using (MemoryStream mem = new MemoryStream())
                    using (BinaryWriter bw = new BinaryWriter(mem))
                    {
                        bw.Write(values.Length);
                        foreach (Int32 value in values)
                            bw.Write(value);
                        return mem.ToArray();
                    }
                },
                [typeof(Int64)] = v => v ?? DBNull.Value,
                [typeof(SByte)] = v => v ?? DBNull.Value,
                [typeof(Single)] = v => v ?? DBNull.Value,
                [typeof(String)] = v => v ?? DBNull.Value,
                [typeof(UInt16)] = v => v ?? DBNull.Value,
                [typeof(UInt32)] = v => v ?? DBNull.Value,
                [typeof(UInt64)] = v => v ?? DBNull.Value,
                [typeof(Point[])] = r =>
                {
                    if (r == null) return DBNull.Value;
                    Point[] points = (Point[])r;
                    using (MemoryStream mem = new MemoryStream())
                    using (BinaryWriter bw = new BinaryWriter(mem))
                    {
                        bw.Write(points.Length);
                        foreach (Point point in points)
                        {
                            bw.Write(point.X);
                            bw.Write(point.Y);
                        }
                        return mem.ToArray();
                    }
                },
                [typeof(TimeSpan)] = v => v == null ? DBNull.Value : (object)((TimeSpan)v).TotalMilliseconds,
                [typeof(Color)] = v => v == null ? DBNull.Value : (object)((Color)v).ToArgb(),
                [typeof(Stats)] = r =>
                {
                    if (r == null) return DBNull.Value;
                    Stats stats = r as Stats;
                    if (stats == null)
                        throw new InvalidCastException($"Cannot convert type {r.GetType()} to Stats for Stats serialization to SQL.");
                    using (MemoryStream mem = new MemoryStream())
                    using (BinaryWriter bw = new BinaryWriter(mem))
                    {
                        stats.Write(bw); // Assumes stats.Write(BinaryWriter) method exists
                        return mem.ToArray();
                    }
                },
                [typeof(BitArray)] = r =>
                {
                    if (r == null) return DBNull.Value;
                    BitArray array = r as BitArray;
                    if (array == null)
                         throw new InvalidCastException($"Cannot convert type {r.GetType()} to BitArray for BitArray serialization to SQL.");
                    byte[] bytes = new byte[(int)Math.Ceiling(array.Length / 8d)];
                    array.CopyTo(bytes, 0);
                    return bytes;
                },
                [typeof(Point)] = v => 
                {
                    if (v == null) return DBNull.Value;
                    if (v is Point p) return (object)$"<{p.X},{p.Y}>";
                    // This case should ideally not be reached if v is always a Point or null.
                    // However, to satisfy the compiler and provide a fallback string if v was something unexpected:
                    return (object)$"<{Point.Empty.X},{Point.Empty.Y}>"; 
                },
                [typeof(Size)] = v =>
                {
                    if (v == null) return DBNull.Value;
                    if (v is Size s) return (object)$"<{s.Width},{s.Height}>";
                    // Fallback, similar to Point:
                    return (object)$"<{Size.Empty.Width},{Size.Empty.Height}>";
                }
            };
            #endregion
        }
        /// <summary>
        /// 公共字符串属性名
        /// </summary>
        public string PropertyName { get; }
        /// <summary>
        /// 公共类型属性类型
        /// </summary>
        public Type PropertyType { get; }
        /// <summary>
        /// 公共属性信息属性
        /// </summary>
        public PropertyInfo Property { get; }
        /// <summary>
        /// 数据值(二进制读取 类型)
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="type"></param>
        public DBValue(BinaryReader reader, Type type)
        {
            PropertyName = reader.ReadString();
            PropertyType = TypeList[reader.ReadString()];

            PropertyInfo property = type?.GetProperty(PropertyName);

            if (property != null)
                if (property.GetCustomAttribute<IgnoreProperty>() != null) return;

            Property = property;
        }
        /// <summary>
        /// 数据值(属性信息)
        /// </summary>
        /// <param name="property"></param>
        public DBValue(PropertyInfo property)
        {
            Property = property;

            PropertyName = property.Name;

            if (property.PropertyType.IsEnum)
            {
                PropertyType = property.PropertyType.GetEnumUnderlyingType();
                return;
            }

            if (property.PropertyType.IsSubclassOf(typeof(DBObject)))
            {
                PropertyType = typeof(int);
                return;
            }

            PropertyType = property.PropertyType;
        }
        /// <summary>
        /// 保存(二进制写入程序)
        /// </summary>
        /// <param name="writer"></param>
        public void Save(BinaryWriter writer)
        {
            writer.Write(PropertyName);
            writer.Write(PropertyType.FullName);
        }
        /// <summary>
        /// 读取值(二进制读取)
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public object ReadValue(BinaryReader reader)
        {
            return TypeRead[PropertyType](reader);
        }
        /// <summary>
        /// 写入值(对象值, 二进制写入程序)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="writer"></param>
        public void WriteValue(object value, BinaryWriter writer)
        {
            TypeWrite[PropertyType](value, writer);
        }

        #region MYSQL类型转换
        public object ReadTypeToColumnSQL()
        {
            return TypeToColumnSQL[PropertyType.FullName];
        }
        public object ReadValueSQL(object value)
        {
            return TypeReadFromeSQL[PropertyType](value);
        }
        public object WriteValueSQL(object value)
        {
            return TypeWriteToSQL[PropertyType](value);
        }
        #endregion

        /// <summary>
        /// 匹配(数据值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsMatch(DBValue value)
        {
            return string.Compare(PropertyName, value.PropertyName, StringComparison.Ordinal) == 0 && PropertyType == value.PropertyType;
        }
    }
}
