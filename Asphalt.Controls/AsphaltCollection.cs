using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

#if ASPHALT_DESIGN
using System;
using System.Collections;
using System.Security.Permissions;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
#endif // ASPHALT_DESIGN

using Asphalt.Controls.Theme;


namespace Asphalt.Controls
{
    [ToolboxItem( true )]
    public partial class AsphaltCollection : Panel, IPropertyEvents, IHasBackgroundShader
    {
        #region Properties_Pages

        // pile of panels we can flip through
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content ), Category( "Asphalt.Collection" )]
        public Toolkit.TypedCollection<Panel> Pages { get; } = new Toolkit.TypedCollection<Panel>();

        private int  _SelectionIndex = -1;

        // which page we're looking at; 
        [DefaultValue( -1 )]
        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Collection" )]
        public int SelectionIndex
        {
            get => _SelectionIndex;
            set {
                int previousIndex = _SelectionIndex;
                _SelectionIndex = value;

                PageChanged?.Invoke( SelectedPanel() );
            }
        }

        // callback for when the selection index changes
        public delegate void PageSelectionChanged( Panel selectedItem );
        public event PageSelectionChanged PageChanged;

        #endregion

        #region Properties_Shading

        private Shader _PanelShader = null;

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Collection" )]
        public Shader PanelShader
        {
            get { return _PanelShader; }
            set { Prop.Exchange( ref _PanelShader, value, this ); }
        }

        void IPropertyEvents.OnChange( object sender, PropertyChangedEventArgs e )
        {
            Invalidate();
        }

        #endregion


        // return the current page control
        public Panel SelectedPanel()
        {
            if ( SelectionIndex < 0 || SelectionIndex >= Pages.Count )
                return null;

            return Pages[SelectionIndex];
        }

        // try and set the given page to be active; unless we don't control it, in which
        // case this returns false and does nothing
        public bool SelectPanel( Panel p )
        {
            var panelIndex = Pages.IndexOf(p);
            if ( panelIndex != -1 )
            {
                SelectionIndex = panelIndex;
                return true;
            }
            return false;
        }




        public AsphaltCollection()
        {
            SetStyle(
                ControlStyles.ContainerControl |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint |
                ControlStyles.Opaque, true );

            PanelShader = new Shader( Scheme.Base, Pigment.Primary, Shade.Darker );

            Padding = new Padding( 4 );

            // on a new page being added to the pile, we set some visual defaults and set it to be selected immediately
            Pages.Inserted += ( sender, args ) =>
            {
                args.Item.Dock      = DockStyle.Fill;
                args.Item.BackColor = Color.Transparent;

                Controls.Add( args.Item );

                SelectionIndex = Pages.Count - 1;
            };

            Pages.Removing += ( sender, args ) =>
            {
                Controls.Remove( args.Item );

                if ( Pages.IndexOf( args.Item ) == SelectionIndex )
                {
                    if ( SelectionIndex == Pages.Count - 1 )
                        _SelectionIndex--;  // will update visibility in Removed callback, so edit the backing store directly
                }
            };

            Pages.Removed += ( sender, args ) =>
            {
                SelectionIndex = _SelectionIndex;
            };

            PageChanged += ( selectedItem ) =>
            {
                UpdatePanelVisibilityForSelection();
            };
        }

        private void UpdatePanelVisibilityForSelection()
        {
            for (int i=0; i<Pages.Count; i++ )
            {
                if ( i == SelectionIndex )
                    Pages[i].Visible = true;
                else
                    Pages[i].Visible = false;
            }
        }

        Shader IHasBackgroundShader.GetCurrentBackgroundShader()
        {
            return PanelShader;
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            e.Graphics.Clear( Current.Instance.Color( _PanelShader ) );
        }
    }


