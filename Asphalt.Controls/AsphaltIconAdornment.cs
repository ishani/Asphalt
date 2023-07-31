using System.ComponentModel;
using System.Drawing;

using Asphalt.Controls.Theme;

namespace Asphalt.Controls
{
    [TypeConverter( typeof( ExpandableObjectConverter ) )]
    public partial class AsphaltIconAdornment : INotifyPropertyChanged, IPropertyEvents
    {
        public enum ShapeChoice
        {
            None,
            Circle,
            Square
        }

        private ShapeChoice      _Shape = ShapeChoice.None;
        private float             _Size = 1.2f;
        private float       _ShadowSize = 4.0f;
        private SizeF           _Offset = new SizeF(0,0);

        public override string ToString()
        {
            return "Icon Adornment";
        }

        #region Properties

        [DefaultValue( ShapeChoice.None )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always )]
        public ShapeChoice Shape
        {
            get => _Shape;
            set { _Shape = value; (this as IPropertyEvents).OnChange(this, new PropertyChangedEventArgs(nameof(Shape))); }
        }

        [DefaultValue( 1.2f )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always )]
        public float Size
        {
            get => _Size; 
            set { _Size = value; (this as IPropertyEvents).OnChange(this, new PropertyChangedEventArgs(nameof(Shape))); }
        }

        [DefaultValue( 4.0f )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always )]
        public float ShadowSize
        {
            get => _ShadowSize; 
            set { _ShadowSize = value; (this as IPropertyEvents).OnChange(this, new PropertyChangedEventArgs(nameof(Shape))); }
        }

        [DefaultValue( typeof(SizeF), "0, 0" )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always )]
        public SizeF Offset
        {
            get => _Offset; 
            set { _Offset = value; (this as IPropertyEvents).OnChange(this, new PropertyChangedEventArgs(nameof(Shape))); }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        void IPropertyEvents.OnChange(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }

        #endregion
    }

#if ASPHALT_DESIGN
    
    [Editor( typeof( UIGenericCopyPaste ), typeof( System.Drawing.Design.UITypeEditor ) )]
    public partial class AsphaltIconAdornment
    {
    }

#endif // ASPHALT_DESIGN
}
