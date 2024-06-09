namespace CountMinSketchs
{
    public class CountMinSketchSizeCalculator
    {
        public static (int width, int depth) CalculateWidthAndDepth(double epsilon, double delta)
        {
            int width = (int)Math.Ceiling(Math.E / epsilon);
            int depth = (int)Math.Ceiling(Math.Log(1 / delta));
            return (width, depth);
        }
    }
}
