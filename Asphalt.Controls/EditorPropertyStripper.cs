using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;


namespace Asphalt.Controls
{
    public static class PropertiesToRemove
    {
        internal static readonly List<string> NameList = new List<string>() {
            "BackColor",
            "BackgroundImage",
            "BackgroundImageLayout",
            "BorderStyle",
            "Font",
            "ForeColor",
            "RightToLeft"
        };

        public static void StripFromDictionary( ref IDictionary properties )
        {
            foreach ( string Name in NameList )
            {
                properties.Remove( Name );
            }
        }
    }

    /*
     * assigned inside [Designer] attribute to our controls so that these default low-level parameters are automatically hidden.
     * all of Asphalt's styling overrides these things so we don't want them hanging around being confusing.

    [PermissionSet( SecurityAction.Demand, Name = "FullTrust" )]
    public class ParentControlWithStrippedProperies : ParentControlDesigner
    {
        protected override void PreFilterProperties( IDictionary properties )
        {
            base.PreFilterProperties( properties );
            PropertiesToRemove.StripFromDictionary( ref properties );
        }
    }

     */

    /*
     * can alternatively inherit from this instead of Control to obfuscate and hide those standard properties,
     * triggering Obsolete warnings if they are accessed from code too
     * 
     */
    [ToolboxItem( false )]
    public class ControlWithoutDefaultAppearances : Control
    {
        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never ), Obsolete( "Unused", true )]
        public new Font Font { get; set; }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never ), Obsolete( "Unused", true )]
        public new Color BackColor { get; set; }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never ), Obsolete( "Unused", true )]
        public new Image BackgroundImage { get; set; }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never ), Obsolete( "Unused", true )]
        public new ImageLayout BackgroundImageLayout { get; set; } = ImageLayout.Tile;

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never ), Obsolete( "Unused", true )]
        public new Color ForeColor { get; set; }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never ), Obsolete( "Unused", true )]
        public new Cursor Cursor { get; set; }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never ), Obsolete( "Unused", true )]
        public new RightToLeft RightToLeft { get; set; }
    }

    [ToolboxItem(false)]
    public abstract class ControlWithNewRendering : ControlWithoutDefaultAppearances
    {
        internal abstract void Repaint();
        internal abstract void RenderBG( Graphics gfx );
        internal abstract void Render( Graphics gfx );
        
        internal void SetDefaultControlStyles()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint |
                ControlStyles.Opaque, true );
        }

        internal Bitmap _CachedDisabledImage = null;

        internal void DiscardCachedDisabledImage()
        {
            if ( _CachedDisabledImage != null )
            {
                _CachedDisabledImage.Dispose();
                _CachedDisabledImage = null;
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            DiscardCachedDisabledImage();
            Repaint();
        }


        protected override void OnPaint( PaintEventArgs e )
        {
            RenderBG( e.Graphics );

            if ( !Enabled )
            {
                if ( _CachedDisabledImage        == null   ||
                     _CachedDisabledImage.Width  != Width  ||
                     _CachedDisabledImage.Height != Height )
                {
                    DiscardCachedDisabledImage();

                    _CachedDisabledImage = new Bitmap( Width, Height );
                    using ( Graphics gfx = Graphics.FromImage( _CachedDisabledImage ) )
                    {
                        Render( gfx );
                    }
                }

                using ( ImageAttributes attributes = new ImageAttributes() )
                {
                    attributes.SetColorMatrix( Colour.ColourBase.DisabledColorMatrix );
                    e.Graphics.DrawImage( _CachedDisabledImage, ClientRectangle, 0, 0, Width, Height, GraphicsUnit.Pixel, attributes );
                }
            }
            else
            {
                Render( e.Graphics );
            }
        }

    }
}
