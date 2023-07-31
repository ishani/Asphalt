using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;

namespace Asphalt.Controls.Colour
{
    // https://www.materialui.co/flatuicolors
    public static class ConstantsFlatUI
    {
        public static readonly Color Turqoise           = Color.FromArgb( 26,188,156 );
        public static readonly Color Emerland           = Color.FromArgb( 46,204,113 );
        public static readonly Color PeterRiver         = Color.FromArgb( 52,152,219 );
        public static readonly Color Amethyst           = Color.FromArgb( 155,89,182 );
        public static readonly Color WetAsphalt         = Color.FromArgb( 52,73,94 );
                                                                          
        public static readonly Color GreenSea           = Color.FromArgb( 22,160,133 );
        public static readonly Color Nephritis          = Color.FromArgb( 39,174,96 );
        public static readonly Color BelizeHole         = Color.FromArgb( 41,128,185 );
        public static readonly Color Wisteria           = Color.FromArgb( 142,68,173 );
        public static readonly Color MidnightBlue       = Color.FromArgb( 44,62,80 );
                                                                          
        public static readonly Color Sunflower          = Color.FromArgb( 241,196,15 );
        public static readonly Color Carrot             = Color.FromArgb( 230,126,34 );
        public static readonly Color Alizarin           = Color.FromArgb( 231,76,60 );
        public static readonly Color Clouds             = Color.FromArgb( 236,240,241 );
        public static readonly Color Concrete           = Color.FromArgb( 149,165,166 );
                                                                          
        public static readonly Color Orange             = Color.FromArgb( 243,156,18 );
        public static readonly Color Pumpkin            = Color.FromArgb( 211,84,0 );
        public static readonly Color Pomegranate        = Color.FromArgb( 192,57,43 );
        public static readonly Color Silver             = Color.FromArgb( 189,195,199 );
        public static readonly Color Asbestos           = Color.FromArgb( 127,140,141 );
    }

    // https://www.materialui.co/metrocolors
    public static class ConstantsMetro
    {
        public static readonly Color Lime               = Color.FromArgb( 164,196,0 );
        public static readonly Color Green              = Color.FromArgb( 96,169,23 );
        public static readonly Color Emerald            = Color.FromArgb( 0,138,0 );
        public static readonly Color Teal               = Color.FromArgb( 0,171,169 );
        public static readonly Color Cyan               = Color.FromArgb( 27,161,226 );
        public static readonly Color Cobalt             = Color.FromArgb( 0,80,239 );
        public static readonly Color Indigo             = Color.FromArgb( 106,0,255 );
                                              
        public static readonly Color Violet             = Color.FromArgb( 170,0,255 );
        public static readonly Color Pink               = Color.FromArgb( 244,114,208 );
        public static readonly Color Magenta            = Color.FromArgb( 216,0,115 );
        public static readonly Color Crimson            = Color.FromArgb( 162,0,37 );
        public static readonly Color Red                = Color.FromArgb( 229,20,0 );
        public static readonly Color Orange             = Color.FromArgb( 250,104,0 );
        public static readonly Color Amber              = Color.FromArgb( 240,163,10 );
    }

    // https://flatuicolors.com/palette/gb
    public static class ConstantsGB
    {
        public static readonly Color ProtossPylon       = Color.FromArgb( 0, 168, 255 );
        public static readonly Color VanadylBlue        = Color.FromArgb( 0, 151, 230 );

        public static readonly Color DownloadProgress   = Color.FromArgb( 76, 209, 55 );
        public static readonly Color SkirretGreen       = Color.FromArgb( 68, 189, 50 );

        public static readonly Color NasturcianFlower   = Color.FromArgb( 232, 65, 24 );
        public static readonly Color HarleyOrange       = Color.FromArgb( 194, 54, 22 );

        public static readonly Color MazarineBlue       = Color.FromArgb( 39, 60, 117 );
        public static readonly Color PicoVoid           = Color.FromArgb( 25, 42, 86 );

