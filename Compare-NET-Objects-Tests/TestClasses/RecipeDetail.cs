using System;
using System.Collections.Generic;
using System.Text;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class RecipeDetail
    {
        private bool _isDirty = false;

        public bool IsNew { get; set; }
        public bool IsDirty 
        {
            get { return _isDirty; }
        }

        public string Ingredient { get; set; }

        private string SecretIngredient 
        {
            get;
            set;
        }

        public RecipeDetail(bool isDirty, string secretIngredient)
        {
            _isDirty = isDirty;
            SecretIngredient = secretIngredient;
        }
    }
}
