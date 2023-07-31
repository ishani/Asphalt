using System.ComponentModel;
using System.Windows.Forms;

using Asphalt.Controls.Theme;

#if ASPHALT_DESIGN
using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Security.Permissions;
using System.Windows.Forms.Design;
#endif // ASPHALT_DESIGN

namespace Asphalt.Controls
{
    // very basic container panel with an Asphalt shader for the background
    public partial class AsphaltContainer : Panel, IPropertyEvents, IHasBackgroundShader
    {
        #region Properties_Shading

        private Shader _PanelShader = null;

        [Browsable( true ), EditorBrowsable( EditorBrowsableState.Always ), Category( "Asphalt.Container" )]
        public Shader PanelShader
        {
            get => _PanelShader;
            set { Prop.Exchange( ref _PanelShader, value, this ); }
        }

        void IPropertyEvents.OnChange( object sender, PropertyChangedEventArgs e )
        {
            Invalidate();
        }

        #endregion

        public AsphaltContainer()
        {
            SetStyle(
                ControlStyles.ContainerControl |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint |
                ControlStyles.Opaque, true);

            PanelShader = new Shader(Scheme.Base, Pigment.Primary, Shade.Darker);
        }

        Shader IHasBackgroundShader.GetCurrentBackgroundShader()
        {
            return PanelShader;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear( Current.Instance.Color( PanelShader ) );
        }
    }

#if ASPHALT_DESIGN

    [ToolboxItem( true ), Designer( typeof( AsphaltContainerDesigner ) )]
    public partial class AsphaltContainer
    {
        // ability to disable drag-drop editing in editor, container by container
        internal bool _EditorDragChangeEnabled = true;
    }

    [PermissionSet( SecurityAction.Demand, Name = "FullTrust" )]
    internal class AsphaltContainerDesigner : ParentControlDesigner
    {
        private DesignerActionUIService _ActionUIService;
        private AsphaltContainer        _ParentControl => (Control as AsphaltContainer);

        public override void Initialize( IComponent component )
        {
            base.Initialize( component );

            _ActionUIService = base.GetService( typeof( DesignerActionUIService ) ) as DesignerActionUIService;
            
            EnableDragDrop( _ParentControl._EditorDragChangeEnabled );
        }

        public override DesignerVerbCollection Verbs
        {
            get {
                if ( _ParentControl == null )
                    return null;

                var toggleString = (_ParentControl._EditorDragChangeEnabled ? "Disable" : "Enable") + " Drag Editing";

                var verbs = new DesignerVerbCollection()
                {
                    new DesignerVerb( toggleString, OnToggle )
                };

                return verbs;
            }
        }

        public override bool CanParent( Control control )
        {
            // if drag-editing is disabled, we reject all control re-parenting
            if ( _ParentControl != null &&
                !_ParentControl._EditorDragChangeEnabled )
                return false;

            return base.CanParent( control );
        }
    
        private void OnToggle( Object sender, EventArgs e )
        {
            _ParentControl._EditorDragChangeEnabled = !_ParentControl._EditorDragChangeEnabled;

            EnableDragDrop( _ParentControl._EditorDragChangeEnabled );

            _ActionUIService.Refresh( Component );
        }

        protected override void PreFilterProperties( IDictionary properties )
        {
            base.PreFilterProperties( properties );
            PropertiesToRemove.StripFromDictionary( ref properties );
        }
    }

#endif // ASPHALT_DESIGN
}
