using System;
using System.Collections.Generic;
using System.Text;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    [Serializable]
    public class PrimitiveFields
    {
        //Instance
        public bool BooleanField= false;
        public byte ByteField = 0;
        public sbyte SByteField = 0;
        public int IntMaxField = 0;
        public int IntMinField = 0;
        public long LongMaxField = 0;
        public long LongMinField= 0;
        public Int16 Int16Field = 0;
        public UInt16 UInt16Field = 0; 
        public Int32 Int32Field = 0;
        public UInt32 UInt32Field = 0;
        public Int64 Int64Field = 0;
        public UInt64 UInt64Field = 0;
        public char CharField = '0';
        public double DoubleField= 0;
        public float FloatField = 0;

        //Static
        public static bool BooleanFieldStatic = false;
        public static byte ByteFieldStatic = 0;
        public static sbyte SByteFieldStatic = 0;
        public static int IntMaxFieldStatic = 0;
        public static int IntMinFieldStatic = 0;
        public static long LongMaxFieldStatic = 0;
        public static long LongMinFieldStatic = 0;
        public static Int16 Int16FieldStatic = 0;
        public static UInt16 UInt16FieldStatic = 0;
        public static Int32 Int32FieldStatic = 0;
        public static UInt32 UInt32FieldStatic = 0;
        public static Int64 Int64FieldStatic = 0;
        public static UInt64 UInt64FieldStatic = 0;
        public static char CharFieldStatic = '0';
        public static double DoubleFieldStatic = 0;
        public static float FloatFieldStatic = 0;

        public void Setup()
        {
            //Instance Fields Setup
            BooleanField = true;
            ByteField = 1;
            SByteField = 2;
            IntMaxField = int.MaxValue;
            IntMinField = int.MinValue;
            LongMaxField = long.MaxValue;
            LongMinField= long.MinValue;
            Int16Field = -3;
            UInt16Field = 4; 
            Int32Field = -5;
            UInt32Field = 6;
            Int64Field = -7;
            UInt64Field = 8;
            CharField = '\0';
            DoubleField= 1 / 3;
            FloatField = 1 / 3;

            //Static Fields Setup
            BooleanFieldStatic = true;
            ByteFieldStatic = 1;
            SByteFieldStatic = 2;
            IntMaxFieldStatic = int.MaxValue;
            IntMinFieldStatic = int.MinValue;
            LongMaxFieldStatic = long.MaxValue;
            LongMinFieldStatic = long.MinValue;
            Int16FieldStatic = -3;
            UInt16FieldStatic = 4;
            Int32FieldStatic = -5;
            UInt32FieldStatic = 6;
            Int64FieldStatic = -7;
            UInt64FieldStatic = 8;
            CharFieldStatic = '\0';
            DoubleFieldStatic = 1 / 3;
            FloatFieldStatic = 1 / 3;
        }
    }
}
