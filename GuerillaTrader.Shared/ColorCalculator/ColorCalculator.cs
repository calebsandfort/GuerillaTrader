using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace GuerillaTrader.Shared
{
    public enum ColorScale
    {
        Rainbow,
        Heated,
        WhiteToBlack,
        Red,
        Green,
        Yellow,
        Benguela_White,
        Benguela_NoGreen,
        Grades
    }

    public class ColorCalculator
    {
        #region Properties

        public Color startColor;
        public Color lowColor;
        public Color highColor;
        public Color endColor;
        public Color finalColor;

        private ColorScale currentColorScale;

        public Color LightColor
        {
            get
            {
                Color lightColor = CreateColor((int)finalColor.R - (int)((int)finalColor.R * .2),
                    (int)finalColor.G - (int)((int)finalColor.G * .2),
                    (int)finalColor.B - (int)((int)finalColor.B * .2));
                lightColor.A = (byte)255;


                return lightColor;
            }
        }

        public ColorScale CurrentColorScale
        {
            get { return currentColorScale; }
            set { currentColorScale = value; }
        }

        private Double openPercentage;

        public Double OpenPercentage
        {
            get { return openPercentage; }
            set { openPercentage = value; }
        }
        #endregion

        public ColorCalculator(ColorScale cs, Double per)
        {
            CurrentColorScale = cs;
            OpenPercentage = per;
            UpdateColors();
            GetColor();
        }

        public void SetColorCalculator(ColorScale cs, Double per)
        {
            CurrentColorScale = cs;
            OpenPercentage = per;
            UpdateColors();
            GetColor();
        }

        public ColorCalculator()
        {
        }

        public String SetPercentage(Double per)
        {
            if (per > 1.0) per = 1.0;
            else if (per < 0.0) per = 0.0;

            OpenPercentage = per;
            GetColor();

            return this.GetHexString(this.finalColor);
        }

        public void SetColorScale(ColorScale cs)
        {
            CurrentColorScale = cs;
            UpdateColors();
            GetColor();
        }

        public void GetColor()
        {
            Double s;

            if (OpenPercentage <= .33)
            {
                s = OpenPercentage / .33;
                InterpolateColors(startColor, lowColor, s);
            }
            else if (OpenPercentage <= .66)
            {
                s = (OpenPercentage - .33) / .33;
                InterpolateColors(lowColor, highColor, s);
            }
            else
            {
                s = (OpenPercentage - .66) / .34;
                InterpolateColors(highColor, endColor, s);
            }
        }

        public void InterpolateColors(Color a, Color b, Double s)
        {
            finalColor.R = (byte)InterpolateDouble((Double)a.R, (Double)b.R, s);
            finalColor.G = (byte)InterpolateDouble((Double)a.G, (Double)b.G, s);
            finalColor.B = (byte)InterpolateDouble((Double)a.B, (Double)b.B, s);
            finalColor.A = (byte)255;
        }

        public int InterpolateDouble(Double x0, Double x1, Double s)
        {

            return (int)Math.Round(x0 + (s * (x1 - x0)));
        }

        public void UpdateColors()
        {

            switch (CurrentColorScale)
            {
                case ColorScale.Rainbow:
                    startColor = CreateColor(0, 0, 255);
                    lowColor = CreateColor(0, 255, 0);
                    highColor = CreateColor(255, 255, 0);
                    endColor = CreateColor(255, 0, 0);
                    break;
                case ColorScale.Grades:
                    startColor = CreateColor(255, 0, 0);
                    lowColor = CreateColor(255, 106, 0);
                    highColor = CreateColor(255, 216, 0);
                    endColor = CreateColor(0, 255, 0);
                    break;
                case ColorScale.Heated:
                    startColor = CreateColor(255, 255, 255);
                    lowColor = CreateColor(255, 255, 0);
                    highColor = CreateColor(255, 102, 0);
                    endColor = CreateColor(255, 0, 0);
                    break;
                case ColorScale.WhiteToBlack:
                    startColor = CreateColor(255, 255, 255);
                    lowColor = CreateColor(255, 255, 0);
                    highColor = CreateColor(255, 0, 0);
                    endColor = CreateColor(0, 0, 0);
                    break;
                case ColorScale.Red:
                    startColor = CreateColor(255, 69, 0);
                    lowColor = CreateColor(255, 0, 0);
                    highColor = CreateColor(178, 34, 34);
                    endColor = CreateColor(165, 42, 42);
                    break;
                case ColorScale.Green:
                    startColor = CreateColor(240, 230, 140);
                    lowColor = CreateColor(124, 252, 0);
                    highColor = CreateColor(34, 139, 34);
                    endColor = CreateColor(0, 100, 0);
                    break;
                case ColorScale.Yellow:
                    startColor = CreateColor(255, 255, 0);
                    lowColor = CreateColor(255, 215, 0);
                    highColor = CreateColor(218, 165, 32);
                    endColor = CreateColor(184, 134, 11);
                    break;
                case ColorScale.Benguela_White:
                    startColor = CreateColor(0, 255, 255);
                    lowColor = CreateColor(41, 90, 255);
                    highColor = CreateColor(255, 57, 49);
                    endColor = CreateColor(255, 255, 0);
                    break;
                case ColorScale.Benguela_NoGreen:
                    startColor = CreateColor(0, 0, 102);
                    lowColor = CreateColor(123, 78, 255);
                    highColor = CreateColor(250, 0, 74);
                    endColor = CreateColor(102, 0, 0);
                    break;
                default:
                    break;
            }
        }

        public Color CreateColor(int r, int g, int b)
        {
            Color c = new Color();
            c.R = (byte)r;
            c.G = (byte)g;
            c.B = (byte)b;
            return c;
        }

        #region Method - GetHexString()
        /// <summary>
        /// Convert a color to it's hexadecimal value
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public string GetHexString(Color color)
        {
            string retVal = String.Concat(
            color.R.ToString("X2", null),
            color.G.ToString("X2", null),
            color.B.ToString("X2", null));
            return retVal;
        }
        #endregion
    }
}
