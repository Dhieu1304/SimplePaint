   M S B A M L   `   `   `  ���� F  BSimplePaint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:SimplePaintSimplePaint      SimplePaint.MainWindow   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:SimplePaint     -     � Title$  SimplePaint��$	��800��$
��1500�� � Icon$ Images/Icon.png��.��� .+�]� $
��True��.+���  �� Orientation$ 
Horizontal=�$	��Top=�.+��� ���	#FFACACAC$��0,0,0,1q�._�]� $��20�����	#FFDDDDDD.+�o� $	��Top=�.@�m� $
��File��.@�m� -   $
��Exit��m� -   $
��Save��m� -   $
��Open��m� $��Save as��.@�m� -   $	��BMP��m� -   $	��PNG��m� -   $	��JPG��m� -   $��About���� -	   $��prototypesStackPanel��
�������$ 
Horizontal=�$	��Top=�$��40��.+��� -
   $��	textStyle��$ 
Horizontal=�$	��300��$y�Hidden=�.+��� -    ��Name$ cbx_FontFamily��$��5 0 5 0q�$y�Visible=�$	��120��$��32���� -   $ cbx_FontSize��$��5 0 5 0q�$y�Visible=�$��40��$��32��d� -    d�Name$ btn_B��
�������$y�Visible=�$��32��$��32��$��1 0 1 0q�$��B��$
��Bold"�d� -   $ btn_I��
�������$y�Visible=�$��32��$��32��$��1 0 1 0q�$��I��$
��Bold"�$��Italic$�d� -   $ btn_U��
�������$y�Visible=�$��32��$��32��$��1 0 1 0q�$
��Bold"�.���� $��	Underline{�$��U���� -    ��Name$ delete��$��30��$��30��
���  ��$��5 0 5 0q�.���� $��Images/delete.png�� �� BitmapScalingMode$ HighQuality=��� -   $
 undo��$��30��$��30��
����� �$��5 0 5 0q�.���� $��Images/undo.png��$ HighQuality=��� -   $
 redo��$��30��$��30��
����� �$��5 0 5 0q�.���� $��Images/redo.png��$ HighQuality=��� -   $��thicknessBox��$��90��$��15��$��5 0 5 0q�.@���  �� 
IsSelected$
 True��.���� $��Images/1.png��$ HighQuality=�$��80��$��15���� .���� $��Images/2.png��$ HighQuality=�$��80��$��15���� .���� $��Images/3.png��$ HighQuality=�$��80��$��15���� .���� $��Images/4.png��$ HighQuality=�$��80��$��15���� -   $��DashStyleBox��$��90��$��15��$��5 0 5 0q�.@��� $
 True��.���� $��Images/DashSty0.png��$ HighQuality=�$��80��$��15���� .���� $��Images/DashSty1.png��$ HighQuality=�$��80��$��15���� .���� $��Images/DashSty2.png��$ HighQuality=�$��80��$��15���� .���� $��Images/DashSty3.png��$ HighQuality=�$��80��$��15���� .���� $��Images/DashSty4.png��$ HighQuality=�$��80��$��15���� .���� $��Images/DashSty5.png��$ HighQuality=�$��80��$��15���� -   $ ChooseFillColorBtn��
�������$��20��$��20��$��	25 10 5 0q��� -   $ ChooseColorBtn��
���   �$��20��$��20��$��	-50 0 0 0q�� .+��� -    ��Name$ canvas��
�������$��3���� -   	 ��Name$	 canvas_border��
������ 