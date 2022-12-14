using System;
using System.Collections.Generic;
using System.Text;
using static Scada.BatchCommand.PropertyBag;

namespace Scada.BatchCommand
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class PropertySpec
    {
        #region Fields
        private Attribute[] attributes;
        private string category;
        private object defaultValue;
        private string description;
        private string editor;
        private string name;
        private string type;
        private string typeConverter;
        private PropertySpecCollection mSubPropertys;
        /// <summary>
        /// 实现带子属性
        /// </summary>
        public PropertySpecCollection SubPropertys
        {
            set { mSubPropertys = value; }
            get { return mSubPropertys; }

        }
        #endregion

        #region Properties
        /// <summary>
        ///或得到额外的集合（collection属性这个性质。这可以

        /// specify属性是用来超越的那些支持intrinsically

        ///propertyspec类，如只读和浏览。
        /// </summary>
        public Attribute[] Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        /// <summary>
        /// Gets or sets the category name of this property.
        /// </summary>
        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        /// <summary>
        /// Gets or sets the fully qualified name of the type converter
        /// type for this property.
        /// </summary>
        public string ConverterTypeName
        {
            get { return typeConverter; }
            set { typeConverter = value; }
        }

        /// <summary>
        /// Gets or sets the default value of this property.
        /// </summary>
        public object DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        /// <summary>
        /// Gets or sets the help text description of this property.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Gets or sets the fully qualified name of the editor type for
        /// this property.
        /// </summary>
        public string EditorTypeName
        {
            get { return editor; }
            set { editor = value; }
        }
        public bool readOnly=false;
        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }
        /// <summary>
        /// Gets or sets the name of this property.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the fully qualfied name of the type of this
        /// property.
        /// </summary>
        public string TypeName
        {
            get { return type; }
            set { type = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the PropertySpec class.
        /// </summary>
        /// <param name="name">The name of the property displayed in the property grid.</param>
        /// <param name="type">The fully qualified name of the type of the property.</param>
        public PropertySpec(string name, string type) : this(name, type, null, null, null) { }

        /// <summary>
        /// Initializes a new instance of the PropertySpec class.
        /// </summary>
        /// <param name="name">The name of the property displayed in the property grid.</param>
        /// <param name="type">A Type that represents the type of the property.</param>
        public PropertySpec(string name, Type type)
            :
            this(name, type.AssemblyQualifiedName, null, null, null) { }

        /// <summary>
        /// Initializes a new instance of the PropertySpec class.
        /// </summary>
        /// <param name="name">The name of the property displayed in the property grid.</param>
        /// <param name="type">The fully qualified name of the type of the property.</param>
        /// <param name="category">The category under which the property is displayed in the
        /// property grid.</param>
        public PropertySpec(string name, string type, string category) : this(name, type, category, null, null) { }

        /// <summary>
        /// Initializes a new instance of the PropertySpec class.
        /// </summary>
        /// <param name="name">The name of the property displayed in the property grid.</param>
        /// <param name="type">A Type that represents the type of the property.</param>
        /// <param name="category"></param>
        public PropertySpec(string name, Type type, string category)
            :
            this(name, type.AssemblyQualifiedName, category, null, null) { }

        /// <summary>
        /// Initializes a new instance of the PropertySpec class.
        /// </summary>
        /// <param name="name">The name of the property displayed in the property grid.</param>
        /// <param name="type">The fully qualified name of the type of the property.</param>
        /// <param name="category">The category under which the property is displayed in the
        /// property grid.</param>
        /// <param name="description">A string that is displayed in the help area of the
        /// property grid.</param>
        public PropertySpec(string name, string type, string category, string description)
            :
            this(name, type, category, description, null) { }

        /// <summary>
        /// Initializes a new instance of the PropertySpec class.
        /// </summary>
        /// <param name="name">The name of the property displayed in the property grid.</param>
        /// <param name="type">A Type that represents the type of the property.</param>
        /// <param name="category">The category under which the property is displayed in the
        /// property grid.</param>
        /// <param name="description">A string that is displayed in the help area of the
        /// property grid.</param>
        public PropertySpec(string name, Type type, string category, string description)
            :
            this(name, type.AssemblyQualifiedName, category, description, null) { }

        /// <summary>
        /// Initializes a new instance of the PropertySpec class.
        /// </summary>
        /// <param name="name">The name of the property displayed in the property grid.</param>
        /// <param name="type">The fully qualified name of the type of the property.</param>
        /// <param name="category">The category under which the property is displayed in the
        /// property grid.</param>
        /// <param name="description">A string that is displayed in the help area of the
        /// property grid.</param>
        /// <param name="defaultValue">The default value of the property, or null if there is
        /// no default value.</param>
        public PropertySpec(string name, string type, string category, string description, object defaultValue)
        {
            this.name = name;
            this.type = type;
            this.category = category;
            this.description = description;
            this.defaultValue = defaultValue;
            this.attributes = null;
            this.readOnly = false;
        }
        public PropertySpec(string name, string type, string category, string description, object defaultValue,bool readOnly)
        {
            this.name = name;
            this.type = type;
            this.category = category;
            this.description = description;
            this.defaultValue = defaultValue;
            this.attributes = null;
            this.readOnly = readOnly;
        }
        /// <summary>
        /// Initializes a new instance of the PropertySpec class.
        /// </summary>
        /// <param name="name">The name of the property displayed in the property grid.</param>
        /// <param name="type">A Type that represents the type of the property.</param>
        /// <param name="category">The category under which the property is displayed in the
        /// property grid.</param>
        /// <param name="description">A string that is displayed in the help area of the
        /// property grid.</param>
        /// <param name="defaultValue">The default value of the property, or null if there is
        /// no default value.</param>
        public PropertySpec(string name, Type type, string category, string description, object defaultValue)
            :
            this(name, type.AssemblyQualifiedName, category, description, defaultValue) { }
        public PropertySpec(string name, Type type, string category, string description, object defaultValue,bool readOnly)
          :
          this(name, type.AssemblyQualifiedName, category, description, defaultValue, readOnly)
        { }

        /// <summary>
        /// Initializes a new instance of the PropertySpec class.
        /// </summary>
        /// <param name="name">The name of the property displayed in the property grid.</param>
        /// <param name="type">The fully qualified name of the type of the property.</param>
        /// <param name="category">The category under which the property is displayed in the
        /// property grid.</param>
        /// <param name="description">A string that is displayed in the help area of the
        /// property grid.</param>
        /// <param name="defaultValue">The default value of the property, or null if there is
        /// no default value.</param>
        /// <param name="editor">The fully qualified name of the type of the editor for this
        /// property.  This type must derive from UITypeEditor.</param>
        /// <param name="typeConverter">The fully qualified name of the type of the type
        /// converter for this property.  This type must derive from TypeConverter.</param>
        public PropertySpec(string name, string type, string category, string description, object defaultValue,
            string editor, string typeConverter)
            : this(name, type, category, description, defaultValue)
        {
            this.editor = editor;
            this.typeConverter = typeConverter;
        }

        /// <summary>
        /// Initializes a new instance of the PropertySpec class.
        /// </summary>
        /// <param name="name">The name of the property displayed in the property grid.</param>
        /// <param name="type">A Type that represents the type of the property.</param>
        /// <param name="category">The category under which the property is displayed in the
        /// property grid.</param>
        /// <param name="description">A string that is displayed in the help area of the
        /// property grid.</param>
        /// <param name="defaultValue">The default value of the property, or null if there is
        /// no default value.</param>
        /// <param name="editor">The fully qualified name of the type of the editor for this
        /// property.  This type must derive from UITypeEditor.</param>
        /// <param name="typeConverter">The fully qualified name of the type of the type
        /// converter for this property.  This type must derive from TypeConverter.</param>
        public PropertySpec(string name, Type type, string category, string description, object defaultValue,
            string editor, string typeConverter)
            :
            this(name, type.AssemblyQualifiedName, category, description, defaultValue, editor, typeConverter) { }

        /// <summary>
        /// Initializes a new instance of the PropertySpec class.
        /// </summary>
        /// <param name="name">The name of the property displayed in the property grid.</param>
        /// <param name="type">The fully qualified name of the type of the property.</param>
        /// <param name="category">The category under which the property is displayed in the
        /// property grid.</param>
        /// <param name="description">A string that is displayed in the help area of the
        /// property grid.</param>
        /// <param name="defaultValue">The default value of the property, or null if there is
        /// no default value.</param>
        /// <param name="editor">The Type that represents the type of the editor for this
        /// property.  This type must derive from UITypeEditor.</param>
        /// <param name="typeConverter">The fully qualified name of the type of the type
        /// converter for this property.  This type must derive from TypeConverter.</param>
        public PropertySpec(string name, string type, string category, string description, object defaultValue,
            Type editor, string typeConverter)
            :
            this(name, type, category, description, defaultValue, editor.AssemblyQualifiedName,
            typeConverter) { }

        /// <summary>
        /// Initializes a new instance of the PropertySpec class.
        /// </summary>
        /// <param name="name">The name of the property displayed in the property grid.</param>
        /// <param name="type">A Type that represents the type of the property.</param>
        /// <param name="category">The category under which the property is displayed in the
        /// property grid.</param>
        /// <param name="description">A string that is displayed in the help area of the
        /// property grid.</param>
        /// <param name="defaultValue">The default value of the property, or null if there is
        /// no default value.</param>
        /// <param name="editor">The Type that represents the type of the editor for this
        /// property.  This type must derive from UITypeEditor.</param>
        /// <param name="typeConverter">The fully qualified name of the type of the type
        /// converter for this property.  This type must derive from TypeConverter.</param>
        public PropertySpec(string name, Type type, string category, string description, object defaultValue,
            Type editor, string typeConverter)
            :
            this(name, type.AssemblyQualifiedName, category, description, defaultValue,
            editor.AssemblyQualifiedName, typeConverter) { }

        /// <summary>
        /// Initializes a new instance of the PropertySpec class.
        /// </summary>
        /// <param name="name">The name of the property displayed in the property grid.</param>
        /// <param name="type">The fully qualified name of the type of the property.</param>
        /// <param name="category">The category under which the property is displayed in the
        /// property grid.</param>
        /// <param name="description">A string that is displayed in the help area of the
        /// property grid.</param>
        /// <param name="defaultValue">The default value of the property, or null if there is
        /// no default value.</param>
        /// <param name="editor">The fully qualified name of the type of the editor for this
        /// property.  This type must derive from UITypeEditor.</param>
        /// <param name="typeConverter">The Type that represents the type of the type
        /// converter for this property.  This type must derive from TypeConverter.</param>
        public PropertySpec(string name, string type, string category, string description, object defaultValue,
            string editor, Type typeConverter)
            :
            this(name, type, category, description, defaultValue, editor, typeConverter.AssemblyQualifiedName) { }

        /// <summary>
        /// Initializes a new instance of the PropertySpec class.
        /// </summary>
        /// <param name="name">The name of the property displayed in the property grid.</param>
        /// <param name="type">A Type that represents the type of the property.</param>
        /// <param name="category">The category under which the property is displayed in the
        /// property grid.</param>
        /// <param name="description">A string that is displayed in the help area of the
        /// property grid.</param>
        /// <param name="defaultValue">The default value of the property, or null if there is
        /// no default value.</param>
        /// <param name="editor">The fully qualified name of the type of the editor for this
        /// property.  This type must derive from UITypeEditor.</param>
        /// <param name="typeConverter">The Type that represents the type of the type
        /// converter for this property.  This type must derive from TypeConverter.</param>
        public PropertySpec(string name, Type type, string category, string description, object defaultValue,
            string editor, Type typeConverter)
            :
            this(name, type.AssemblyQualifiedName, category, description, defaultValue, editor,
            typeConverter.AssemblyQualifiedName) { }

        /// <summary>
        /// Initializes a new instance of the PropertySpec class.
        /// </summary>
        /// <param name="name">The name of the property displayed in the property grid.</param>
        /// <param name="type">The fully qualified name of the type of the property.</param>
        /// <param name="category">The category under which the property is displayed in the
        /// property grid.</param>
        /// <param name="description">A string that is displayed in the help area of the
        /// property grid.</param>
        /// <param name="defaultValue">The default value of the property, or null if there is
        /// no default value.</param>
        /// <param name="editor">The Type that represents the type of the editor for this
        /// property.  This type must derive from UITypeEditor.</param>
        /// <param name="typeConverter">The Type that represents the type of the type
        /// converter for this property.  This type must derive from TypeConverter.</param>
        public PropertySpec(string name, string type, string category, string description, object defaultValue,
            Type editor, Type typeConverter)
            :
            this(name, type, category, description, defaultValue, editor.AssemblyQualifiedName,
            typeConverter.AssemblyQualifiedName) { }

        /// <summary>
        /// Initializes a new instance of the PropertySpec class.
        /// </summary>
        /// <param name="name">The name of the property displayed in the property grid.</param>
        /// <param name="type">A Type that represents the type of the property.</param>
        /// <param name="category">The category under which the property is displayed in the
        /// property grid.</param>
        /// <param name="description">A string that is displayed in the help area of the
        /// property grid.</param>
        /// <param name="defaultValue">The default value of the property, or null if there is
        /// no default value.</param>
        /// <param name="editor">The Type that represents the type of the editor for this
        /// property.  This type must derive from UITypeEditor.</param>
        /// <param name="typeConverter">The Type that represents the type of the type
        /// converter for this property.  This type must derive from TypeConverter.</param>
        public PropertySpec(string name, Type type, string category, string description, object defaultValue,
            Type editor, Type typeConverter)
            :
            this(name, type.AssemblyQualifiedName, category, description, defaultValue,
            editor.AssemblyQualifiedName, typeConverter.AssemblyQualifiedName) { }


        #endregion
    }
}
