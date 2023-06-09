using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using HelperToolMakeImageSet;


namespace HelperToolMakeImageSet
{

    public partial class ControlPanel : Form
    {
        public class ObjectImage
        {
            public readonly string FullPathImage;
            public List<ObjectName> objectNames = new List<ObjectName>();
            private readonly string _nameImage;
            public readonly List<CustomPanel> _pnlObjs = new List<CustomPanel>();

            public ObjectImage(string fullPathImage)
            {
                _nameImage = Path.GetFileName(fullPathImage);
                this.FullPathImage = fullPathImage;
            }

            public override string ToString()
            {
                return _nameImage;
            }
        }
        
        public class ObjectName
        {
            public readonly string _nameObj;
            public readonly string _nameObjForDisplay;
            private readonly int _order;
            public readonly KnownColor _color;

            public ObjectName(string nameObj, KnownColor color, int order)
            {
                _nameObjForDisplay = order+" - "+nameObj;
                _nameObj = nameObj;
                _color = color;
                _order = order;
            }

            public override string ToString()
            {
                return _nameObjForDisplay;
            }
        }
        
        public enum CursPos : int
        {
            WithinSelectionArea = 0,
            Default,
            TopLine,
            BottomLine,
            LeftLine,
            RightLine,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            NoPos
        }

        public enum Colors
        {
            Black = KnownColor.Black,
            Blue = KnownColor.Blue,
            Brown = KnownColor.Brown,
            CadetBlue = KnownColor.CadetBlue,
            Crimson = KnownColor.Crimson,
            DarkGoldenrod = KnownColor.DarkGoldenrod,
            DarkGray = KnownColor.DarkGray,
            DarkMagenta = KnownColor.DarkMagenta,
            DarkOliveGreen = KnownColor.DarkOliveGreen,
            DarkOrange = KnownColor.DarkOrange,
            DarkOrchid = KnownColor.DarkOrchid,
            DarkRed = KnownColor.DarkRed,
            DarkSalmon = KnownColor.DarkSalmon,
            DeepPink = KnownColor.DeepPink,
            DodgerBlue = KnownColor.DodgerBlue,
            ForestGreen = KnownColor.ForestGreen,
            Fuchsia = KnownColor.Fuchsia
        }

        public enum ClickAction : int
        {
            NoClick = 0,
            Dragging,
            Outside,
            TopSizing,
            BottomSizing,
            LeftSizing,
            TopLeftSizing,
            BottomLeftSizing,
            RightSizing,
            TopRightSizing,
            BottomRightSizing
        }

        private float scalingFactor;
        public Point ClickPoint = new Point();
        public Point CurrentTopLeft = new Point();
        public Point CurrentBottomRight = new Point();
        public ClickAction CurrentAction;
        public bool LeftButtonDown = false;
        public bool RectangleDrawn = false;
        public bool ReadyToDrag = false;
        public int RectangleHeight = new int();
        public int RectangleWidth = new int();
        public Point DragClickRelative = new Point();
        Pen MyPen = new Pen(Color.Black, 1);
        Pen EraserPen = new Pen(Color.FromArgb(255, 255, 192), 1);
        private static CustomPanel _pnlCurrent;
        private CursPos _curCursorPos;
        private ObjectImage _prevObjectImageItem;

        string ScreenPath;
        private OpenFileDialog  openFileDialog_LoadPixture;

        public ControlPanel()
        {
            InitializeComponent();
            lstObjs.DrawMode = DrawMode.OwnerDrawFixed;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            new List<CustomPanel>();
            IntExtentions.scaleFactor = GetScalingFactor(Screen.FromControl(this));
            // check files
            if (File.Exists("objects.names"))
            {
                IEnumerable<string> lines = File.ReadLines("objects.names");
                var colors = (Colors[])Enum.GetValues(typeof(Colors));
                var i = 0;
                foreach (var line in lines)
                {
                    lstObjs.Items.Add(new ObjectName(line, (KnownColor) colors[i], i+1)); 
                    i++;
                }

                lstObjs.SelectedIndex = 0;
            }
        }

