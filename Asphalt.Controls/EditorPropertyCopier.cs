using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Drawing.Design;
using System.Windows.Forms.Design;

/* 
 * [Editor( typeof( UIGenericCopyPaste ), typeof( UITypeEditor ) )]
 * 
 * Custom type editor that provides a drop-down with Copy and Paste buttons that use XML serialization to allow large, composite
 * properties to be easily moved around (or even edited in an external text editor).
 * 
 * As far as I know, the winforms editor doesn't support generic Ctrl-C/V on all properties, so we have to do bespoke shit like this.
 * 
 */

namespace Asphalt.Controls
{
    public class UIGenericCopyPaste : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context )
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
        {
            var feService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if ( feService == null )
                return null;

            var propertyEditor = new EditorPropertyCopier(feService, value, context.PropertyDescriptor.PropertyType);
            feService.DropDownControl( propertyEditor );

            return propertyEditor.GetResult();
        }
    }

    // tiny little box with two buttons on it; [copy] / [paste]
    //
    [ToolboxItem( false )]
    public partial class EditorPropertyCopier : UserControl
    {
        private IWindowsFormsEditorService _EditorService;
        private object                       _StoredValue = null;
        private object                     _IncomingValue = null;
        private string           _SerializedIncomingValue;

        public EditorPropertyCopier( IWindowsFormsEditorService service, object value, Type valueType )
        {
            InitializeComponent();

            _EditorService = service;

            var ioXML = new XmlSerializer(valueType);

            // can we definitely serialize this thing to XML? multiple selections won't work as the value passed in is null - 
            // but we can certainly Paste into a multiple selection
            try
            {
                _StoredValue = value;

                var serializeBuffer = new StringBuilder();
                using ( TextWriter writer = new StringWriter( serializeBuffer ) )
                {
                    ioXML.Serialize( writer, _StoredValue );
                }

                // stash the serialized data to push into the clipboard if required
                _SerializedIncomingValue = serializeBuffer.ToString();
            }
            catch
            {
                _StoredValue = null;
            }

            // couldn't do it? disable copying
            CopyButton.Enabled = ( _StoredValue != null );
            if ( !CopyButton.Enabled )
                CopyButton.Text = " - - Cannot Copy - - ";

            // ... and now for pasting - try deserializing whatever is in the clipboard and see if it succeeds
            // if so, we have a value we could return as a new item
            try
            {
                if ( Clipboard.ContainsText() )
                {
                    string PotentialIncomingSerialized = Clipboard.GetText();

                    using ( TextReader reader = new StringReader( PotentialIncomingSerialized ) )
                    {
                        _IncomingValue = ioXML.Deserialize( reader );
                    }
                }
            }
            catch
            {
                _IncomingValue = null;
            }

            // or not
            PasteButton.Enabled = ( _IncomingValue != null );
            if ( !PasteButton.Enabled )
                PasteButton.Text = " - - Cannot Paste - - ";
        }

        public object GetResult()
        {
            return _StoredValue;
        }

        // Copy moves the XML into the clipboard as [TEXT] and kills the drop-down
        private void CopyButton_Click( object sender, EventArgs e )
        {
            Clipboard.SetText( _SerializedIncomingValue );
            _EditorService.CloseDropDown();
        }

        // Paste just returns the safely-deserialized object we built above as the new value to use
        private void PasteButton_Click( object sender, EventArgs e )
        {
            _StoredValue = _IncomingValue;
            _EditorService.CloseDropDown();
        }
    }
}
