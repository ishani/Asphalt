using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Asphalt.Controls
{
    public static class Extensions
    {
        // offset a rectangle without moving outside the original bounds
        // eg. +ve value will move the left/top edge (and shrink the width/height)
        //     -ve value will move the bottom/right edge by shrinking the size
        //
        public static void EdgeShift( this ref Rectangle r, int w, int h )
        {
            if ( w > 0 )
            {
                r.X += w;
                r.Width -= w;
            }
            else
            {
                r.Width += w;
            }

            if ( h > 0 )
            {
                r.Y += h;
                r.Height -= h;
            }
            else
            {
                r.Height += h;
            }
        }

        // as above, but for float-Rectangle
        public static void EdgeShift( this ref RectangleF r, float w, float h )
        {
            if ( w > 0 )
            {
                r.X += w;
                r.Width -= w;
            }
            else
            {
                r.Width += w;
            }

            if ( h > 0 )
            {
                r.Y += h;
                r.Height -= h;
            }
            else
            {
                r.Height += h;
            }
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static double Clamp( this double v, double min, double max )
        {
            return Math.Min( Math.Max( v, min ), max );
        }

        // clamp 0..1
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static double Saturate( this double v )
        {
            return Math.Min( Math.Max( v, 0.0 ), 1.0 );
        }

        // deep copy a control using reflection to move property data
        public static T Clone<T>( this T controlToClone ) where T : Control
        {
            PropertyInfo[] controlProperties = typeof(T).GetProperties( BindingFlags.Public | BindingFlags.Instance );

            T instance = Activator.CreateInstance<T>();

            foreach ( PropertyInfo propInfo in controlProperties )
            {
                if ( propInfo.CanWrite )
                {
                    if ( propInfo.Name != "WindowTarget" )
                        propInfo.SetValue( instance, propInfo.GetValue( controlToClone, null ), null );
                }
            }

            return instance;
        }
    }
}
