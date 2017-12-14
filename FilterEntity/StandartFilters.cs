namespace FilterEntity
{
    public static class StandartFilters
    {
        public static Filter BoxFilter
        {
            get
            {
                var box = new Filter(5, 5) { Name = "Box Filter" };
                foreach (var i in box.Matrix)
                {
                    foreach (var j in i)
                    {
                        j.Coeficient = (float)(1.0 / 25);
                    }
                }

                box.Matrix[2][2].IsAnchor = true;

                return box;
            }
        }

        public static Filter RightXDerivative
        {
            get
            {
                var xdev = new Filter(1, 3) { Name = "Right X Derivative" };
                xdev.Matrix[0][0] = new FilterItem(-1);

                xdev.Matrix[0][1] = new FilterItem(0);

                xdev.Matrix[0][2] = new FilterItem(1);

                xdev.Matrix[0][1].IsAnchor = true;

                return xdev;
            }
        }

        public static Filter LeftXDerivative
        {
            get
            {
                var xdev = new Filter(1, 3) { Name = "Left X Derivative" };
                xdev.Matrix[0][0] = new FilterItem(1);

                xdev.Matrix[0][1] = new FilterItem(0);

                xdev.Matrix[0][2] = new FilterItem(-1);

                xdev.Matrix[0][1].IsAnchor = true;

                return xdev;
            }
        }

        public static Filter UpperYDerivative
        {
            get
            {
                Filter dev = new Filter(3,1) {Name = "Upper Y Derivative"};
                dev.Matrix[0][0] = new FilterItem(1);
                dev.Matrix[1][0] = new FilterItem(0) {IsAnchor = true};
                dev.Matrix[2][0] = new FilterItem(-1);
                return dev;
            }
        }

        public static Filter LowerYDerivative
        {
            get
            {
                Filter dev = new Filter(3, 1) { Name = "Lower Y Derivative" };
                dev.Matrix[0][0] = new FilterItem(-1);
                dev.Matrix[1][0] = new FilterItem(0) { IsAnchor = true };
                dev.Matrix[2][0] = new FilterItem(1);
                return dev;
            }
        }

        public static Filter GaussianFilter
        {
            get
            {
                Filter dev = new Filter(5, 5, new NCalc.Expression("(1/(3.14*2))*Exp(-(Pow(x-2,2)+Pow(y-2,2))/2)")) { Name = "Gaussian Filter" };
                return dev;
            }
        }

        public static Filter Sharpen
        {
            get
            {
                Filter sharpen = new Filter(3, 3) { Name = "Sharpen" };
                sharpen.Matrix[0][1] = new FilterItem(-1);
                sharpen.Matrix[2][1] = new FilterItem(-1);

                sharpen.Matrix[1][0] = new FilterItem(-1);
                sharpen.Matrix[1][2] = new FilterItem(-1);

                sharpen.Matrix[1][1] = new FilterItem(5) {IsAnchor = true};

                return sharpen;
            }
        }
    }
}