        public void key_press(object sender, KeyEventArgs e)
        {
            keyTest(e);
        }


        private void keyTest(KeyEventArgs e)
        {
        }


        private void Form_Close(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            openFileDialog_LoadPixture = new OpenFileDialog();
            openFileDialog_LoadPixture.Filter = "images (*.jpg) | * jpg";
            openFileDialog_LoadPixture.RestoreDirectory = true;
            if (openFileDialog_LoadPixture.ShowDialog() == DialogResult.OK)
            {
                foreach (var fullpath in openFileDialog_LoadPixture.FileNames)
                {
                    var objImage = new ObjectImage(fullpath);
                    var ddd = objImage.ToString();
                    lstImages.Items.Add(objImage);
                }
            }

            lstImages.SelectedIndex = lstImages.Items.Count-1;
        }

        private void btnAddObj_Click(object sender, EventArgs e)
        {
            string value = "";
            if (InputBox("Add new object", "Name", ref value) == DialogResult.OK)
                lstObjs.Items.Add(value);
        }
        
        private void btnDelObj_Click(object sender, EventArgs e)
        {
            if (lstObjs.SelectedItem!=null)
                lstObjs.Items.Remove(lstObjs.SelectedItem);
        }
        
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();
            form.Text = title;
            label.Text = promptText;
            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;
            label.SetBounds(36, 36, 372, 13);
            textBox.SetBounds(36, 86, 700, 20);
            buttonOk.SetBounds(228, 160, 160, 60);
            buttonCancel.SetBounds(400, 160, 160, 60);
            label.AutoSize = true;
            form.ClientSize = new Size(796, 307);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;
            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private void SetClickAction(CursPos cur)
        {
            switch (cur)
            {
                case CursPos.BottomLine:
                    CurrentAction = ClickAction.BottomSizing;
                    break;
                case CursPos.TopLine:
                    CurrentAction = ClickAction.TopSizing;
                    break;
                case CursPos.LeftLine:
                    CurrentAction = ClickAction.LeftSizing;
                    break;
                case CursPos.TopLeft:
                    CurrentAction = ClickAction.TopLeftSizing;
                    break;
                case CursPos.BottomLeft:
                    CurrentAction = ClickAction.BottomLeftSizing;
                    break;
                case CursPos.RightLine:
                    CurrentAction = ClickAction.RightSizing;
                    break;
                case CursPos.TopRight:
                    CurrentAction = ClickAction.TopRightSizing;
                    break;
                case CursPos.BottomRight:
                    CurrentAction = ClickAction.BottomRightSizing;
                    break;
                case CursPos.WithinSelectionArea:
                    CurrentAction = ClickAction.Dragging;
                    break;
                case CursPos.NoPos:
                    CurrentAction = ClickAction.Outside;
                    break;
            }

        }

        private Cursor GetCursorByDirection(CursPos cur)
        {
            switch (cur)
            {
                case CursPos.BottomLine:
                    return Cursors.SizeNS;
                case CursPos.TopLine:
                    return Cursors.SizeNS;
                case CursPos.LeftLine:
                    return Cursors.SizeWE;
                case CursPos.TopLeft:
                    return Cursors.SizeNWSE;
                case CursPos.BottomLeft:
                    return Cursors.SizeNESW;
                case CursPos.RightLine:
                   return Cursors.SizeWE;
                case CursPos.TopRight:
                    return Cursors.SizeNESW;
                case CursPos.BottomRight:
                   return Cursors.SizeNWSE;
                case CursPos.WithinSelectionArea:
                    return Cursors.Hand;
                default: return Cursors.No;
            }
        }
        
