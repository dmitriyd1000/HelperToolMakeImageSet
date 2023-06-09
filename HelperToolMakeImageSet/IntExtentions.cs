namespace HelperToolMakeImageSet
{
    public static class IntExtentions
    {
        public static float scaleFactor;
        public static int byScaleFactor(this int value)
        {
            if (scaleFactor != null || scaleFactor != 0)
                return (int)(value / scaleFactor);
            return value;
        }
    }
}