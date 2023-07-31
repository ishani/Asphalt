using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

using Asphalt.Controls.Theme;

namespace Asphalt.Controls
{
    public class TrackEventArgs : EventArgs
    {
        public TrackEventArgs( float oldValue, float newValue )
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public float OldValue { get; set; }
        public float NewValue { get; set; }
    }

    public delegate void TrackEventHandler( object sender, TrackEventArgs e );



    [ToolboxItem( true )]
    public class AsphaltTrack : ControlWithNewRendering, IPropertyEvents
    {
        public enum OriginPoint
        {
            FromMinimumValue,
            FromDefaultValue
        }

        private bool                        _IsHovering = false;
        private bool                        _IsDragging = false;
        private AsphaltPanelShading       _TrackShading;

        private float                     _MinimumValue = -1;
        private float                     _MaximumValue = 1;
        private float                     _DefaultValue = 0;
        private float                     _CurrentValue = 0;
                                         
        private float                         _SlotSize = 8;
        private float                          _BarSize = 1.5f;

        private Direction                    _Direction = Direction.Horizontal;
        private OriginPoint                  _BarOrigin = OriginPoint.FromMinimumValue;
        private bool                       _DrawNotches = false;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Asphalt")]
        public event TrackEventHandler ValueChanged;


        public AsphaltTrack()
        {
            SetDefaultControlStyles();

            Name         = "AsphaltTrack";
            Size         = new System.Drawing.Size( 320, 50 );
            Padding      = new Padding( 8, 0, 8, 0 );

            TrackShading = new AsphaltPanelShading();
        }


        #region Properties_Shading

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt" )]
        public AsphaltPanelShading TrackShading
        {
            get { return _TrackShading; }
            set { Prop.Exchange( ref _TrackShading, value, this ); }
        }

        void IPropertyEvents.OnChange( object sender, PropertyChangedEventArgs e )
        {
            Repaint();
        }

        #endregion

        #region Properties_Range