         private CursPos GetCursorByPanel(CustomPanel pnl)
         {
             var cursorPos = pnlImage.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));
            if (cursorPos.X > pnl.Left - 10 &&
                  cursorPos.X < pnl.Left + 10 &&
                cursorPos.Y > pnl.Top + 10 &&
                 cursorPos.Y < pnl.Bottom - 10)
            {
                return CursPos.LeftLine;
            }
            if (cursorPos.X >= pnl.Left - 10 &&
                cursorPos.X <= pnl.Left + 10 &&
                cursorPos.Y >= pnl.Top - 10 &&
                cursorPos.Y <= pnl.Top + 10)
            {
                return CursPos.TopLeft;
            }
            if (cursorPos.X >= pnl.Left - 10 &&
                cursorPos.X <= pnl.Left + 10 &&
                cursorPos.Y >= pnl.Bottom - 10 &&
                cursorPos.Y <= pnl.Bottom + 10)
            {
                return CursPos.BottomLeft;
            }
            if (cursorPos.X >= pnl.Right - 10 &&
                  cursorPos.X <= pnl.Right + 10 &&
                cursorPos.Y >= pnl.Top + 10 &&
                 cursorPos.Y <= pnl.Bottom  - 10)
            {
                return CursPos.RightLine;
            }
            if (cursorPos.X > pnl.Right - 10 &&
                  cursorPos.X < pnl.Right + 10 &&
                cursorPos.Y > pnl.Top - 10 &&
                cursorPos.Y < pnl.Top + 10)
            {
                return CursPos.TopRight;
            }
            if (cursorPos.X > pnl.Right - 10 &&
                  cursorPos.X < pnl.Right + 10 &&
                cursorPos.Y > pnl.Bottom - 10 &&
                 cursorPos.Y < pnl.Bottom + 10)
            {
                return CursPos.BottomRight;
            }
            if (cursorPos.Y > pnl.Top - 10 &&
                 cursorPos.Y < pnl.Top + 10 &&
                cursorPos.X > pnl.Left + 10 &&
                  cursorPos.X < pnl.Right - 10)
            {
                return CursPos.TopLine;
            }
            if (cursorPos.Y > pnl.Bottom - 10 &&
                 cursorPos.Y < pnl.Bottom + 10 &&
                cursorPos.X > pnl.Left + 10 &&
                  cursorPos.X < pnl.Right - 10)
            {
                return CursPos.BottomLine;
            }
            if (
                cursorPos.X >= pnl.Left + 10 &&
                 cursorPos.X <= pnl.Right - 10 &&
                cursorPos.Y >= pnl.Top + 10 &&
                 cursorPos.Y <= pnl.Bottom - 10)
            {
                return CursPos.WithinSelectionArea;
            }
            