#if ASPHALT_DESIGN

    [Designer( typeof( AsphaltCollectionDesigner ) )]
    public partial class AsphaltCollection
    {
    }

    [PermissionSet( SecurityAction.Demand, Name = "FullTrust" )]
    internal class AsphaltCollectionDesigner : ParentControlDesigner
    {
        private AsphaltCollection _ParentControl => (Control as AsphaltCollection);

        private DesignerVerbCollection  _verbs = new DesignerVerbCollection();
        private IDesignerHost           _DesignerHost;
        private ISelectionService       _SelectionService;
        private IUIService              _UIService;
        private DesignerActionUIService _ActionUIService;

        public override void Initialize( IComponent component )
        {
            // validate the parameter reference
            if ( component == null )
                throw new ArgumentNullException( nameof( component ) );

            // let base class do standard stuff
            base.Initialize( component );

            _DesignerHost     = base.GetService( typeof( IDesignerHost ) ) as IDesignerHost;
            _SelectionService = base.GetService( typeof( ISelectionService ) ) as ISelectionService;
            _UIService        = base.GetService( typeof( IUIService ) ) as IUIService;
            _ActionUIService  = base.GetService( typeof( DesignerActionUIService ) ) as DesignerActionUIService;

            _ParentControl.PageChanged += UpdateVerbsAndRefreshUI;
        }

        private void UpdateVerbsAndRefreshUI( Panel selectedPanel )
        {
            UpdateVerbs();
            _ActionUIService.Refresh( Component );
        }

        public override SelectionRules SelectionRules
        {
            get {
                return Control.Dock == DockStyle.Fill ? SelectionRules.Visible : base.SelectionRules;
            }
        }

        public override IList SnapLines
        {
            get {
                IList snapLines = base.SnapLines;

                snapLines.Add( new SnapLine( SnapLineType.Baseline, _ParentControl.ClientRectangle.Height / 2, SnapLinePriority.Always ) );
                snapLines.Add( new SnapLine( SnapLineType.Vertical, _ParentControl.ClientRectangle.Width / 2, SnapLinePriority.Always ) );

                return snapLines;
            }
        }

        public override DesignerVerbCollection Verbs => UpdateVerbs();

        private DesignerVerbCollection UpdateVerbs()
        {
            var parentControl = _ParentControl;

            // Verbs is called post-delete of Control; so guard against that freaking out
            if ( parentControl == null )
                return null;

            _verbs.Clear();

            var verbAdd    = new DesignerVerb( "Add Page", OnAddPage );
            var verbRemove = new DesignerVerb( "Remove Page", OnRemovePage )
            {
                Enabled = ( parentControl.Pages.Count != 0 && parentControl.SelectionIndex >= 0 )
            };

            _verbs.Add( verbAdd );
            _verbs.Add( verbRemove );

            for ( var panelIndex = 0; panelIndex < parentControl.Pages.Count; panelIndex ++ )
            {
                var panel = parentControl.Pages[panelIndex];
                var localIndex = panelIndex;

                var checkTag = (localIndex == parentControl.SelectionIndex) ? "> " : "  ";

                _verbs.Add( new DesignerVerb( $"{checkTag}Activate '{panel.Name}'", 
                    ( sender, e ) =>
                    {
                        using ( DesignerTransaction transaction = _DesignerHost.CreateTransaction( "Asphalt.Collection Change Selection" ) )
                        {
                            try
                            {
                                var propertyChanging = TypeDescriptor.GetProperties( parentControl )["SelectionIndex"];
                                int previousIndex    = parentControl.SelectionIndex;

                                RaiseComponentChanging( propertyChanging );
                                parentControl.SelectionIndex = localIndex;
                                RaiseComponentChanged( propertyChanging, previousIndex, localIndex );

                                transaction.Commit();
                            }
                            catch
                            {
                                transaction.Cancel();
                                throw;
                            }
                        }
                    })
                );
            }

            return _verbs;
        }
        
        private void OnAddPage( Object sender, EventArgs e )
        {
            var parentControl = _ParentControl;

            using ( DesignerTransaction transaction = _DesignerHost.CreateTransaction( "Asphalt.Collection Add Page" ) )
            {
                try
                {
                    var propertyChanging = TypeDescriptor.GetProperties( parentControl )["Panels"];
                    var oldTabs          = parentControl.Pages;

                    RaiseComponentChanging( propertyChanging );

                    var newPanel = _DesignerHost.CreateComponent(typeof(Panel)) as Panel;
                    parentControl.Pages.Add( newPanel );

                    RaiseComponentChanged( propertyChanging, oldTabs, parentControl.Pages );

                    transaction.Commit();
                }
                catch
                {
                    transaction.Cancel();
                    throw;
                }
            }
        }

        private void OnRemovePage( Object sender, EventArgs e )
        {
            var parentControl =_ParentControl;

            if ( parentControl == null ||
                 parentControl.SelectionIndex < 0 )
            {
                return;
            }

            using ( DesignerTransaction transaction = _DesignerHost.CreateTransaction( "Asphalt.Collection Remove Page" ) )
            {
                try
                {
                    var propertyChanging = TypeDescriptor.GetProperties( parentControl )["Panels"];
                    var oldTabs = parentControl.Pages;

                    RaiseComponentChanging( propertyChanging );

                    _DesignerHost.DestroyComponent( parentControl.Pages[parentControl.SelectionIndex] );
                    parentControl.Pages.RemoveAt( parentControl.SelectionIndex );

                    RaiseComponentChanged( propertyChanging, oldTabs, parentControl.Pages );

                    _SelectionService.SetSelectedComponents( new IComponent[]
                    {
                    parentControl
                    }, SelectionTypes.Auto );

                    transaction.Commit();
                }
                catch
                {
                    transaction.Cancel();
                    throw;
                }
            }
        }

        protected override void PreFilterProperties( IDictionary properties )
        {
            base.PreFilterProperties( properties );
            PropertiesToRemove.StripFromDictionary( ref properties );
        }
    }

#endif // ASPHALT_DESIGN

}
