   M S B A M L   `   `   `  ����E  AFluentWPF, Version=0.10.2.0, Culture=neutral, PublicKeyToken=nullP6clr-namespace:SourceChord.FluentWPF;assembly=FluentWPFSourceChord.FluentWPF  5      P LNavigation Drawer App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:Navigation_Drawer_AppNavigation_Drawer_App 5      $   Navigation_Drawer_App.AddImage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Efw6clr-namespace:SourceChord.FluentWPF;assembly=FluentWPF     5      5local#clr-namespace:Navigation_Drawer_App     5        � 	Resources  5
          booleanToVisibilityConverter��%   &	        �� 5   
   6'   5      .��5      � -   5         �Name$
 grit��5   ,   *   $SourceChord.FluentWPF.PointerTracker   Enabled .  � ColumnDefinitions 5   
   �� 5      $	��30* �6   �� 5      $
��349* �6   �� 5      $	��30* �6   .+�5   
   �� -   5        $��imgPhoto��5   '   $��151,122,10,10q�6   $
��Fill=�5      $��1��6   $��2��6/   $	��670��6C   �� 5   
   -   5        $��InstructionText��5      $<��6Click on the upload button below to display the image:��6+   $��1��6i   $��226,19,52,533q�6y   $��20(�6�   
�������6�   �� 5   
   -   5        $
��Load��5      $��50��63   $	��149��6?   $��0q�6K      ButtonRoundAccentRevealStyle#��[ 6`    J� RenderTransformOrigin$ 
6.88,-0.72&�6�   $��183,67,351,483q�6�   $��1��6�   .��5      �� -   5        $��UploadSymbol��5      
��� ���$��Segoe MDL2 Assets)�6@   $	����6_   $��16(�6o   $��1��6}   $��0q�6�   $��2��6�   5      �� 5   
   -   5         ��Name$ 
ImagePlace��5   +   $��151,122,10,10q�6   
���   �$��1��5!   �   $��2��6�   ��5      1   +SourceChord.FluentWPF.AcrylicBrushExtension  *grid+y�5!      �� *	IsChecked+ �� ElementName$ chkShowAcrylicLayer�� �� 	Converter# [  5"      �� 5#   
   -   5        $��Folder��5#      
��� ���$��Segoe MDL2 Assets)�66   $	����6U   $��56(�6e   $��1��6s   $��464,340,-464,-340q�6�   $��2��6�   �� 5$   
   -   5        $
��Plus��5$      
��� ���$��Segoe MDL2 Assets)�64   $	����6S   $��36(�6c   $��1��6q   $��510,380,-510,-380q�6�   $��2��6�   �� 5%   
   -	   5         ��Name$ canvas��5%      $��1��6!   $��151,122,10,10q�61   $��2��6H   5&   O   �� 5(   
   -
   5        $ canvasPolygon��5(      $��1��6(   $��151,122,10,10q�68   $��2��6O   �� 5*   
   -   5        	 ��Name$	 anotherImage��5*      $�� ��6*   $��1��62   $��226,50,394,533q�6B   $��10(�6[   
�������6i   �� 5,   
   -   5        $	 SubmitPolygonMessage��5,      $8��2Do you want to lock your computer with this image?��62   $��1��5-      $��226,19,52,533q�6$   $y�Hidden=�6;   
�������$��20(�6b   �� 5.   
   -   5        
 ��Name$
 FinalSubmit��5.      $	��Yes��6&   $��308,72,394,490q�64   $��1��6L   $y�Hidden=�6\   6p   �� 5/   
   -   5        $
 ChoosePolygonAgain��5/      $��No��6-   $��510,72,192,490q�6:   $��1��6R   $y�Hidden=�6b   6v   51      52      