   M S B A M L   `   `   `  ����?  ;Lab1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:Lab1Lab1  5   	       Lab1.AdminWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:Lab1     5   	   -   5          � Title$  AdminWindow��5   	   $	��450��6   $	��800��6*    � WindowStartupLocation$ CenterScreen=�6O    � 
ResizeMode$ CanMinimize=�6t   .��5	      � .+�5
   
   �� -   5        $��usernameField��5
      $
��Left=�6'   $��23��6B   $��	85,14,0,0q�6N    �� TextWrapping$
 Wrap=�6a   $	��Top=�6u   $	��120��6�   G� 5   
   -   5        $��passwordField��5      $
��Left=�6+   $��23��6F   $��	85,41,0,0q�6R   $	��Top=�6e   $	��120��6}   �� 5   
   $��Username��6   $
��Left=�6#   $��	10,10,0,0q�6>   $	��Top=�6Q   �� 5   
   $��Password��6   $
��Left=�6#   $��	10,41,0,0q�6>   $	��Top=�6Q   �� 5   
   -   5        $��AddUser��5      $��Add User��6    $
��Left=�63   $��	10,89,0,0q�6N   $	��Top=�6a   $��75��6y   6�   �� 5   
   -   5        $��isAdmin��5      $��Admin��6"   $
��Left=�62   $��	85,69,0,0q�6M   $	��Top=�6`   &   System.Windows.Controls.DataGrid  5   
   -   5        $��	usersGrid��5      $
��Left=�6$   $	��152��6?   $��
222,14,0,0q�6L   $	��Top=�6`   $	��560��6x      AutoGenerateColumns . 6�   �� 5   
   -   5        $��UpdateButton��5      $��Update��6%   $
��Left=�66   $��222,171,0,0q�6Q   $	��Top=�6f   $��75��6~   6�   �� 5   
   -   5        $��DeleteButton��5      $��Delete��6%   $
��Left=�66   $��329,171,0,0q�6Q   $	��Top=�6f   $��75��6~   6�   5      5      