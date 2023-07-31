using Asphalt.Controls.Theme;
using System.ComponentModel;
using System.Drawing.Design;


namespace Asphalt.Controls
{
    [Editor(typeof(UIGenericCopyPaste), typeof(UITypeEditor))]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AsphaltScrollerShading : INotifyPropertyChanged, IPropertyEvents
    {
        private Shader _PanelShader   = new Shader(Scheme.Base,  Pigment.Primary,    Shade.Darker);
        private Shader _ScrollShader  = new Shader(Scheme.KeyD,  Pigment.Primary,    Shade.Default);
        private Shader _BarShader     = new Shader(Scheme.KeyD,  Pigment.Primary,    Shade.Lighter);
        private Shader _BarOnShader   = new Shader(Scheme.KeyD,  Pigment.Analogous,  Shade.Default);
        private Shader _TextShader    = new Shader(Scheme.Panel, Pigment.Foreground, Shade.Default);

        public override string ToString()
        {
            return "Scroller Shading";
        }


        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public Shader PanelShader
        {
            get { return _PanelShader; }
            set { Prop.Exchange(ref _PanelShader, value, this); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public Shader ScrollShader
        {
            get { return _ScrollShader; }
            set { Prop.Exchange(ref _ScrollShader, value, this); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public Shader BarShader
        {
            get { return _BarShader; }
            set { Prop.Exchange(ref _BarShader, value, this); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public Shader BarOnShader
        {
            get { return _BarOnShader; }
            set { Prop.Exchange(ref _BarOnShader, value, this); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public Shader TextShader
        {
            get { return _TextShader; }
            set { Prop.Exchange(ref _TextShader, value, this); }
        }

        #region Events

        void IPropertyEvents.OnChange(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
