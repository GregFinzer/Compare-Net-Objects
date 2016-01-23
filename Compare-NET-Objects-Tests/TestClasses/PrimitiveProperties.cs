#region Includes
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    [Serializable]
    public class PrimitiveProperties
    {
        #region Class Variables
        //Instance
        private bool _booleanProperty = false;
        private byte _byteProperty = 0;
        private sbyte _sByteProperty = 0;
        private int _intMaxProperty = 0;
        private int _intMinProperty = 0;
        private long _longMaxProperty = 0;
        private long _longMinProperty= 0;
        private Int16 _int16Property = 0;
        private UInt16 _uInt16Property = 0; 
        private Int32 _int32Property = 0;
        private UInt32 _uInt32Property = 0;
        private Int64 _int64Property = 0;
        private UInt64 _uInt64Property = 0;
        private char _charProperty = '0';
        private double _doubleProperty = 0;
        private float _floatProperty = 0;

        //Static
        private static bool _booleanPropertyStatic = false;
        private static byte _bytePropertyStatic = 0;
        private static sbyte _sBytePropertyStatic = 0;
        private static int _intMaxPropertyStatic = 0;
        private static int _intMinPropertyStatic = 0;
        private static long _longMaxPropertyStatic = 0;
        private static long _longMinPropertyStatic = 0;
        private static Int16 _int16PropertyStatic = 0;
        private static UInt16 _uInt16PropertyStatic = 0;
        private static Int32 _int32PropertyStatic = 0;
        private static UInt32 _uInt32PropertyStatic = 0;
        private static Int64 _int64PropertyStatic = 0;
        private static UInt64 _uInt64PropertyStatic = 0;
        private static char _charPropertyStatic = '0';
        private static double _doublePropertyStatic = 0;
        private static float _floatPropertyStatic = 0;
        #endregion

        #region Instance Properties
        /// <summary>
        /// Property Get/Set for Boolean Property
        /// </summary>
        public bool BooleanProperty
        {
            get { return _booleanProperty; }
            set { _booleanProperty = value; }
        }

        /// <summary>
        /// Property Get/Set for Byte Property
        /// </summary>
        public byte ByteProperty
        {
            get { return _byteProperty; }
            set { _byteProperty = value; }
        }

        /// <summary>
        /// Property Get/Set for Sbyte Property
        /// </summary>
        public sbyte SByteProperty
        {
            get { return _sByteProperty; }
            set { _sByteProperty = value; }
        }

        /// <summary>
        /// Property Get/Set for Int Max Property
        /// </summary>
        public int IntMaxProperty
        {
            get { return _intMaxProperty; }
            set { _intMaxProperty = value; }
        }

        /// <summary>
        /// Property Get/Set for Int Min Property
        /// </summary>
        public int IntMinProperty
        {
            get { return _intMinProperty; }
            set { _intMinProperty = value; }
        }

        /// <summary>
        /// Property Get/Set for Long Max Property
        /// </summary>
        public long LongMaxProperty
        {
            get { return _longMaxProperty; }
            set { _longMaxProperty = value; }
        }

        /// <summary>
        /// Property Get/Set for Long Min Property
        /// </summary>
        public long LongMinProperty
        {
            get { return _longMinProperty; }
            set { _longMinProperty = value; }
        }

        /// <summary>
        /// Property Get/Set for Int 16 Property
        /// </summary>
        public Int16 Int16Property
        {
            get { return _int16Property; }
            set { _int16Property = value; }
        }

        /// <summary>
        /// Property Get/Set for U Int 16 Property
        /// </summary>
        public UInt16 UInt16Property
        {
            get { return _uInt16Property; }
            set { _uInt16Property = value; }
        }

        /// <summary>
        /// Property Get/Set for Int 32 Property
        /// </summary>
        public Int32 Int32Property
        {
            get { return _int32Property; }
            set { _int32Property = value; }
        }

        /// <summary>
        /// Property Get/Set for U Int 32 Property
        /// </summary>
        public UInt32 UInt32Property
        {
            get { return _uInt32Property; }
            set { _uInt32Property = value; }
        }

        /// <summary>
        /// Property Get/Set for Int 64 Property
        /// </summary>
        public Int64 Int64Property
        {
            get { return _int64Property; }
            set { _int64Property = value; }
        }

        /// <summary>
        /// Property Get/Set for U Int 64 Property
        /// </summary>
        public UInt64 UInt64Property
        {
            get { return _uInt64Property; }
            set { _uInt64Property = value; }
        }

        /// <summary>
        /// Property Get/Set for Char Property
        /// </summary>
        public char CharProperty
        {
            get { return _charProperty; }
            set { _charProperty = value; }
        }

        /// <summary>
        /// Property Get/Set for Double Property
        /// </summary>
        public double DoubleProperty
        {
            get { return _doubleProperty; }
            set { _doubleProperty = value; }
        }

        /// <summary>
        /// Property Get/Set for Float Property
        /// </summary>
        public float FloatProperty
        {
            get { return _floatProperty; }
            set { _floatProperty = value; }
        }


        #endregion
        
        #region Static Properties
        /// <summary>
        /// Property Get/Set for Boolean Property
        /// </summary>
        public static bool BooleanPropertyStatic
        {
            get { return _booleanPropertyStatic; }
            set { _booleanPropertyStatic = value; }
        }

        /// <summary>
        /// Property Get/Set for Byte Property
        /// </summary>
        public static byte BytePropertyStatic
        {
            get { return _bytePropertyStatic; }
            set { _bytePropertyStatic = value; }
        }

        /// <summary>
        /// Property Get/Set for Sbyte Property
        /// </summary>
        public static sbyte SBytePropertyStatic
        {
            get { return _sBytePropertyStatic; }
            set { _sBytePropertyStatic = value; }
        }

        /// <summary>
        /// Property Get/Set for Int Max Property
        /// </summary>
        public static int IntMaxPropertyStatic
        {
            get { return _intMaxPropertyStatic; }
            set { _intMaxPropertyStatic = value; }
        }

        /// <summary>
        /// Property Get/Set for Int Min Property
        /// </summary>
        public static int IntMinPropertyStatic
        {
            get { return _intMinPropertyStatic; }
            set { _intMinPropertyStatic = value; }
        }

        /// <summary>
        /// Property Get/Set for Long Max Property
        /// </summary>
        public static long LongMaxPropertyStatic
        {
            get { return _longMaxPropertyStatic; }
            set { _longMaxPropertyStatic = value; }
        }

        /// <summary>
        /// Property Get/Set for Long Min Property
        /// </summary>
        public static long LongMinPropertyStatic
        {
            get { return _longMinPropertyStatic; }
            set { _longMinPropertyStatic = value; }
        }

        /// <summary>
        /// Property Get/Set for Int 16 Property
        /// </summary>
        public static Int16 Int16PropertyStatic
        {
            get { return _int16PropertyStatic; }
            set { _int16PropertyStatic = value; }
        }

        /// <summary>
        /// Property Get/Set for U Int 16 Property
        /// </summary>
        public static UInt16 UInt16PropertyStatic
        {
            get { return _uInt16PropertyStatic; }
            set { _uInt16PropertyStatic = value; }
        }

        /// <summary>
        /// Property Get/Set for Int 32 Property
        /// </summary>
        public static Int32 Int32PropertyStatic
        {
            get { return _int32PropertyStatic; }
            set { _int32PropertyStatic = value; }
        }

        /// <summary>
        /// Property Get/Set for U Int 32 Property
        /// </summary>
        public static UInt32 UInt32PropertyStatic
        {
            get { return _uInt32PropertyStatic; }
            set { _uInt32PropertyStatic = value; }
        }

        /// <summary>
        /// Property Get/Set for Int 64 Property
        /// </summary>
        public static Int64 Int64PropertyStatic
        {
            get { return _int64PropertyStatic; }
            set { _int64PropertyStatic = value; }
        }

        /// <summary>
        /// Property Get/Set for U Int 64 Property
        /// </summary>
        public static UInt64 UInt64PropertyStatic
        {
            get { return _uInt64PropertyStatic; }
            set { _uInt64PropertyStatic = value; }
        }

        /// <summary>
        /// Property Get/Set for Char Property
        /// </summary>
        public static char CharPropertyStatic
        {
            get { return _charPropertyStatic; }
            set { _charPropertyStatic = value; }
        }

        /// <summary>
        /// Property Get/Set for Double Property
        /// </summary>
        public static double DoublePropertyStatic
        {
            get { return _doublePropertyStatic; }
            set { _doublePropertyStatic = value; }
        }

        /// <summary>
        /// Property Get/Set for Float Property
        /// </summary>
        public static float FloatPropertyStatic
        {
            get { return _floatPropertyStatic; }
            set { _floatPropertyStatic = value; }
        }
        #endregion

        #region Methods
        public void Setup()
        {
            //Instance Propertys Setup
            BooleanProperty = true;
            ByteProperty = 1;
            SByteProperty = 2;
            IntMaxProperty = int.MaxValue;
            IntMinProperty = int.MinValue;
            LongMaxProperty = long.MaxValue;
            LongMinProperty = long.MinValue;
            Int16Property = -3;
            UInt16Property = 4;
            Int32Property = -5;
            UInt32Property = 6;
            Int64Property = -7;
            UInt64Property = 8;
            CharProperty = '\0';
            DoubleProperty = 1 / 3;
            FloatProperty = 1 / 3;

            //Static Propertys Setup
            BooleanPropertyStatic = true;
            BytePropertyStatic = 1;
            SBytePropertyStatic = 2;
            IntMaxPropertyStatic = int.MaxValue;
            IntMinPropertyStatic = int.MinValue;
            LongMaxPropertyStatic = long.MaxValue;
            LongMinPropertyStatic = long.MinValue;
            Int16PropertyStatic = -3;
            UInt16PropertyStatic = 4;
            Int32PropertyStatic = -5;
            UInt32PropertyStatic = 6;
            Int64PropertyStatic = -7;
            UInt64PropertyStatic = 8;
            CharPropertyStatic = '\0';
            DoublePropertyStatic = 1 / 3;
            FloatPropertyStatic = 1 / 3;
        }
        #endregion
    }
}
