   M S B A M L   `   `   `  ���� J  FREFLEX Launcher, Version=1.0.3.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:Volcano_Launcher.WindowsVolcano_Launcher.Windows  .    (Volcano_Launcher.Windows.BackendFetching   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     8local&clr-namespace:Volcano_Launcher.Windows     M IWpf.Ui, Version=4.0.0.0, Culture=neutral, PublicKeyToken=11f9f5cc97b3ffd6/ui&http://schemas.lepo.co/wpfui/2022/xaml    � WindowStyle$
  None=� � Title$ REFLEX Launcher��$	��350��$	��640�� � 
ResizeMode$ NoResize=����	#FF1E1E1E � WindowStartupLocation$ CenterScreen=� � 	Resources    SpinAnimation g� RepeatBehavior �� TargetProperty W� From	 W� To	 g� Duration  CircularProgressBar
 �� 
TargetType �� Property �� Value �� 
TargetType �� CenterX �� CenterY �� Triggers P� Property P� Value N� EnterActions��%1  &	        &	 o     7   �� $ Forever��.��W� $9 3(UIElement.RenderTransform).(RotateTransform.Angle)� 0 360$	 0:0:1D��� "
 �.���  � �� �� " �.R�� .+�A� #��z��#��z��#��z��$��3��z��� $ 50Q�$ 50Q� P�  �@�IsIndeterminate$
 True�� �� 8��  .��� .+�  Wpf.Ui.Controls.TitleBar     CanMaximize$ False��   ShowMaximize$ False��$��32��
�������   ButtonsForeground
 ������� $��Center=�$��Center=�$	��400��$��0,40,0,0q�.+��� $��REFLEX Launcher��$��28(�$
��Bold"�
�������$��Center=�$��0,0,0,30q�� -    �Name$ LoadingProgressCircle��#��[ $	��100��$	��100�� � IsIndeterminate$
 True��
������ ���	#FF007ACC$��Center=��� -    ��Name$ 
StatusText��$*��$Checking if the Backend is online...��$��16(�
������� �� TextAlignment$ Center=�$
��Wrap=�$��0,20,0,0q�$��Center=