        public static readonly Color BlueNights         = Color.FromArgb( 53, 59, 72 );
        public static readonly Color Electromagnetic    = Color.FromArgb( 47, 54, 64 );
    }

    // https://flatuicolors.com/palette/us
    public static class ConstantsUS
    {
        public static readonly Color ChiGong           = Color.FromArgb( 214, 48, 49 );

        public static readonly Color PrunusAvium       = Color.FromArgb( 232, 67, 147 );

        public static readonly Color AmericanRiver     = Color.FromArgb( 99, 110, 114 );
        public static readonly Color DraculaOrchid     = Color.FromArgb( 45, 52, 54 );
    }

    // https://flatuicolors.com/palette/nl
    public static class ConstantsNL
    {
        public static readonly Color PixelatedGrass    = Color.FromArgb( 0, 148, 50 );

        public static readonly Color MerchantMarine    = Color.FromArgb( 6, 82, 221 );

        public static readonly Color LavenderTea       = Color.FromArgb( 217, 128, 250 );
        public static readonly Color ForgottenPurple   = Color.FromArgb( 153, 128, 250 );
        public static readonly Color Circumorbital     = Color.FromArgb( 87, 88, 187 );

        public static readonly Color VeryBerry         = Color.FromArgb( 181, 52, 113 );

        public static readonly Color Hollyhock         = Color.FromArgb( 131, 52, 113 );
        public static readonly Color MagentaPurple     = Color.FromArgb( 111, 30, 81 );
    }

    // https://flatuicolors.com/palette/ru
    public static class ConstantsRU
    {
        public static readonly Color PencilLead        = Color.FromArgb( 89, 98, 117 );
        public static readonly Color Biscay            = Color.FromArgb( 48, 57, 82 );
    }

    // https://flatuicolors.com/palette/se
    public static class ConstantsSE
    {
        public static readonly Color GreenTeal         = Color.FromArgb( 5, 196, 107 );

        public static readonly Color ChromeYellow      = Color.FromArgb( 255, 168, 1 );

        public static readonly Color GoodNight         = Color.FromArgb( 72, 84, 96 );
        public static readonly Color BlackPearl        = Color.FromArgb( 30, 39, 46 );
    }

    // https://codepen.io/devi8/pen/lvIeh ( by Chris Cifonie )
    public static class ConstantsErigon
    {
        public static readonly Color Grapefruit_L       = Color.FromArgb( 238, 88, 100 );
        public static readonly Color Grapefruit_D       = Color.FromArgb( 219, 71, 82 );
        public static readonly Color Bittersweet_L      = Color.FromArgb( 253, 113, 80 );
        public static readonly Color Bittersweet_D      = Color.FromArgb( 234, 90, 62 );
        public static readonly Color Sunflower_L        = Color.FromArgb( 253, 209, 84 );
        public static readonly Color Sunflower_D        = Color.FromArgb( 244, 190, 66 );
        public static readonly Color Grass_L            = Color.FromArgb( 155, 214, 104 );
        public static readonly Color Grass_D            = Color.FromArgb( 135, 195, 83 );
                                              
        public static readonly Color Mint_L             = Color.FromArgb( 64, 207, 173 );
        public static readonly Color Mint_D             = Color.FromArgb( 46, 188, 155 );
        public static readonly Color Aqua_L             = Color.FromArgb( 82, 191, 233 );
        public static readonly Color Aqua_D             = Color.FromArgb( 64, 173, 218 );
        public static readonly Color BlueJeans_L        = Color.FromArgb( 100, 152, 236 );
        public static readonly Color BlueJeans_D        = Color.FromArgb( 82, 133, 220 );
        public static readonly Color Lavender_L         = Color.FromArgb( 176, 143, 236 );
        public static readonly Color Lavender_D         = Color.FromArgb( 155, 118, 220 );

