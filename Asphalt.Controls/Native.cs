/**
 * Portions of the native declarations are from MetroFramework, licence notice below
 */

/**
 * MetroFramework - Modern UI for WinForms
 * 
 * The MIT License (MIT)
 * Copyright (c) 2011 Sven Walter, http://github.com/viperneo
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */


using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;


namespace Asphalt.Controls.Native
{
    [DebuggerDisplay( "({X},{Y})" )]
    [StructLayout( LayoutKind.Sequential )]
    [Obsolete( "Use System.Drawing.Point" )]
    public struct POINT
    {
        public int X;
        public int Y;

        public static POINT Empty;

        public POINT( int x, int y )
        {
            X = x;
            Y = y;
        }

        public POINT( Point other )
        {
            X = other.X;
            Y = other.Y;
        }

        public Point ToPoint()
        {
            return new Point( X, Y );
        }

        public static implicit operator POINT( Point other )
        {
            return new POINT( other );
        }

        public static implicit operator Point( POINT other )
        {
            return other.ToPoint();
        }
    }

    [DebuggerDisplay( "({Width},{Height})" )]
    [StructLayout( LayoutKind.Sequential )]
    [Obsolete( "Use System.Drawing.Size" )]
    public struct SIZE
    {
        public int Width;
        public int Height;

        public static SIZE Empty;

        public SIZE( int width, int height )
        {
            Width = width;
            Height = height;
        }

        public SIZE( Size other )
        {
            Width = other.Width;
            Height = other.Height;
        }

        public Size ToSize()
        {
            return new Size( Width, Height );
        }

        public static implicit operator SIZE( Size other )
        {
            return new SIZE( other );
        }

        public static implicit operator Size( SIZE other )
        {
            return other.ToSize();
        }
    }


    [DebuggerDisplay( "({Top},{Left}) ({Bottom},{Right})" )]
    [StructLayout( LayoutKind.Sequential )]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public static RECT Empty;

        public RECT( Rectangle rect )
        {
            Left = rect.Left;
            Top = rect.Top;
            Right = rect.Right;
            Bottom = rect.Bottom;
        }