        [DefaultValue( -1 )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Range" )]
        public float MinimumValue
        {
            get { return _MinimumValue; }
            set { _MinimumValue = value; Repaint(); }
        }

        [DefaultValue( 1 )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Range" )]
        public float MaximumValue
        {
            get { return _MaximumValue; }
            set { _MaximumValue = value; Repaint(); }
        }

        [DefaultValue( 0 )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Range" )]
        public float DefaultValue
        {
            get { return _DefaultValue; }
            set { _DefaultValue = value; Repaint(); }
        }

        [DefaultValue( 0 )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Range" )]
        public float CurrentValue
        {
            get { return _CurrentValue; }
            set {
                var eventArgs = new TrackEventArgs( _CurrentValue, value );

                _CurrentValue = value;
                ValueChanged?.Invoke( this, eventArgs );
                Repaint();
            }
        }

        // internal state 
        [Browsable(false)]
        public float InitialState
        {
            set {
                _DefaultValue = value;
                 CurrentValue = value;
            }
        }

        #endregion

        #region Properties

        [DefaultValue( 8 )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt" )]
        public float SlotSize
        {
            get { return _SlotSize; }
            set { _SlotSize = value; Repaint(); }
        }

        [DefaultValue( 1.5f )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt" )]
        public float BarSize
        {
            get { return _BarSize; }
            set { _BarSize = value; Repaint(); }
        }

        [DefaultValue( Direction.Horizontal )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt" )]
        public Direction Direction
        {
            get { return _Direction; }
            set { _Direction = value; Repaint(); }
        }

        [DefaultValue( OriginPoint.FromMinimumValue )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt" )]
        public OriginPoint BarOrigin
        {
            get { return _BarOrigin; }
            set { _BarOrigin = value; Repaint(); }
        }

        [DefaultValue( false )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt" )]
        public bool DrawNotches
        {
            get { return _DrawNotches; }
            set { _DrawNotches = value; Repaint(); }
        }

        #endregion



        protected override void OnMouseLeave( EventArgs e )
        {
            base.OnMouseLeave( e );

            _IsHovering = false;
            _IsDragging = false;
            Repaint();
        }

        protected override void OnMouseDown( MouseEventArgs e )
        {
            base.OnMouseDown( e );

            if ( e.Button == MouseButtons.Left )
            {
                RectangleF innerTrack = GetInnerTrackRectangle( Math.Max(SlotSize, BarSize) );

                if ( innerTrack.Contains( e.Location ) )
                {
                    CurrentValue = GetValueFromCanvasPosition( e.Location );
                    _IsDragging = true;
                }
            }
            if ( e.Button == MouseButtons.Right )
            {
                CurrentValue = DefaultValue;
            }
        }

        protected override void OnMouseMove( MouseEventArgs e )
        {
            base.OnMouseMove( e );

            if ( _IsDragging )
            {
                CurrentValue = GetValueFromCanvasPosition( e.Location );
            }
            else
            {
                RectangleF innerTrack = GetInnerTrackRectangle( Math.Max(SlotSize, BarSize) );

                if ( innerTrack.Contains( e.Location ) )
                {
                    _IsHovering = true;
                    Repaint();
                }
            }
        }

        protected override void OnMouseUp( MouseEventArgs e )
        {
            base.OnMouseUp( e );

            _IsDragging = false;
        }


        private RectangleF GetInnerTrackRectangle( float trackSize )
        {
            float minorDimension          = (float)( ( Direction == Direction.Horizontal ) ? Height : Width );

            float centerOffset            = ( minorDimension * 0.5f ) - ( trackSize * 0.5f );

            if ( Direction == Direction.Horizontal )
            {
                return new RectangleF(
                    (float)Padding.Left,
                    centerOffset,
                    Width - ( Padding.Left + Padding.Right ),
                    trackSize );
            }
            else
            {
                return new RectangleF(
                    centerOffset,
                    (float)Padding.Top,
                    trackSize,
                    Height - ( Padding.Top + Padding.Bottom ) );
            }
        }

        private float GetValueFromCanvasPosition( Point location )
        {
            RectangleF innerTrackRect    = GetInnerTrackRectangle( SlotSize );

            float majorDimension         = ( Direction == Direction.Horizontal ) ? innerTrackRect.Width : innerTrackRect.Height;
            float edgeStart              = ( Direction == Direction.Horizontal ) ? innerTrackRect.Left  : innerTrackRect.Bottom;

            float fullValueRange         = ( MaximumValue - MinimumValue );
            float floatIncrementPerPixel = majorDimension / fullValueRange;

            float whichValue             = ( Direction == Direction.Horizontal ) ? location.X : location.Y;

            if ( Direction == Direction.Horizontal )
            {
                whichValue = Math.Max( 0, ( whichValue - edgeStart ) );
            }
            else
            {
                whichValue = Math.Max( 0, ( edgeStart - whichValue ) );
            }

            whichValue /= floatIncrementPerPixel;

            return Math.Min( MinimumValue + whichValue, MaximumValue );

        }

        private float GetCanvasPositionFromValue( float value )
        {
            RectangleF innerTrackRect    = GetInnerTrackRectangle( SlotSize );

            float majorDimension         = ( Direction == Direction.Horizontal ) ? innerTrackRect.Width : innerTrackRect.Height;
            float edgeStart              = ( Direction == Direction.Horizontal ) ? innerTrackRect.Left  : innerTrackRect.Bottom;

            float fullValueRange         = ( MaximumValue - MinimumValue );
            float floatIncrementPerPixel = majorDimension / fullValueRange;

            float pixelDistance          = ( (value - MinimumValue) * floatIncrementPerPixel );

            if ( Direction == Direction.Horizontal )
            {
                return edgeStart + pixelDistance;
            }
            else
            {
                return edgeStart - pixelDistance;
            }
        }

        private PointF GetHalfDims()
        {
            float widthF                 = (float)Width;
            float heightF                = (float)Height;

            float halfWidthF             = widthF * 0.5f;
            float halfHeightF            = heightF * 0.5f;

            return new PointF( halfWidthF, halfHeightF );
        }

        private void PaintNotch( in Graphics g, in Pen pen, float value, float size )
        {
            var halfDims = GetHalfDims();

            float canvasValueXPt    = GetCanvasPositionFromValue(value);
            float halfBarSizeL      = size + ( SlotSize * 0.5f );

            PointF defaultLinePtNeg;
            PointF defaultLinePt;
            PointF defaultLinePtPos;

            if ( Direction == Direction.Horizontal )
            {
                defaultLinePtNeg    = new PointF( canvasValueXPt, halfDims.Y - halfBarSizeL );
                defaultLinePt       = new PointF( canvasValueXPt, halfDims.Y );
                defaultLinePtPos    = new PointF( canvasValueXPt, halfDims.Y + halfBarSizeL );
            }
            else
            {
                defaultLinePtNeg    = new PointF( halfDims.X - halfBarSizeL, canvasValueXPt );
                defaultLinePt       = new PointF( halfDims.X,                canvasValueXPt );
                defaultLinePtPos    = new PointF( halfDims.X + halfBarSizeL, canvasValueXPt );
            }

            g.DrawLine( pen, defaultLinePtNeg, defaultLinePtPos );
        }


        internal override void Repaint()
        {
            Invalidate();
        }

        internal override void RenderBG(Graphics gfx)
        {
            gfx.Clear(Current.Instance.Color(_TrackShading.PanelShader));
        }

        internal override void Render(Graphics gfx)
        {
            gfx.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            gfx.SmoothingMode     = SmoothingMode.HighQuality;

            var halfDims = GetHalfDims();


            RectangleF innerTrackRect    = GetInnerTrackRectangle( SlotSize );

            float defaultValueXPt        = GetCanvasPositionFromValue(DefaultValue);
            float currentValueXPt        = GetCanvasPositionFromValue(CurrentValue);


            PointF minimumPt;
            PointF defaultLinePt;
            PointF currentValuePt;
     
            if ( Direction == Direction.Horizontal )
            {
                minimumPt           = new PointF( innerTrackRect.Left, halfDims.Y );
                defaultLinePt       = new PointF( defaultValueXPt, halfDims.Y );
                currentValuePt      = new PointF( currentValueXPt, halfDims.Y );
            }
            else
            {
                minimumPt           = new PointF( halfDims.X, innerTrackRect.Bottom );
                defaultLinePt       = new PointF( halfDims.X, defaultValueXPt );
                currentValuePt      = new PointF( halfDims.X, currentValueXPt );
            }

            using ( var notchPen = new Pen( Current.Instance.Color(_TrackShading.IconShader), 1.2f ) )
            {
                if (DrawNotches)
                {
                    PaintNotch(gfx, notchPen, DefaultValue, 4.0f);
                }

                gfx.FillRectangle( Current.Instance.SolidBrush(_TrackShading.EdgeShader), innerTrackRect );

                if (DrawNotches)
                {
                    PaintNotch(gfx, notchPen, MinimumValue, 2.0f);
                    PaintNotch(gfx, notchPen, MaximumValue, 2.0f);
                }
            }

            Shader frontShader = _IsHovering ? _TrackShading.TextShader : _TrackShading.AdornmentShader;

            gfx.DrawLine( new Pen( Current.Instance.Color( frontShader ), BarSize ), ( BarOrigin == OriginPoint.FromMinimumValue ) ? minimumPt : defaultLinePt, currentValuePt );

            float markerBarSize = ( BarSize * 0.5f ) + 1f;
            float rectW         = ( Direction == Direction.Horizontal ) ? 3 : markerBarSize;
            float rectH         = ( Direction == Direction.Horizontal ) ? markerBarSize : 3;

            PointF drawPos = new PointF( currentValuePt.X, currentValuePt.Y );
            gfx.FillRectangle( Current.Instance.SolidBrush( _TrackShading.IconShader ), drawPos.X - rectW, drawPos.Y - rectH, rectW * 2, rectH * 2 );
        }
    }
}
