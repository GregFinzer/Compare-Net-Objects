using System;
using System.Collections.Generic;
using System.Text;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    [Serializable]
    public class PrimitiveFieldsNullable
    {
        //Instance
        public bool? BooleanField= true;
        public byte? ByteField = 0;
        public sbyte? SByteField = 0;
        public int? IntField = 0;
        public long? LongField = 0;
        public Int16? Int16Field = 0;
        public UInt16? UInt16Field = 0;
        public Int32? Int32Field = 0;
        public UInt32? UInt32Field = 0;
        public Int64? Int64Field = 0;
        public UInt64? UInt64Field = 0;
        public char? CharField = '0';
        public double? DoubleField = 0;
        public float? FloatField = 0;

        //Static
        public static bool? BooleanFieldStatic = true;
        public static byte? ByteFieldStatic = 0;
        public static sbyte? SByteFieldStatic = 0;
        public static int? IntFieldStatic = 0;
        public static long? LongFieldStatic = 0;
        public static Int16? Int16FieldStatic = 0;
        public static UInt16? UInt16FieldStatic = 0;
        public static Int32? Int32FieldStatic = 0;
        public static UInt32? UInt32FieldStatic = 0;
        public static Int64? Int64FieldStatic = 0;
        public static UInt64? UInt64FieldStatic = 0;
        public static char? CharFieldStatic = '0';
        public static double? DoubleFieldStatic = 0;
        public static float? FloatFieldStatic = 0;

        public void Setup()
        {
            //Instance Fields Setup
            BooleanField = null;
            ByteField = null;
            SByteField = null;
            IntField = null;
            LongField = null;
            Int16Field = null;
            UInt16Field = null;
            Int32Field = null;
            UInt32Field = null;
            Int64Field = null;
            UInt64Field = null;
            CharField = null;
            DoubleField = null;
            FloatField = null;

            //Static Fields Setup
            BooleanFieldStatic = null;
            ByteFieldStatic = null;
            SByteFieldStatic = null;
            IntFieldStatic = null;
            LongFieldStatic = null;
            Int16FieldStatic = null;
            UInt16FieldStatic = null;
            Int32FieldStatic = null;
            UInt32FieldStatic = null;
            Int64FieldStatic = null;
            UInt64FieldStatic = null;
            CharFieldStatic = null;
            DoubleFieldStatic = null;
            FloatFieldStatic = null;
        }
    }
}
