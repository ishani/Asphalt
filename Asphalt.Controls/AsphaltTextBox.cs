using Asphalt.Controls.Theme;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asphalt.Controls
{
    [ToolboxItem(true)]
    public class AsphaltSimpleText : TextBox, IPropertyEvents
    {
        private AsphaltPanelShading      _Shading = new AsphaltPanelShading();
        private FontChoice              _TextFont = FontChoice.Regular;
        private FontSize                _TextSize = FontSize.Smaller;

        #region Properties_Shading

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Asphalt.Shading")]
        public AsphaltPanelShading PanelShading
        {
            get { return _Shading; }
            set { Prop.Exchange(ref _Shading, value, this); }
        }

        #endregion

        #region Text 

        [DefaultValue(FontChoice.Regular)]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Asphalt.Fonts")]
        public FontChoice TextFont
        {
            get { return _TextFont; }
            set { _TextFont = value; Prop.TriggerChange(this); }
        }

        [DefaultValue(FontSize.Smaller)]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Asphalt.Fonts")]
        public FontSize TextSize
        {
            get { return _TextSize; }
            set { _TextSize = value; Prop.TriggerChange(this); }
        }

        #endregion

        #region Update_Events

        void IPropertyEvents.OnChange(object sender, PropertyChangedEventArgs e)
        {
            base.BackColor = Current.Instance.Color(_Shading.PanelShader);
            base.ForeColor = Current.Instance.Color(_Shading.TextShader);
            base.Font      = FontLibrary.Instance.Get(_TextFont, _TextSize);

            Invalidate();
            Update();
        }

        #endregion


        #region HideOverridenBaseProperties

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Color BackColor { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Color ForeColor { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Font Font { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new BorderStyle BorderStyle { get; set; }

        #endregion


        public AsphaltSimpleText()
        {

            Prop.TriggerChange(this);

            base.BorderStyle = BorderStyle.None;
        }
    }
}
