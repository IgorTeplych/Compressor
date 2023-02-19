using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Compressor.ViewModel
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class LocDescriptionAttribute : DescriptionAttribute
    {
        #region Public methods.
        // ------------------------------------------------------------------

        /// 
        /// Initializes a new instance of the 
        ///  class.
        /// 
        /// Property Name
        /// Type of the resources.
        public LocDescriptionAttribute(string propertyName, Type resourcesType)
          : base(propertyName)
        {
            _resProp = resourcesType.GetProperty(propertyName);
            if (_resProp == null)
                throw new ArgumentException("Property not found", propertyName);
            MethodInfo mi = _resProp.GetGetMethod();
            if (mi == null)
                throw new ArgumentException("Property has no getter", propertyName);
            if (!mi.IsStatic)
                throw new ArgumentException("Expected static property", propertyName);
        }

        #endregion

        #region Public properties.

        /// 
        /// Get the string value from the resources.
        /// 
        /// 
        /// The description stored in this attribute.
        public override string Description
        {
            get
            {
                return _resProp.GetValue(null, null).ToString();
            }
        }
        #endregion

        #region Private variables.

        private readonly PropertyInfo _resProp;

        #endregion
    }
}
