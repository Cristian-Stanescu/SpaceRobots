namespace SpaceRobots.Domain.ValueObjects
{
    public struct Area
    {
        public Area(int x1, int y1, int x2, int y2)
        {
            TopLeft = new GridPosition(x1, y1);
            BottomRight = new GridPosition(x2, y2);
        }
        public GridPosition TopLeft { get; set; }
        public GridPosition BottomRight { get; set; }

        public static Area EmptyArea = new(0, 0, 0, 0);
    }
}