        public RECT( int left, int top, int right, int bottom )
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle( Left, Top, Width, Height );
        }

        public int Height
        {
            get => Bottom - Top;
            set => Bottom = Top + value;
        }

        public Size Size => new Size( Width, Height );

        public int Width
        {
            get => Right - Left;
            set => Right = Left + value;
        }

        public void Inflate( int px )
        {
            Left -= px;
            Top -= px;
            Right += px;
            Bottom += px;
        }

        public static implicit operator Rectangle( RECT other )
        {
            return other.ToRectangle();
        }

        public static implicit operator RECT( Rectangle other )
        {
            return new RECT( other );
        }
    }

    [SuppressUnmanagedCodeSecurity]
    internal class DwmApi
    {
        #region Structs

        [StructLayout( LayoutKind.Sequential )]
        public struct DWM_BLURBEHIND
        {
            public int dwFlags;
            public int fEnable;
            public IntPtr hRgnBlur; // HRGN
            public int fTransitionOnMaximized;

            private DWM_BLURBEHIND( bool enable )
            {
                dwFlags = DWM_BB_ENABLE;
                fEnable = enable ? 1 : 0;
                hRgnBlur = IntPtr.Zero;
                fTransitionOnMaximized = 0;
            }

            public static DWM_BLURBEHIND Enable  = new DWM_BLURBEHIND(true);
            public static DWM_BLURBEHIND Disable = new DWM_BLURBEHIND(false);

            public const int DWM_BB_ENABLE                = 1;
            public const int DWM_BB_BLURREGION            = 2;
            public const int DWM_BB_TRANSITIONONMAXIMIZED = 4;
        }

        [StructLayout( LayoutKind.Sequential )]
        public struct DWM_PRESENT_PARAMETERS
        {
            public int cbSize;
            public int fQueue;
            public long cRefreshStart;
            public int cBuffer;
            public int fUseSourceRate;
            public UNSIGNED_RATIO rateSource;
            public int cRefreshesPerFrame;
            public DWM_SOURCE_FRAME_SAMPLING eSampling;
        }

        [StructLayout( LayoutKind.Sequential )]
        public struct DWM_THUMBNAIL_PROPERTIES
        {
            public int dwFlags;
            public RECT rcDestination;
            public RECT rcSource;
            public byte opacity;
            public int fVisible;
            public int fSourceClientAreaOnly;
        }

        [StructLayout( LayoutKind.Sequential )]
        public struct DWM_TIMING_INFO
        {
            public int cbSize;
            public UNSIGNED_RATIO rateRefresh;
            public UNSIGNED_RATIO rateCompose;
            public long qpcVBlank;
            public long cRefresh;
            public long qpcCompose;
            public long cFrame;
            public long cRefreshFrame;
            public long cRefreshConfirmed;
            public int cFlipsOutstanding;
            public long cFrameCurrent;
            public long cFramesAvailable;
            public long cFrameCleared;
            public long cFramesReceived;
            public long cFramesDisplayed;
            public long cFramesDropped;
            public long cFramesMissed;
        }

        [StructLayout( LayoutKind.Sequential )]
        public struct UNSIGNED_RATIO
        {
            public int uiNumerator;
            public int uiDenominator;
        }

        [StructLayout( LayoutKind.Sequential )]
        public struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;

            public MARGINS( int All )
            {
                cxLeftWidth    = All;
                cxRightWidth   = All;
                cyTopHeight    = All;
                cyBottomHeight = All;
            }

            public MARGINS( int Left, int Right, int Top, int Bottom )
            {
                cxLeftWidth    = Left;
                cxRightWidth   = Right;
                cyTopHeight    = Top;
                cyBottomHeight = Bottom;
            }
        }

        [StructLayout( LayoutKind.Sequential )]
        public struct WTA_OPTIONS
        {
            public uint Flags;
            public uint Mask;
        }

        #endregion

        #region Enums

        public enum DWM_SOURCE_FRAME_SAMPLING
        {
            DWM_SOURCE_FRAME_SAMPLING_POINT,
            DWM_SOURCE_FRAME_SAMPLING_COVERAGE,
            DWM_SOURCE_FRAME_SAMPLING_LAST
        }

        public enum DWMNCRENDERINGPOLICY
        {
            DWMNCRP_USEWINDOWSTYLE,
            DWMNCRP_DISABLED,
            DWMNCRP_ENABLED
        }

        public enum DWMWINDOWATTRIBUTE : uint
        {
            DWMWA_NCRENDERING_ENABLED = 1,
            DWMWA_NCRENDERING_POLICY,
            DWMWA_TRANSITIONS_FORCEDISABLED,
            DWMWA_ALLOW_NCPAINT,
            DWMWA_CAPTION_BUTTON_BOUNDS,
            DWMWA_NONCLIENT_RTL_LAYOUT,
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            DWMWA_FLIP3D_POLICY,
            DWMWA_EXTENDED_FRAME_BOUNDS,
            DWMWA_HAS_ICONIC_BITMAP,
            DWMWA_DISALLOW_PEEK,
            DWMWA_EXCLUDED_FROM_PEEK,
            DWMWA_CLOAK,
            DWMWA_CLOAKED,
            DWMWA_FREEZE_REPRESENTATION,
            DWMWA_LAST
        }

        public enum WindowThemeAttributeType
        {
            WTA_NONCLIENT                     = 1
        }

        #endregion

        #region Fields

        public static uint WTNCA_NODRAWCAPTION              = 0x1;
        public static uint WTNCA_NODRAWICON                 = 0x2;
        public static uint WTNCA_NOSYSMENU                  = 0x4;
        public static uint WTNCA_NOMIRRORHELP               = 0x8;

        public const int DWM_BB_BLURREGION                  = 2;
        public const int DWM_BB_ENABLE                      = 1;
        public const int DWM_BB_TRANSITIONONMAXIMIZED       = 4;
        public const string DWM_COMPOSED_EVENT_BASE_NAME    = "DwmComposedEvent_";
        public const string DWM_COMPOSED_EVENT_NAME_FORMAT  = "%s%d";
        public const int DWM_COMPOSED_EVENT_NAME_MAX_LENGTH = 0x40;
        public const int DWM_FRAME_DURATION_DEFAULT         = -1;
        public const int DWM_TNP_OPACITY                    = 4;
        public const int DWM_TNP_RECTDESTINATION            = 1;
        public const int DWM_TNP_RECTSOURCE                 = 2;
        public const int DWM_TNP_SOURCECLIENTAREAONLY       = 0x10;
        public const int DWM_TNP_VISIBLE                    = 8;
        public static readonly bool DwmApiAvailable         = (Environment.OSVersion.Version.Major >= 6);

        public const int WM_DWMCOMPOSITIONCHANGED           = 0x31e;

        #endregion

        #region API Calls

        [DllImport( "dwmapi.dll" )]
        public static extern int DwmDefWindowProc( IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref IntPtr result );
        [DllImport( "dwmapi.dll" )]
        public static extern int DwmEnableComposition( int fEnable );
        [DllImport( "dwmapi.dll" )]
        public static extern int DwmEnableMMCSS( int fEnableMMCSS );
        [DllImport( "dwmapi.dll" )]
        public static extern int DwmExtendFrameIntoClientArea( IntPtr hdc, ref MARGINS marInset );
        [DllImport( "dwmapi.dll" )]
        public static extern int DwmGetColorizationColor( ref int pcrColorization, ref int pfOpaqueBlend );
        [DllImport( "dwmapi.dll" )]
        public static extern int DwmGetCompositionTimingInfo( IntPtr hwnd, ref DWM_TIMING_INFO pTimingInfo );
        [DllImport( "dwmapi.dll" )]
        public static extern int DwmGetWindowAttribute( IntPtr hwnd, int dwAttribute, IntPtr pvAttribute, int cbAttribute );
        [DllImport( "dwmapi.dll" )]
        public static extern int DwmIsCompositionEnabled( [MarshalAs( UnmanagedType.Bool )] out bool pfEnabled );
        [DllImport( "dwmapi.dll" )]
        public static extern int DwmModifyPreviousDxFrameDuration( IntPtr hwnd, int cRefreshes, int fRelative );
        [DllImport( "dwmapi.dll" )]
        public static extern int DwmQueryThumbnailSourceSize( IntPtr hThumbnail, ref Size pSize );
        [DllImport( "dwmapi.dll" )]
        public static extern int DwmRegisterThumbnail( IntPtr hwndDestination, IntPtr hwndSource, ref Size pMinimizedSize, ref IntPtr phThumbnailId );
        [DllImport( "dwmapi.dll" )]
        public static extern int DwmSetDxFrameDuration( IntPtr hwnd, int cRefreshes );
        [DllImport( "dwmapi.dll" )]
        public static extern int DwmSetPresentParameters( IntPtr hwnd, ref DWM_PRESENT_PARAMETERS pPresentParams );
        [DllImport( "dwmapi.dll", PreserveSig = true )]
        public static extern int DwmSetWindowAttribute( IntPtr hwnd, DWMWINDOWATTRIBUTE attr, ref int attrValue, int attrSize );
        [DllImport( "dwmapi.dll" )]
        public static extern int DwmUnregisterThumbnail( IntPtr hThumbnailId );
        [DllImport( "dwmapi.dll" )]
        public static extern int DwmUpdateThumbnailProperties( IntPtr hThumbnailId, ref DWM_THUMBNAIL_PROPERTIES ptnProperties );
        [DllImport( "dwmapi.dll" )]
        public static extern int DwmEnableBlurBehindWindow( IntPtr hWnd, ref DWM_BLURBEHIND pBlurBehind );
        [DllImport( "uxtheme.dll" )]
        public static extern int SetWindowThemeAttribute( IntPtr hWnd, WindowThemeAttributeType wtype, ref WTA_OPTIONS attributes, uint size );

        #endregion
    }

    [SuppressUnmanagedCodeSecurity]
    internal static class WinDbg
    {
        [DllImport( "kernel32.dll" )]
        public static extern void OutputDebugString( string lpOutputString );
    }

    [SuppressUnmanagedCodeSecurity]
    internal static class WinApi
    {
        public static Point PointFromLParam(IntPtr _xy)
        {
            uint xy = unchecked(IntPtr.Size == 8 ? (uint)_xy.ToInt64() : (uint)_xy.ToInt32());
            int x = unchecked((short)xy);
            int y = unchecked((short)(xy >> 16));
            return new Point(x, y);
        }

        [StructLayout( LayoutKind.Sequential, Pack = 1 )]
        public struct ARGB
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;
        }

        [StructLayout( LayoutKind.Sequential, Pack = 1 )]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [StructLayout( LayoutKind.Sequential )]
        public struct TCHITTESTINFO
        {
            public Point pt;
            public uint flags;
        }

        [StructLayout( LayoutKind.Sequential )]
        public struct NCCALCSIZE_PARAMS
        {
            /// <summary>
            ///     IN: New window rectangle; OUT: New client rectangle (all in parent coordinates).
            /// </summary>
            /// <remarks>
            ///     Contains the new coordinates of a window that has been moved or resized, that is, 
            ///     it is the proposed new window coordinates.
            ///     On return, Windows expects the new client rectangle (in parent coordinates).
            /// </remarks>
            public RECT rect0;

            ///  <summary>
            ///     IN: Old window rectangle; OUT: destination rectangle (all in parent coordinates)
            /// </summary>
            /// <remarks>
            ///     Contains the coordinates of the window before it was moved or resized.
            ///     When returning anything other than 0, Windows expects the 
            ///     client's new / destination rectangle (in parent coordinates).
            /// </remarks>
            public RECT rect1;

            /// <summary>
            ///     IN: Old client rectangle; OUT: Source rectangle (all in parent coordinates).
            /// </summary>
            /// <remarks>
            ///     Contains the coordinates of the window's client area before the window was moved or resized.
            ///     When returning anything other than 0, Windows expects the
            ///     source rectangle (in parent coordinates).
            /// </remarks>
            public RECT rect2;

            /// <summary>
            ///     Pointer to a <see cref="WINDOWPOS"/> structure that contains the size and position values specified 
            ///     in the operation that moved or resized the window.
            /// </summary>
            public IntPtr lpPos;
        }

        [StructLayout( LayoutKind.Sequential )]
        public struct MINMAXINFO
        {
            public Point ptReserved;
            /// <summary>
            ///     The size a window should be maximized to. This depends on the screen it will end up, 
            ///     so the window manager will request this info when we move the window around.
            /// </summary>
            public Size MaxSize;
            /// <summary>
            ///     The position of the window when maximized. Must be relative to the current screen, 
            ///     so it's often (0,0) or close to that if the task bar is in the way.
            /// </summary>
            public Point MaxPosition;
            /// <summary>
            ///     The minimum size a window should be allowed to be resized to by dragging it's border or resize handle.
            /// </summary>
            public Size MinTrackSize;
            /// <summary>
            ///     The maximum size a window should be allowed to be resized to by dragging it's border or resize handle.
            ///     This is usually the maximum dimensions of the virtual screen, i.e. the bounding box containing all screens.
            /// </summary>
            public Size MaxTrackSize;
        }

        [StructLayout( LayoutKind.Sequential )]
        public struct APPBARDATA
        {
            public uint cbSize;
            public IntPtr hWnd;
            public uint uCallbackMessage;
            public ABE uEdge;
            public RECT rc;
            public int lParam;
        }

        [StructLayout( LayoutKind.Sequential )]
        public struct WINDOWPOS
        {
            public int hwnd;
            public int hWndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int flags;
        }


        public enum ABM : uint
        {
            New              = 0x00000000,
            Remove           = 0x00000001,
            QueryPos         = 0x00000002,
            SetPos           = 0x00000003,
            GetState         = 0x00000004,
            GetTaskbarPos    = 0x00000005,
            Activate         = 0x00000006,
            GetAutoHideBar   = 0x00000007,
            SetAutoHideBar   = 0x00000008,
            WindowPosChanged = 0x00000009,
            SetState         = 0x0000000A,
        }

        public enum ABE : uint
        {
            Left             = 0,
            Top              = 1,
            Right            = 2,
            Bottom           = 3
        }

        public enum ScrollBar
        {
            Horizontal       = 0,
            Vertical         = 1,
            Control          = 2,
            Both             = 3
        }

        public enum HitTest
        {
            HTNOWHERE        = 0,
            HTCLIENT         = 1,
            HTCAPTION        = 2,
            HTGROWBOX        = 4,
            HTSIZE           = HTGROWBOX,
            HTMINBUTTON      = 8,
            HTMAXBUTTON      = 9,
            HTLEFT           = 10,
            HTRIGHT          = 11,
            HTTOP            = 12,
            HTTOPLEFT        = 13,
            HTTOPRIGHT       = 14,
            HTBOTTOM         = 15,
            HTBOTTOMLEFT     = 16,
            HTBOTTOMRIGHT    = 17,
            HTREDUCE         = HTMINBUTTON,
            HTZOOM           = HTMAXBUTTON,
            HTSIZEFIRST      = HTLEFT,
            HTSIZELAST       = HTBOTTOMRIGHT,
            HTTRANSPARENT    = -1
        }

        public enum TabControlHitTest
        {
            TCHT_NOWHERE     = 1,
        }

        internal static class Messages
        {
            public const  int WM_NULL                   = 0x0;
            public const  int WM_CREATE                 = 0x1;
            public const  int WM_DESTROY                = 0x2;
            public const  int WM_MOVE                   = 0x3;
            public const  int WM_SIZE                   = 0x5;
            public const  int WM_ACTIVATE               = 0x6;
            public const  int WM_SETFOCUS               = 0x7;
            public const  int WM_KILLFOCUS              = 0x8;
            public const  int WM_ENABLE                 = 0xa;
            public const  int WM_SETREDRAW              = 0xb;
            public const  int WM_SETTEXT                = 0xc;
            public const  int WM_GETTEXT                = 0xd;
            public const  int WM_GETTEXTLENGTH          = 0xe;
            public const  int WM_PAINT                  = 0xf;
            public const  int WM_CLOSE                  = 0x10;
            public const  int WM_QUERYENDSESSION        = 0x11;
            public const  int WM_QUERYOPEN              = 0x13;
            public const  int WM_ENDSESSION             = 0x16;
            public const  int WM_QUIT                   = 0x12;
            public const  int WM_ERASEBKGND             = 0x14;
            public const  int WM_SYSCOLORCHANGE         = 0x15;
            public const  int WM_SHOWWINDOW             = 0x18;
            public const  int WM_WININICHANGE           = 0x1a;
            public const  int WM_SETTINGCHANGE          = WM_WININICHANGE;
            public const  int WM_DEVMODECHANGE          = 0x1b;
            public const  int WM_ACTIVATEAPP            = 0x1c;
            public const  int WM_FONTCHANGE             = 0x1d;
            public const  int WM_TIMECHANGE             = 0x1e;
            public const  int WM_CANCELMODE             = 0x1f;
            public const  int WM_SETCURSOR              = 0x20;
            public const  int WM_MOUSEACTIVATE          = 0x21;
            public const  int WM_CHILDACTIVATE          = 0x22;
            public const  int WM_QUEUESYNC              = 0x23;
            public const  int WM_GETMINMAXINFO          = 0x24;
            public const  int WM_PAINTICON              = 0x26;
            public const  int WM_ICONERASEBKGND         = 0x27;
            public const  int WM_NEXTDLGCTL             = 0x28;
            public const  int WM_SPOOLERSTATUS          = 0x2a;
            public const  int WM_DRAWITEM               = 0x2b;
            public const  int WM_MEASUREITEM            = 0x2c;
            public const  int WM_DELETEITEM             = 0x2d;
            public const  int WM_VKEYTOITEM             = 0x2e;
            public const  int WM_CHARTOITEM             = 0x2f;
            public const  int WM_SETFONT                = 0x30;
            public const  int WM_GETFONT                = 0x31;
            public const  int WM_SETHOTKEY              = 0x32;
            public const  int WM_GETHOTKEY              = 0x33;
            public const  int WM_QUERYDRAGICON          = 0x37;
            public const  int WM_COMPAREITEM            = 0x39;
            public const  int WM_GETOBJECT              = 0x3d;
            public const  int WM_COMPACTING             = 0x41;
            public const  int WM_COMMNOTIFY             = 0x44;
            public const  int WM_WINDOWPOSCHANGING      = 0x46;
            public const  int WM_WINDOWPOSCHANGED       = 0x47;
            public const  int WM_POWER                  = 0x48;
            public const  int WM_COPYDATA               = 0x4a;
            public const  int WM_CANCELJOURNAL          = 0x4b;
            public const  int WM_NOTIFY                 = 0x4e;
            public const  int WM_INPUTLANGCHANGEREQUEST = 0x50;
            public const  int WM_INPUTLANGCHANGE        = 0x51;
            public const  int WM_TCARD                  = 0x52;
            public const  int WM_HELP                   = 0x53;
            public const  int WM_USERCHANGED            = 0x54;
            public const  int WM_NOTIFYFORMAT           = 0x55;
            public const  int WM_CONTEXTMENU            = 0x7b;
            public const  int WM_STYLECHANGING          = 0x7c;
            public const  int WM_STYLECHANGED           = 0x7d;
            public const  int WM_DISPLAYCHANGE          = 0x7e;
            public const  int WM_GETICON                = 0x7f;
            public const  int WM_SETICON                = 0x80;
            public const  int WM_NCCREATE               = 0x81;
            public const  int WM_NCDESTROY              = 0x82;
            public const  int WM_NCCALCSIZE             = 0x83;
            public const  int WM_NCHITTEST              = 0x84;
            public const  int WM_NCPAINT                = 0x85;
            public const  int WM_NCACTIVATE             = 0x86;
            public const  int WM_GETDLGCODE             = 0x87;
            public const  int WM_SYNCPAINT              = 0x88;
            public const  int WM_NCMOUSEMOVE            = 0xa0;
            public const  int WM_NCLBUTTONDOWN          = 0xa1;
            public const  int WM_NCLBUTTONUP            = 0xa2;
            public const  int WM_NCLBUTTONDBLCLK        = 0xa3;
            public const  int WM_NCRBUTTONDOWN          = 0xa4;
            public const  int WM_NCRBUTTONUP            = 0xa5;
            public const  int WM_NCRBUTTONDBLCLK        = 0xa6;
            public const  int WM_NCMBUTTONDOWN          = 0xa7;
            public const  int WM_NCMBUTTONUP            = 0xa8;
            public const  int WM_NCMBUTTONDBLCLK        = 0xa9;
            public const  int WM_NCXBUTTONDOWN          = 0xab;
            public const  int WM_NCXBUTTONUP            = 0xac;
            public const  int WM_NCXBUTTONDBLCLK        = 0xad;
            public const  int WM_INPUT                  = 0xff;
            public const  int WM_KEYFIRST               = 0x100;
            public const  int WM_KEYDOWN                = 0x100;
            public const  int WM_KEYUP                  = 0x101;
            public const  int WM_CHAR                   = 0x102;
            public const  int WM_DEADCHAR               = 0x103;
            public const  int WM_SYSKEYDOWN             = 0x104;
            public const  int WM_SYSKEYUP               = 0x105;
            public const  int WM_SYSCHAR                = 0x106;
            public const  int WM_SYSDEADCHAR            = 0x107;
            public const  int WM_UNICHAR                = 0x109;
            public const  int WM_KEYLAST                = 0x108;
            public const  int WM_IME_STARTCOMPOSITION   = 0x10d;
            public const  int WM_IME_ENDCOMPOSITION     = 0x10e;
            public const  int WM_IME_COMPOSITION        = 0x10f;
            public const  int WM_IME_KEYLAST            = 0x10f;
            public const  int WM_INITDIALOG             = 0x110;
            public const  int WM_COMMAND                = 0x111;
            public const  int WM_SYSCOMMAND             = 0x112;
            public const  int WM_TIMER                  = 0x113;
            public const  int WM_HSCROLL                = 0x114;
            public const  int WM_VSCROLL                = 0x115;
            public const  int WM_INITMENU               = 0x116;
            public const  int WM_INITMENUPOPUP          = 0x117;
            public const  int WM_MENUSELECT             = 0x11f;
            public const  int WM_MENUCHAR               = 0x120;
            public const  int WM_ENTERIDLE              = 0x121;
            public const  int WM_MENURBUTTONUP          = 0x122;
            public const  int WM_MENUDRAG               = 0x123;
            public const  int WM_MENUGETOBJECT          = 0x124;
            public const  int WM_UNINITMENUPOPUP        = 0x125;
            public const  int WM_MENUCOMMAND            = 0x126;
            public const  int WM_CHANGEUISTATE          = 0x127;
            public const  int WM_UPDATEUISTATE          = 0x128;
            public const  int WM_QUERYUISTATE           = 0x129;
            public const  int WM_CTLCOLOR               = 0x19;
            public const  int WM_CTLCOLORMSGBOX         = 0x132;
            public const  int WM_CTLCOLOREDIT           = 0x133;
            public const  int WM_CTLCOLORLISTBOX        = 0x134;
            public const  int WM_CTLCOLORBTN            = 0x135;
            public const  int WM_CTLCOLORDLG            = 0x136;
            public const  int WM_CTLCOLORSCROLLBAR      = 0x137;
            public const  int WM_CTLCOLORSTATIC         = 0x138;
            public const  int WM_MOUSEFIRST             = 0x200;
            public const  int WM_MOUSEMOVE              = 0x200;
            public const  int WM_LBUTTONDOWN            = 0x201;
            public const  int WM_LBUTTONUP              = 0x202;
            public const  int WM_LBUTTONDBLCLK          = 0x203;
            public const  int WM_RBUTTONDOWN            = 0x204;
            public const  int WM_RBUTTONUP              = 0x205;
            public const  int WM_RBUTTONDBLCLK          = 0x206;
            public const  int WM_MBUTTONDOWN            = 0x207;
            public const  int WM_MBUTTONUP              = 0x208;
            public const  int WM_MBUTTONDBLCLK          = 0x209;
            public const  int WM_MOUSEWHEEL             = 0x20a;
            public const  int WM_XBUTTONDOWN            = 0x20b;
            public const  int WM_XBUTTONUP              = 0x20c;
            public const  int WM_XBUTTONDBLCLK          = 0x20d;
            public const  int WM_MOUSELAST              = 0x20d;
            public const  int WM_PARENTNOTIFY           = 0x210;
            public const  int WM_ENTERMENULOOP          = 0x211;
            public const  int WM_EXITMENULOOP           = 0x212;
            public const  int WM_NEXTMENU               = 0x213;
            public const  int WM_SIZING                 = 0x214;
            public const  int WM_CAPTURECHANGED         = 0x215;
            public const  int WM_MOVING                 = 0x216;
            public const  int WM_POWERBROADCAST         = 0x218;
            public const  int WM_DEVICECHANGE           = 0x219;
            public const  int WM_MDICREATE              = 0x220;
            public const  int WM_MDIDESTROY             = 0x221;
            public const  int WM_MDIACTIVATE            = 0x222;
            public const  int WM_MDIRESTORE             = 0x223;
            public const  int WM_MDINEXT                = 0x224;
            public const  int WM_MDIMAXIMIZE            = 0x225;
            public const  int WM_MDITILE                = 0x226;
            public const  int WM_MDICASCADE             = 0x227;
            public const  int WM_MDIICONARRANGE         = 0x228;
            public const  int WM_MDIGETACTIVE           = 0x229;
            public const  int WM_MDISETMENU             = 0x230;
            public const  int WM_ENTERSIZEMOVE          = 0x231;
            public const  int WM_EXITSIZEMOVE           = 0x232;
            public const  int WM_DROPFILES              = 0x233;
            public const  int WM_MDIREFRESHMENU         = 0x234;
            public const  int WM_IME_SETCONTEXT         = 0x281;
            public const  int WM_IME_NOTIFY             = 0x282;
            public const  int WM_IME_CONTROL            = 0x283;
            public const  int WM_IME_COMPOSITIONFULL    = 0x284;
            public const  int WM_IME_SELECT             = 0x285;
            public const  int WM_IME_CHAR               = 0x286;
            public const  int WM_IME_REQUEST            = 0x288;
            public const  int WM_IME_KEYDOWN            = 0x290;
            public const  int WM_IME_KEYUP              = 0x291;
            public const  int WM_MOUSEHOVER             = 0x2a1;
            public const  int WM_MOUSELEAVE             = 0x2a3;
            public const  int WM_NCMOUSELEAVE           = 0x2a2;
            public const  int WM_WTSSESSION_CHANGE      = 0x2b1;
            public const  int WM_TABLET_FIRST           = 0x2c0;
            public const  int WM_TABLET_LAST            = 0x2df;
            public const  int WM_CUT                    = 0x300;
            public const  int WM_COPY                   = 0x301;
            public const  int WM_PASTE                  = 0x302;
            public const  int WM_CLEAR                  = 0x303;
            public const  int WM_UNDO                   = 0x304;
            public const  int WM_RENDERFORMAT           = 0x305;
            public const  int WM_RENDERALLFORMATS       = 0x306;
            public const  int WM_DESTROYCLIPBOARD       = 0x307;
            public const  int WM_DRAWCLIPBOARD          = 0x308;
            public const  int WM_PAINTCLIPBOARD         = 0x309;
            public const  int WM_VSCROLLCLIPBOARD       = 0x30a;
            public const  int WM_SIZECLIPBOARD          = 0x30b;
            public const  int WM_ASKCBFORMATNAME        = 0x30c;
            public const  int WM_CHANGECBCHAIN          = 0x30d;
            public const  int WM_HSCROLLCLIPBOARD       = 0x30e;
            public const  int WM_QUERYNEWPALETTE        = 0x30f;
            public const  int WM_PALETTEISCHANGING      = 0x310;
            public const  int WM_PALETTECHANGED         = 0x311;
            public const  int WM_HOTKEY                 = 0x312;
            public const  int WM_PRINT                  = 0x317;
            public const  int WM_PRINTCLIENT            = 0x318;
            public const  int WM_APPCOMMAND             = 0x319;
            public const  int WM_THEMECHANGED           = 0x31a;
            public const  int WM_HANDHELDFIRST          = 0x358;
            public const  int WM_HANDHELDLAST           = 0x35f;
            public const  int WM_AFXFIRST               = 0x360;
            public const  int WM_AFXLAST                = 0x37f;
            public const  int WM_PENWINFIRST            = 0x380;
            public const  int WM_PENWINLAST             = 0x38f;
            public const  int WM_USER                   = 0x400;
            public const  int WM_REFLECT                = 0x2000;
            public const  int WM_APP                    = 0x8000;

            public const int WM_DWMCOMPOSITIONCHANGED   = 0x031E;

            public const int OCM__BASE                  = WM_USER + 0x1C00; // e.g. OCM_COMMAND = OCM__BASE + WM_COMMAND

            public const  int SC_MOVE                   = 0xF010;
            public const  int SC_MINIMIZE               = 0XF020;
            public const  int SC_MAXIMIZE               = 0xF030;
            public const  int SC_RESTORE                = 0xF120;
        }


        [Serializable, StructLayout(LayoutKind.Sequential)]
        struct SCROLLINFO
        {
            public uint cbSize;
            public uint fMask;
            public int  nMin;
            public int  nMax;
            public uint nPage;
            public int  nPos;
            public int  nTrackPos;
        }

        public enum ScrollInfoMask : uint
        {
            SIF_RANGE           = 0x1,
            SIF_PAGE            = 0x2,
            SIF_POS             = 0x4,
            SIF_DISABLENOSCROLL = 0x8,
            SIF_TRACKPOS        = 0x10,
            SIF_ALL             = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS),
        }

        public enum ScrollBarDirection : int
        {
            SB_HORZ             = 0x0,
            SB_VERT             = 0x1,
            SB_CTL              = 0x2,
            SB_BOTH             = 0x3
        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern IntPtr SetCapture(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr wnd, int msg, bool param, int lparam);


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowScrollBar(IntPtr hWnd, ScrollBarDirection wBar, [MarshalAs(UnmanagedType.Bool)] bool bShow);

        /*
            SCROLLINFO si = new SCROLLINFO();
            si.cbSize     = (uint)Marshal.SizeOf(si);
            si.fMask      = (int)ScrollInfoMask.SIF_ALL;

            GetScrollInfo(Handle, ScrollBarDirection.SB_VERT, ref si); 
        */
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetScrollInfo(IntPtr hwnd, ScrollBarDirection fnBar, ref SCROLLINFO lpsi);


        [StructLayout(LayoutKind.Sequential)]
        struct PAINTSTRUCT
        {
            public IntPtr      hdc;
            public bool        fErase;
            public Native.RECT rcPaint;
            public bool        fRestore;
            public bool        fIncUpdate;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] rgbReserved;
        }

        [DllImport("user32.dll")]
        static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        static extern bool EndPaint(IntPtr hWnd, [In] ref PAINTSTRUCT lpPaint);

        [DllImport("gdi32.dll")]
        static extern int SelectClipRgn(IntPtr hdc, IntPtr hrgn);

        [DllImport("user32.dll")]
        static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, [MarshalAs(UnmanagedType.Bool)] bool bErase);
    }
}
