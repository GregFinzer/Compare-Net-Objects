using System;
using System.Drawing;


namespace KellermanSoftware.CompareNetObjects.TypeComparers
{

    /// <summary>
    /// Class FontDescriptorComparer.
    /// </summary>
    public class FontComparer : BaseTypeComparer
    {
        /// <summary>
        /// Protected constructor that references the root comparer
        /// </summary>
        /// <param name="rootComparer">The root comparer.</param>
        public FontComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        /// <summary>
        /// If true the type comparer will handle the comparison for the type
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns><c>true</c> if [is type match] [the specified type1]; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsFont(type1) && TypeHelper.IsFont(type2);
        }

        /// <summary>
        /// Compare the two fonts
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            Font font1 = parms.Object1 as Font;
            Font font2 = parms.Object2 as Font;

            if (font1 == null || font2 == null)
                return;

            CompareProp(parms, font1.Bold, font2.Bold, "Bold");
            CompareProp(parms, font1.FontFamily.Name, font2.FontFamily.Name, "FontFamily.Name");
            CompareProp(parms, font1.OriginalFontName, font2.OriginalFontName, "OriginalFontName");
            CompareProp(parms, font1.Size, font2.Size, "Size");
            CompareProp(parms, font1.SizeInPoints, font2.SizeInPoints, "SizeInPoints");
            CompareProp(parms, font1.Strikeout, font2.Strikeout, "Strikeout");
            CompareProp(parms, font1.Style, font2.Style, "Style");
            CompareProp(parms, font1.SystemFontName, font2.SystemFontName, "SystemFontName");
            CompareProp(parms, font1.Underline, font2.Underline, "Underline");
            CompareProp(parms, font1.Unit, font2.Unit, "Unit");
        }

        private void CompareProp(CompareParms parms, object prop1, object prop2, string propName)
        {
            string currentBreadCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, propName);

            CompareParms childParms = new CompareParms
            {
                Result = parms.Result,
                Config = parms.Config,
                ParentObject1 = parms.Object1,
                ParentObject2 = parms.Object2,
                Object1 = prop1,
                Object2 = prop2,
                BreadCrumb = currentBreadCrumb
            };

            RootComparer.Compare(childParms);
        }
    }
}
