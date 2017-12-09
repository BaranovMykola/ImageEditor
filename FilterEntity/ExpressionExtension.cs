namespace FilterEntity
{
    public static class ExpressionExtension
    {
        public static double ComputeDouble(this NCalc.Expression func)
        {
            var item = func.Evaluate();
            double digit = 0;
            if (item is int)
            {
                digit = (int)item;
            }
            else if (item is double)
            {
                digit = (double)item;
            }
            else if (item is float)
            {
                digit = (float)item;
            }

            return digit;
        }
    }
}
