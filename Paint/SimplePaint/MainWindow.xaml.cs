using Contract;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimplePaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            KeyDown += new KeyEventHandler(OnKeyDown);
        }

        bool _isDrawing = false;
        List<IShape> _shapes = new List<IShape>();
        List<IShape> _uShapes = new List<IShape>();
        IShape _preview;
        string _selectedShapeName = "";
        Dictionary<string, IShape> _prototypes = new Dictionary<string, IShape>();
        int _thickness = 1;
        SolidColorBrush _color = new SolidColorBrush(Colors.Black);
        SolidColorBrush _fillColor;
        string _dashStyle;
        string _imageSrc;
        Point2D _start = new Point2D();

        TextBox _textBox = null;
        TextBox _textBoxPre = new TextBox();


        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = true;

           
            Point pos = e.GetPosition(canvas);
            _preview.HandleStart(pos.X, pos.Y);

            if (_selectedShapeName == "Image")
            {
                _preview.StrokeDashArr(_imageSrc);
            }
            else if (_selectedShapeName == "Text")
            {
                _textBox = new TextBox();
                _start = new Point2D() { X = pos.X, Y = pos.Y };

                getTextStyle();

            }
            else
            {

                getStyleDraw();

                _preview.Color(_color);
                _preview.FillColor(_fillColor);
                _preview.Thickness(_thickness);
                _preview.StrokeDashArr(_dashStyle);
            }

        }

        private void getTextStyle()
        {
            double fontSize = Double.Parse(cbx_FontSize.SelectedItem.ToString());
            FontFamily font = new FontFamily(((ComboBoxItem)cbx_FontFamily.SelectedItem).Content.ToString());
            _textBoxPre.FontSize = fontSize;
            _textBoxPre.FontFamily = font;
        }

        private void getStyleDraw()
        {
            int index = thicknessBox.SelectedIndex;
            switch (index)
            {
                case 0:
                    _thickness = 1;
                    break;
                case 1:
                    _thickness = 3;
                    break;
                case 2:
                    _thickness = 5;
                    break;
                case 3:
                    _thickness = 7;
                    break;
                default:
                    _thickness = 1;
                    break;
            }

            int index1 = DashStyleBox.SelectedIndex;
            switch (index1)
            {
                case 1:
                    _dashStyle = "1,1";
                    break;
                case 2:
                    _dashStyle = "1 6";
                    break;
                case 3:
                    _dashStyle = "6 1";
                    break;
                case 4:
                    _dashStyle = "0.25 1";
                    break;
                case 5:
                    _dashStyle = "1 2 4";
                    break;
                default:
                    _dashStyle = "";
                    break;
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {

                Point pos = e.GetPosition(canvas);
                _preview.HandleEnd(pos.X, pos.Y);

                // Xoá hết các hình vẽ cũ
                canvas.Children.Clear();

                // Vẽ lại các hình trước đó
                reDraw();

                if (_selectedShapeName == "Text")
                {
                    UIElement rect = new Rectangle()
                    {
                        Width = Math.Abs(pos.X - _start.X),
                        Height = Math.Abs(pos.Y - _start.Y),
                        StrokeThickness = 1,
                        Stroke = _color,
                        Fill = _fillColor
                    };

                    if (_start.X < pos.X && _start.Y < pos.Y)
                    {
                        Canvas.SetLeft(rect, _start.X);
                        Canvas.SetTop(rect, _start.Y);
                    }
                    else if (_start.X > pos.X && _start.Y > pos.Y)
                    {
                        Canvas.SetLeft(rect, pos.X);
                        Canvas.SetTop(rect, pos.Y);
                    }
                    else if (_start.X > pos.X && _start.Y < pos.Y)
                    {
                        Canvas.SetLeft(rect, pos.X);
                        Canvas.SetTop(rect, _start.Y);
                    }
                    else if (_start.X < pos.X && _start.Y > pos.Y)
                    {
                        Canvas.SetLeft(rect, _start.X);
                        Canvas.SetTop(rect, pos.Y);
                    }

                    canvas.Children.Add(rect);
                }
                else
                {
                    // Vẽ hình preview đè lên
                    canvas.Children.Add(_preview.Draw());

                }
            }
        }

        private void reDraw()
        {
            foreach (var shape in _shapes)
            {
                canvas.Children.Add(shape.Draw());
            }
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {

            _isDrawing = false;

            Point pos = e.GetPosition(canvas);
            _preview.HandleEnd(pos.X, pos.Y);

            if (_selectedShapeName == "Text")
            {
                _textBox.Width = Math.Abs(pos.X - _start.X);
                _textBox.Height = Math.Abs(pos.Y - _start.Y);
                _textBox.Background = _fillColor;
                _textBox.TextWrapping = TextWrapping.Wrap;
                _textBox.Foreground = _color;
                _textBox.FontSize = _textBoxPre.FontSize;
                _textBox.FontFamily = _textBoxPre.FontFamily;
                _textBox.FontWeight = _textBoxPre.FontWeight;
                _textBox.FontStyle = _textBoxPre.FontStyle;
                _textBox.TextDecorations = _textBoxPre.TextDecorations;
                _textBox.KeyDown += OnKeyDownHandler;

                if (_start.X < pos.X && _start.Y < pos.Y)
                {
                    Canvas.SetLeft(_textBox, _start.X);
                    Canvas.SetTop(_textBox, _start.Y);
                }
                else if (_start.X > pos.X && _start.Y > pos.Y)
                {
                    Canvas.SetLeft(_textBox, pos.X);
                    Canvas.SetTop(_textBox, pos.Y);
                }
                else if (_start.X > pos.X && _start.Y < pos.Y)
                {
                    Canvas.SetLeft(_textBox, pos.X);
                    Canvas.SetTop(_textBox, _start.Y);
                }
                else if (_start.X < pos.X && _start.Y > pos.Y)
                {
                    Canvas.SetLeft(_textBox, _start.X);
                    Canvas.SetTop(_textBox, pos.Y);
                }

                _preview.Color(_color);
                _preview.FillColor(_fillColor);
                canvas.Children.Add(_textBox);
                _textBox.Focus();
            }
            else
            {
                _shapes.Add(_preview);

                // Sinh ra đối tượng mẫu kế
                _preview = _prototypes[_selectedShapeName].Clone();

                // Ve lai Xoa toan bo
                canvas.Children.Clear();

                // Ve lai tat ca cac hinh
                reDraw();
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:

                    Debug.WriteLine("Text: " + _textBox.Text);

                    string str = "";
                    str += _textBox.Text + "| ";
                    str += ((ComboBoxItem)cbx_FontFamily.SelectedItem).Content.ToString() + "| ";
                    str += cbx_FontSize.SelectedItem.ToString() + "| ";
                    str += _textBox.FontWeight.ToString() + "| ";
                    str += _textBox.FontStyle.ToString() + "| ";
                    str += _textBoxPre.Text;

                    // muon ke ham
                    _preview.StrokeDashArr(str);

                    canvas.Children.Clear();


                    _shapes.Add(_preview);

                    _preview = _prototypes[_selectedShapeName].Clone();

                    reDraw();

                    _textBox = null;

                    break;
                case Key.Escape:

                    canvas.Children.Clear();
                    reDraw();

                    break;
                default:
                    break;
            }
            if (e.Key == Key.Enter)
            {

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var exeFolder = AppDomain.CurrentDomain.BaseDirectory;
            var dlls = new DirectoryInfo(exeFolder).GetFiles("*.dll");

            foreach (var dll in dlls)
            {
                var assembly = Assembly.LoadFile(dll.FullName);
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (type.IsClass)
                    {
                        if (typeof(IShape).IsAssignableFrom(type))
                        {
                            var shape = Activator.CreateInstance(type) as IShape;
                            _prototypes.Add(shape.Name, shape);
                        }
                    }
                }
            }

            // Tạo ra các nút bấm hàng mẫu
            foreach (var item in _prototypes)
            {
                var shape = item.Value as IShape;
                string path = System.IO.Path.Combine(Environment.CurrentDirectory, @"BtnIcon\", $"{shape.Name}.png");
                var button = new Button()
                {
                    Content = new Image
                    {
                        //pack://application:,,,/BtnIcon/open.png
                        Source = new BitmapImage(new Uri(path)),
                        VerticalAlignment = VerticalAlignment.Center
                    },
                    Width = 35,
                    Height = 35,
                    Margin = new Thickness(5, 0, 5, 0),
                    Tag = shape.Name
                };
                button.Click += prototypeButton_Click;
                prototypesStackPanel.Children.Add(button);
            }

            _selectedShapeName = _prototypes.First().Value.Name;
            _preview = _prototypes[_selectedShapeName].Clone();


            canvas_border.Cursor = Cursors.Pen;

            FontFamilySource();
            FontSizeSource();
        }

        private void prototypeButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedShapeName = (sender as Button).Tag as string;

            switch (_selectedShapeName)
            {
                case "Text":
                    //_textBox = new TextBox();
                    canvas_border.Cursor = Cursors.IBeam;
                    textStyle.Visibility = Visibility.Visible;
                    break;
                case "Eraser":
                    textStyle.Visibility = Visibility.Hidden;
                    canvas_border.Cursor = Cursors.No;
                    break;
                case "Image":

                    var openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Image(*.jpg,*.jpeg,*.jpe,*.jfif,*.png)|*.jpg;*.jpeg;*.jpe;*.jfif;*.png| All files (*.*)|*.*";
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    if (openFileDialog.ShowDialog() == true)
                    {
                        _imageSrc = openFileDialog.FileName;
                        Debug.WriteLine(_imageSrc);
                    }
                    else
                    {
                        _selectedShapeName = "Brushes";
                        canvas_border.Cursor = Cursors.Pen;
                    }

                    break;
                default:
                    textStyle.Visibility = Visibility.Hidden;
                    canvas_border.Cursor = Cursors.Pen;
                    break;
            }

            _preview = _prototypes[_selectedShapeName].Clone();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            using (StreamWriter writetext = new StreamWriter("autoSave.txt"))
            {
                writetext.WriteLine(_shapes.Count);
                foreach (var shape in _shapes)
                {
                    writetext.WriteLine(shape.toString());
                }
            }
        }

        private void ChooseColorBtn_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.ColorDialog colorDlg = new System.Windows.Forms.ColorDialog();

            if (colorDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Drawing.Color color = colorDlg.Color;
                SolidColorBrush colorBrush = new SolidColorBrush(Color.FromArgb(colorDlg.Color.A, colorDlg.Color.R, colorDlg.Color.G, colorDlg.Color.B));
                ChooseColorBtn.Background = colorBrush;
                //_color = Color.FromArgb(color.A, color.R, color.G, color.B);
                _color = colorBrush;
            }
        }

        private void ChooseFillColorBtn_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.ColorDialog colorDlg = new System.Windows.Forms.ColorDialog();

            if (colorDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Drawing.Color color = colorDlg.Color;
                SolidColorBrush colorBrush = new SolidColorBrush(Color.FromArgb(colorDlg.Color.A, colorDlg.Color.R, colorDlg.Color.G, colorDlg.Color.B));
                ChooseFillColorBtn.Background = colorBrush;
                /*_fillColor = Color.FromArgb(color.A, color.R, color.G, color.B);
                if (_fillColor == Colors.White)
                {
                    _fillColor = new Color();
                }*/
                _fillColor = colorBrush;
                if (_fillColor == new SolidColorBrush(Colors.White))
                {
                    _fillColor = new SolidColorBrush();
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
            saveFileDialog.Title = "Save an Binary File";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {

                FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);

                bw.Write(_shapes.Count);
                foreach (var shape in _shapes)
                {
                    bw.Write(shape.toString());
                }

                bw.Close();
                fs.Close();
            }

        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            _shapes.Clear();
            canvas.Children.Clear();

            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                FileStream fs = new FileStream(filePath, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);

                int count = br.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    string line = br.ReadString();
                    string firstWord = line.Substring(0, line.IndexOf(" "));
                    _preview = _prototypes[firstWord];
                    IShape shape = _preview.Parse(line);
                    _shapes.Add(shape);
                }

                br.Close();
                fs.Close();

                reDraw();

            }

            _preview = _prototypes[_selectedShapeName].Clone();

        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            _shapes.Clear();
            _preview = _prototypes[_selectedShapeName].Clone();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, "Project 2: Simple Paint\nNguyễn Đình Hiệu - 19120512\n", "About us");
        }

        private void SaveImg(string fileName)
        {
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)canvas.ActualWidth, (int)canvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            canvas.Measure(new Size((int)canvas.ActualWidth, (int)canvas.ActualHeight));
            canvas.Arrange(new Rect(new Size((int)canvas.ActualWidth, (int)canvas.ActualHeight)));

            renderBitmap.Render(canvas);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            using (FileStream file = File.Create(fileName))
            {
                encoder.Save(file);
            }
        }

        private void SaveAsBMP_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            saveFileDialog.Title = "Save an bmp File";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                SaveImg(saveFileDialog.FileName);
            }
        }

        private void SaveAsPNG_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
            saveFileDialog.Title = "Save an png File";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                SaveImg(saveFileDialog.FileName);
            }
        }

        private void SaveAsJPG_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            saveFileDialog.Title = "Save an jpg File";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                SaveImg(saveFileDialog.FileName);
            }
        }

        private void undo_Click(object sender, RoutedEventArgs e)
        {
            if (_shapes.Count > 0)
            {
                _uShapes.Insert(0, _shapes[_shapes.Count - 1]);
                _shapes.RemoveAt(_shapes.Count - 1);

                canvas.Children.Clear();
                reDraw();
            }
        }

        private void redo_Click(object sender, RoutedEventArgs e)
        {

            if (_uShapes.Count > 0)
            {
                _shapes.Add(_uShapes[0]);
                _uShapes.RemoveAt(0);

                canvas.Children.Clear();
                reDraw();
            }

        }

        private void btn_B_Unchecked(object sender, RoutedEventArgs e)
        {
            _textBoxPre.FontWeight = FontWeights.Normal;
        }

        private void btn_B_Checked(object sender, RoutedEventArgs e)
        {
            _textBoxPre.FontWeight = FontWeights.Bold;
        }

        private void btn_I_Unchecked(object sender, RoutedEventArgs e)
        {
            _textBoxPre.FontStyle = FontStyles.Normal;
        }

        private void btn_I_Checked(object sender, RoutedEventArgs e)
        {
            _textBoxPre.FontStyle = FontStyles.Italic;
        }

        private void btn_U_Unchecked(object sender, RoutedEventArgs e)
        {
            //muon ke bien :3
            _textBoxPre.Text = "";
            _textBoxPre.TextDecorations = null;
        }

        private void btn_U_Checked(object sender, RoutedEventArgs e)
        {
            //muon ke bien :3
            _textBoxPre.Text = "Underline";
            _textBoxPre.TextDecorations = TextDecorations.Underline;
        }

        private void FontFamilySource()
        {
            List<string> nonReadebleFonts = new List<string>();
            foreach (FontFamily font in Fonts.SystemFontFamilies)
            {
                ComboBoxItem boxItem = new ComboBoxItem();
                boxItem.Content = font.ToString();
                Uri s = font.BaseUri;
                if (!nonReadebleFonts.Contains(font.ToString()))
                    boxItem.FontFamily = font;

                cbx_FontFamily.Items.Add(boxItem);
            }
            cbx_FontFamily.SelectedIndex = 0;
        }

        private void FontSizeSource()
        {
            List<double> listFont = new List<double>()
            {
                8.0, 9.0, 10.0, 11.0, 12.0, 13.0, 14.0, 16.0, 18.0, 20.0, 22.0, 24.0, 26.0, 28.0, 36.0, 48.0, 72.0
            };

            cbx_FontSize.ItemsSource = listFont;
            cbx_FontSize.SelectedIndex = 0;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Z && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                undo_Click(sender, e);
            }
            else if (e.Key == Key.Y && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                redo_Click(sender, e);
            }
            else if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                SaveAsBMP_Click(sender, e);
            }

        }

    }
}
