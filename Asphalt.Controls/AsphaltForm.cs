using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Security;
using System.Windows.Forms;

using Asphalt.Controls.Theme;

namespace Asphalt.Controls
{
    public class AsphaltForm : Form, IPropertyEvents, IHasBackgroundShader
    {
        #region Properties_Shading

        private Shader _PanelShader = null;
        private Shader _EdgeShader  = null;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Asphalt.Panel")]
        public Shader PanelShader
        {
            get => _PanelShader;
            set => Prop.Exchange(ref _PanelShader, value, this);
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Asphalt.Panel")]
        public Shader EdgeShader
        {
            get => _EdgeShader;
            set => Prop.Exchange(ref _EdgeShader, value, this);
        }

        void IPropertyEvents.OnChange(object sender, PropertyChangedEventArgs e)
        {
            Invalidate();
        }

        #endregion

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Asphalt.Behaviour")]
        public bool CanResize { get; set; } = false;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Asphalt.Behaviour")]
        public Int32 ResizeGripSize { get; set; } = 8;


        public AsphaltForm()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true );

            FormBorderStyle = FormBorderStyle.None;
            StartPosition   = FormStartPosition.CenterScreen;
            Padding         = new Padding( 4 );

            PanelShader = new Shader(Scheme.Base, Pigment.Primary, Shade.Default);
            EdgeShader  = new Shader(Scheme.Base, Pigment.Analogous, Shade.Darker);
        }

        protected override CreateParams CreateParams
        {
            get {
                CreateParams cp = base.CreateParams;
                if ( !DesignMode )
                {
                    cp.ClassStyle |= 0x20000;   // CS_DROPSHADOW
                    cp.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED
                }
                return cp;
            }
        }

        Shader IHasBackgroundShader.GetCurrentBackgroundShader()
        {
            return PanelShader;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            ForwardMouseDownToNonClient(Handle);
        }

        static public void ForwardMouseDownToNonClient(IntPtr hwnd)
        {
            Native.WinApi.ReleaseCapture();
            Native.WinApi.SendMessage(hwnd, Native.WinApi.Messages.WM_NCLBUTTONDOWN, (int)Native.WinApi.HitTest.HTCAPTION, 0);
        }

        protected override void OnPaintBackground( PaintEventArgs e )
        {
            try // without ControlStyles.AllPaintingInWmPaint, we need our own error handling
            {
                var background      = Current.Instance.SolidBrush( _PanelShader );
                Color backgroundD   = Current.Instance.Color( _EdgeShader );

                using ( var borderPen = new Pen( backgroundD, 6 ) )
                {
                    e.Graphics.FillRectangle( background, 0, 0, Width, Height );
                    e.Graphics.DrawRectangle( borderPen,  0, 0, Width, Height );
                }
            }
            catch ( Exception ex )
            {
                Debug.WriteLine( ex );
                Invalidate();
            }
        }

        private Rectangle SizeGripRectangle
        {
            get {
                int padMax = ResizeGripSize;
                var cs = ClientSize;
                return new Rectangle( cs.Width - padMax, cs.Height - padMax, padMax, padMax );
            }
        }

        private Native.WinApi.HitTest HitTestNCA( IntPtr hwnd, IntPtr wparam, IntPtr lparam )
        {
            Point pc = PointToClient( new Point((int)lparam) );

            if ( CanResize )
            {
                if ( SizeGripRectangle.Contains( pc ) )
                    return Native.WinApi.HitTest.HTBOTTOMRIGHT;
                if ( pc.X > ClientSize.Width - ResizeGripSize )
                    return Native.WinApi.HitTest.HTRIGHT;
                if ( pc.Y > ClientSize.Height - ResizeGripSize )
                    return Native.WinApi.HitTest.HTBOTTOM;
                if ( pc.X < ResizeGripSize )
                    return Native.WinApi.HitTest.HTLEFT;
                if ( pc.Y < ResizeGripSize )
                    return Native.WinApi.HitTest.HTTOP;
            }

            return Native.WinApi.HitTest.HTCLIENT;
        }

        protected override void WndProc( ref Message m )
        {
            if (DesignMode)
            {
                base.WndProc(ref m);
                return;
            }

            switch (m.Msg)
            {
                case Native.WinApi.Messages.WM_NCHITTEST:
                    Native.WinApi.HitTest ht = HitTestNCA(m.HWnd, m.WParam, m.LParam);
                    if (ht != Native.WinApi.HitTest.HTCLIENT)
                    {
                        m.Result = (IntPtr)ht;
                        return;
                    }
                    break;

                case Native.WinApi.Messages.WM_SYSCOMMAND:
                    int sc = m.WParam.ToInt32() & 0xFFF0;
                    switch (sc)
                    {
                        case Native.WinApi.Messages.SC_MOVE:
                            break;
                        case Native.WinApi.Messages.SC_MAXIMIZE:
                            break;
                        case Native.WinApi.Messages.SC_RESTORE:
                            break;
                    }
                    break;

                case Native.WinApi.Messages.WM_NCLBUTTONDBLCLK:
                case Native.WinApi.Messages.WM_LBUTTONDBLCLK:
                    if (!MaximizeBox) return;
                    break;

                case Native.WinApi.Messages.WM_SIZING:
                    OnWmSizing(m.WParam, m.LParam);
                    break;
            }

            base.WndProc(ref m);

            // some messages are better post-processed ...

            switch (m.Msg)
            {
                case Native.WinApi.Messages.WM_GETMINMAXINFO:
                    OnGetMinMaxInfo(m.HWnd, m.LParam);
                    break;
            }
        }


        private Size _minTrackSize;

        [SecuritySafeCritical]
        private unsafe void OnWmSizing(IntPtr wParam, IntPtr lParam)
        {
            Native.RECT* rc = (Native.RECT*)lParam;
            rc->Width = Math.Max(rc->Width, _minTrackSize.Width);
            rc->Height = Math.Max(rc->Height, _minTrackSize.Height);
        }

        [SecuritySafeCritical]
        private unsafe void OnGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            Native.WinApi.MINMAXINFO* pmmi = (Native.WinApi.MINMAXINFO*)lParam;

            //  NOTE: MaxPosition is always relative to the origin of the window's current screen
            // e.g. usually (0, 0) unless the taskbar is on the left or top.

            Screen s = Screen.FromHandle(hwnd);
            pmmi->MaxSize = s.WorkingArea.Size;
            pmmi->MaxPosition.X = Math.Abs(s.WorkingArea.Left - s.Bounds.Left);
            pmmi->MaxPosition.Y = Math.Abs(s.WorkingArea.Top - s.Bounds.Top);

            // I guess these should have the normal window frame dimensions added to be correct
            // see SystemInformation.XXX
            _minTrackSize.Width = Math.Max(_minTrackSize.Width, pmmi->MinTrackSize.Width);
            _minTrackSize.Height = Math.Max(_minTrackSize.Height, pmmi->MinTrackSize.Height);
            _minTrackSize = SizeFromClientSize(_minTrackSize);
        }
    }
}
