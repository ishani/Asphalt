using System.ComponentModel;

using Asphalt.Controls.Theme;

namespace Asphalt.Controls
{
    [TypeConverter( typeof( ExpandableObjectConverter ) )]
    public partial class AsphaltPanelShading : INotifyPropertyChanged, IPropertyEvents
    {
        private Shader             _TextShader = new Shader( Scheme.Panel, Pigment.Foreground, Shade.Default );
        private Shader             _IconShader = new Shader( Scheme.Panel, Pigment.Foreground, Shade.Default );
        private Shader        _AdornmentShader = new Shader( Scheme.KeyA,  Pigment.Primary, Shade.Default );
        private Shader            _PanelShader = new Shader( Scheme.Panel, Pigment.Primary, Shade.Default );
        private Shader             _EdgeShader = new Shader( Scheme.KeyB,  Pigment.Primary, Shade.Default );

        public override string ToString()
        {
            return "Panel Shading";
        }


        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always )]
        public Shader TextShader
        {
            get => _TextShader;
            set { Prop.Exchange( ref _TextShader, value, this); }
        }

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always )]
        public Shader IconShader
        {
            get => _IconShader; 
            set { Prop.Exchange( ref _IconShader, value, this); }
        }

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always )]
        public Shader AdornmentShader
        {
            get => _AdornmentShader; 
            set { Prop.Exchange( ref _AdornmentShader, value, this); }
        }

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always )]
        public Shader PanelShader
        {
            get => _PanelShader; 
            set { Prop.Exchange( ref _PanelShader, value, this); }
        }

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always )]
        public Shader EdgeShader
        {
            get => _EdgeShader; 
            set { Prop.Exchange( ref _EdgeShader, value, this ); }
        }


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
    public partial class AsphaltPanelShading
    {
    }

#endif // ASPHALT_DESIGN
}
