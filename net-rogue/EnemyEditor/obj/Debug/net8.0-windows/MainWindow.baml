   M S B A M L   `   `   `  ����F  BEnemyEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:EnemyEditorEnemyEditor  5   	       EnemyEditor.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:EnemyEditor     5   	   ���	#FF212121  � Title$  Rogue Enemy Editor��5   	   $	��450��6$   $	��800��61    � 	Resources 5	         EnemyTemplate��%2   &	        �� 5
   
   .R�5      � 5      5      .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��0.1* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��18(�6   
�������.��61   	Enemies6:   �� 5   
   -   5         ��Name$ 
ErrorLabel��5      $��18(�6$   
���  ��$��3��6C   6R   �� 5   
   -   5         ��Name$ 	EnemyList��5      $��1��6   ���	#FF212121
�������$��14(�6\   5      �� 5   
   $��1��6   $��18(�6    
�������.��6A   Information6N   �� 5   
   $��1��6   $��1��6"   $��
5, 0, 0, 0q�62   .+�5       �� -   5         ��Name$ EnemyNameText��5       $��15(�6/   $��5,5,0,5q�6=   
�������Name6g   �� 5!      -   5         ��Name$ 	EnemyName��5!      $��25��6)   $	��185��65   $
��Left=�6A   $��14(�6\   $��5,0,0,0q�6j   ���	#FF4C4C4C
�������6�   �� 5"      -   5        $ SpriteIdText��5"      $��15(�6.   $��5,5,0,5q�6<   
�������	Sprite ID6k   �� 5#      -   5        $ SpriteId��5#      $��25��6(   $��40��64   $
��Left=�6?   $��14(�6Z   $��5,0,0,0q�6h   ���	#FF4C4C4C
�������6�   �� 5$      -   5        $ PositionText��5$      $��15(�6.   $��5,5,0,5q�6<   
�������Position (X, Y)6q   �� 5%       �� Orientation$ 
Horizontal=�6   $
��Left=�62   .+�5&      �� -   5        $ 	PositionX��5&      $��25��6-   $��30��69   $
��Left=�6D   $��14(�6_   $��5,0,0,0q�6m   ���	#FF4C4C4C
�������6�   �� 5'      -	   5        $ 	PositionY��5'      $��25��6-   $��30��69   $
��Left=�6D   $��14(�6_   $��5,0,0,0q�6m   ���	#FF4C4C4C
�������6�   5(      �� 5)      -
   5        	 ��Name$	 SaveToJsonButton��5)      $��5,5,0,0q�6N   $
��Left=�6_   $��38��6z   $��90��6�   .��6�   Save to JSON6�   5*      5+      5,      