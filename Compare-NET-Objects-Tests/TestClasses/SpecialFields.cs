#region Includes
using System;
#endregion

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    [Serializable]
    public class SpecialFields
    {
        #region Class Variables
        //String
        public string StringField = string.Empty;
        public string StringFieldNull = string.Empty;
        public static string StringFieldStatic = string.Empty;
        public static string StringFieldStaticNull = string.Empty;
        private string _stringProperty = string.Empty;

        //DateTime
        public DateTime DateTimeField = DateTime.Today;
        public DateTime? DateTimeFieldNull = DateTime.Today;
        public static DateTime DateTimeFieldStatic = DateTime.Today;
        public static DateTime? DateTimeFieldStaticNull = DateTime.Today;
        private DateTime _dateTimeProperty = DateTime.Today;

        //decimal
        public decimal DecimalField;
        public decimal? DecimalFieldNull = 0;
        public static decimal DecimalFieldStatic;
        public static decimal? DecimalFieldStaticNull = 0;
        private decimal _decimalProperty;

        //Guid
        public Guid GuidField = Guid.NewGuid();
        public Guid? GuidFieldNull =  Guid.NewGuid();
        public static Guid GuidFieldStatic = Guid.NewGuid();
        public static Guid? GuidFieldStaticNull =  Guid.NewGuid();
        private Guid _guidProperty = Guid.NewGuid();

        //TimeSpan
        public TimeSpan TimeSpanField = new TimeSpan(0);
        public TimeSpan? TimeSpanFieldNull = new TimeSpan(0);
        public static TimeSpan TimeSpanFieldStatic = new TimeSpan(0);
        public static TimeSpan? TimeSpanFieldStaticNull = new TimeSpan(0);
        private TimeSpan _timeSpanProperty = new TimeSpan(0);
        #endregion

        #region Properties
        /// <summary>
        /// Property Get/Set for String Property
        /// </summary>
        public string StringProperty
        {
            get { return _stringProperty; }
            set { _stringProperty = value; }
        }

        /// <summary>
        /// Property Get/Set for Date Time Property
        /// </summary>
        public DateTime DateTimeProperty
        {
            get { return _dateTimeProperty; }
            set { _dateTimeProperty = value; }
        }

        /// <summary>
        /// Property Get/Set for Decimal Property
        /// </summary>
        public decimal DecimalProperty
        {
            get { return _decimalProperty; }
            set { _decimalProperty = value; }
        }

        /// <summary>
        /// Property Get/Set for Guid Property
        /// </summary>
        public Guid GuidProperty
        {
            get { return _guidProperty; }
            set { _guidProperty = value; }
        }

        /// <summary>
        /// Property Get/Set for Time Span Property
        /// </summary>
        public TimeSpan TimeSpanProperty
        {
            get { return _timeSpanProperty; }
            set { _timeSpanProperty = value; }
        }
        #endregion

        #region Methods
        public void Setup()
        {
            //String
            StringField = "Greg";
            StringFieldNull = null;
            StringFieldStatic = "Greg";
            StringFieldStaticNull = null;

            //DateTime
            DateTimeField = new DateTime(2009, 5, 29, 23, 21, 50);
            DateTimeFieldNull = null;
            DateTimeFieldStatic = new DateTime(2009, 5, 29, 23, 21, 50);
            DateTimeFieldStaticNull = null;

            //decimal
            DecimalField = 1 / 3;
            DecimalFieldNull = null;
            DecimalFieldStatic = 1 / 3;
            DecimalFieldStaticNull = null;

            //Guid
            GuidField = Guid.NewGuid();
            GuidFieldNull = null;
            GuidFieldStatic = Guid.NewGuid();
            GuidFieldStaticNull = null;

            //TimeSpan
            TimeSpanField = DateTime.Now.AddMinutes(91) - DateTime.Now;
            TimeSpanFieldNull = null;
            TimeSpanFieldStatic = DateTime.Now.AddMinutes(92) - DateTime.Now;
            TimeSpanFieldStaticNull = null;
        }
        #endregion

    }
}
