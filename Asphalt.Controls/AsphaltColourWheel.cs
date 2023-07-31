using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

using Asphalt.Controls.Colour;
using Asphalt.Controls.Theme;

namespace Asphalt.Controls
{
    public delegate void ColourWheelEventHandler(object sender, RGB current);

    [ToolboxItem(true)]
    public partial class AsphaltColourWheel : ControlWithoutDefaultAppearances, IPropertyEvents
    {
        private double                           _Hue = 0.0;
        private double                    _Saturation = 1.0;
        private double                         _Value = 1.0;

        private AsphaltScrollerShading       _Shading = null;

        private enum DragMode
        {
            None,
            Hue,
            SV
        }
        private DragMode                    _DragMode = DragMode.None;

        // size cache
        private Rectangle                    _HueRect;
        private Rectangle                     _SVRect;
        private LinearGradientBrush         _HueBrush;
        private PathGradientBrush            _SVBrush;


        #region Properties_Shading

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Asphalt.ColourPicker")]
        public AsphaltScrollerShading Shading
        {
            get { return _Shading; }
            set { Prop.Exchange(ref _Shading, value, this); }
        }

        void IPropertyEvents.OnChange(object sender, PropertyChangedEventArgs e)
        {
            Invalidate();
        }

        [DefaultValue(1.0)]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Asphalt.ColourPicker")]
        public double Hue
        {
            get { return _Hue; }
            set { _Hue = value; HSVChanged(); }
        }

        [DefaultValue(1.0)]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Asphalt.ColourPicker")]
        public double Saturation
        {
            get { return _Saturation; }
            set { _Saturation = value; HSVChanged(); }
        }

        [DefaultValue(1.0)]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Asphalt.ColourPicker")]
        public double Value
        {
            get { return _Value; }
            set { _Value = value; HSVChanged(); }
        }

        #endregion


        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Asphalt")]
        public event ColourWheelEventHandler ColourChanged;


        public RGB GetRGB()
        {
            return Colour.RGB.FromHSV(_Hue, _Saturation, _Value);
        }

        public void SetHSV(in double H, in double S, in double V)
        {
            _Hue = H;
            _Saturation = S;
            _Value = V;
            HSVChanged();
        }


        public AsphaltColourWheel()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true);

            Shading = new AsphaltScrollerShading();


            SuspendLayout();
            Name = "AsphaltColourWheel";
            Size = new System.Drawing.Size(256, 256);
            ResumeLayout(false);

