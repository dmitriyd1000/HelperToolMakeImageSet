using System.Drawing;
using System.Windows.Forms;

namespace HelperToolMakeImageSet
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class CustomPanel : Panel
    {
        private KnownColor colorBorder;
        public CustomPanel(KnownColor col)
        {
            colorBorder = col;
            SetStyle(
                ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer |
                ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            LogHelper.Log(LogTarget.Console, $"Paint panel");
            using (SolidBrush brush = new SolidBrush(BackColor))
                e.Graphics.FillRectangle(brush, ClientRectangle);
            var pen = new Pen(Color.FromKnownColor(colorBorder));
            e.Graphics.DrawRectangle(pen, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
        }

    }
}