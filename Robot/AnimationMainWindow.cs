using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Robot
{
   public partial class MainWindow
    {
        /// <summary>
        /// первая загрузка анимации
        /// </summary>
       private void animaionLoad()
        {
            DoubleAnimation buttonAnimation = new DoubleAnimation();
            buttonAnimation.From = connectBtn.ActualWidth;
            buttonAnimation.To = 150;
            buttonAnimation.Duration = TimeSpan.FromSeconds(1.5);
            connectBtn.BeginAnimation(Button.WidthProperty, buttonAnimation);

            DoubleAnimation labelAnimation = new DoubleAnimation();
            labelAnimation.From = connectOrDisconnectLbl.ActualWidth;
            labelAnimation.To = 250;
            labelAnimation.Duration = TimeSpan.FromSeconds(3);
            connectOrDisconnectLbl.BeginAnimation(Label.WidthProperty, labelAnimation);

            DoubleAnimation logoAnimation = new DoubleAnimation();
            logoAnimation.From = logoImage.ActualWidth;
            logoAnimation.To = 250;
            logoAnimation.Duration = TimeSpan.FromSeconds(5);
            logoImage.BeginAnimation(Image.WidthProperty, logoAnimation);

            DoubleAnimation robotAnimation = new DoubleAnimation();
            robotAnimation.From = logoImage.ActualWidth;
            robotAnimation.To = 250;
            robotAnimation.Duration = TimeSpan.FromSeconds(5);
            robotImage.BeginAnimation(Image.WidthProperty, robotAnimation);
          
        }
    }
}
