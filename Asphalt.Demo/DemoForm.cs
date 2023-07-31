using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using Asphalt.Controls;
using Asphalt.Controls.Theme;

namespace Asphalt.Demo
{
    public partial class DemoForm : AsphaltForm
    {
        public DemoForm()
        {
            InitializeComponent();
        }

        private void asphaltPanel2_PanelClicked(object sender, MouseEventArgs e)
        {
            asphaltSimpleText1.Text += "Clicked " + e.X.ToString() + ", " + e.Y.ToString() + Environment.NewLine;
        }

        private void DemoForm_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            asphaltSimpleText1.Text += "Click";
        }

        private void asphaltPanel26_PanelClicked(object sender, MouseEventArgs e)
        {
            asphaltPanel2.Enabled = !asphaltPanel2.Enabled;
            asphaltPanel16.Enabled = !asphaltPanel16.Enabled;
        }

        private void asphaltTrack4_ValueChanged( object sender, TrackEventArgs e )
        {
            tieToTrack1.Text = ((Int32)Math.Ceiling(e.NewValue * 300.0f)).ToString();
        }

        private void asphaltPanel31_Click( object sender, EventArgs e )
        {
            var SenderPanel = (sender as AsphaltPanel);

            DropDownHelper.DoPopUp(
                SenderPanel,
                new List<(string, object)> {
                    ( "Alpha"  , " 0" ),
                    ( "Beta"   , "10" ),
                    ( "Gamma"  , "60" ),
                    ( "Gamma1" , "61" ),
                    ( "Gamma2" , "62" ),
                    ( "Gamma3" , "63" ),
                    ( "Gamma4" , "64" ),
                    ( "Gamma5" , "65" ),
                    ( "Gamma6" , "66" ),
                    ( "Gamma7" , "67" ),
                    ( "Gamma8" , "68" ),
                    ( "Gamma9" , "69" ),
                    ( "Gamma0" , "70" ),
                    ( "Gamma11", "71" ),
                    ( "Gamma12", "72" )
                },
                (text, tag) =>
                {
                    SenderPanel.Text = text;
                });
        }

        private void asphaltColourWheel1_ColourChanged(object sender, Controls.Colour.RGB current)
        {
            hwColourBlock.BackColor = current.ToColor();
        }

        private void asphaltPanel35_Click(object sender, EventArgs e)
        {
            var ctrls = asphaltScroller4.EditablePanel.Controls;

            var lastCtrl = ctrls[ctrls.Count - 1];

            var newCtrl = (lastCtrl as AsphaltPanel).Clone();

            newCtrl.Left += lastCtrl.Width + 10;

            asphaltScroller4.EditablePanel.Width += lastCtrl.Width + 10;

            ctrls.Add(newCtrl);
        }

        private void asphaltScroller2_Load( object sender, EventArgs e )
        {

        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            var SenderPanel = (sender as PictureBox);

            var localPt = new Point(SenderPanel.Location.X, SenderPanel.Location.Y + SenderPanel.Height);

            AsphaltColourWheel.PopupAt(SenderPanel.Parent.PointToScreen(localPt), SenderPanel.BackColor, (o, rgb) =>
            {
                SenderPanel.BackColor = rgb.ToColor();
            });
        }

        private void AsphaltPanel40_PanelClicked(object sender, MouseEventArgs e)
        {
            asphaltPanel42.Enabled = !asphaltPanel42.Enabled;
            asphaltPanel43.Enabled = !asphaltPanel43.Enabled;
            asphaltPanel44.Enabled = !asphaltPanel44.Enabled;
            asphaltPanel45.Enabled = !asphaltPanel45.Enabled;
            asphaltPanel46.Enabled = !asphaltPanel46.Enabled;

            asphaltPanel10.Enabled = !asphaltPanel10.Enabled;
            asphaltPanel11.Enabled = !asphaltPanel11.Enabled;

            tieToTrack1.Enabled = !tieToTrack1.Enabled;
            asphaltTrack1.Enabled = !asphaltTrack1.Enabled;
            asphaltTrack4.Enabled = !asphaltTrack4.Enabled;

            asphaltScroller1.Enabled = !asphaltScroller1.Enabled;

            asphaltColourWheel1.Enabled = !asphaltColourWheel1.Enabled;
        }

        private void asphaltPanel62_Click( object sender, EventArgs e )
        {
            var fontBox = new DemoFonts();
            fontBox.ShowDialog();
        }
    }
}