            //if (!((ObjectImage)lstImages.SelectedItem).objectNames.Contains((ObjectName)lstObjs.SelectedItem))
              //  return CursPos.Default;
              return CursPos.NoPos;
           // Cursors.Hand
         }
         
         private void DrawSelection()
         {
             Cursor = Cursors.Arrow;
             //Erase the previous rectangle
             //g.Clear(pnlImage.BackColor);
             //LogHelper.Log(LogTarget.Console, $"Erase rectangle by coords {CurrentTopLeft.X}, {CurrentTopLeft.Y}, {CurrentBottomRight.X - CurrentTopLeft.X}, {CurrentBottomRight.Y - CurrentTopLeft.Y}");
             //g.DrawRectangle(EraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, CurrentBottomRight.X - CurrentTopLeft.X, CurrentBottomRight.Y - CurrentTopLeft.Y);
             //Calculate X Coordinates
             var cursorPos = pnlImage.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));
             if (cursorPos.X < ClickPoint.X)
             {
                 CurrentTopLeft.X = cursorPos.X;
                 CurrentBottomRight.X = ClickPoint.X;
             }
             else
             {
                 CurrentTopLeft.X = ClickPoint.X;
                 CurrentBottomRight.X = cursorPos.X;
             }

             //Calculate Y Coordinates
             if (cursorPos.Y < ClickPoint.Y)
             {
                 CurrentTopLeft.Y = cursorPos.Y;
                 CurrentBottomRight.Y = ClickPoint.Y;
             }
             else
             {
                 CurrentTopLeft.Y = ClickPoint.Y;
                 CurrentBottomRight.Y = cursorPos.Y;
             }
             //Draw a new rectangle

             _pnlCurrent.Location = new Point(CurrentTopLeft.X, CurrentTopLeft.Y);
             _pnlCurrent.Size = new Size( CurrentBottomRight.X - CurrentTopLeft.X, CurrentBottomRight.Y - CurrentTopLeft.Y);
             //pnlCurrent.Invalidate();
         }
         
         private void DragSelection()
        {
            //Ensure that the rectangle stays within the bounds of the screen
            var cursorPos = pnlImage.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));
            
            if (cursorPos.X - DragClickRelative.X > 0 && cursorPos.X - DragClickRelative.X + _pnlCurrent.Width < pnlImage.Width)
            {
                CurrentTopLeft.X = cursorPos.X - DragClickRelative.X;
                CurrentBottomRight.X = CurrentTopLeft.X + _pnlCurrent.Width;
            }
            else
                //Selection area has reached the right side of the screen
                if (cursorPos.X - DragClickRelative.X > 0)
                {
                    CurrentTopLeft.X = pnlImage.Width - _pnlCurrent.Width;
                    CurrentBottomRight.X = CurrentTopLeft.X + _pnlCurrent.Width;
                }
                //Selection area has reached the left side of the screen
                else
                {
                    CurrentTopLeft.X = 0;
                    CurrentBottomRight.X = CurrentTopLeft.X + _pnlCurrent.Width;
                }

            if (cursorPos.Y - DragClickRelative.Y > 0 && cursorPos.Y - DragClickRelative.Y + _pnlCurrent.Height < pnlImage.Height)
            {
                CurrentTopLeft.Y = cursorPos.Y - DragClickRelative.Y;
                CurrentBottomRight.Y = CurrentTopLeft.Y + _pnlCurrent.Height;
            }
            else
                //Selection area has reached the bottom of the screen
                if (cursorPos.Y - DragClickRelative.Y > 0)
                {
                    CurrentTopLeft.Y = pnlImage.Height - _pnlCurrent.Height;
                    CurrentBottomRight.Y = CurrentTopLeft.Y + _pnlCurrent.Height;
                }
                //Selection area has reached the top of the screen
                else
                {
                    CurrentTopLeft.Y = 0;
                    CurrentBottomRight.Y = CurrentTopLeft.Y + _pnlCurrent.Height;
                }

            //Draw a new rectangle
            _pnlCurrent.Location = new Point(CurrentTopLeft.X, CurrentTopLeft.Y);
            _pnlCurrent.Size = new Size(_pnlCurrent.Width, _pnlCurrent.Height);
        }

         private void ResizeSelection()
         {
             var cursorByPanel = pnlImage.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));
             if (CurrentAction == ClickAction.LeftSizing)
             {
                 if (cursorByPanel.X < _pnlCurrent.Right - 10)
                 {
                     RectangleWidth = _pnlCurrent.Right - cursorByPanel.X;
                     _pnlCurrent.Location = new Point(cursorByPanel.X, _pnlCurrent.Top);
                     _pnlCurrent.Size = new Size(RectangleWidth, _pnlCurrent.Height);
                 }
             }

             if (CurrentAction == ClickAction.TopLeftSizing)
             {
                 if (cursorByPanel.X < _pnlCurrent.Right - 10 && cursorByPanel.Y < _pnlCurrent.Bottom - 10)
                 {
                     RectangleWidth = _pnlCurrent.Right - cursorByPanel.X;
                     RectangleHeight = _pnlCurrent.Bottom - cursorByPanel.Y;
                     _pnlCurrent.Location = new Point(cursorByPanel.X, cursorByPanel.Y);
                     _pnlCurrent.Size = new Size(RectangleWidth, RectangleHeight);
                 }
             }

             if (CurrentAction == ClickAction.BottomLeftSizing)
             {
                 if (cursorByPanel.X < _pnlCurrent.Right - 10 && cursorByPanel.Y > _pnlCurrent.Top + 10)
                 {
                     RectangleWidth = _pnlCurrent.Right - cursorByPanel.X;
                     RectangleHeight = cursorByPanel.Y - _pnlCurrent.Top;
                     _pnlCurrent.Location = new Point(cursorByPanel.X, _pnlCurrent.Top);
                     _pnlCurrent.Size = new Size( RectangleWidth, RectangleHeight);
                 }
             }

             if (CurrentAction == ClickAction.RightSizing)
             {
                 if (cursorByPanel.X > _pnlCurrent.Left + 10)
                 {
                     RectangleWidth = cursorByPanel.X - _pnlCurrent.Left;
                     _pnlCurrent.Size = new Size( RectangleWidth, _pnlCurrent.Height);
                 }
             }

             if (CurrentAction == ClickAction.TopRightSizing)
             {
                 if (cursorByPanel.X > _pnlCurrent.Left + 10 && cursorByPanel.Y < _pnlCurrent.Bottom - 10)
                 {
                     RectangleWidth =  cursorByPanel.X - _pnlCurrent.Left;
                     RectangleHeight = _pnlCurrent.Bottom - cursorByPanel.Y;
                     _pnlCurrent.Location = new Point(_pnlCurrent.Left, cursorByPanel.Y);
                     _pnlCurrent.Size = new Size(RectangleWidth, RectangleHeight);
                 }
             }

             if (CurrentAction == ClickAction.BottomRightSizing)
             {
                 if (cursorByPanel.X > _pnlCurrent.Left + 10 && cursorByPanel.Y > _pnlCurrent.Top + 10)
                 {
                     RectangleWidth = cursorByPanel.X - _pnlCurrent.Left;
                     RectangleHeight = cursorByPanel.Y - _pnlCurrent.Top;
                     _pnlCurrent.Size = new Size( RectangleWidth, RectangleHeight);
                 }
             }

             if (CurrentAction == ClickAction.TopSizing)
             {
                 if (cursorByPanel.Y < _pnlCurrent.Bottom - 10)
                 {
                     RectangleHeight = _pnlCurrent.Bottom - cursorByPanel.Y;
                     _pnlCurrent.Location = new Point(_pnlCurrent.Left, cursorByPanel.Y);
                     _pnlCurrent.Size = new Size(RectangleWidth, RectangleHeight);
                 }
             }

             if (CurrentAction == ClickAction.BottomSizing)
             {
                 if (cursorByPanel.Y > _pnlCurrent.Top + 10)
                 {
                     RectangleHeight = cursorByPanel.Y - _pnlCurrent.Top;
                     _pnlCurrent.Size = new Size(_pnlCurrent.Width, RectangleHeight);
                 }
             }
         }

         private void event_MouseDown(object sender, MouseEventArgs e)
         {
             //IntExtentions.scaleFactor = GetScalingFactor(Screen.FromControl(this));
             //LogHelper.Log(LogTarget.Console, $"Mouse Down {e.X}, {e.Y}");
             if (lstImages.Items.Count == 0)
                 return;
             if (e.Button == MouseButtons.Left)
             {
                 LeftButtonDown = true;
                 ClickPoint = new Point(e.X, e.Y);
                 _pnlCurrent = GetCurrentPanel();
                 if (_pnlCurrent == null)
                 {
                     _pnlCurrent = GetNewPanel();
                     if (_pnlCurrent!=null)
                     {
                         RectangleDrawn = false;
                         (lstImages.SelectedItem as ObjectImage)?._pnlObjs.Add(_pnlCurrent);
                         pnlImage.Controls.Add(_pnlCurrent);
                     }
                 }
                 else
                 {
                     Cursor = GetCursorByDirection(_curCursorPos);
                     SetClickAction(_curCursorPos);
                 }

                 if (RectangleDrawn && CurrentAction==ClickAction.Dragging)
                 {
                     var cursorPos = pnlImage.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));
                     DragClickRelative.X = cursorPos.X - _pnlCurrent.Left;
                     DragClickRelative.Y = cursorPos.Y - _pnlCurrent.Top;
                 }
             }
         }
         
         private void event_MouseMove(object sender, MouseEventArgs e)
         {
             //LogHelper.Log(LogTarget.Console, $"Mouse Move {e.X}, {e.Y}");
             
             if (LeftButtonDown && !RectangleDrawn)
             {
                 DrawSelection();
             }
             else if (LeftButtonDown && RectangleDrawn)
             {
                 if (CurrentAction == ClickAction.Dragging)
                 {
                     DragSelection();
                 }
                 if (CurrentAction == ClickAction.BottomSizing || CurrentAction == ClickAction.LeftSizing ||
                     CurrentAction == ClickAction.RightSizing || CurrentAction == ClickAction.TopLeftSizing ||
                     CurrentAction == ClickAction.BottomLeftSizing || CurrentAction == ClickAction.TopSizing ||
                     CurrentAction == ClickAction.BottomRightSizing || CurrentAction == ClickAction.TopRightSizing)
                 {
                     LogHelper.Log(LogTarget.Console, $"Resize pnlCurrent {_pnlCurrent.Left}, {_pnlCurrent.Top} ClickAction {CurrentAction}");
                     ResizeSelection();
                     
                 }
             }
             else
             {
                 _pnlCurrent = GetCurrentPanel();
                 Cursor = GetCursorByDirection(_curCursorPos);
             }
         }

        
         private void event_MouseUp(object sender, MouseEventArgs e)
         {
             RectangleDrawn = true;
             LeftButtonDown = false;
             CurrentAction = ClickAction.NoClick;
         }

         private void lstPictures_SelectedIndexChanged(object sender, EventArgs e)
         {
             var imgPath = (ObjectImage)lstImages.SelectedItem;
             if (imgPath!=null)
             {
                 Image img = Image.FromFile(imgPath.FullPathImage);
                 pnlImage.BackgroundImage = img;
                 pnlImage.Invalidate();
             }

             if (_prevObjectImageItem !=null && _prevObjectImageItem != (ObjectImage)lstImages.SelectedItem)
             {
                 foreach (var pnl in  _prevObjectImageItem._pnlObjs)
                 {
                     pnl.Visible = false;
                     pnl.Invalidate();
                 }
                 
                 foreach (var pnl in  ((ObjectImage)lstImages.SelectedItem)?._pnlObjs)
                 {
                     pnl.Visible = true;
                     pnl.Invalidate();
                 }
             }
             _prevObjectImageItem = (ObjectImage)lstImages.SelectedItem;
         }
         
         private CustomPanel GetNewPanel()
         {
             KnownColor color = KnownColor.Black;
             if (lstObjs.Items.Count > 0)
             {
                 // check object wasn't drawn before
                if (((ObjectImage)lstImages.SelectedItem).objectNames.Contains((ObjectName)lstObjs.SelectedItem))
                    return null;
                color = ((ObjectName)lstObjs.SelectedItem)._color;
                ((ObjectImage) lstImages.SelectedItem).objectNames.Add((ObjectName)lstObjs.SelectedItem);
             }
             CustomPanel newPanel = new CustomPanel(color);
             newPanel.BackColor = Color.Transparent;
             newPanel.BorderStyle = BorderStyle.FixedSingle;
             newPanel.Location = new Point(0, 0);
             newPanel.Name = ((ObjectName)lstObjs.SelectedItem)._nameObj;
             newPanel.Size = new Size(0, 0);
             newPanel.TabIndex = 0;
             Label objName = new Label();
             objName.Text = ((ObjectName)lstObjs.SelectedItem)._nameObj;
             objName.Location = new Point(5, 5);
             objName.ForeColor = Color.FromKnownColor(((ObjectName)lstObjs.SelectedItem)._color);
             newPanel.Controls.Add(objName);
             newPanel.MouseDown += event_MouseDown;
             newPanel.MouseMove += event_MouseMove;
             newPanel.MouseUp += event_MouseUp;
             return newPanel;
         }

         private CustomPanel GetCurrentPanel()
         {
             var customPanels = (lstImages.SelectedItem as ObjectImage)?._pnlObjs;
             if (customPanels != null)
                 foreach (var obj in customPanels)
                 {
                     _curCursorPos = GetCursorByPanel(obj);
                     if (_curCursorPos != CursPos.NoPos)
                     {
                         RectangleDrawn = true;
                         return obj;
                     }
                 }

             return null;
         }
         
         [DllImport("gdi32.dll")]
         static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);
        
         [DllImport("gdi32.dll")]
         static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
         public enum DeviceCap
         {
             VERTRES = 10,
             DESKTOPVERTRES = 117,

             // http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
         }
    
         public float GetScalingFactor(Screen s)
         {
             IntPtr desktop = CreateDC(null,s.DeviceName,null, IntPtr.Zero);
             int logicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
             int physicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES); 

             return (float) physicalScreenHeight / logicalScreenHeight;
         }

         private void lstObjs_DrawItem(object sender, DrawItemEventArgs e)
         {
             e.DrawBackground();
             if (lstObjs.Items[e.Index] is ObjectName item)
             {
                 e.Graphics.DrawString(item._nameObjForDisplay,
                     e.Font,
                     new SolidBrush(Color.FromKnownColor(item._color)),
                     e.Bounds);
             }
         }
         
         private void ControlPanel_KeyDown(object sender, KeyEventArgs e)
         {
             Enum[] keys = new Enum[]
                 { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };
             int ind;
             if (keys.Contains(e.KeyCode) && !e.Alt)
             {
                 ind = Convert.ToInt32(e.KeyCode.ToString().Replace("D", ""));
                 lstObjs.SelectedIndex = ind > lstObjs.Items.Count ? lstObjs.Items.Count-1 : ind-1;
             }
             
             if (keys.Contains(e.KeyCode) && e.Alt)
             {
                 ind = Convert.ToInt32(e.KeyCode.ToString().Replace("D", ""))+10;
                 lstObjs.SelectedIndex = ind > lstObjs.Items.Count ? lstObjs.Items.Count-1 : ind-1;
             }
             e.Handled = false;
         }

         private void btnSaveImageData_Click(object sender, EventArgs e)
         {
             // create or open file
             if (lstImages.SelectedItem == null || (lstImages.SelectedItem as ObjectImage)?._pnlObjs.Count == 0) return;
             var fullPathName = (lstImages.SelectedItem as ObjectImage).FullPathImage;
             var f = File.CreateText(Path.GetDirectoryName(fullPathName) 
                                     +"\\"+ Path.GetFileNameWithoutExtension(fullPathName)
                 + ".txt");
             // calculate proportions
             var imageScaleFactor = Math.Min((double) pnlImage.Size.Height / pnlImage.BackgroundImage.Height,
                 (double) pnlImage.Size.Width / pnlImage.BackgroundImage.Width);
             var imageMarginY = (pnlImage.Size.Height - imageScaleFactor * pnlImage.BackgroundImage.Height)/2;
             var imageMarginX = (pnlImage.Size.Width - imageScaleFactor * pnlImage.BackgroundImage.Width)/2;
             foreach (var obj1 in (lstImages.SelectedItem as ObjectImage)._pnlObjs)
             {
                 var imageSfPanelCenterX = ((imageMarginX + obj1.Left + obj1.Width / 2) / imageScaleFactor)/pnlImage.BackgroundImage.Width;
                 var imageSfPanelCenterY = ((imageMarginY + obj1.Top + obj1.Height / 2) / imageScaleFactor)/pnlImage.BackgroundImage.Height;
                 var imageSfPanelSizeX = (obj1.Width / imageScaleFactor)/pnlImage.BackgroundImage.Width;
                 var imageSfPanelSizeY = (obj1.Height/ imageScaleFactor)/pnlImage.BackgroundImage.Height;
                 int index = 0;
                 for (index = 0; index < lstObjs.Items.Count; index++)
                 {
                     if (((ObjectName)lstObjs.Items[index])._nameObj == obj1.Name)
                         break;
                 }
                 f.WriteLine($"{index.ToString()} {imageSfPanelCenterX:0.####} {imageSfPanelCenterY:0.####} {imageSfPanelSizeX:0.####} {imageSfPanelSizeY:0.####}");
             }
             f.Close();
         }
    }
}