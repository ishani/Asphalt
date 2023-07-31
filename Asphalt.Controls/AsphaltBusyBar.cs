using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Asphalt.Controls.Colour;
using Asphalt.Controls.Theme;

namespace Asphalt.Controls
{
    [ToolboxItem( true )]
    public class AsphaltBusyBar : ControlWithoutDefaultAppearances, IPropertyEvents
    {
        private Shader          _InactiveShader = new Shader( Scheme.Panel, Pigment.Primary, Shade.Default );
        private Shader            _ActiveShader = new Shader( Scheme.KeyD, Pigment.Primary, Shade.Lighter );
        private Shader               _BarShader = new Shader( Scheme.KeyD, Pigment.Analogous, Shade.Lighter );

        private bool                 _Activated = false;


        private Timer              _RedrawTimer = new Timer();
        private int                 _BusyTicker = 0;

        private float       _BarActivationValue = 0.0f;

        private HSV                _InactiveHSV = new HSV();
        private HSV                  _ActiveHSV = new HSV();
        private HSV                     _BarHSV = new HSV();


        #region Properties

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt" )]
        public Shader InactiveShader
        {
            get { return _InactiveShader; }
            set { Prop.Exchange( ref _InactiveShader, value, this ); }
        }

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt" )]
        public Shader ActiveShader
        {
            get { return _ActiveShader; }
            set { Prop.Exchange( ref _ActiveShader, value, this ); }
        }

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt" )]
        public Shader BarShader
        {
            get { return _BarShader; }
            set { Prop.Exchange( ref _BarShader, value, this ); }
        }

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt" )]
        public bool Activated
        {
            get { return _Activated; }
            set { _Activated = value; _RedrawTimer.Start(); }
        }

        void IPropertyEvents.OnChange( object sender, PropertyChangedEventArgs e )
        {
            UpdateCachedHSVs();
            Repaint();
        }

        #endregion


        public AsphaltBusyBar()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint |
                ControlStyles.Opaque, true );

            // force a reassignment using the properties so that the shaders have the OnChanged callbacks attached
            InactiveShader  = _InactiveShader;
            ActiveShader    = _ActiveShader;
            BarShader       = _BarShader;

            UpdateCachedHSVs();

            Name = "AsphaltBusyBar";
            Size = new System.Drawing.Size( 320, 15 );

            _RedrawTimer.Tick += RedrawTimer_Tick;
            _RedrawTimer.Interval = 33;
        }

        // convert the shaders to HSV, ready to interp during OnPaint
        private void UpdateCachedHSVs()
        {
            _InactiveHSV = new HSV( Current.Instance.Color( _InactiveShader ) );
            _ActiveHSV   = new HSV( Current.Instance.Color( _ActiveShader ) );
            _BarHSV      = new HSV( Current.Instance.Color( _BarShader ) );
        }

        protected override void OnParentChanged( EventArgs e )
        {
            base.OnParentChanged( e );

            if ( Activated )
            {
                _BarActivationValue = 1.0f;
                _RedrawTimer.Start();
            }
        }

        private void RedrawTimer_Tick( object sender, EventArgs e )
        {
            // wrap the ticker around per bar-block
            _BusyTicker = ( _BusyTicker + 1 ) % 25;

            // lerp the activation value up or down based on if we're activated or not
            if ( Activated )
                _BarActivationValue += ( 1.0f - _BarActivationValue ) * 0.333f;
            else
                _BarActivationValue += ( -0.1f - _BarActivationValue ) * 0.333f;

            // if we just hit 0, we don't need to have any furhter notifications to draw
            if ( _BarActivationValue <= 0.0f )
            {
                _BarActivationValue = 0.0f;
                _RedrawTimer.Stop();
            }

            Repaint();
        }

        private void Repaint()
        {
            Invalidate();
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            // if the activation value is 0 - and we are therefore not active - just clear to the BG
            if ( _BarActivationValue <= 0.0f )
            {
                e.Graphics.Clear( Current.Instance.Color( _InactiveShader ) );
                return;
            }
            else
            {
                // cheap HSV blend for both background and bar
                HSV hsvBlendedBG  = HSV.Lerp( _InactiveHSV, _ActiveHSV, _BarActivationValue );
                HSV hsvBlendedBar = HSV.Lerp( _InactiveHSV, _BarHSV, _BarActivationValue );

                e.Graphics.Clear( hsvBlendedBG.ToColor() );
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

                // find how many bars it will take to cover the panel
                int barCount    = (int)Math.Ceiling((float)Width / 25.0f) + 1;
                float barWidth  = 10.0f;

                using ( var p = new Pen( hsvBlendedBar.ToColor(), barWidth ) )
                {
                    for ( int i = 0; i < barCount; i++ )
                    {
                        // start one bar back to cover one emerging from the left
                        float x = (float)(i - 1) * 25;

                        // render a line strike with a slight incline
                        e.Graphics.DrawLine( p, x + (float)_BusyTicker, -5, x + (float)_BusyTicker + 8, Height + 5 );
                    }
                }
            }
        }
    }

}