        public static readonly Color PinkRose_L         = Color.FromArgb( 238, 134, 192 );
        public static readonly Color PinkRose_D         = Color.FromArgb( 217, 111, 173 );
        public static readonly Color LightGray_L        = Color.FromArgb( 245, 247, 250 );
        public static readonly Color LightGray_D        = Color.FromArgb( 230, 233, 237 );
        public static readonly Color MediumGray_L       = Color.FromArgb( 204, 209, 217 );
        public static readonly Color MediumGray_D       = Color.FromArgb( 170, 178, 189 );
        public static readonly Color DarkGray_L         = Color.FromArgb( 101, 108, 120 );
        public static readonly Color DarkGray_D         = Color.FromArgb( 67, 73, 84 );

        public static readonly Color Teal_L             = Color.FromArgb( 160, 206, 203 );
        public static readonly Color Teal_D             = Color.FromArgb( 125, 177, 177 );
        public static readonly Color Straw_L            = Color.FromArgb( 232, 206, 77 );
        public static readonly Color Straw_D            = Color.FromArgb( 224, 195, 65 );
        public static readonly Color Plum_L             = Color.FromArgb( 128, 103, 183 );
        public static readonly Color Plum_D             = Color.FromArgb( 106, 80, 167 );
        public static readonly Color Ruby_L             = Color.FromArgb( 216, 51, 74 );
        public static readonly Color Ruby_D             = Color.FromArgb( 191, 38, 60 );
        public static readonly Color Charcoal_L         = Color.FromArgb( 60, 59, 61 );
        public static readonly Color Charcoal_D         = Color.FromArgb( 50, 49, 51 );

        public static readonly Color Void_L             = Color.FromArgb( 27, 27, 28 );
        public static readonly Color Void_D             = Color.FromArgb( 15, 14, 15 );
    }


    public abstract class ColourBase
    {
        // desaturation colour filter used to dim the control OnPaint() results when it is disabled
        static internal readonly ColorMatrix DisabledColorMatrix = new ColorMatrix( 
                                                                           new float[][] {
                                                                           new float[] { .3f  * 0.2f,  .3f * 0.2f,  .3f * 0.2f, 0, 0 },
                                                                           new float[] { .59f * 0.2f, .59f * 0.2f, .59f * 0.2f, 0, 0 },
                                                                           new float[] { .11f * 0.2f, .11f * 0.2f, .11f * 0.2f, 0, 0 },
                                                                           new float[] {           0,           0,           0, 1, 0 },
                                                                           new float[] {        0.1f,        0.1f,        0.1f, 0, 1 }
                                                                           });

        #region Events

        protected void ProcessSetProperty( ref double storage, in double v, in string pname )
        {
            if ( v < 0.0 || v > 1.0 )
                throw new ArgumentOutOfRangeException( pname, "must be between 0.0 and 1.0" );

            storage = v;

            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( pname ) );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    [TypeConverter( typeof( ExpandableObjectConverter ) )]
    public class RGB : ColourBase
    {
        #region Properties 

        private double   Red        = 0;
        private double   Green      = 0;
        private double   Blue       = 0;

        public double R  { get { return Red; }          set { ProcessSetProperty( ref Red, value, "Red" );                  } }
        public double G  { get { return Green; }        set { ProcessSetProperty( ref Green, value, "Green" );              } }
        public double B  { get { return Blue; }         set { ProcessSetProperty( ref Blue, value, "Blue" );                } }

        #endregion


        public override string ToString() => "RGB";

        public RGB()
        {
        }

        public RGB( in double inR, in double inG, in double inB )
        {
            R = inR;
            G = inG;
            B = inB;
        }

        public RGB( in Color from )
        {
            R = (double)from.R / 255.0;
            G = (double)from.G / 255.0;
            B = (double)from.B / 255.0;
        }

        #region Conversion

        public Color ToColor()
        {
            return Color.FromArgb(
                (int)( R * 255.0 ),
                (int)( G * 255.0 ),
                (int)( B * 255.0 ) );
        }

        public static RGB FromHSV(in HSV input)
        {
            return FromHSV(input.H, input.S, input.V);
        }

