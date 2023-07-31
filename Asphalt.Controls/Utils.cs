using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Asphalt.Controls
{
    internal class PlacementResult
    {
        public PointF PositionTopLeft;
        public PointF PositionCenter;
        public SizeF  Size;

        #region RoundedIntegerProperties

        public Point IntPositionTopLeft => new Point( (int) PositionTopLeft.X, (int) PositionTopLeft.Y );
        public Point IntPositionCenter  => new Point( (int) PositionCenter.X,  (int) PositionCenter.Y );
        public Size  IntSize            => new Size(  (int) Size.Width,        (int) Size.Height );

        #endregion
    }

    // public utility functions are exposed in 'Tools'
    public static class Tools
    {
        public static RectangleF AspectFitImage( float sourceW, float sourceH, float destW, float destH )
        {
            float scale = Math.Min(destW / sourceW, destH / sourceH);

            float drawW = sourceW * scale;
            float drawH = sourceH * scale;

            float offX = (destW / 2) - (drawW / 2);
            float offY = (destH / 2) - (drawH / 2);

            return new RectangleF(offX, offY, drawW, drawH);
        }
    }

    // library-internal utilities are kept in the unexposed 'Utils'
    internal static class Utils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public static TextFormatFlags AlignmentToTextFormat(ContentAlignment alignment)
        {
            switch (alignment)
            {
                case ContentAlignment.TopLeft: return TextFormatFlags.Left | TextFormatFlags.Top;
                case ContentAlignment.TopCenter: return TextFormatFlags.HorizontalCenter | TextFormatFlags.Top;
                case ContentAlignment.TopRight: return TextFormatFlags.Right | TextFormatFlags.Top;

                case ContentAlignment.MiddleLeft: return TextFormatFlags.Left | TextFormatFlags.VerticalCenter;
                case ContentAlignment.MiddleCenter: return TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                case ContentAlignment.MiddleRight: return TextFormatFlags.Right | TextFormatFlags.VerticalCenter;

                case ContentAlignment.BottomLeft: return TextFormatFlags.Left | TextFormatFlags.Bottom;
                case ContentAlignment.BottomCenter: return TextFormatFlags.HorizontalCenter | TextFormatFlags.Bottom;
                case ContentAlignment.BottomRight: return TextFormatFlags.Right | TextFormatFlags.Bottom;

                default:
                    throw new ArgumentOutOfRangeException(nameof(alignment), alignment, "invalid ContentAlignment value");
            }
        }

        public static PointF CalculateAlignmentOverlap(ContentAlignment icon, ContentAlignment txta)
        {
            PointF overlaps = new Point(0, 0);

            switch (icon)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    overlaps.X = 1.0f;
                    break;

                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    overlaps.X = -1.0f;
                    break;

                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    break;

                default:
                    break;
            }

            switch (icon)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    if (txta != ContentAlignment.BottomLeft && txta != ContentAlignment.BottomCenter && txta != ContentAlignment.BottomRight)
                    {
                        overlaps.Y = 1.0f;
                    }
                    break;

                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    if (txta == ContentAlignment.BottomLeft || txta == ContentAlignment.BottomCenter || txta == ContentAlignment.BottomRight)
                    {
                        overlaps.Y = -1.0f;
                    }
                    break;

                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    break;

                default:
                    break;
            }

            return overlaps;
        }

        public static PointF GetCenterPointForTextAlignment(ContentAlignment alignment, SizeF measuredText, RectangleF canvasRect)
        {
            SizeF measuredTextHalf = new SizeF(measuredText.Width * 0.5f, measuredText.Height * 0.5f);
            SizeF paddedRectHalf = new SizeF(canvasRect.Width * 0.5f, canvasRect.Height * 0.5f);

            PointF textCenter = new PointF();
            switch (alignment)
            {
                case ContentAlignment.TopLeft: textCenter = new PointF(0 + measuredTextHalf.Width, 0 + measuredTextHalf.Height); break;
                case ContentAlignment.TopCenter: textCenter = new PointF(paddedRectHalf.Width, 0 + measuredTextHalf.Height); break;
                case ContentAlignment.TopRight: textCenter = new PointF(canvasRect.Width - measuredTextHalf.Width, 0 + measuredTextHalf.Height); break;

                case ContentAlignment.MiddleLeft: textCenter = new PointF(0 + measuredTextHalf.Width, paddedRectHalf.Height); break;
                case ContentAlignment.MiddleCenter: textCenter = new PointF(paddedRectHalf.Width, paddedRectHalf.Height); break;
                case ContentAlignment.MiddleRight: textCenter = new PointF(canvasRect.Width - measuredTextHalf.Width, paddedRectHalf.Height); break;

                case ContentAlignment.BottomLeft: textCenter = new PointF(0 + measuredTextHalf.Width, canvasRect.Height - measuredTextHalf.Height); break;
                case ContentAlignment.BottomCenter: textCenter = new PointF(paddedRectHalf.Width, canvasRect.Height - measuredTextHalf.Height); break;
                case ContentAlignment.BottomRight: textCenter = new PointF(canvasRect.Width - measuredTextHalf.Width, canvasRect.Height - measuredTextHalf.Height); break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(alignment), alignment, "invalid ContentAlignment value");
            }

            textCenter.X += canvasRect.X;
            textCenter.Y += canvasRect.Y;

            return textCenter;
        }

        public static RectangleF GetPaddedRectangle(Control control)
        {
            var rect = control.ClientRectangle;
            var pad = control.Padding;
            return new RectangleF(rect.X + pad.Left,
                                   rect.Y + pad.Top,
                                   rect.Width - (pad.Left + pad.Right),
                                   rect.Height - (pad.Top + pad.Bottom));
        }
    }

    public static class Prop
    { 
        public static void Exchange<T>( ref T val, T incoming, IPropertyEvents ipr ) where T : INotifyPropertyChanged
        {
            if ( val != null )
                val.PropertyChanged -= ipr.OnChange;

            val = incoming;
            val.PropertyChanged += ipr.OnChange;

            ipr.OnChange(null, null);
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void TriggerChange<T>( T type ) where T : IPropertyEvents
        {
            type.OnChange(type, null);
        }
    }

    public interface IPropertyEvents
    {
        void OnChange( object sender, PropertyChangedEventArgs e );
    }
}