            HSVChanged();
        }

        private void InvalidateSizeCache()
        {
            Int32 edgeGap   = 6;
            Int32 hueHeight = 32;
            Int32 midGap    = 8;

            _HueRect    = new Rectangle(
                edgeGap, 
                edgeGap, 
                ClientRectangle.Width - (edgeGap * 2), 
                hueHeight
                );

            _SVRect     = new Rectangle(
                edgeGap,
                _HueRect.Bottom + midGap, 
                ClientRectangle.Width - (edgeGap * 2), 
                ClientRectangle.Height - ( (edgeGap * 2) + (hueHeight + midGap) )
                );

            Int32 blendSlots = 12;
            double delta = 1.0 / (double)(blendSlots - 1);
            double hue = 0;

            ColorBlend cb = new ColorBlend
            {
                Positions = new float[blendSlots],
                Colors    = new Color[blendSlots]
            };

            for (int i = 0; i < blendSlots; i++)
            {
                cb.Positions[i] = (float)hue;
                cb.Colors[i] = Colour.RGB.FromHSV(hue, 1.0f, 1.0f).ToColor();

                hue += delta;
            }

            _HueBrush = new LinearGradientBrush( _HueRect, Color.Black, Color.Black, 0, false )
            {
                InterpolationColors = cb
            };


            _SVBrush = new PathGradientBrush( new PointF[4] {
                new PointF(_SVRect.Left, _SVRect.Top),
                new PointF(_SVRect.Right, _SVRect.Top),
                new PointF(_SVRect.Right, _SVRect.Bottom),
                new PointF(_SVRect.Left, _SVRect.Bottom)
            })
            {
                CenterPoint = new PointF( _SVRect.Left, _SVRect.Bottom ),
                CenterColor = Color.Black
            };
        }

        private void HSVChanged()
        {
            ColourChanged?.Invoke(this, GetRGB());
            Repaint();
        }

        private void Repaint()
        {
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            InvalidateSizeCache();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                
                if ( _SVRect.Contains(e.X, e.Y) )
                {
                    _DragMode = DragMode.SV;
                }
                else if (_HueRect.Contains(e.X, e.Y))
                {
                    _DragMode = DragMode.Hue;
                }

                if ( _DragMode != DragMode.None )
                    UpdateMouseInput(e.X, e.Y);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_DragMode != DragMode.None)
            {
                UpdateMouseInput(e.X, e.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _DragMode = DragMode.None;
        }

        private void UpdateMouseInput(Int32 X, Int32 Y)
        {
            if ( _DragMode == DragMode.SV )
            {
                X -= _SVRect.Left;
                Y -= _SVRect.Top;

                var newSat = (double)X / (double)_SVRect.Width;
                var newVal = (double)Y / (double)_SVRect.Height;

                newVal = 1.0 - newVal;

                Saturation = newSat.Saturate();
                Value = newVal.Saturate();
            }
            else if (_DragMode == DragMode.Hue)
            {
                X -= _HueRect.Left;
                var newHue = (double)X / (double)_HueRect.Width;

                Hue = newHue.Saturate();
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode   = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;


            var backgroundColour       = Current.Instance.Color(_Shading.PanelShader);
            var backgroundBrush        = Current.Instance.SolidBrush(_Shading.PanelShader);
            e.Graphics.Clear(backgroundColour);

            {
                e.Graphics.FillRectangle(_HueBrush, _HueRect);
            }
            {
                var fullSV = RGB.FromHSV(_Hue, 1.0, 1.0);
                _SVBrush.SurroundColors = new Color[3] { Color.White, fullSV.ToColor(), Color.Black };

                e.Graphics.FillRectangle(_SVBrush, _SVRect);
            }



            Int32 hueDotX   = _HueRect.Left + (Int32)Math.Round(_Hue * (double)_HueRect.Width);
            Int32 hueDotY   = _HueRect.Top  + (_HueRect.Height / 2);

            Int32 svDotX    = _SVRect.Left  + (Int32)Math.Round(_Saturation * (double)_SVRect.Width);
            Int32 svDotY    = _SVRect.Top   + (Int32)Math.Round( (1.0 - _Value) * (double)_SVRect.Height);


            e.Graphics.FillEllipse(backgroundBrush, hueDotX - 6,    hueDotY - 6,    12, 12);
            e.Graphics.FillEllipse(Brushes.White,   hueDotX - 3,    hueDotY - 3,    6,  6);

            e.Graphics.FillEllipse(backgroundBrush, svDotX - 6,     svDotY - 6,     12, 12);
            e.Graphics.FillEllipse(Brushes.White,   svDotX - 3,     svDotY - 3,     6,  6);
        }

        public static void PopupAt( in Point popupPoint, Color startColour, ColourWheelEventHandler changeEvent )
        {
            var popUp = new AsphaltForm
            {
                StartPosition = FormStartPosition.Manual,
                ShowInTaskbar = false,
                TopMost       = true,
                Location      = popupPoint,
            };
            popUp.SuspendLayout();

            var wheel = new AsphaltColourWheel
            {
                TabStop       = false,
                Size          = new Size(220, 220),
                Location      = new Point(2, 4),
                Margin        = new Padding(0),
                Padding       = new Padding(0)
            };
            wheel.ColourChanged += changeEvent;

            popUp.Controls.Add(wheel);

            var pickInput = new Controls.Colour.HSV(startColour);
            wheel.SetHSV(pickInput.H, pickInput.S, pickInput.V);

            // auto-size the form to fit the layout
            popUp.AutoSize = true;
            popUp.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // show it, and close it if we lose focus
            popUp.ResumeLayout();
            popUp.Show();
            popUp.Focus();
            popUp.LostFocus += (o, ev) =>
            {
                (o as AsphaltForm).Close();
            };
        }
    }
}
