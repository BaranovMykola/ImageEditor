namespace WPF_GUI.ImageContainer
{
    public static class StandartFilters
    {
        public static Filter BoxFilter
        {
            get
            {
                var box = new Filter(5, 5) {Name = "Box Filter"};
                foreach (var i in box.Matrix)
                {
                    foreach (var j in i)
                    {
                        j.Coeficient = (float) (1.0/25);
                    }
                }
                return box;
            }
        }

        public static Filter RightXDerivative
        {
            get
            {
                var xdev = new Filter(3, 3) { Name = "Right Horizontal Derivative" };
                xdev.Matrix[0][0] = new FilterItem(-1);
                xdev.Matrix[1][0] = new FilterItem(-1);
                xdev.Matrix[2][0] = new FilterItem(-1);

                xdev.Matrix[0][1] = new FilterItem(0);
                xdev.Matrix[1][1] = new FilterItem(0);
                xdev.Matrix[2][1] = new FilterItem(0);

                xdev.Matrix[0][2] = new FilterItem(1);
                xdev.Matrix[1][2] = new FilterItem(1);
                xdev.Matrix[2][2] = new FilterItem(1);
                return xdev;
            }
        }

        public static Filter LeftXDerivative
        {
            get
            {
                var xdev = new Filter(3, 3) { Name = "Left Horizontal Derivative" };
                xdev.Matrix[0][0] = new FilterItem(1);
                xdev.Matrix[1][0] = new FilterItem(1);
                xdev.Matrix[2][0] = new FilterItem(1);

                xdev.Matrix[0][1] = new FilterItem(0);
                xdev.Matrix[1][1] = new FilterItem(0);
                xdev.Matrix[2][1] = new FilterItem(0);

                xdev.Matrix[0][2] = new FilterItem(-1);
                xdev.Matrix[1][2] = new FilterItem(-1);
                xdev.Matrix[2][2] = new FilterItem(-1);
                return xdev;
            }
        }
    }
}