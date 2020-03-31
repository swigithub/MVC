
using System.Drawing;


namespace AirView.DBLayer.AirView.BLL
{
  public  class ColorCollection
    {


        public Color LTE_PciId(int SelectedIndex) {
            Color clr = new Color();
            if (SelectedIndex >= 0)
            {
                if (SelectedIndex == 0)
                {
                    clr = Color.Red;
                }
                else if (SelectedIndex == 1)
                {
                    clr = Color.Navy;
                }
                else if (SelectedIndex == 2)
                {
                    clr = Color.Green;
                }
                else if (SelectedIndex == 3)
                {
                    clr = Color.LimeGreen;
                }
                else if (SelectedIndex == 4)
                {
                    clr = Color.Violet;
                }
                else if (SelectedIndex == 5)
                {
                    clr = Color.Gold;
                }
                else
                {
                    clr = Color.Gray;
                }
            }
            else
            {
                clr = Color.Gray;
            }

            return clr;

        }

        public Color LTERsrp(int LteRsrp)
        {
            Color clr = new Color();
            if (LteRsrp > -75 && LteRsrp < -40)
            {
                clr = Color.DarkGreen;
            }
            else if (LteRsrp > -85 && LteRsrp < -75)
            {
                clr = Color.LightGreen;
            }
            else if (LteRsrp > -95 && LteRsrp < -85)
            {
                clr = Color.Yellow;
            }
            else if (LteRsrp > -105 && LteRsrp < -95)
            {
                clr = Color.Orange;
            }
            else if (LteRsrp > -130 && LteRsrp < -105)
            {
                clr = Color.Red;
            }

            return clr;

        }

        public Color LTERsrq(int LteRsrq)
        {
            Color clr = new Color();
            if (LteRsrq > -7 && LteRsrq < -0)
            {
                clr = Color.DarkGreen;
            }
            else if (LteRsrq > -12 && LteRsrq < -7)
            {
                clr = Color.LightGreen;
            }
            else if (LteRsrq > -16 && LteRsrq < -12)
            {
                clr = Color.Yellow;
            }

            return clr;

        }

        public Color LTEsinr(int Ltesinr)
        {
            Color clr = new Color();
            if (Ltesinr > 25 && Ltesinr < 30)
            {
                clr= Color.DarkGreen;
            }
            else if (Ltesinr > 15 && Ltesinr < 25)
            {
                clr= Color.LightGreen;
            }
            else if (Ltesinr > 5 && Ltesinr < 15)
            {
                clr= Color.Yellow;
            }
            else if (Ltesinr > -15 && Ltesinr < 5)
            {
                clr= Color.Orange;
            }

            return clr;

        }


        public Color WCDMAEcio(int WcdmaEcio)
        {
            Color clr = new Color();
            if (WcdmaEcio > -6 && WcdmaEcio < 0)
            {
                clr = Color.DarkGreen;
            }
            else if (WcdmaEcio > -10 && WcdmaEcio < -6)
            {
                clr = Color.LightGreen;
            }
            else if (WcdmaEcio > -14 && WcdmaEcio < -10)
            {
                clr = Color.Yellow;
            }
            else if (WcdmaEcio > -16 && WcdmaEcio < -14)
            {
                clr = Color.Orange;
            }
            else if (WcdmaEcio > -40 && WcdmaEcio < -16)
            {
                clr = Color.Red;
            }

            return clr;

        }

        public Color GsmRssi(int GsmRssi)
        {
            Color clr = new Color();
            if (GsmRssi > -6 && GsmRssi < 0)
            {
                clr = Color.DarkGreen;
            }
            else if (GsmRssi > -10 && GsmRssi < -6)
            {
                clr = Color.LightGreen;
            }
            else if (GsmRssi > -14 && GsmRssi < -10)
            {
                clr = Color.Yellow;
            }
            else if (GsmRssi > -16 && GsmRssi < -14)
            {
                clr = Color.Orange;
            }
            else if (GsmRssi > -40 && GsmRssi < -16)
            {
                clr = Color.Red;
            }

            return clr;

        }




    }
}
