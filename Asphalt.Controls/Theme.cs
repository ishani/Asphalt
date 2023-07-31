using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;

namespace Asphalt.Controls.Theme
{
    public enum Scheme
    {
        Base,
        Panel,

        KeyA,
        KeyB,
        KeyC,
        KeyD,
        KeyE,
        KeyF,
    }

    public enum Pigment
    {
        Primary,
        Analogous,
        Foreground
    }

    public enum Shade
    {
        Lighter,
        Default,
        Darker
    }

    public enum Thickness
    {
        Slim,
        Medium,
        Fat
    }


    // given a colour, generate a darker and lighter shade automatically, along with a standard brush
    public class Shades
    {
        public Shades( Color value, double offsetMult )
        {
            _Default = value;

            {
                Colour.HSV baseHSV = new Colour.HSV( _Default );
                baseHSV.OffsetV( offsetMult );

                _Lighter = baseHSV.ToColor();
            }
            {
                Colour.HSV baseHSV = new Colour.HSV( _Default );
                baseHSV.OffsetV( -offsetMult );

                _Darker = baseHSV.ToColor();
            }

            _DarkerBrush = new SolidBrush( _Darker );
            _DefaultBrush = new SolidBrush( _Default );
            _LighterBrush = new SolidBrush( _Lighter );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public Color Color(Shade shade)
        {
            switch ( shade )
            {
                case Shade.Darker:  return _Darker;
                case Shade.Default: return _Default;
                case Shade.Lighter: return _Lighter;
            }
            throw new ArgumentOutOfRangeException( "Shade" );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public SolidBrush SolidBrush( Shade shade )
        {
            switch ( shade )
            {
                case Shade.Darker:  return _DarkerBrush;
                case Shade.Default: return _DefaultBrush;
                case Shade.Lighter: return _LighterBrush;
            }
            throw new ArgumentOutOfRangeException( "Shade" );
        }


        private Color       _Darker;
        private SolidBrush  _DarkerBrush;

        private Color       _Default;
        private SolidBrush  _DefaultBrush;

        private Color       _Lighter;
        private SolidBrush  _LighterBrush;
    }


    // draw the colour result for a Shader in the editor UI
    public class UIPreviewShader : UIGenericCopyPaste
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            base.PaintValue(e);

            var Shading = (e.Value as Shader);
            if (Shading != null)
            {
                e.Graphics.FillRectangle(Theme.Current.Instance.SolidBrush(Shading), e.Bounds);
            }
        }
    }

    // Shader is a combination of [scheme / pigment / shade] that forms the endpoint for a data-driven colour choice
    [Editor(typeof(UIPreviewShader), typeof(UITypeEditor))]
    [TypeConverter( typeof( ExpandableObjectConverter ) )]
    public class Shader : INotifyPropertyChanged
    {
        private Scheme     _Scheme = Scheme.Base;
        private Pigment   _Pigment = Pigment.Primary;
        private Shade       _Shade = Shade.Default;

        public Shader()
        {
        }

        public Shader( Scheme scheme, Pigment pigment, Shade shade )
        {
            _Scheme     = scheme;
            _Pigment    = pigment;
            _Shade      = shade;
        }

        public override string ToString()
        {
            return "Shader";
        }

        #region Events

        private void ProcessSetProperty<T>( ref T storage, T v, string propertyName )
        {
            storage = v;
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        [DefaultValue( Scheme.Base )]
        public Scheme Scheme
        {
            get => _Scheme;
            set => ProcessSetProperty( ref _Scheme, value, nameof( Scheme ) );
        }

        [DefaultValue( Pigment.Primary )]
        public Pigment Pigment
        {
            get => _Pigment;
            set => ProcessSetProperty( ref _Pigment, value, nameof( Pigment ) );
        }

        [DefaultValue( Shade.Default )]
        public Shade Shade
        {
            get => _Shade;
            set => ProcessSetProperty( ref _Shade, value, nameof( Shade ) );
        }
    }


    // a blob that holds settings for how we draw the background of a control
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Canvas : INotifyPropertyChanged
    {
        public enum Mode
        {
            PanelShader,
            Auto,
            HorizontalGradient,
            VerticalGradient
        }

        private Mode   _Mode;

        [DefaultValue( Mode.PanelShader )]
        public Mode     Background
        {
            get => _Mode;
            set => ProcessSetProperty( ref _Mode, value, "Scheme" );
        }

        public Shader   GradientBegin  { get; set; } = new Shader();
        public Shader   GradientEnd    { get; set; } = new Shader();


        public Canvas()
        {
            GradientBegin.Scheme           = Scheme.KeyA;
            GradientBegin.Pigment          = Pigment.Primary;
            GradientEnd.Scheme             = Scheme.KeyA;
            GradientEnd.Pigment            = Pigment.Analogous;

            GradientBegin.PropertyChanged += Gradient_PropertyChanged;
            GradientEnd.PropertyChanged   += Gradient_PropertyChanged;
        }


        public override string ToString()
        {
            return "Canvas";
        }

        #region Events

        private void Gradient_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Gradient"));
        }

        private void ProcessSetProperty<T>( ref T storage, T v, string propertyName )
        {
            storage = v;
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public Brush GetGradientBrush( Rectangle client )
        {
            switch (_Mode)
            {
                case Mode.HorizontalGradient:
                    return new LinearGradientBrush( client, Current.Instance.Color(GradientBegin), Current.Instance.Color(GradientEnd), LinearGradientMode.Horizontal );
                case Mode.VerticalGradient:
                    return new LinearGradientBrush( client, Current.Instance.Color(GradientBegin), Current.Instance.Color(GradientEnd), LinearGradientMode.Vertical );

                default:
                    throw new InvalidEnumArgumentException("GetGradientBrush called when mode is not gradient");
            }
        }
    }


