using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Windows.Input;

using Asphalt.Controls.Theme;
using System.Drawing.Design;

namespace Asphalt.Controls
{
    public delegate void PressedChangeEventHandler( object sender, bool previous, bool current );

    [ToolboxItem( true )]
    public class AsphaltPanel : ControlWithNewRendering, IPropertyEvents, IHasBackgroundShader
    {
        // the Panel can do a lot of different things
        public enum ActionMode
        {
            None,       // just display stuff, no reactive behavior
            Button,     // press and release button
            Toggle,     // toggle button, also check-box
            Radio,      // one-choice-per-group button, arranged by tag
            Collapser   // special action for controlling a collapsible container
        }

        private bool                  _ParentHasChanged = false;
        private bool                   _StateIsHovering = false;
        private bool                   _StateIsMouseDown = false;
        private bool                         _IsPressed = false;
        private int              _CachedContainerHeight = -1;

        private Int32                       _RadioGroup = 1;
        private List<AsphaltPanel>    _CachedRadioGroup = new List<AsphaltPanel>();

        private FontChoice                    _TextFont = FontChoice.Regular;
        private FontSize                      _TextSize = FontSize.Normal;
        private ContentAlignment             _TextAlign = ContentAlignment.MiddleCenter;
        private SizeF                       _TextOffset = new SizeF(0,0);
        private float                   _TextShadowSize = 0.0f;
        private Shader                _TextShadowShader = null;

        private IconID                          _IconID = IconID.None;
        private IconID                   _ToggledIconID = IconID.None;
        private FontSize                      _IconSize = FontSize.Normal;
        private bool                 _IconConnectToText = true;
        private ContentAlignment             _IconAlign = ContentAlignment.MiddleCenter;
        private SizeF                      _IconPadding = new SizeF(0,0);
        private SizeF                       _IconOffset = new SizeF(0,0);

        private AsphaltIconAdornment     _IconAdornment;

        private bool                    _IconFineTuning = false;
        private SizeF                  _IconTuneFixSize = new SizeF(34,34);
        private Point                    _IconTuneShift = new Point(0,0);

        private AnchorStyles                 _PanelEdge = AnchorStyles.Bottom;
        private Thickness           _PanelEdgeThickness = Thickness.Medium;
        private bool                 _PanelEdgeOverlaps = false;
        private Canvas                          _Canvas = new Canvas();

        private Image                       _PanelImage = null;


        private AsphaltPanelShading       _PanelShading;
        private AsphaltPanelShading  _PanelShadingHover;
        private AsphaltPanelShading   _PanelShadingDown;


        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Asphalt")]
        public event MouseEventHandler PanelClicked;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Asphalt")]
        public event PressedChangeEventHandler PressedChanged;


        #region Properties_Action

