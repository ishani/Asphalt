using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#if ASPHALT_DESIGN
using System.Collections;
using System.ComponentModel.Design;
using System.Security.Permissions;
using System.Windows.Forms.Design;
#endif // ASPHALT_DESIGN

using Asphalt.Controls.Theme;


namespace Asphalt.Controls
{
    public partial class AsphaltScroller : UserControl, IPropertyEvents, IHasBackgroundShader
    {
        private Point                      _lastMouse;
        private Point              _lastPanelLocation;

        private AsphaltScrollerShading       _Shading = null;
        private int                     _BarThickness = 12;

        private Direction                  _Direction = Direction.Horizontal;


        #region Properties

        // tracking if we are currently scrolling; automatically sets a is-scrolling mouse pointer
        private bool _barActivated = false;
        private bool BarActivated
        {
            get { return _barActivated;  }
            set {
                _barActivated = value;
                if (_barActivated)
                {
                    if (Direction == Direction.Horizontal)
                        Cursor.Current = Cursors.NoMoveVert;
                    else
                        Cursor.Current = Cursors.NoMoveHoriz;
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Asphalt")]
        public Panel EditablePanel { get; }

        [DefaultValue(12)]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Asphalt")]
        public int BarThickness
        {
            get { return _BarThickness; }
            set { _BarThickness = value; RescalePanel(); Repaint(); }
        }

        [DefaultValue(Direction.Horizontal)]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Asphalt")]
        public Direction Direction
        {
            get { return _Direction; }
            set { _Direction = value; RescalePanel(); Repaint(); }
        }

        #endregion

        #region Properties_Shading

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt" )]
        public AsphaltScrollerShading Shading
        {
            get { return _Shading; }
            set { Prop.Exchange( ref _Shading, value, this ); }
        }

        void IPropertyEvents.OnChange( object sender, PropertyChangedEventArgs e )
        {
            Invalidate();
        }

        #endregion



        public AsphaltScroller()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true );

            Shading = new AsphaltScrollerShading();


            EditablePanel               = new Panel();
            EditablePanel.BackColor     = Color.Transparent;
            EditablePanel.Location      = new System.Drawing.Point( -1, -1 );
            EditablePanel.Name          = "ScrollPanel";
            EditablePanel.Size          = new System.Drawing.Size( 268, 409 );

            EditablePanel.SizeChanged  += Scroller_SizeChanged;

            EditablePanel.MouseDown    += Scroller_MouseDown;
            EditablePanel.MouseMove    += Scroller_MouseMove;
            EditablePanel.MouseUp      += Scroller_MouseUp;

            Controls.Add( EditablePanel );

            Name = "AsphaltScroller";
            Size = new System.Drawing.Size( 285, 229 );
        }

        public Shader GetCurrentBackgroundShader()
        {
            return _Shading.PanelShader;
        }

        private void Scroller_SizeChanged( object sender, EventArgs e )
        {
            _lastPanelLocation = EditablePanel.Location;
            ApplyOffset( 1 );

            if ( Direction == Direction.Horizontal )
            {
                if ( EditablePanel.Top > 0 )
                    EditablePanel.Top = 0;
            }
            else
            {
                if ( EditablePanel.Left > 0 )
                    EditablePanel.Left = 0;
            }

            Invalidate();
        }

        private void Scroller_MouseMove( object sender, MouseEventArgs e )
        {
            if ( e.Button == MouseButtons.Left )
                OnMouseMoveLeft( Cursor.Position );
        }

        private void Scroller_MouseUp( object sender, MouseEventArgs e )
        {
            if ( e.Button == MouseButtons.Left )
                OnMouseUpLeft( Cursor.Position );
        }

        private void Scroller_MouseDown( object sender, MouseEventArgs e )
        {
            if ( e.Button == MouseButtons.Left )
                OnMouseDownLeft( Cursor.Position );
        }



        private void ApplyOffset( int offset )
        {
            Rectangle scrollBarRect;
            ComputeScrollBarRect( out scrollBarRect );

            int panelToPanelOverlap;
            int totalOffset;

            if ( Direction == Direction.Horizontal )
            {
                panelToPanelOverlap = EditablePanel.Height - Height;
                totalOffset = Height - scrollBarRect.Height;
            }
            else
            {
                panelToPanelOverlap = EditablePanel.Width - Width;
                totalOffset = Width - scrollBarRect.Width;
            }

            float noffset = (float)panelToPanelOverlap / (float)totalOffset;
            int h = (int)((float)offset * noffset);

            int newTopValue;

            if ( Direction == Direction.Horizontal )
                newTopValue = Math.Min( 0, _lastPanelLocation.Y + h );
            else
                newTopValue = Math.Min( 0, _lastPanelLocation.X + h );

            newTopValue = Math.Max( -panelToPanelOverlap, newTopValue );

            if ( Direction == Direction.Horizontal )
                EditablePanel.Top = newTopValue;
            else
                EditablePanel.Left = newTopValue;
        }

        private Rectangle GetScollerBarRectangle()
        {
            if ( Direction == Direction.Horizontal )
                return new Rectangle( Width - _BarThickness, 0, _BarThickness, Height );
            else
                return new Rectangle( 0, Height - _BarThickness, Width, _BarThickness );
        }

        private void Repaint()
        {
            Invalidate( GetScollerBarRectangle() );
        }

        protected override void OnMouseWheel( MouseEventArgs e )
        {
            base.OnMouseWheel( e );

            _lastPanelLocation = EditablePanel.Location;
            ApplyOffset( e.Delta / 4 );

            Repaint();
        }

        protected override void OnMouseEnter( EventArgs e )
        {
            base.OnMouseEnter( e );
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseLeave( EventArgs e )
        {
            base.OnMouseLeave( e );
            Cursor = Cursors.Default;

            BarActivated = false;
        }

        internal void OnMouseDownLeft( Point Location )
        {
            Rectangle scrollBarRect;
            if ( ComputeScrollBarRect( out scrollBarRect ) )
            {
                //if (scrollBarRect.Contains(Location.X, Location.Y))
                {
                    BarActivated = true;
                    _lastMouse = Location;
                    _lastPanelLocation = EditablePanel.Location;

                    Repaint();
                }
            }
        }

        internal void OnMouseMoveLeft( Point Location )
        {
            if ( BarActivated )
            {
                int offset = 0;

                if ( Direction == Direction.Horizontal )
                    offset = ( _lastMouse.Y - Location.Y );
                else
                    offset = ( _lastMouse.X - Location.X );

                ApplyOffset( offset );

                Repaint();
            }
        }

        internal void OnMouseUpLeft( Point Location )
        {
            BarActivated = false;
            Repaint();
        }

        protected override void OnMouseDown( MouseEventArgs e )
        {
            base.OnMouseDown( e );

            if ( e.Button == MouseButtons.Left )
                OnMouseDownLeft( e.Location );
        }

        protected override void OnMouseMove( MouseEventArgs e )
        {
            base.OnMouseMove( e );

            if ( e.Button == MouseButtons.Left )
                OnMouseMoveLeft( e.Location );
        }

        protected override void OnMouseUp( MouseEventArgs e )
        {
            base.OnMouseUp( e );

            if ( e.Button == MouseButtons.Left )
                OnMouseUpLeft( e.Location );
        }

        private void RescalePanel()
        {
            if ( Direction == Direction.Horizontal )
                EditablePanel.Width = Width - _BarThickness;
            else
                EditablePanel.Height = Height - _BarThickness;
        }

        protected override void OnSizeChanged( EventArgs e )
        {
            base.OnSizeChanged( e );
            RescalePanel();
        }

        protected override void OnResize( EventArgs e )
        {
            base.OnResize( e );
            RescalePanel();
        }

        protected override void OnPaintBackground( PaintEventArgs e )
        {
            e.Graphics.Clear( Current.Instance.Color( _Shading.PanelShader ) );
        }

        protected bool ComputeScrollBarRect( out Rectangle result )
        {
            int heightDiff;
            int dimValue;
            int panelTop;

            if ( Direction == Direction.Horizontal )
            {
                heightDiff = EditablePanel.Height - Height;
                dimValue = Height;
                panelTop = EditablePanel.Top;
            }
            else
            {
                heightDiff = EditablePanel.Width - Width;
                dimValue = Width;
                panelTop = EditablePanel.Left;
            }

            if ( heightDiff <= 0 )
            {
                result = new Rectangle();
                return false;
            }

            float barSizeN  = 1.0f - Math.Min(1.0f, (float)heightDiff / 600.0f);
            int barSizeI    = (int)(barSizeN * (float)dimValue);
                barSizeI    = Math.Max( 40, barSizeI );

            float offset    = (float)(-panelTop) / (float)heightDiff;

            int totalOffset = dimValue - barSizeI;
            int h           = (int)( (float)totalOffset * offset );

            if ( Direction == Direction.Horizontal )
                result = new Rectangle( Width - _BarThickness, h, _BarThickness, barSizeI );
            else
                result = new Rectangle( h, Height - _BarThickness, barSizeI, _BarThickness );

            return true;
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            e.Graphics.FillRectangle( Current.Instance.SolidBrush( _Shading.ScrollShader ), GetScollerBarRectangle() );

            Rectangle scrollBarRect;
            bool hasScrollBar = ComputeScrollBarRect(out scrollBarRect);

            if ( hasScrollBar )
            {
                var barBrush = Current.Instance.SolidBrush(BarActivated ? _Shading.BarOnShader : _Shading.BarShader);

                e.Graphics.FillRectangle( barBrush, scrollBarRect );
            }
        }
    }


#if ASPHALT_DESIGN

    [Designer( typeof( AsphaltScrollerDesigner ) )]
    public partial class AsphaltScroller
    {
    }

    [PermissionSet( SecurityAction.Demand, Name = "FullTrust" )]
    internal class AsphaltScrollerDesigner : ParentControlDesigner
    {
        public override void Initialize( IComponent component )
        {
            base.Initialize( component );

            if ( Control is AsphaltScroller ctrl )
            {
                EnableDesignMode( ctrl.EditablePanel, ctrl.EditablePanel.Name );
            }

            EnableDragDrop( false );
        }

        protected override void WndProc( ref Message m )
        {
            if ( Control is AsphaltScroller ctrl )
            {
                switch ( m.Msg )
                {
                    case Native.WinApi.Messages.WM_LBUTTONDOWN:
                        ctrl.OnMouseDownLeft( Native.WinApi.PointFromLParam( m.LParam ) );
                        break;

                    case Native.WinApi.Messages.WM_MOUSEMOVE:
                        if ( m.WParam.ToInt32() == 0x0001 )
                            ctrl.OnMouseMoveLeft( Native.WinApi.PointFromLParam( m.LParam ) );
                        break;

                    case Native.WinApi.Messages.WM_LBUTTONUP:
                        ctrl.OnMouseUpLeft( Native.WinApi.PointFromLParam( m.LParam ) );
                        break;
                }
            }

            base.WndProc( ref m );
        }

        protected override void PreFilterProperties( IDictionary properties )
        {
            base.PreFilterProperties( properties );
            PropertiesToRemove.StripFromDictionary( ref properties );
        }
    }

#endif // ASPHALT_DESIGN

}