    public interface IHasBackgroundShader
    {
        Shader GetCurrentBackgroundShader();
    }


    public class SchemeInstance 
    {
        public SchemeInstance( Color primary, Color analogous, Color foreground, double shadeOffset )
        {
            _Primary    = new Shades( primary, shadeOffset);
            _Analogous  = new Shades( analogous, shadeOffset);
            _Foreground = new Shades( foreground, shadeOffset);
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public Color Color( Pigment scheme, Shade shade )
        {
            switch ( scheme )
            {
                case Pigment.Primary:       return _Primary.Color( shade );
                case Pigment.Analogous:     return _Analogous.Color( shade );
                case Pigment.Foreground:    return _Foreground.Color( shade );
            }
            throw new ArgumentOutOfRangeException( "Scheme" );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public SolidBrush SolidBrush( Pigment scheme, Shade shade )
        {
            switch ( scheme )
            {
                case Pigment.Primary:       return _Primary.SolidBrush( shade );
                case Pigment.Analogous:     return _Analogous.SolidBrush( shade );
                case Pigment.Foreground:    return _Foreground.SolidBrush( shade );
            }
            throw new ArgumentOutOfRangeException( "Scheme" );
        }

        private Shades   _Primary;
        private Shades   _Analogous;
        private Shades   _Foreground;
    }

    public class Current 
    {
        public enum Palette
        {
            Dark,
            Slate
        }

        private static readonly Lazy<Current> lazy = new Lazy<Current>( () => new Current( Palette.Dark ) );
        public static Current Instance => lazy.Value;


        private Dictionary<Scheme, SchemeInstance>    SchemeInstances = new Dictionary<Scheme, SchemeInstance>(8);

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public SchemeInstance Get( Scheme scheme )
        {
            if ( !SchemeInstances.ContainsKey( scheme ) )
                throw new ArgumentOutOfRangeException( "Scheme" );

            return SchemeInstances[scheme];
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public Color Color( Shader shader )
        {
            return Get( shader.Scheme ).Color( shader.Pigment, shader.Shade );
        }

        // shortcut to a brush
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public SolidBrush SolidBrush( Shader shader )
        {
            return Get( shader.Scheme ).SolidBrush( shader.Pigment, shader.Shade );
        }

        // create a theme with the given palette choice
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public Current( Palette p )
        {
            switch ( p )
            {
                case Palette.Dark:  SetupDark();  break;
                case Palette.Slate: SetupSlate(); break;
            }
        }

        private void SetupDark()
        {
            SchemeInstances.Add( Scheme.Base,  new SchemeInstance( Colour.ConstantsErigon.Void_L,         Colour.ConstantsErigon.Void_D,         Colour.ConstantsErigon.DarkGray_L,    0.02f ) );
            SchemeInstances.Add( Scheme.Panel, new SchemeInstance( Colour.ConstantsErigon.Charcoal_D,     Colour.ConstantsErigon.Charcoal_L,     Colour.ConstantsErigon.LightGray_D,   0.08f ) );

            SetupCommonKeys();
        }

        private void SetupSlate()
        {
            SchemeInstances.Add( Scheme.Base,  new SchemeInstance( Colour.ConstantsSE.BlackPearl,         Colour.ConstantsSE.GoodNight,          Colour.ConstantsErigon.DarkGray_L,    0.05f ) );
            SchemeInstances.Add( Scheme.Panel, new SchemeInstance( Colour.ConstantsGB.Electromagnetic,    Colour.ConstantsGB.BlueNights,         Colour.ConstantsFlatUI.Clouds,        0.1f ) );

            SetupCommonKeys();
        }

        private void SetupCommonKeys()
        {
            SchemeInstances.Add( Scheme.KeyA,  new SchemeInstance( Colour.ConstantsFlatUI.Pumpkin,        Colour.ConstantsFlatUI.Sunflower,      Colour.ConstantsErigon.LightGray_L,   0.15f ) );
            SchemeInstances.Add( Scheme.KeyB,  new SchemeInstance( Colour.ConstantsMetro.Magenta,         Colour.ConstantsMetro.Indigo,          Colour.ConstantsErigon.LightGray_L,   0.15f ) );
            SchemeInstances.Add( Scheme.KeyC,  new SchemeInstance( Colour.ConstantsGB.VanadylBlue,        Colour.ConstantsMetro.Cobalt,          Colour.ConstantsErigon.LightGray_L,   0.15f ) );
            SchemeInstances.Add( Scheme.KeyD,  new SchemeInstance( Colour.ConstantsFlatUI.MidnightBlue,   Colour.ConstantsErigon.Mint_D,         Colour.ConstantsErigon.LightGray_L,   0.20f ) );
            SchemeInstances.Add( Scheme.KeyE,  new SchemeInstance( Colour.ConstantsNL.Circumorbital,      Colour.ConstantsNL.LavenderTea,        Colour.ConstantsErigon.LightGray_L,   0.15f ) );
            SchemeInstances.Add( Scheme.KeyF,  new SchemeInstance( Colour.ConstantsGB.SkirretGreen,       Colour.ConstantsNL.PixelatedGrass,     Colour.ConstantsErigon.LightGray_L,   0.20f ) );
        }
    }
}
