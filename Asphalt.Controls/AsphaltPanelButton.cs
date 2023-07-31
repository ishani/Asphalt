using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asphalt.Controls
{
    public class AsphaltPanelButton : AsphaltPanel
    {
        [DefaultValue( IconID.Bolt )]
        public new IconID IconID
        {
            get => base.IconID;
            set => base.IconID = value;
        }

        public AsphaltPanelButton()
            : base()
        {
            Text            = "Asphalt Button";
            TextOffset      = new System.Drawing.SizeF(4F, 0F);
            Size            = new System.Drawing.Size(275, 52);

            Action          = ActionMode.Button;
            ReactToHover    = true;

            EdgeFlipOnPress = true;
            Edges           = System.Windows.Forms.AnchorStyles.Top;
            EdgeThickness   = Theme.Thickness.Slim;

            IconAlign       = System.Drawing.ContentAlignment.MiddleLeft;
            IconID          = IconID.Bolt;
            IconSize        = FontSize.Smaller;

            var CoreScheme = Theme.Scheme.KeyC;

            PanelShading = new AsphaltPanelShading()
            {
                AdornmentShader = new Theme.Shader()
                {
                    Scheme      = Theme.Scheme.KeyA
                },
                EdgeShader      = new Theme.Shader()
                {
                    Scheme      = CoreScheme
                },
                IconShader      = new Theme.Shader()
                {
                    Pigment     = Theme.Pigment.Foreground,
                    Scheme      = Theme.Scheme.Panel,
                },
                PanelShader     = new Theme.Shader()
                {
                    Scheme      = CoreScheme,
                    Shade       = Theme.Shade.Darker
                },
                TextShader      = new Theme.Shader()
                {
                    Pigment     = Theme.Pigment.Foreground,
                    Scheme      =  Theme.Scheme.Panel
                }
            };

            PanelShadingDown = new AsphaltPanelShading()
            {
                AdornmentShader = new Theme.Shader()
                {
                    Scheme      = Theme.Scheme.KeyA
                },
                EdgeShader      = new Theme.Shader()
                {
                    Pigment     = Theme.Pigment.Analogous,
                    Scheme      = CoreScheme,
                    Shade       = Theme.Shade.Lighter
                },
                IconShader      = new Theme.Shader()
                {
                    Pigment     = Theme.Pigment.Foreground,
                    Scheme      = Theme.Scheme.Panel,
                    Shade       = Theme.Shade.Lighter
                },
                PanelShader     = new Theme.Shader()
                {
                    Pigment     = Theme.Pigment.Analogous,
                    Scheme      = CoreScheme,
                    Shade       = Theme.Shade.Darker
                },
                TextShader      = new Theme.Shader()
                {
                    Pigment     = Theme.Pigment.Foreground,
                    Scheme      = Theme.Scheme.Panel,
                    Shade       = Theme.Shade.Lighter
                }
            };

            PanelShadingHover = new AsphaltPanelShading()
            {
                AdornmentShader = new Theme.Shader()
                {
                    Scheme      = Theme.Scheme.KeyA
                },
                EdgeShader      = new Theme.Shader()
                {
                    Scheme      = CoreScheme,
                    Shade       = Theme.Shade.Lighter
                },
                IconShader      = new Theme.Shader()
                {
                    Pigment     = Theme.Pigment.Foreground,
                    Scheme      = Theme.Scheme.Panel,
                    Shade       = Theme.Shade.Lighter
                },
                PanelShader     = new Theme.Shader()
                {
                    Scheme      = CoreScheme
                },
                TextShader      = new Theme.Shader()
                {
                    Pigment     = Theme.Pigment.Foreground,
                    Scheme      = Theme.Scheme.Panel,
                    Shade       = Theme.Shade.Lighter
                }
            };
        }
    }
}
