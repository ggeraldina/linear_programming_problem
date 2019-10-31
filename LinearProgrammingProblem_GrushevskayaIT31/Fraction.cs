using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearProgrammingProblem_GrushevskayaIT31
{
    public class Fraction
    {
        public int Numerator { get; set; }
        public int Denumerator { get; set; }
        public string Text { get; set; }
        public bool IsSupport { get; set; }

        public Fraction(int num, int denum)
        {
            Denumerator = denum;
            Numerator = num;
            Text = "";
            IsSupport = false;
        }
        public Fraction(int num)
            : this(num, 1)
        {
        }
        public Fraction()
            : this(0, 1)
        {  
        }

        public static int LeastCommonMultiple(int elem1, int elem2)//НОК
        {
            int lcm = 0;
            int count = 1;
            if (elem1 == 0 || elem2 == 0)
            {
                return 0;
            }
            while (lcm == 0)
            {
                if (count % elem1 == 0 && count % elem2 == 0)
                    lcm = count;
                count++;
            }
            return lcm;
        }
        public static Fraction operator -(Fraction elem1, Fraction elem2)
        {
            int num1, num2, denum1, denum2, num3;
            denum1 = elem1.Denumerator;
            num1 = elem1.Numerator;
            denum2 = elem2.Denumerator;
            num2 = elem2.Numerator;
            int lcm = LeastCommonMultiple(denum1, denum2);
            num3 = (num1 * (lcm / denum1)) - (num2 * (lcm / denum2));
            if (num3 == 0)
            {
                lcm = 1;
            }
            if (lcm == 0)
            {
                lcm = 1;
            }
            Fraction fr = new Fraction(num3, lcm);
            return Reduce(fr);           
        }
        public static Fraction operator +(Fraction elem1, Fraction elem2)//правильно ли?
        {
            int num1, num2, denum1, denum2, num3;
            denum1 = elem1.Denumerator;
            num1 = elem1.Numerator;
            denum2 = elem2.Denumerator;
            num2 = elem2.Numerator;
            int lcm = LeastCommonMultiple(denum1, denum2);
            num3 = (num1 * (lcm / denum1)) + (num2 * (lcm / denum2));
            if (num3 == 0)
            {
                lcm = 1;
            }
            Fraction fr = new Fraction(num3, lcm);
            return Reduce(fr);  
        }
        public static Fraction operator *(Fraction elem1, Fraction elem2)
        {
            int num1, num2, denum1, denum2, num3, denum3;
            denum1 = elem1.Denumerator;
            num1 = elem1.Numerator;
            denum2 = elem2.Denumerator;
            num2 = elem2.Numerator;
            num3 = num1 * num2;
            denum3 = denum1 * denum2;            
            if (num3 == 0)
            {
                denum3 = 1;
            }
            Fraction fr = new Fraction(num3, denum3);
            return Reduce(fr);
        }
        public static Fraction operator *(int elem1, Fraction elem2)
        {
            int num, denum;            
            denum = elem2.Denumerator;
            num = elem1 * elem2.Numerator;
            if (num == 0)
            {
                denum = 1;
            }
            Fraction fr = new Fraction(num, denum);
            return Reduce(fr);
        }
        public static Fraction operator /(Fraction elem1, Fraction elem2)
        {
            int num1, num2, denum1, denum2, num3, denum3;
            denum1 = elem1.Denumerator;
            num1 = elem1.Numerator;
            denum2 = elem2.Denumerator;
            num2 = elem2.Numerator;
            Fraction fr = null;
            num3 = num1 * denum2;
            denum3 = denum1 * num2;
            if (num3 == 0)
            {
                denum3 = 1;
            }
            fr = new Fraction(num3, denum3);
            return Reduce(fr);
        }
        public static bool operator <(Fraction elem1, Fraction elem2)
        {
            int num1, num2, denum1, denum2;
            denum1 = elem1.Denumerator;
            num1 = elem1.Numerator;
            denum2 = elem2.Denumerator;
            num2 = elem2.Numerator;
            if (((float)num1 / denum1) < ((float)num2 / denum2))
                return true;
            else
                return false;
        }
        public static bool operator >(Fraction elem1, Fraction elem2)
        {
            int num1, num2, denum1, denum2;
            denum1 = elem1.Denumerator;
            num1 = elem1.Numerator;
            denum2 = elem2.Denumerator;
            num2 = elem2.Numerator;
            if (((float)num1 / denum1) > ((float)num2 / denum2))
                return true;
            else
                return false;
        }
        public static bool operator <(Fraction elem1, int integ)
        {
            int num1, denum1;
            denum1 = elem1.Denumerator;
            num1 = elem1.Numerator;
            if (((float)num1 / denum1) < integ)
                return true;
            else
                return false;
        }
        public static bool operator >(Fraction elem1, int integ)
        {
            int num1, denum1;
            denum1 = elem1.Denumerator;
            num1 = elem1.Numerator;
            if (((float)num1 / denum1) > integ)
                return true;
            else
                return false;
        }
        public static bool operator <=(Fraction elem1, int integ)
        {
            int num1, denum1;
            denum1 = elem1.Denumerator;
            num1 = elem1.Numerator;
            if (((float)num1 / denum1) <= integ)
                return true;
            else
                return false;
        }
        public static bool operator >=(Fraction elem1, int integ)
        {
            int num1, denum1;
            denum1 = elem1.Denumerator;
            num1 = elem1.Numerator;
            if (((float)num1 / denum1) >= integ)
                return true;
            else
                return false;
        }

        public bool IsEquals(Fraction elem2)
        {
            int num1, num2, denum1, denum2;
            denum1 = this.Denumerator;
            num1 = this.Numerator;
            denum2 = elem2.Denumerator;
            num2 = elem2.Numerator;
            if (((float)num1 / denum1) == ((float)num2 / denum2))
                return true;
            else
                return false;
        }
        private static int GreatestCommonDivisor(int elem1, int elem2)
        {
            elem1 = Math.Abs(elem1);
            elem2 = Math.Abs(elem2);
            int min;
            int gcd = 0;
            if (elem1 > elem2)
                min = elem2;
            else
                min = elem1;
            for (int i = 1; i <= min; i++)
            {
                if (elem1 % i == 0 && elem2 % i == 0)
                {
                    if (i > gcd)
                        gcd = i;
                }
            }
            return gcd;
        }
        public static Fraction Reduce(Fraction fr)
        {
            int num = fr.Numerator;
            int denum = fr.Denumerator;
            int gcd = GreatestCommonDivisor(num, denum);
            if (gcd != 0)
            {
                num /= gcd;
                denum /= gcd;
            }            
            return new Fraction(num, denum);
        }
        public override string ToString()
        {
        String str = "";
        if (Text.Length != 0) 
        { 
            str = Text; 
        }
        else
        {
            if (Numerator < 0 && Denumerator == -1 || Numerator >= 0 && Denumerator == 1)
            {
                str = (Math.Abs(Numerator)).ToString();
                return str;
            }
            if (Numerator < 0 && Denumerator == 1 || Numerator >= 0 && Denumerator == -1)
            {
                str = "-" + (Math.Abs(Numerator));
                return str;
            }

            if (((double)Numerator / Denumerator) > 0 && ((double)(Numerator * 1000) % Denumerator) == 0)
            {
                str = (((double)Numerator / Denumerator)).ToString();
                return str;
            }
            if (((double)Numerator / Denumerator) < 0 && ((double)(Numerator * 1000) % Denumerator) == 0)
            {
                str = (((double)Numerator / Denumerator)).ToString();
                return str;
            }

            if (Numerator < 0 && Denumerator < 0 || Numerator >= 0 && Denumerator > 0)
            {
                str = (Math.Abs(Numerator)) + "/" + Math.Abs(Denumerator);
                return str;
            }
            if (Numerator < 0 && Denumerator > 0 || Numerator >= 0 && Denumerator < 0)
            {
                str = "-" + (Math.Abs(Numerator)) + "/" + Math.Abs(Denumerator);
                return str;
            }
        }

        return str;
    }
        //int GetInteger()
        //{
        //    return integer;
        //}
        //int GetNumerator()
        //{
        //    return numerator;
        //}
        //int GetDenumerator()
        //{
        //    return denumerator;
        //}
        //void GetInteger(int integ)
        //{
        //    integer = integ;
        //}
        //void SetNumerator(int num)
        //{
        //    numerator = num;
        //}
        //void SetDenumerator(int denum)
        //{
        //    denumerator = denum;
        //}
    }
}
