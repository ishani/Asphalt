using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Asphalt.Controls
{
    public delegate void DropDownSelection(String text, object tag);

    // a little tool to set up a Form full of selectable Panels, designed to be used as a pop-up dropdown selector. pass it an existing panel to use as a owner, whereupon it
    // will use the style and placement of that owner to drive the offered items in the pop-up
    public static class DropDownHelper
    {
        public static void DoPopUp(in AsphaltPanel OwningPanel, in IEnumerable<(string, object)> Entries, DropDownSelection SelectionDelegate, object ActiveTag = null)
        {
            const int flowPanelPadding = 2;

            AsphaltForm popUp   = new AsphaltForm
            {
                StartPosition   = FormStartPosition.Manual,
                ShowInTaskbar   = false,
                TopMost         = true,
            };
            popUp.SuspendLayout();

            var formPaddingWidth  = popUp.Padding.Left + flowPanelPadding;
            var formPaddingHeight = popUp.Padding.Top;

            // set to start, by default, just below the owner panel
            var localPt = new Point(OwningPanel.Location.X - formPaddingWidth, OwningPanel.Location.Y + OwningPanel.Height);
            popUp.Location = OwningPanel.Parent.PointToScreen(localPt);


            AsphaltContainer ac = new AsphaltContainer
            {
                Parent          = popUp,
                BackColor       = Color.Transparent,
                Dock            = DockStyle.Fill,
                AutoSizeMode    = AutoSizeMode.GrowAndShrink,
                Padding         = new Padding(0),
                Margin          = new Padding(0),
                TabStop         = false,
                AutoSize        = true
            };
            popUp.Controls.Add(ac);

            FlowLayoutPanel flp = new FlowLayoutPanel
            {
                AutoSize        = true,
                AutoSizeMode    = AutoSizeMode.GrowAndShrink,
                Padding         = new Padding(flowPanelPadding, 0, flowPanelPadding, flowPanelPadding),
                Margin          = new Padding(0),
                TabStop         = false,
                FlowDirection   = FlowDirection.TopDown
            };
            ac.Controls.Add(flp);

            // keep track of how much vertical space the new panels will occupy - in case we want to move the form up, dropping the 'active' panel on top of the original owner
            Int32 yOffset = 0;

            // for each entry, we create a new AsphaltPanel per entry - each copies the broad styling of our owning panel
            // they will be arranged vertically in the FlowLayoutPanel
            foreach (var soe in Entries)
            {
                AsphaltPanel ap       = new AsphaltPanel
                {
                    Name              = "dd_panel",
                    Text              = soe.Item1,
                    Tag               = soe.Item2,
                    TextFont          = OwningPanel.TextFont,
                    TextSize          = OwningPanel.TextSize,
                    TextAlign         = OwningPanel.TextAlign,
                    TabStop           = false,
                    Padding           = OwningPanel.Padding,
                    Margin            = new Padding(0, 2, 0, 0),
                    Edges             = OwningPanel.Edges,
                    EdgeThickness     = OwningPanel.EdgeThickness,
                    ReactToHover      = true,
                    PanelShading      = OwningPanel.PanelShadingDown,
                    PanelShadingHover = OwningPanel.PanelShadingHover,
                    Size              = new Size(OwningPanel.Width, OwningPanel.Height)
                };

                yOffset += OwningPanel.Height + 2;

                // redirect to our selection callback if user clicks on something, then close the popup
                ap.Click += (o, ev)   =>
                {
                    SelectionDelegate(ap.Text, ap.Tag);
                    popUp.Close();
                };

                // if we find an entry in the pile that matches our current owner panel - by either Text or Tag
                // then we shunt up the pop-up to start overlapping it (so the current selection is right under the mouse)
                // and also set the panel shading to the 'Down' theme to mark it as visually different to the rest
                if ( soe.Item1 == OwningPanel.Text || (ActiveTag != null && soe.Item2 == ActiveTag) )
                {
                    popUp.Location = new Point(popUp.Location.X, popUp.Location.Y - formPaddingHeight - yOffset);

                    ap.ReactToHover = false;
                    ap.PanelShading = OwningPanel.PanelShading;
                }

                flp.Controls.Add(ap);
            }

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