        [DefaultValue( false )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Action" )]
        public bool ReactToHover { get; set; } = false;

        [DefaultValue( ActionMode.None )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Action" )]
        public ActionMode Action { get; set; } = ActionMode.None;

        [DefaultValue( 1 )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Action" )]
        public Int32 RadioGroup
        {
            get { return _RadioGroup; }
            set {
                _RadioGroup = value;
                _CachedRadioGroup.Clear();
            }
        }

        [DefaultValue( false )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Action" )]
        public bool EdgeFlipOnPress { get; set; } = false;

        #endregion

        #region Properties_State

        [DefaultValue( false )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.State" )]
        public bool Pressed
        {
            get { return _IsPressed; }
            set {
                // only do anything if the value is actually changing
                if ( _IsPressed != value )
                {
                    bool previousValue = _IsPressed;
                    _IsPressed = value;

                    HandlePressedChangeActions();

                    PressedChanged?.Invoke( this, previousValue, _IsPressed );
                }
            }
        }

        #endregion

        #region Properties_Shading

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel" )]
        public AsphaltPanelShading PanelShading
        {
            get { return _PanelShading; }
            set { Prop.Exchange( ref _PanelShading, value, this ); }
        }

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel" )]
        public AsphaltPanelShading PanelShadingHover
        {
            get { return _PanelShadingHover; }
            set { Prop.Exchange( ref _PanelShadingHover, value, this ); }
        }

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel" )]
        public AsphaltPanelShading PanelShadingDown
        {
            get { return _PanelShadingDown; }
            set { Prop.Exchange( ref _PanelShadingDown, value, this ); }
        }

        #endregion

        #region Properties_Text

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Text" )]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; Repaint(); }
        }

        [DefaultValue( FontChoice.Regular )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Text" )]
        public FontChoice TextFont
        {
            get { return _TextFont; }
            set { _TextFont = value; Repaint(); }
        }

        [DefaultValue( FontSize.Normal )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Text" )]
        public FontSize TextSize
        {
            get { return _TextSize; }
            set { _TextSize = value; Repaint(); }
        }

        [DefaultValue( ContentAlignment.MiddleCenter )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Text" )]
        public ContentAlignment TextAlign
        {
            get { return _TextAlign; }
            set { _TextAlign = value; Repaint(); }
        }

        [DefaultValue( typeof( SizeF ), "0, 0" )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Text" )]
        public SizeF TextOffset
        {
            get => _TextOffset;
            set { _TextOffset = value; Repaint(); }
        }

        [DefaultValue( 0.0f )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Text" )]
        public float TextShadowSize
        {
            get => _TextShadowSize;
            set { _TextShadowSize = value; Repaint(); }
        }

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Text" )]
        public Shader TextShadowShader
        {
            get => _TextShadowShader;
            set { Prop.Exchange( ref _TextShadowShader, value, this ); }
        } 

        #endregion

        #region Properties_Icon

        [DefaultValue( IconID.None )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Icon" )]
        public IconID IconID
        {
            get { return _IconID; }
            set { _IconID = value; Repaint(); }
        }

        [DefaultValue( IconID.None )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Icon" )]
        public IconID ToggledIconID
        {
            get { return _ToggledIconID; }
            set { _ToggledIconID = value; Repaint(); }
        }

        [DefaultValue( FontSize.Normal )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Icon" )]
        public FontSize IconSize
        {
            get { return _IconSize; }
            set { _IconSize = value; Repaint(); }
        }

        [DefaultValue( true )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Icon" )]
        public bool IconConnectToText
        {
            get { return _IconConnectToText; }
            set { _IconConnectToText = value; Repaint(); }
        }

        [DefaultValue( ContentAlignment.MiddleCenter )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Icon" )]
        public ContentAlignment IconAlign
        {
            get { return _IconAlign; }
            set { _IconAlign = value; Repaint(); }
        }

        [DefaultValue( typeof( SizeF ), "0, 0" )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Icon" )]
        public SizeF IconPadding
        {
            get { return _IconPadding; }
            set { _IconPadding = value; Repaint(); }
        }

        [DefaultValue( typeof( SizeF ), "0, 0" )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Icon" )]
        public SizeF IconOffset
        {
            get { return _IconOffset; }
            set { _IconOffset = value; Repaint(); }
        }

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Icon" )]
        public AsphaltIconAdornment IconAdornment
        {
            get { return _IconAdornment; }
            set { Prop.Exchange(ref _IconAdornment, value, this); }
        }

        #endregion

        #region Properties_Icon_Tuning

        [DefaultValue( false )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Icon.Tuning" )]
        public bool IconFineTuning
        {
            get { return _IconFineTuning; }
            set { _IconFineTuning = value; Repaint(); }
        }

        [DefaultValue( typeof( SizeF ), "34, 34" )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Icon.Tuning" )]
        public SizeF IconTuneFixSize
        {
            get { return _IconTuneFixSize; }
            set { _IconTuneFixSize = value; Repaint(); }
        }

        [DefaultValue( typeof( Point ), "0, 0" )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel.Icon.Tuning" )]
        public Point IconTuneShift
        {
            get { return _IconTuneShift; }
            set { _IconTuneShift = value; Repaint(); }
        }

        #endregion

        #region Properties_Panel

        [DefaultValue( AnchorStyles.Bottom )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel" )]
        public AnchorStyles Edges
        {
            get { return _PanelEdge; }
            set { _PanelEdge = value; Repaint(); }
        }

        [DefaultValue( Thickness.Medium )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel" )]
        public Thickness EdgeThickness
        {
            get { return _PanelEdgeThickness; }
            set { _PanelEdgeThickness = value; Repaint(); }
        }

        [DefaultValue( false )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel" )]
        public bool PanelEdgeOverlaps
        {
            get { return _PanelEdgeOverlaps; }
            set { _PanelEdgeOverlaps = value; Repaint(); }
        }

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Panel" )]
        public Canvas Canvas
        {
            get { return _Canvas; }
            set { Prop.Exchange( ref _Canvas, value, this ); }
        }

        #endregion

        #region Properties_Image

        [DefaultValue( null )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Image" )]
        public Image Image
        {
            get { return _PanelImage; }
            set { _PanelImage = value; Repaint(); }
        }

        #endregion

        #region Properties_DesignerOnly

        [DefaultValue( false )]
        [Localizable( false ), DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        [DesignOnly( true ), EditorBrowsable( EditorBrowsableState.Never ), Category( "Asphalt.Debug" )]
        public bool DesignTestHovering { get; set; }

        #endregion


        void IPropertyEvents.OnChange( object sender, PropertyChangedEventArgs e )
        {
            Repaint();
        }

        // internal actions to perform when the Pressed state changes; ie. modifying other
        // radio group members, changing the collapsable panels etc
        private void HandlePressedChangeActions()
        {
            if ( Action != ActionMode.None && Enabled )
            {
                if ( Action == ActionMode.Toggle )
                {
                }
                else if ( Action == ActionMode.Collapser )
                {
                    HandleCollapser();
                }
                else if ( Action == ActionMode.Radio )
                {
                    // only toggle items in the radio group if we activating this panel
                    if ( Pressed )
                    {
                        // if we haven't cached which other controls are in the group, do so
                        if ( _CachedRadioGroup.Count == 0 )
                            UpdateRadioGroup();

                        // unpress each one
                        foreach ( AsphaltPanel ap in _CachedRadioGroup )
                        {
                            if ( ap != this )
                            {
                                ap.Pressed = false;
                            }
                        }
                    }
                }

                Repaint();
            }
        }

        // collapsers toggle the height of a parent container, packing it down to the height of the button
        // .. the idea being you have a collapser-panel at the top of the container
        private void HandleCollapser()
        {
            var hostContainer = ( Parent as AsphaltContainer );
            if ( hostContainer != null )
            {
                if ( Pressed )
                {
                    _CachedContainerHeight = hostContainer.Height;
                    hostContainer.Height   = Height;
                }
                else
                {
                    if ( _CachedContainerHeight < 0 )
                        throw new Exception( "didn't cache container height for collapser" );

                    hostContainer.Height   = _CachedContainerHeight;
                    _CachedContainerHeight = -1;
                }

                hostContainer.PerformLayout();
            }
        }

        public void UpdateRadioGroup()
        {
            if ( Parent == null )
                return;

            _CachedRadioGroup.Clear();
            foreach ( var ct in Parent.Controls )
            {
                if ( ct is AsphaltPanel ap &&
                     ap.Action == ActionMode.Radio &&
                     ap.RadioGroup == RadioGroup )
                {
                    _CachedRadioGroup.Add(ap);
                }
            }
        }


        internal AsphaltPanelShading GetCurrentShading()
        {
            if ( Pressed || _StateIsMouseDown )
                return PanelShadingDown;

            if ( ( _StateIsHovering && ReactToHover ) || ( this.DesignMode && DesignTestHovering ) )
                return PanelShadingHover;

            return PanelShading;
        }

        Shader IHasBackgroundShader.GetCurrentBackgroundShader()
        {
            return GetCurrentShading().PanelShader;
        }

        public AsphaltPanel()
        {
            SetDefaultControlStyles();

            Name = "AsphaltPanel";
            Size = new System.Drawing.Size( 270, 130 );

            PaddingChanged   += AsphaltPanel_InvalidateOnPropertyChange;
            MarginChanged    += AsphaltPanel_InvalidateOnPropertyChange;


            Canvas            = new Canvas();
            IconAdornment     = new AsphaltIconAdornment();
            PanelShading      = new AsphaltPanelShading();
            PanelShadingHover = new AsphaltPanelShading();
            PanelShadingDown  = new AsphaltPanelShading();
            TextShadowShader  = new Shader( Scheme.Base, Pigment.Primary, Shade.Darker );
        }

        protected override void OnParentChanged( EventArgs e )
        {
            base.OnParentChanged( e );

            _ParentHasChanged = true;
        }

        protected override void OnLayout( LayoutEventArgs levent )
        {
            base.OnLayout( levent );

            if ( _ParentHasChanged && !DesignMode )
            {
                if ( ( Action == ActionMode.Button ) && Pressed )
                {
                    Pressed = false;
                }

                bool StartCollapsed = ( Action == ActionMode.Collapser && Pressed );
                if ( StartCollapsed )
                {
                    HandleCollapser();
                }

                _ParentHasChanged = false;
            }
        }


        #region EventHandlers

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if ( Action != ActionMode.None && Enabled )
            {
                _StateIsMouseDown = false;

                if ( this.ClientRectangle.Contains( e.Location ) )
                {
                    if ( Action == ActionMode.Toggle )
                    {
                        Pressed = !Pressed;
                    }
                    else if ( Action == ActionMode.Collapser )
                    {
                        Pressed = !Pressed;
                    }
                    else if ( Action == ActionMode.Radio )
                    {
                        Pressed = true;
                    }

                    PanelClicked?.Invoke( this, e );
                }

                Repaint();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if ( Action != ActionMode.None && Enabled )
            {
                _StateIsMouseDown = true;
                Repaint();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            if ( !_StateIsHovering && Enabled )
            {
                _StateIsHovering = true;
                Repaint();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if ( _StateIsHovering && Enabled )
            {
                _StateIsHovering = false;
                Repaint();
            }
        }

        private void InvalidateOn_PropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            Repaint();
        }

        private void AsphaltPanel_InvalidateOnPropertyChange( object sender, EventArgs e )
        {
            Repaint();
        }

        #endregion


        protected AnchorStyles GetEdgeFlags()
        {
            AnchorStyles styleResult = AnchorStyles.None;

            if ( EdgeFlipOnPress && ( Pressed || _StateIsMouseDown) )
            {
                if ( _PanelEdge.HasFlag( AnchorStyles.Top ) )
                    styleResult |= AnchorStyles.Bottom;

                if ( _PanelEdge.HasFlag( AnchorStyles.Bottom ) )
                    styleResult |= AnchorStyles.Top;

                if ( _PanelEdge.HasFlag( AnchorStyles.Left ) )
                    styleResult |= AnchorStyles.Right;

                if ( _PanelEdge.HasFlag( AnchorStyles.Right ) )
                    styleResult |= AnchorStyles.Left;
            }
            else
            {
                styleResult = _PanelEdge;
            }
            return styleResult;
        }

        protected float GetWidthOfEdge()
        {
            float edgeWidth = 2.0f;
            switch ( EdgeThickness )
            {
                case Thickness.Medium:  edgeWidth = 4.0f; break;
                case Thickness.Fat:     edgeWidth = 8.0f; break;
                default:
                    break;
            }

            return edgeWidth;
        }

        protected void DeflateRectangleForEdge( ref RectangleF rect )
        {
            if ( PanelEdgeOverlaps )
                return;

            int edgeWidth         = (int)GetWidthOfEdge();
            AnchorStyles edgeMask = GetEdgeFlags();

            if ( edgeMask.HasFlag( AnchorStyles.Top ) )
                rect.EdgeShift( 0, edgeWidth );

            if ( edgeMask.HasFlag( AnchorStyles.Bottom ) )
                rect.EdgeShift( 0, -edgeWidth );

            if ( edgeMask.HasFlag( AnchorStyles.Left ) )
                rect.EdgeShift( edgeWidth, 0 );

            if ( edgeMask.HasFlag( AnchorStyles.Right ) )
                rect.EdgeShift( -edgeWidth, 0 );
        }

        private void PaintIconAdornment( Graphics g, PlacementResult pr, AsphaltPanelShading aps )
        {
            if ( IconAdornment.Shape == AsphaltIconAdornment.ShapeChoice.None )
                return;

            float largestEdge = Math.Max(pr.Size.Width, pr.Size.Height) * IconAdornment.Size;

            PointF shapeCenter = pr.PositionCenter;
            shapeCenter.X -= largestEdge * 0.5f;
            shapeCenter.Y -= largestEdge * 0.5f;

            shapeCenter += IconAdornment.Offset;

            float shadowEdgeSize       = IconAdornment.ShadowSize;
            float shadowEdgeSizeDouble = shadowEdgeSize * 2.0f;

            switch ( IconAdornment.Shape )
            {
                case AsphaltIconAdornment.ShapeChoice.Circle:
                {
                    if ( shadowEdgeSize > 0 )
                    {
                        g.FillEllipse(
                            Current.Instance.SolidBrush( aps.PanelShader ),
                            shapeCenter.X - shadowEdgeSize,
                            shapeCenter.Y - shadowEdgeSize,
                            largestEdge + shadowEdgeSizeDouble,
                            largestEdge + shadowEdgeSizeDouble );
                    }

                    g.FillEllipse(
                        Current.Instance.SolidBrush( aps.AdornmentShader ),
                        shapeCenter.X,
                        shapeCenter.Y,
                        largestEdge,
                        largestEdge );
                }
                break;

                case AsphaltIconAdornment.ShapeChoice.Square:
                {
                    if ( shadowEdgeSize > 0 )
                    {
                        g.FillRectangle(
                            Current.Instance.SolidBrush( aps.PanelShader ),
                            shapeCenter.X - shadowEdgeSize,
                            shapeCenter.Y - shadowEdgeSize,
                            largestEdge + shadowEdgeSizeDouble,
                            largestEdge + shadowEdgeSizeDouble );
                    }

                    g.FillRectangle(
                        Current.Instance.SolidBrush( aps.AdornmentShader ),
                        shapeCenter.X,
                        shapeCenter.Y,
                        largestEdge,
                        largestEdge );
                }
                break;
            }

            //g.DrawRectangle(new Pen(Color.White), pr.PositionTopLeft.X, pr.PositionTopLeft.Y, pr.Size.Width, pr.Size.Height);
        }

        private RectangleF GetPaddedClippedInteriorRect()
        {
            RectangleF paddedRect = Utils.GetPaddedRectangle(this);
            DeflateRectangleForEdge( ref paddedRect );

            return paddedRect;
        }

        private PlacementResult ComputeIconPlacement( Graphics g, ref RectangleF interior )
        {
            if ( IconID == IconID.None )
                throw new InvalidOperationException( "No icon set" );

            Font        textFont = FontLibrary.Instance.Get( TextFont, TextSize );
            Font        iconFont = FontLibrary.Instance.GetIcons( IconSize );
            string      iconText = FontLibrary.FormatIconID( IconID );

            SizeF measuredIcon = IconTuneFixSize;
            if ( !IconFineTuning )
            {
                measuredIcon = TextRenderer.MeasureText( iconText, iconFont, Size.Round( interior.Size ), TextFormatFlags.GlyphOverhangPadding );
                measuredIcon.Width -= ( measuredIcon.Height / 8 );
            }


            if ( IconConnectToText )
            {
                PointF iconAlignOverlap = Utils.CalculateAlignmentOverlap( IconAlign, TextAlign );
                iconAlignOverlap.X *= measuredIcon.Width + ( IconPadding.Width * 2 );
                iconAlignOverlap.Y *= measuredIcon.Height + ( IconPadding.Height * 2 );

                //g.FillEllipse(new SolidBrush(Color.DodgerBlue), iconAlignOverlap.X - 2.0f, iconAlignOverlap.Y - 2.0f, 4.0f, 4.0f);

                interior.EdgeShift( iconAlignOverlap.X, iconAlignOverlap.Y );

                SizeF measuredText = TextRenderer.MeasureText( Text, textFont );
                //measuredText.Width -= measuredText.Height / 2;

                //g.DrawRectangle(new Pen(Color.DodgerBlue), Rectangle.Round(interior));

                PointF iconRenderPos = Utils.GetCenterPointForTextAlignment( TextAlign, measuredText, interior );
                //g.FillEllipse(new SolidBrush(Color.OrangeRed), iconRenderPos.X - 4.0f, iconRenderPos.Y - 4.0f, 8.0f, 8.0f);

                PointF iconOffset = new PointF();

                PointF shift = new PointF( ( measuredText.Width * 0.5f ) + ( measuredIcon.Width * 0.5f ), ( measuredText.Height * 0.5f ) + ( measuredIcon.Height * 0.5f ) );

                shift.X += IconPadding.Width;
                shift.Y += IconPadding.Height;


                switch ( IconAlign )
                {
                    case ContentAlignment.TopLeft:      iconOffset = new PointF( -shift.X, -shift.Y ); break;
                    case ContentAlignment.TopCenter:    iconOffset = new PointF( 0, -shift.Y ); break;
                    case ContentAlignment.TopRight:     iconOffset = new PointF( +shift.X, -shift.Y ); break;

                    case ContentAlignment.MiddleLeft:   iconOffset = new PointF( -shift.X, 0 ); break;
                    case ContentAlignment.MiddleCenter: iconOffset = new PointF( 0, 0 ); break;
                    case ContentAlignment.MiddleRight:  iconOffset = new PointF( +shift.X, 0 ); break;

                    case ContentAlignment.BottomLeft:   iconOffset = new PointF( -shift.X, +shift.Y ); break;
                    case ContentAlignment.BottomCenter: iconOffset = new PointF( 0, +shift.Y ); break;
                    case ContentAlignment.BottomRight:  iconOffset = new PointF( +shift.X, +shift.Y ); break;
                }

                iconRenderPos.X += iconOffset.X;
                iconRenderPos.Y += iconOffset.Y;

                iconRenderPos.X += IconOffset.Width;
                iconRenderPos.Y += IconOffset.Height;

                PointF centralPoint = iconRenderPos;

                iconRenderPos.X -= measuredIcon.Width / 2;
                iconRenderPos.Y -= measuredIcon.Height / 2;

                return new PlacementResult { PositionTopLeft = iconRenderPos, PositionCenter = centralPoint, Size = measuredIcon };
            }
            else
            {
                // shift the draw rectangle about to apply padding & offset
                RectangleF paddedOffsetInterior = interior;
                paddedOffsetInterior.Inflate( -IconPadding.Width, -IconPadding.Height );
                paddedOffsetInterior.Offset( IconOffset.Width, IconOffset.Height );

                PointF iconRenderPos = Utils.GetCenterPointForTextAlignment(IconAlign, measuredIcon, paddedOffsetInterior);

                PointF centralPoint = iconRenderPos;

                // return top-left coordinate
                iconRenderPos.X -= measuredIcon.Width / 2;
                iconRenderPos.Y -= measuredIcon.Height / 2;

                return new PlacementResult { PositionTopLeft = iconRenderPos, PositionCenter = centralPoint, Size = measuredIcon };
            }
        }


        internal override void Repaint()
        {
            Invalidate();
        }

        internal override void RenderBG( Graphics gfx )
        {
            AsphaltPanelShading activePanelShading = GetCurrentShading();

            if ( Canvas.Background == Canvas.Mode.Auto )
            {
                Control p = Parent;
                while ( p != null && !( p is IHasBackgroundShader ) )
                {
                    p = p.Parent;
                }

                if ( p is IHasBackgroundShader )
                {
                    var bgShader = (p as IHasBackgroundShader).GetCurrentBackgroundShader();
                    gfx.Clear( Current.Instance.Color( bgShader ) );
                }
            }
            else if ( Canvas.Background == Canvas.Mode.PanelShader )
            {
                // panels with Key colour backgrounds will look unusual when the rest of the content
                // is monochromed when disabled - so do a little hack to help unify the disabled look, 
                // force a default panel shader instead
                if ( !Enabled )
                {
                    if ( activePanelShading.PanelShader.Scheme != Scheme.Base &&
                         activePanelShading.PanelShader.Scheme != Scheme.Panel )
                    {
                        gfx.Clear( Current.Instance.Color( new Shader() ));
                        return;
                    }
                }

                gfx.Clear( Current.Instance.Color( activePanelShading.PanelShader ) );
            }
            else
            {
                using ( var bgb = Canvas.GetGradientBrush( ClientRectangle ) )
                {
                    gfx.FillRectangle( bgb, ClientRectangle );
                }
            }
        }

        internal override void Render( Graphics gfx )
        {
            AsphaltPanelShading activePanelShading = GetCurrentShading();

            float penWidth        = GetWidthOfEdge();
            AnchorStyles edgeMask = GetEdgeFlags();

            using ( var edgePen = new Pen( Current.Instance.Color( activePanelShading.EdgeShader ), penWidth * 2.0f ) )
            {
                if ( edgeMask.HasFlag( AnchorStyles.Top ) )
                    gfx.DrawLine( edgePen, 0, 0, Width, 0 );

                if ( edgeMask.HasFlag( AnchorStyles.Bottom ) )
                    gfx.DrawLine( edgePen, 0, Height, Width, Height );

                if ( edgeMask.HasFlag( AnchorStyles.Left ) )
                    gfx.DrawLine( edgePen, 0, 0, 0, Height );

                if ( edgeMask.HasFlag( AnchorStyles.Right ) )
                    gfx.DrawLine( edgePen, Width, 0, Width, Height );
            }

            gfx.TextRenderingHint  = TextRenderingHint.ClearTypeGridFit;
            gfx.SmoothingMode      = SmoothingMode.HighQuality;
            gfx.CompositingQuality = CompositingQuality.HighQuality;
            gfx.InterpolationMode  = InterpolationMode.HighQualityBicubic;


            RectangleF interior = GetPaddedClippedInteriorRect();


            // render any background image
            if ( Image != null )
            {
                RectangleF imageBlitRect = Tools.AspectFitImage( (float)_PanelImage.Width, (float)_PanelImage.Height, interior.Width, interior.Height );

                imageBlitRect.X += interior.X;
                imageBlitRect.Y += interior.Y;

                gfx.DrawImage( Image, imageBlitRect );
            }

            // render icon
            if ( IconID != IconID.None )
            {
                Color       iconForeground = Current.Instance.Color( activePanelShading.IconShader );
                Font              iconFont = FontLibrary.Instance.GetIcons( IconSize );
                string            iconText = FontLibrary.FormatIconID( IconID );

                if ( ToggledIconID != IconID.None && Pressed )
                {
                    iconText = FontLibrary.FormatIconID( ToggledIconID );
                }


                PlacementResult iconPlacement = ComputeIconPlacement(gfx, ref interior);

                PaintIconAdornment( gfx, iconPlacement, activePanelShading );

                Point iconTunedRenderPos = iconPlacement.IntPositionTopLeft;
                if ( IconFineTuning )
                {
                    iconTunedRenderPos.X += IconTuneShift.X;
                    iconTunedRenderPos.Y += IconTuneShift.Y;
                }

                TextRenderer.DrawText( gfx, iconText, iconFont, iconTunedRenderPos, iconForeground, TextFormatFlags.GlyphOverhangPadding );
            }

            // render label
            if ( Text.Length > 0 )
            {
                Color textForeground       = Current.Instance.Color(activePanelShading.TextShader);
                Font textFont              = FontLibrary.Instance.Get(TextFont, TextSize);

                interior.EdgeShift( TextOffset.Width, TextOffset.Height );

                // an absolutely stupid way to draw an outline shadow, but .. this is all we got, folks. 
                // iterate a small 3x3 grid and render the text with the offsets
                if ( TextShadowSize > 0 )
                {
                    float offset = TextShadowSize;
                    for ( int i = -1; i <= 1; i++ )
                    {
                        for ( int j = -1; j <= 1; j++ )
                        {
                            if ( i == 0 && j == 0 )
                                continue;

                            RectangleF shadowOffset = interior;
                            shadowOffset.Offset( (float)i * offset, (float)j * offset );

                            TextRenderer.DrawText( gfx, Text, textFont, Rectangle.Round( shadowOffset ), Current.Instance.Color( TextShadowShader ), Utils.AlignmentToTextFormat( TextAlign ) );
                        }
                    }
                }

                SizeF measuredText = TextRenderer.MeasureText( Text, textFont );
                measuredText.Height += 6;
                measuredText.Width += 6;

                PointF textRenderPos = Utils.GetCenterPointForTextAlignment( TextAlign, measuredText, interior );
#if FOO
                gfx.FillRectangle(
                    Current.Instance.SolidBrush( activePanelShading.PanelShader ),
                    textRenderPos.X - ( (measuredText.Width / 2) + 1 ),
                    textRenderPos.Y - ( (measuredText.Height / 2) + 0 ), 
                    measuredText.Width, 
                    measuredText.Height);
#endif
                TextRenderer.DrawText( gfx, Text, textFont, Rectangle.Round( interior ), textForeground, Utils.AlignmentToTextFormat( TextAlign ) );
            }
        }
    }
}