        // conversion from HSV to RGB
        public static RGB FromHSV( in double H, in double S, in double V )
        {
            RGB result = new RGB();

            double i = Math.Floor( H * 6.0 );

            double f = H * 6.0 - i;
            double p = V * ( 1.0 - S );
            double q = V * ( 1.0 - f * S );
            double t = V * ( 1.0 - ( 1.0 - f ) * S );

            switch ( (int)i % 6 )
            {
                case 0: result.R = V;       result.G = t;          result.B = p;        break;
                case 1: result.R = q;       result.G = V;          result.B = p;        break;
                case 2: result.R = p;       result.G = V;          result.B = t;        break;
                case 3: result.R = p;       result.G = q;          result.B = V;        break;
                case 4: result.R = t;       result.G = p;          result.B = V;        break;
                case 5: result.R = V;       result.G = p;          result.B = q;        break;
            }
            
            return result;
        }

        #endregion
    }

    [TypeConverter( typeof( ExpandableObjectConverter ) )]
    public class HSV : ColourBase
    {
        #region Properties 

        private double   Hue        = 0;
        private double   Saturation = 0;
        private double   Value      = 0;

        public double H  { get { return Hue; }          set { ProcessSetProperty( ref Hue, value, "Hue" );                  } }
        public double S  { get { return Saturation; }   set { ProcessSetProperty( ref Saturation, value, "Saturation" );    } }
        public double V  { get { return Value; }        set { ProcessSetProperty( ref Value, value, "Value" );              } }

        #endregion

 
        public override string ToString() => "HSV";

        public HSV()
        {
        }

        public HSV( in double inH, in double inS, in double inV )
        {
            H = inH;
            S = inS;
            V = inV;
        }

        public HSV( in Color from )
        {
            HSV converted = FromRGB( new RGB(from) );
            H = converted.H;
            S = converted.S;
            V = converted.V;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static HSV Lerp( in HSV lhs, in HSV rhs, in float t )
        {
            float invT = 1.0f - t;

            return new HSV(
                ( lhs.H * invT ) + ( rhs.H * t ),
                ( lhs.S * invT ) + ( rhs.S * t ),
                ( lhs.V * invT ) + ( rhs.V * t )
                );
        }

        #region Adjustments

        public void ScaleH( in double factor )
        {
            H = ( Hue * factor ).Saturate();
        }

        public void ScaleS( in double factor )
        {
            S = ( Saturation * factor ).Saturate();
        }

        public void ScaleV( in double factor )
        {
            V = ( Value * factor ).Saturate();
        }

        public void OffsetH( in double offset )
        {
            H = ( Hue + offset ).Saturate();
        }

        public void OffsetS( in double offset )
        {
            S = ( Saturation + offset ).Saturate();
        }

        public void OffsetV( in double offset )
        {
            V = ( Value + offset ).Saturate();
        }

        #endregion

        #region Conversion

        // convert to .NET Colour via conversion to RGB first
        public Color ToColor()
        {
            return RGB.FromHSV( this ).ToColor();
        }

        // conversion from RGB to HSV
        public static HSV FromRGB( in RGB input )
        {
            // extract the RGB values as we will modify them inline
            double input_R = input.R;
            double input_G = input.G;
            double input_B = input.B;

            HSV result = new HSV();

            const double avoidDbZ = 1e-20;

            double K = 0;

            if ( input_G < input_B )
            {
                Utils.Swap( ref input_G, ref input_B );
                K = -1.0;
            }

            if ( input_R < input_G )
            {
                Utils.Swap( ref input_R, ref input_G );
                K = -2.0 / 6.0 - K;
            }

            double chroma = input_R - Math.Min( input_G, input_B );

            result.H = Math.Abs( K + ( input_G - input_B ) / ( 6.0 * chroma + avoidDbZ ) );
            result.S = chroma / ( input_R + avoidDbZ );
            result.V = input_R;

            return result;
        }

        #endregion
    }
}
